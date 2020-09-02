using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    /// <summary>
	/// All this dialog does is set the patnum and it is up to the calling form to do an immediate refresh, or possibly just change the patnum back to what it was.
	/// So the other patient fields must remain intact during all logic in this form, especially if SelectionModeOnly.
	/// </summary>
    public partial class FormPatientSelect : FormBase
	{
		private List<DisplayField> displayFields;

		/// <summary>
		/// Use when you want to specify a patient without changing the current patient.
		/// If true, then the Add Patient button will not be visible.
		/// </summary>
		public bool SelectionModeOnly { get; set; }

		/// <summary>
		/// When closing the form, this indicates whether a new patient was added from within this form.
		/// </summary>
		public bool NewPatientAdded { get; private set; }

		/// <summary>
		/// Only used when double clicking blank area in Appts. Sets this value to the currently selected pt.
		/// That patient will come up on the screen already selected and user just has to click OK. Or they can select a different pt or add a new pt.
		/// If 0, then no initial patient is selected.
		/// </summary>
		public long InitialPatientId { get; set; }

		/// <summary>
		/// When closing the form, this will hold the value of the newly selected PatNum.
		/// </summary>
		public long SelectedPatientId { get; set; }

		/// <summary>
		/// If set, initial patient list will be set to these patients.
		/// </summary>
		public List<long> ExplicitPatNums { get; set; }




		/// <summary>
		/// Set to true if constructor passed in patient object to prefill text boxes.
		/// Used to make sure fillGrid is not called before FormSelectPatient_Load.
		/// </summary>
		private bool _isPreFillLoad = false;

		/// <summary>
		/// Local cache of the pref PatientSelectSearchMinChars, since this will be 
		/// used in every textbox.TextChanged event and we don't want to parse the pref and convert to an int with every character entered.
		/// </summary>
		private int _patSearchMinChars = 1;

		/// <summary>
		/// Used to adjust gridpat contextmenu for right click, and unmask text
		/// </summary>
		private Point _lastClickedPoint;

		private DataTable _DataTablePats;
		private ODThread _fillGridThread = null;
		private DateTime _dateTimeLastSearch;
		private DateTime _dateTimeLastRequest;
		private PtTableSearchParams _ptTableSearchParams;


		/// <summary>
		/// Initializes a new instance of the <see cref="FormPatientSelect"/> class.
		/// </summary>
		public FormPatientSelect() : this(null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FormPatientSelect"/> class.
		/// </summary>
		/// <param name="patient">A (partially) constructed patient used to pre-fill search fields.</param>
		public FormPatientSelect(Patient patient)
		{
			InitializeComponent();

			if (patient != null)
			{
				PreFillSearchBoxes(patient);
			}
		}

		public void FormSelectPatient_Load(object sender, EventArgs e)
		{
			checkShowInactive.Checked = Prefs.GetBool(PrefName.PatientSelectShowInactive);
			if (SelectionModeOnly)
			{
				groupAddPt.Visible = false;
			}

			// Cannot add new patients from OD select patient interface.  Patient must be added from HL7 message.
			if (HL7Defs.IsExistingHL7Enabled())
			{
				HL7Def def = HL7Defs.GetOneDeepEnabled();
				if (def.ShowDemographics != HL7ShowDemographics.ChangeAndAdd)
				{
					groupAddPt.Visible = false;
				}
			}

			billingTypeComboBox.Items.Add("All");
			billingTypeComboBox.SelectedIndex = 0;
			foreach (var billingType in Definitions.GetByCategory(DefinitionCategory.BillingTypes))
			{
				billingTypeComboBox.Items.Add(billingType.Name);
			}

			if (Prefs.GetBool(PrefName.EasyHidePublicHealth))
			{
				siteComboBox.Visible = false;
				siteLabel.Visible = false;
			}
			else
			{
				siteComboBox.Items.Add("All");
				siteComboBox.SelectedIndex = 0;

				foreach (var site in Sites.GetDeepCopy())
				{
					siteComboBox.Items.Add(site);
				}
			}

			if (PrefC.HasClinicsEnabled)
			{
				if (Clinics.ClinicId == 0)
				{
					clinicComboBox.IsAllSelected = true;
				}
				else
				{
					clinicComboBox.SelectedClinicNum = Clinics.ClinicId;
				}
			}

			if (Prefs.GetBool(PrefName.PatientSSNMasked, true))
			{
				// Add "View SS#" right click option, MenuItemPopup() will show and hide it as needed.
				if (patientsGrid.ContextMenu == null)
				{
					patientsGrid.ContextMenu = new ContextMenu(); // ODGrid will automatically attach the default Popups
				}

				var menu = patientsGrid.ContextMenu;
                var menuItemUnmaskSSN = new MenuItem
                {
                    Enabled = false,
                    Visible = false,
                    Name = "ViewSS#",
                    Text = "View SS#"
                };

                if (CultureInfo.CurrentCulture.Name.EndsWith("CA")) // Canadian. en-CA or fr-CA
				{
					menuItemUnmaskSSN.Text = "View SIN";
				}

				if (CultureInfo.CurrentCulture.Name == "nl-NL")
				{
					menuItemUnmaskSSN.Text = "BSN";
				}

				menuItemUnmaskSSN.Click += MenuItemUnmaskSSN_Click;
				menu.MenuItems.Add(menuItemUnmaskSSN);
				menu.Popup += MenuItemPopupUnmaskSSN;
			}

			if (Prefs.GetBool(PrefName.PatientDOBMasked))
			{
				// Add "View DOB" right click option, MenuItemPopup() will show and hide it as needed.
				if (patientsGrid.ContextMenu == null)
				{
					patientsGrid.ContextMenu = new ContextMenu(); // ODGrid will automatically attach the default Popups
				}

				var menu = patientsGrid.ContextMenu;
                var menuItemUnmaskDOB = new MenuItem
                {
                    Enabled = false,
                    Visible = false,
                    Name = "ViewDOB",
                    Text = "View DOB"
                };

                menuItemUnmaskDOB.Click += MenuItemUnmaskDOB_Click;
				menu.MenuItems.Add(menuItemUnmaskDOB);
				menu.Popup += MenuItemPopupUnmaskDOB;
			}

			InitializeSearchOptions();
			InitializeGridColumns();


			//Using Prefs.GetString on the following two prefs so that we can call PIn.Int with hasExceptions=false and using the Math.Max and Math.Min we
			//are guaranteed to get a valid number from these prefs.
			patientsGridFillTimer.Interval = PIn.Int(Prefs.GetString(PrefName.PatientSelectSearchPauseMs));
			_patSearchMinChars = PIn.Int(Prefs.GetString(PrefName.PatientSelectSearchMinChars));
			if (ExplicitPatNums != null && ExplicitPatNums.Count > 0)
			{
				FillGrid(false, ExplicitPatNums);
				return;
			}
			if (InitialPatientId != 0)
			{
				Patient iPatient = Patients.GetLim(InitialPatientId);
				textLName.Text = iPatient.LName;
				FillGrid(false);
				return;
			}

			//Always fillGrid if _isPreFilledLoad.  Since the first name and last name are pre-filled, the results should be minimal.
			//Also FillGrid if checkRefresh is checked and either PatientSelectSearchWithEmptyParams is set or there is a character in at least one textbox
			if (_isPreFillLoad || DoRefreshGrid())
			{
				FillGrid(true);
				_isPreFillLoad = false;
			}
		}

		private void InitializeSearchOptions()
		{
            refreshCheckBox.Checked = ComputerPrefs.LocalComputer.PatSelectSearchMode switch
            {
                SearchMode.Default => !Prefs.GetBool(PrefName.PatientSelectUsesSearchButton),
                SearchMode.RefreshWhileTyping => true,
                _ => false,
            };
        }

		private void InitializeGridColumns()
		{
			patientsGrid.BeginUpdate();
			patientsGrid.ListGridColumns.Clear();

			displayFields = DisplayFields.GetForCategory(DisplayFieldCategory.PatientSelect);
			foreach (var displayField in displayFields)
			{
                var gridColumn = new GridColumn(displayField.ToString(), displayField.ColumnWidth)
                {
                    Tag = displayField
                };

                patientsGrid.ListGridColumns.Add(gridColumn);
			}

			patientsGrid.EndUpdate();
		}

		///<summary>The pat must not be null.  Takes a partially built patient object and uses it to fill the search by textboxes.
		///Currently only implements FName, LName, and HmPhone.</summary>
		public void PreFillSearchBoxes(Patient patient)
		{
			_isPreFillLoad = true; //Set to true to stop FillGrid from being called as a result of textChanged events

			if (patient.LName != "")
			{
				textLName.Text = patient.LName;
			}

			if (patient.FName != "")
			{
				textFName.Text = patient.FName;
			}

			if (patient.HmPhone != "")
			{
				textPhone.Text = patient.HmPhone;
			}
		}

		///<summary>Returns the count of chars in all of the textboxes on the form.  For ValidPhone textboxes only digit chars are counted.</summary>
		private int TextBoxCharCount()
		{
			//only count digits in ValidPhone textboxes because we auto-format with special chars, ex: (xxx)xxx-xxxx
			return this.GetAllControls()
				.OfType<TextBox>()//ValidPhone is a TextBox
				.Sum(x => x is ValidPhone ? x.Text.Count(y => char.IsDigit(y)) : x.TextLength);
		}

		/// <summary>
		/// Returns false if either checkRefresh is not checked or PatientSelectSearchWithEmptyParams is Yes or Unknown and all of the textboxes are empty.
		/// Otherwise returns true.
		/// </summary>
		private bool DoRefreshGrid()
		{
			return refreshCheckBox.Checked && (PIn.Enum<YN>(PrefC.GetInt(PrefName.PatientSelectSearchWithEmptyParams)) != YN.No || TextBoxCharCount() > 0);
		}

		/// <summary>
		/// Just prior to displaying the context menu, enable or disables the UnmaskSSN option
		/// </summary>
		private void MenuItemPopupUnmaskSSN(object sender, EventArgs e)
		{
			MenuItem menuItemSSN = patientsGrid.ContextMenu.MenuItems.OfType<MenuItem>().FirstOrDefault(x => x.Name == "ViewSS#");
			if (menuItemSSN == null) { return; }//Should not happen
			MenuItem menuItemSeperator = patientsGrid.ContextMenu.MenuItems.OfType<MenuItem>().FirstOrDefault(x => x.Text == "-");
			if (menuItemSeperator == null) { return; }//Should not happen

			int idxGridPatSSNCol = -1;

			for (int i = 0; i < patientsGrid.ListGridColumns.Count; i++)
            {
				if (patientsGrid.ListGridColumns[i].Tag is DisplayField displayField && displayField.InternalName == "SSN")
                {
					idxGridPatSSNCol = i;

					break;
				}
            }

			int idxColClick = patientsGrid.PointToCol(_lastClickedPoint.X);
			int idxRowClick = patientsGrid.PointToRow(_lastClickedPoint.Y);
			if (idxRowClick > -1 && idxColClick > -1 && idxGridPatSSNCol == idxColClick)
			{
				if (Security.IsAuthorized(Permissions.PatientSSNView, true) && patientsGrid.ListGridRows[idxRowClick].Cells[idxColClick].Text != "")
				{
					menuItemSSN.Visible = true;
					menuItemSSN.Enabled = true;
				}
				else
				{
					menuItemSSN.Visible = true;
					menuItemSSN.Enabled = false;
				}
				menuItemSeperator.Visible = true;
				menuItemSeperator.Enabled = true;
			}
			else
			{
				menuItemSSN.Visible = false;
				menuItemSSN.Enabled = false;
				if (patientsGrid.ContextMenu.MenuItems.OfType<MenuItem>().Count(x => x.Visible == true && x.Text != "-") > 1)
				{
					//There is more than one item showing, we want the seperator.
					menuItemSeperator.Visible = true;
					menuItemSeperator.Enabled = true;
				}
				else
				{
					//We dont want the seperator to be there with only one option.
					menuItemSeperator.Visible = false;
					menuItemSeperator.Enabled = false;
				}
			}
		}

		private void MenuItemUnmaskSSN_Click(object sender, EventArgs e)
		{
			if (_fillGridThread != null)
			{//still filtering results (rarely happens). 
			 //Slightly annoying to be unresponsive to the click, but the grid is going to overwrite what we fill so don't bother.
				return;
			}

			//Preference and permissions check has already happened by this point.
			//Guaranteed to be clicking on a valid row & column.
			int idxColClick = patientsGrid.PointToCol(_lastClickedPoint.X);
			int idxRowClick = patientsGrid.PointToRow(_lastClickedPoint.Y);
			long patNumClicked = PIn.Long(_DataTablePats.Rows[idxRowClick]["PatNum"].ToString());
			patientsGrid.BeginUpdate();
			patientsGrid.ListGridRows[idxRowClick].Cells[idxColClick].Text = Patients.SSNFormatHelper(Patients.GetPat(patNumClicked).SSN, false);
			patientsGrid.EndUpdate();
            string logtext;
            if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{//Canadian. en-CA or fr-CA
				logtext = "Social Insurance Number";
			}
			else
			{
				logtext = "Social Security Number";
			}
			logtext += " unmasked in Patient Select";
			SecurityLogs.MakeLogEntry(Permissions.PatientSSNView, patNumClicked, logtext);
		}

		///<summary>Just prior to displaying the context menu, enable or disables the UnmaskDOB option</summary>
		private void MenuItemPopupUnmaskDOB(object sender, EventArgs e)
		{
			MenuItem menuItemDOB = patientsGrid.ContextMenu.MenuItems.OfType<MenuItem>().FirstOrDefault(x => x.Name == "ViewDOB");
			if (menuItemDOB == null) { return; }//Should not happen
			MenuItem menuItemSeperator = patientsGrid.ContextMenu.MenuItems.OfType<MenuItem>().FirstOrDefault(x => x.Text == "-");
			if (menuItemSeperator == null) { return; }//Should not happen

			int idxGridPatDOBCol = -1;

			for (int i = 0; i < patientsGrid.ListGridColumns.Count; i++)
			{
				if (patientsGrid.ListGridColumns[i].Tag is DisplayField displayField && displayField.InternalName == "Birthdate")
				{
					idxGridPatDOBCol = i;

					break;
				}
			}

			int idxColClick = patientsGrid.PointToCol(_lastClickedPoint.X);
			int idxRowClick = patientsGrid.PointToRow(_lastClickedPoint.Y);
			if (idxRowClick > -1 && idxColClick > -1 && idxGridPatDOBCol == idxColClick)
			{
				if (Security.IsAuthorized(Permissions.PatientDOBView, true) && patientsGrid.ListGridRows[idxRowClick].Cells[idxColClick].Text != "")
				{
					menuItemDOB.Visible = true;
					menuItemDOB.Enabled = true;
				}
				else
				{
					menuItemDOB.Visible = true;
					menuItemDOB.Enabled = false;
				}
				menuItemSeperator.Visible = true;
				menuItemSeperator.Enabled = true;
			}
			else
			{
				menuItemDOB.Visible = false;
				menuItemDOB.Enabled = false;
				if (patientsGrid.ContextMenu.MenuItems.OfType<MenuItem>().Count(x => x.Visible == true && x.Text != "-") > 1)
				{
					//There is more than one item showing, we want the seperator.
					menuItemSeperator.Visible = true;
					menuItemSeperator.Enabled = true;
				}
				else
				{
					//We dont want the seperator to be there with only one option.
					menuItemSeperator.Visible = false;
					menuItemSeperator.Enabled = false;
				}
			}
		}

		private void MenuItemUnmaskDOB_Click(object sender, EventArgs e)
		{
			if (_fillGridThread != null)
			{//still filtering results (rarely happens). 
			 //Slightly annoying to be unresponsive to the click, but the grid is going to overwrite what we fill so don't bother.
				return;
			}
			//Preference and permissions check has already happened by this point.
			//Guaranteed to be clicking on a valid row & column.
			int idxColClick = patientsGrid.PointToCol(_lastClickedPoint.X);
			int idxRowClick = patientsGrid.PointToRow(_lastClickedPoint.Y);
			long patNumClicked = PIn.Long(_DataTablePats.Rows[idxRowClick]["PatNum"].ToString());
			DateTime birthdate = PIn.Date(_DataTablePats.Rows[idxRowClick]["Birthdate"].ToString());
			patientsGrid.BeginUpdate();
			patientsGrid.ListGridRows[idxRowClick].Cells[idxColClick].Text = Patients.DOBFormatHelper(birthdate, false);
			patientsGrid.EndUpdate();
			string logtext = "Date of birth unmasked in Patient Select";
			SecurityLogs.MakeLogEntry(Permissions.PatientDOBView, patNumClicked, logtext);
		}

		private void TextBox_TextChanged(object sender, EventArgs e)
		{
			patientsGridFillTimer.Stop();

			if (TextBoxCharCount() < _patSearchMinChars)
			{
				patientsGridFillTimer.Start(); // count of characters entered into all textboxes is < _patSearchMinChars, restart the timer

				return;
			}

			OnDataEntered(); // count of characters entered into all textboxes is >= _patSearchMinChars, fill the grid
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
			{
				patientsGrid.Invalidate();

				e.Handled = true;
			}
		}

		private void RefreshCheckBox_Click(object sender, EventArgs e)
		{
			patientsGridFillTimer.Stop();
			if (refreshCheckBox.Checked)
			{
				ComputerPrefs.LocalComputer.PatSelectSearchMode = SearchMode.RefreshWhileTyping;
				if (DoRefreshGrid())
				{//only fill grid if PatientSelectSearchWithEmptyParams is true or there is something in at least one textbox
					FillGrid(true);
				}
			}
			else
			{
				ComputerPrefs.LocalComputer.PatSelectSearchMode = SearchMode.UseSearchButton;
			}

			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
		}

		private void ShowArchivedCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			// We are only going to give the option to hide merged patients when Show Archived is checked.
			checkShowMerged.Visible = checkShowArchived.Checked;

			OnDataEntered();
		}

		private void SearchButton_Click(object sender, EventArgs e)
		{
			patientsGridFillTimer.Stop();

			_ptTableSearchParams = null;//this will force a grid refresh

			FillGrid(true);
		}

		private void GetAllButton_Click(object sender, EventArgs e)
		{
			patientsGridFillTimer.Stop();

			_ptTableSearchParams = null;//this will force a grid refresh

			FillGrid(false);
		}

		private void OnDataEntered(object sender = null, EventArgs e = null)
		{
			patientsGridFillTimer.Stop();//stop the timer, otherwise the timer tick will just fire this again
								 //Do not call FillGrid unless _isPreFillLoad=false.  Since the first name and last name are pre-filled, the results should be minimal.
								 //DoRefreshGrid will return true if checkRefresh is checked and either PatientSelectSearchWithEmptyParams is true (or unset) or there is some
								 //text in at least one of the textboxes
			if (!_isPreFillLoad && DoRefreshGrid())
			{
				FillGrid(true);
			}
		}

		private void FillGrid(bool doLimitOnePage, List<long> listtExplicitPatNums = null)
		{
			patientsGridFillTimer.Stop();//stop the timer, we're filling the grid now
			_dateTimeLastRequest = DateTime.Now;
			if (_fillGridThread != null)
			{
				return;
			}
			_dateTimeLastSearch = _dateTimeLastRequest;

			var billingType = billingTypeComboBox.SelectedItem as Definition;
			var site = siteComboBox.SelectedItem as Site;


			DateTime birthdate = PIn.Date(textBirthdate.Text); //this will frequently be minval.
			string clinicNums = "";
			if (PrefC.HasClinicsEnabled)
			{
				if (clinicComboBox.IsAllSelected)
				{
					//When below preference is false, don't hide user restricted clinics from view. Just return clinicNums as an empty string.
					//If this preference is true, we DO hide user restricted clinics from view.
					if (Prefs.GetBool(PrefName.PatientSelectFilterRestrictedClinics) && (Security.CurrentUser.ClinicIsRestricted || !checkShowArchived.Checked))
					{
						//only set clinicNums if user is unrestricted and showing hidden clinics, otherwise the search will show patients from all clinics
						clinicNums = string.Join(",", clinicComboBox.ListClinics
							//.Where(x => !x.IsHidden || checkShowArchived.Checked)//Only show hidden clinics if "Show Archived" is checked
							.Select(x => x.Id));
					}
				}
				else
				{
					clinicNums = clinicComboBox.SelectedClinicNum.ToString();
					if (checkShowArchived.Checked)
					{
						foreach (Clinic clinic in clinicComboBox.ListClinics)
						{
							if (clinic.IsHidden)
							{
								clinicNums += "," + clinic.Id.ToString();
							}
						}
					}
				}
			}
			bool hasSpecialty = displayFields.Any(x => x.InternalName == "Specialty");
			bool hasNextLastVisit = displayFields.Any(x => x.InternalName.In("NextVisit", "LastVisit"));
			DataTable dataTablePats = new DataTable();
			//Because hiding merged patients makes the query take longer, we will default to showing merged patients if the user has not had the 
			//opportunity to set this check box.
			bool doShowMerged = true;
			if (checkShowMerged.Visible)
			{
				//Only allow hiding merged if the Show Archived box is checked (and Show Merged is therefore visible).
				doShowMerged = checkShowMerged.Checked;
			}
			PtTableSearchParams ptTableSearchParamsCur = new PtTableSearchParams(doLimitOnePage, textLName.Text, textFName.Text, textPhone.Text, textAddress.Text,
				!checkShowInactive.Checked, textCity.Text, textState.Text, textSSN.Text, textPatNum.Text, textChartNumber.Text, billingType?.Id ?? 0, checkGuarantors.Checked,
				checkShowArchived.Checked, birthdate, site?.SiteNum ?? 0, textSubscriberID.Text, textEmail.Text, "", "", clinicNums, "",
				textInvoiceNumber.Text, listtExplicitPatNums, InitialPatientId, doShowMerged, hasSpecialty, hasNextLastVisit);
			if (_ptTableSearchParams != null && _ptTableSearchParams.Equals(ptTableSearchParamsCur))
			{//fill grid search params haven't changed, just return
				return;
			}
			_ptTableSearchParams = ptTableSearchParamsCur.Copy();
			_fillGridThread = new ODThread(new ODThread.WorkerDelegate((ODThread o) =>
			{
				dataTablePats = Patients.GetPtDataTable(ptTableSearchParamsCur);
			}));
			_fillGridThread.AddExitHandler(new ODThread.WorkerDelegate((ODThread o) =>
			{
				_fillGridThread = null;
				try
				{
					this.BeginInvoke((Action)(() =>
					{
						_DataTablePats = dataTablePats;
						FillGridFinal(doLimitOnePage);
					}));
				}
				catch (Exception) { } //do nothing. Usually just a race condition trying to invoke from a disposed form.
			}));
			_fillGridThread.AddExceptionHandler(new ODThread.ExceptionDelegate((e) =>
			{
				try
				{
					BeginInvoke((Action)(() =>
					{
						ShowError(e.Message);
					}));
				}
				catch (Exception) { } //do nothing. Usually just a race condition trying to invoke from a disposed form.
			}));
			_fillGridThread.Start(true);
		}

		private void FillGridFinal(bool doLimitOnePage)
		{
			if (InitialPatientId != 0 && doLimitOnePage)
			{
				//The InitialPatNum will be at the top, so resort the list alphabetically
				DataView dataView = _DataTablePats.DefaultView;
				dataView.Sort = "LName,FName";

				_DataTablePats = dataView.ToTable();
			}

			patientsGrid.BeginUpdate();
			patientsGrid.ListGridRows.Clear();

			for (int i = 0; i < _DataTablePats.Rows.Count; i++)
			{
				var row = new GridRow();
				for (int f = 0; f < displayFields.Count; f++)
				{
					switch (displayFields[f].InternalName)
					{
						case "LastName":
							row.Cells.Add(_DataTablePats.Rows[i]["LName"].ToString());
							break;
						case "First Name":
							row.Cells.Add(_DataTablePats.Rows[i]["FName"].ToString());
							break;
						case "MI":
							row.Cells.Add(_DataTablePats.Rows[i]["MiddleI"].ToString());
							break;
						case "Pref Name":
							row.Cells.Add(_DataTablePats.Rows[i]["Preferred"].ToString());
							break;
						case "Age":
							row.Cells.Add(_DataTablePats.Rows[i]["age"].ToString());
							break;
						case "SSN":
							row.Cells.Add(Patients.SSNFormatHelper(_DataTablePats.Rows[i]["SSN"].ToString(), Prefs.GetBool(PrefName.PatientSSNMasked)));
							break;
						case "Hm Phone":
							row.Cells.Add(_DataTablePats.Rows[i]["HmPhone"].ToString());
							if (Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled)
							{
								row.Cells[row.Cells.Count - 1].ForeColor = Color.Blue;
								row.Cells[row.Cells.Count - 1].Underline = true;
							}
							break;
						case "Wk Phone":
							row.Cells.Add(_DataTablePats.Rows[i]["WkPhone"].ToString());
							if (Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled)
							{
								row.Cells[row.Cells.Count - 1].ForeColor = Color.Blue;
								row.Cells[row.Cells.Count - 1].Underline = true;
							}
							break;
						case "PatNum":
							row.Cells.Add(_DataTablePats.Rows[i]["PatNum"].ToString());
							break;
						case "ChartNum":
							row.Cells.Add(_DataTablePats.Rows[i]["ChartNumber"].ToString());
							break;
						case "Address":
							row.Cells.Add(_DataTablePats.Rows[i]["Address"].ToString());
							break;
						case "Status":
							row.Cells.Add(_DataTablePats.Rows[i]["PatStatus"].ToString());
							break;
						case "Bill Type":
							row.Cells.Add(_DataTablePats.Rows[i]["BillingType"].ToString());
							break;
						case "City":
							row.Cells.Add(_DataTablePats.Rows[i]["City"].ToString());
							break;
						case "State":
							row.Cells.Add(_DataTablePats.Rows[i]["State"].ToString());
							break;
						case "Pri Prov":
							row.Cells.Add(_DataTablePats.Rows[i]["PriProv"].ToString());
							break;
						case "Clinic":
							row.Cells.Add(_DataTablePats.Rows[i]["clinic"].ToString());
							break;
						case "Birthdate":
							row.Cells.Add(Patients.DOBFormatHelper(PIn.Date(_DataTablePats.Rows[i]["Birthdate"].ToString()), Prefs.GetBool(PrefName.PatientDOBMasked)));
							break;
						case "Site":
							row.Cells.Add(_DataTablePats.Rows[i]["site"].ToString());
							break;
						case "Email":
							row.Cells.Add(_DataTablePats.Rows[i]["Email"].ToString());
							break;
						case "Country":
							row.Cells.Add(_DataTablePats.Rows[i]["Country"].ToString());
							break;
						case "RegKey":
							row.Cells.Add(_DataTablePats.Rows[i]["RegKey"].ToString());
							break;
						case "OtherPhone": //will only be available if OD HQ
							row.Cells.Add(_DataTablePats.Rows[i]["OtherPhone"].ToString());
							break;
						case "Wireless Ph":
							row.Cells.Add(_DataTablePats.Rows[i]["WirelessPhone"].ToString());
							if (Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled)
							{
								row.Cells[row.Cells.Count - 1].ForeColor = Color.Blue;
								row.Cells[row.Cells.Count - 1].Underline = true;
							}
							break;
						case "Sec Prov":
							row.Cells.Add(_DataTablePats.Rows[i]["SecProv"].ToString());
							break;
						case "LastVisit":
							row.Cells.Add(_DataTablePats.Rows[i]["lastVisit"].ToString());
							break;
						case "NextVisit":
							row.Cells.Add(_DataTablePats.Rows[i]["nextVisit"].ToString());
							break;
						case "Invoice Number":
							row.Cells.Add(_DataTablePats.Rows[i]["StatementNum"].ToString());
							break;
						case "Specialty":
							row.Cells.Add(_DataTablePats.Rows[i]["Specialty"].ToString());
							break;
					}
				}
				patientsGrid.ListGridRows.Add(row);
			}

			patientsGrid.EndUpdate();
			if (_dateTimeLastSearch != _dateTimeLastRequest)
			{
				FillGrid(doLimitOnePage);//in case data was entered while thread was running.
			}

			patientsGrid.SetSelected(0, true);
			for (int i = 0; i < _DataTablePats.Rows.Count; i++)
			{
				if (PIn.Long(_DataTablePats.Rows[i][0].ToString()) == InitialPatientId)
				{
					patientsGrid.SetSelected(i, true);
					break;
				}
			}
		}

		private void PatientsGrid_MouseDown(object sender, MouseEventArgs e)
		{
			_lastClickedPoint = e.Location;
		}

		private void PatientsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			CloseFormIfPatientSelected();
		}

		private void PatientsGrid_CellClick(object sender, ODGridClickEventArgs e)
		{
			var gridCell = patientsGrid.ListGridRows[e.Row].Cells[e.Col];

			// Only grid cells with phone numbers are blue and underlined.
			if (gridCell.ForeColor == Color.Blue && 
				gridCell.Underline == true && 
				Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled)
			{
				DentalTek.PlaceCall(gridCell.Text);
			}
		}

		private void AddPatientButton_Click(object sender, System.EventArgs e)
		{
			if (textLName.Text == "" && textFName.Text == "" && textChartNumber.Text == "")
			{
				ShowInfo(
					"Not allowed to add a new patient until you have done a search to see if that patient already exists. " +
					"Hint: just type a few letters into the Last Name box above.");

				return;
			}

			long? primaryProviderId = null;
			if (!Prefs.GetBool(PrefName.PriProvDefaultToSelectProv))
			{
				// Explicitly use the combo clinic instead of FormOpenDental.ClinicNum because the combo box should default to that clinic unless manually changed by the user.
				if (PrefC.HasClinicsEnabled && !clinicComboBox.IsAllSelected)
				{
					// clinics enabled and all isn't selected
					// Set the patients primary provider to the clinic default provider.

					primaryProviderId = Providers.GetDefaultProvider(clinicComboBox.SelectedClinicNum)?.ProvNum;
				}
				else
				{
					// Set the patients primary provider to the practice default provider.
					primaryProviderId = Providers.GetDefaultProvider()?.ProvNum;
				}
			}

			var patient =
				Patients.CreateNewPatient(
					textLName.Text, 
					textFName.Text, 
					PIn.Date(textBirthdate.Text), 
					primaryProviderId, 
					Clinics.ClinicId, 
					"Created from Select Patient window.");

			var family = Patients.GetFamily(patient.PatNum);

            using var formPatientEdit = new FormPatientEdit(patient, family)
            {
                IsNew = true
            };

            if (formPatientEdit.ShowDialog(this) == DialogResult.OK)
			{
				NewPatientAdded = true;

				SelectedPatientId = patient.PatNum;

				DialogResult = DialogResult.OK;
			}
		}

		private void AddManyButton_Click(object sender, EventArgs e)
		{
			if (textLName.Text == "" && textFName.Text == "" && textChartNumber.Text == "")
			{
				ShowInfo(
					"Not allowed to add a new patient until you have done a search to see if that patient already exists. " +
					"Hint: just type a few letters into the Last Name box above.");

				return;
			}

			using var formPatientAddAll = new FormPatientAddAll();

			if (textLName.Text.Length > 1)
			{
				formPatientAddAll.LName = textLName.Text.Substring(0, 1).ToUpper() + textLName.Text.Substring(1);
			}

			if (textFName.Text.Length > 1)
			{
				formPatientAddAll.FName = textFName.Text.Substring(0, 1).ToUpper() + textFName.Text.Substring(1);
			}

			if (textBirthdate.Text.Length > 1)
			{
				formPatientAddAll.Birthdate = PIn.Date(textBirthdate.Text);
			}

			if (formPatientAddAll.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			NewPatientAdded = true;

			SelectedPatientId = formPatientAddAll.SelectedPatNum;

			DialogResult = DialogResult.OK;
		}

		private void CloseFormIfPatientSelected()
		{
			if (_fillGridThread != null) return; // Still filtering results (rarely happens)
			

			long selectedPatientId = PIn.Long(_DataTablePats.Rows[patientsGrid.GetSelectedIndex()]["PatNum"].ToString());
			if (PrefC.HasClinicsEnabled)
			{
				long patientClinicId = PIn.Long(_DataTablePats.Rows[patientsGrid.GetSelectedIndex()]["ClinicNum"].ToString());

				var clinicIds = clinicComboBox.ListClinics.Select(x => x.Id).ToList();
				if (!Security.CurrentUser.ClinicIsRestricted)
				{
					clinicIds.Add(0);
				}

				// If the user has security permissions to search all patients, or patient is assigned to one of the user's unrestricted clinics,
				// or patient has an appointment in one of the user's unrestricted clincis, 
				// allow them to select the patient
				if (Security.IsAuthorized(Permissions.UnrestrictedSearch, true) || patientClinicId.In(clinicIds) || Appointments.GetAppointmentsForPat(selectedPatientId).Select(x => x.ClinicNum).Any(x => x.In(clinicIds)))
				{
					SelectedPatientId = selectedPatientId;

					DialogResult = DialogResult.OK;
				}
				else
				{
					// Otherwise, present the error message explainign why they cannot select the patient.
					ShowError(
						"This patient is assigned to a clinic that you are not authorized for. " +
						"Contact an Administrator to grant you access or to create an appointment in your clinic to avoid patient duplication.");
				}
			}
			else
			{
				SelectedPatientId = selectedPatientId;

				DialogResult = DialogResult.OK;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (patientsGrid.GetSelectedIndex() == -1)
			{
				ShowError("Please select a patient first.");

				return;
			}

			CloseFormIfPatientSelected();
		}

        private void searchGroupBox_Enter(object sender, EventArgs e)
        {

        }
    }
}
