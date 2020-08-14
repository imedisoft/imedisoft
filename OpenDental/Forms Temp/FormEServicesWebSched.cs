using CodeBase;
using OpenDental.UI;
using OpenDental.User_Controls;
using OpenDentBusiness;
using OpenDentBusiness.WebTypes.WebSched.TimeSlot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{

	public partial class FormEServicesWebSched : ODForm
	{
		#region Fields
		///<summary>The fake clinic num used for the "Default" clinic option in the WebSchedVerify clinic combo box.  We need to change this to the constant that's in the combobox for clarity.</summary>
		private const int CLINIC_NUM_DEFAULT = 0;
		///<summary>Set this to true to indicate the time slots grid needs to be refilled.</summary>
		private bool _doRefillTimeSlots;
		///<summary>Keeps track of the last selected index for the Web Sched New Pat Appt URL grid.</summary>
		private int _indexLastNewPatURL = -1;
		///<summary>Set to true whenever the Web Sched new pat appt thread is already running while another thing wants it to refresh yet again.
		///E.g. The window loads which initially starts a fill thread and then the user quickly starts changing filters.</summary>
		private bool _isWebSchedNewPatApptTimeSlotsOutdated = false;
		///<summary>Set to true whenever the Web Sched recall thread is already running while another thing wants it to refresh yet again.
		///E.g. The window loads which initially starts a fill thread and then the user quickly starts changing filters.</summary>
		private bool _isWebSchedTimeSlotsOutdated = false;
		//List of a List that holds information on what preferences have been changed
		private List<(string, string)> _listClinicPrefsWebSchedNewPats = new List<(string, string)>();
		private List<RecallType> _listRecallTypes;
		///<summary>A list of all clinics.  This list could include clinics that the user should not have access to so be careful using it. For the sake of modular code, there are seperate lists for the Integrated Texting (sms) and Automated eConfirmation (eC) tabs.</summary>
		private List<Clinic> _listWebSchedClinics;
		///<summary>A deep copy of Providers.ListShortDeep.  Use the cache instead of this list if you need an up to date list of providers.</summary>
		private List<Provider> _listWebSchedProviders;
		///<summary>A list of all operatories that have IsWebSched set to true.</summary>
		private List<Operatory> _listWebSchedRecallOps;
		///<summary>The in-memory list of updated ClinicPrefs related to WebSchedVerify, used to track changes made while the window is open.</summary>
		private List<(string, string)> _listWebSchedVerifyClinicPrefs = new List<(string, string)>();
		///<summary>The in-memory list of original ClinicPrefs related to WebSchedVerify, used to compare changes when saving on window close.</summary>
		private List<(string, string)> _listWebSchedVerifyClinicPrefs_Old;
		///<summary>The list of prefNames modified by the WebSched Verify tab.</summary>
		private List<string> _listWebSchedVerifyPrefNames = new List<string>() {
				PrefName.WebSchedVerifyRecallType,
				PrefName.WebSchedVerifyRecallText,
				PrefName.WebSchedVerifyRecallEmailSubj,
				PrefName.WebSchedVerifyRecallEmailBody,
				PrefName.WebSchedVerifyNewPatType,
				PrefName.WebSchedVerifyNewPatText,
				PrefName.WebSchedVerifyNewPatEmailSubj,
				PrefName.WebSchedVerifyNewPatEmailBody,
				PrefName.WebSchedVerifyASAPType,
				PrefName.WebSchedVerifyASAPText,
				PrefName.WebSchedVerifyASAPEmailSubj,
				PrefName.WebSchedVerifyASAPEmailBody,
				PrefName.WebSchedVerifyAsapEmailTemplateType,
				PrefName.WebSchedVerifyRecallEmailTemplateType,
				PrefName.WebSchedVerifyNewPatEmailTemplateType,
			};
		WebServiceMainHQProxy.EServiceSetup.SignupOut _signupOut;
		private ODThread _threadFillGridWebSchedNewPatApptTimeSlots = null;
		private ODThread _threadFillGridWebSchedTimeSlots = null;
		///<summary>Clinic number used to filter the Time Slots grid.  0 is treated as 'Unassigned'</summary>
		private long _webSchedClinicNum = 0;
		///<summary>Provider number used to filter the Time Slots grid.  0 is treated as 'All'</summary>
		private long _webSchedProvNum = 0;
		#endregion Fields

		public FormEServicesWebSched(WebServiceMainHQProxy.EServiceSetup.SignupOut signupOut = null, WebSchedTab setTab = WebSchedTab.Recall)
		{
			InitializeComponent();
			
			_signupOut = signupOut;
			switch (setTab)
			{
				case WebSchedTab.NewPat:
					tabControlWebSched.SelectTab(tabWebSchedNewPatAppts);
					break;
				case WebSchedTab.Verify:
					tabControlWebSched.SelectTab(tabWebSchedVerify);
					break;
				default:
					tabControlWebSched.SelectTab(tabWebSchedRecalls);
					break;
			}
		}

		private void FormEServicesWebSched_Load(object sender, EventArgs e)
		{
			if (_signupOut == null)
			{
				_signupOut = FormEServicesSetup.GetSignupOut();
			}
			bool allowEdit = Security.IsAuthorized(Permissions.EServicesSetup, true);
			AuthorizeWebSchedNewPat(allowEdit);
			AuthorizeWebSchedRecall(allowEdit);
			FillTabWebSchedNewPat();
			FillTabWebSchedRecall();
			FillTabWebSchedVerify();
		}

		#region Tab - New Patient
		private void FillTabWebSchedNewPat()
		{
			int newPatApptDays = PrefC.GetInt(PrefName.WebSchedNewPatApptSearchAfterDays);
			textWebSchedNewPatApptMessage.Text = Prefs.GetString(PrefName.WebSchedNewPatApptMessage);
			textWebSchedNewPatApptSearchDays.Text = newPatApptDays > 0 ? newPatApptDays.ToString() : "";
			checkWebSchedNewPatForcePhoneFormatting.Checked = Prefs.GetBool(PrefName.WebSchedNewPatApptForcePhoneFormatting);
			DateTime dateWebSchedNewPatSearch = DateTime.Now;
			dateWebSchedNewPatSearch = dateWebSchedNewPatSearch.AddDays(newPatApptDays);
			textWebSchedNewPatApptsDateStart.Text = dateWebSchedNewPatSearch.ToShortDateString();
			FillWSNPABlockoutTypes();
			FillGridWebSchedNewPatApptHostedURLs();
			FillGridWSNPAReasons();
			FillGridWebSchedNewPatApptOps();
			//This needs to happen after all of the previous fills because it's asynchronous.
			FillGridWebSchedNewPatApptTimeSlotsThreaded();
			long defaultStatus = Prefs.GetLong(PrefName.WebSchedNewPatConfirmStatus);
			comboWSNPConfirmStatuses.Items.AddDefs(Defs.GetDefsForCategory(DefCat.ApptConfirmed, true));
			comboWSNPConfirmStatuses.SetSelectedDefNum(defaultStatus);
			checkNewPatAllowProvSelection.Checked = Prefs.GetBool(PrefName.WebSchedNewPatAllowProvSelection);
			if (!PrefC.HasClinicsEnabled)
			{
				labelWSNPClinic.Visible = false;
				comboWSNPClinics.Visible = false;
				labelWSNPTimeSlots.Visible = false;
			}
			checkWSNPDoubleBooking.Checked = PrefC.GetInt(PrefName.WebSchedNewPatApptDoubleBooking) > 0;//0 = Allow double booking, 1 = prevent
		}

		///<summary>Fills both the Restricted to and Generally Allowed blockout type list boxes.</summary>
		private void FillWSNPABlockoutTypes()
		{
			//Get all WSNPA reasons (defs)
			List<Def> listReasonDefs = Defs.GetDefsForCategory(DefCat.WebSchedNewPatApptTypes, isShort: true);
			//Get every blockout type deflink for the reason defs.
			List<DefLink> listRestrictedToDefLinks = DefLinks.GetDefLinksByTypeAndDefs(DefLinkType.BlockoutType, listReasonDefs.Select(x => x.DefNum).ToList());
			List<long> listRestrictedToDefNums = listRestrictedToDefLinks.Select(x => x.FKey).Distinct().ToList();
			List<Def> listRestrictedToBlockoutDefs = Defs.GetDefs(DefCat.BlockoutTypes, listRestrictedToDefNums);
			List<Def> listAllowedBlockoutDefs = Defs.GetDefs(DefCat.BlockoutTypes, PrefC.GetWebSchedNewPatAllowedBlockouts.FindAll(x => !x.In(listRestrictedToDefNums)));
			//Fill the list box for Restricted to Reasons that will display the blockout name followed by associated reason names.
			List<string> listBlockoutReasonAssociations = new List<string>();
			foreach (Def blockout in listRestrictedToBlockoutDefs)
			{
				//Find the reasons for this restricted to blockout which are FKey def links.
				List<long> listAssociatedReasonNums = listRestrictedToDefLinks.Where(x => x.FKey == blockout.DefNum).Select(x => x.DefNum).ToList();
				List<string> listAssociatedReasonNames = listReasonDefs.Where(x => x.DefNum.In(listAssociatedReasonNums)).Select(x => x.ItemName).ToList();
				listBlockoutReasonAssociations.Add(blockout.ItemName + " (" + string.Join(",", listAssociatedReasonNames) + ")");
			}
			listboxWSNPARestrictedToReasons.SetItems(listBlockoutReasonAssociations);
			//Fill the list box for Generally Allowed
			listboxWebSchedNewPatIgnoreBlockoutTypes.SetItems(listAllowedBlockoutDefs.Select(x => x.ItemName));
		}

		private void SaveTabWebSchedNewPat()
		{
			//Prefs.Set(PrefName.WebSchedNewPatApptMessage, textWebSchedNewPatApptMessage.Text);
			//List<ClinicPref> listClinicPrefs = new List<ClinicPref>();
			//foreach (Control control in panelHostedURLs.Controls)
			//{
			//	if (control.GetType() != typeof(ContrNewPatHostedURL))
			//	{
			//		continue;
			//	}
			//	ContrNewPatHostedURL urlPanel = (ContrNewPatHostedURL)control;
			//	long clinicNum = urlPanel.GetClinicNum();
			//	string allowChildren = urlPanel.GetPrefValue(PrefName.WebSchedNewPatAllowChildren);
			//	string verifyInfo = urlPanel.GetPrefValue(PrefName.WebSchedNewPatVerifyInfo);
			//	string doAuthEmail = urlPanel.GetPrefValue(PrefName.WebSchedNewPatDoAuthEmail);
			//	string doAuthText = urlPanel.GetPrefValue(PrefName.WebSchedNewPatDoAuthText);
			//	string webFormsURL = urlPanel.GetPrefValue(PrefName.WebSchedNewPatWebFormsURL);
			//	if (clinicNum == 0)
			//	{
			//		Prefs.Set(PrefName.WebSchedNewPatAllowChildren, allowChildren);
			//		Prefs.Set(PrefName.WebSchedNewPatVerifyInfo, verifyInfo);
			//		Prefs.Set(PrefName.WebSchedNewPatDoAuthEmail, doAuthEmail);
			//		Prefs.Set(PrefName.WebSchedNewPatDoAuthText, doAuthText);
			//		Prefs.Set(PrefName.WebSchedNewPatWebFormsURL, webFormsURL);
			//		continue;
			//	}
			//	listClinicPrefs.Add(GetClinicPrefToSave(clinicNum, PrefName.WebSchedNewPatAllowChildren, allowChildren));
			//	listClinicPrefs.Add(GetClinicPrefToSave(clinicNum, PrefName.WebSchedNewPatVerifyInfo, verifyInfo));
			//	listClinicPrefs.Add(GetClinicPrefToSave(clinicNum, PrefName.WebSchedNewPatDoAuthEmail, doAuthEmail));
			//	listClinicPrefs.Add(GetClinicPrefToSave(clinicNum, PrefName.WebSchedNewPatDoAuthText, doAuthText));
			//	listClinicPrefs.Add(GetClinicPrefToSave(clinicNum, PrefName.WebSchedNewPatWebFormsURL, webFormsURL));
			//}
			//if (ClinicPrefs.Sync(listClinicPrefs, _listClinicPrefsWebSchedNewPats))
			//{
			//	DataValid.SetInvalid(InvalidType.ClinicPrefs);
			//}
			//if (comboWSNPConfirmStatuses.SelectedIndex != -1)
			//{
			//	Prefs.Set(PrefName.WebSchedNewPatConfirmStatus, comboWSNPConfirmStatuses.GetSelectedDefNum());
			//}
			//Prefs.Set(PrefName.WebSchedNewPatAllowProvSelection, checkNewPatAllowProvSelection.Checked);
			//Prefs.Set(PrefName.WebSchedNewPatApptDoubleBooking, checkWSNPDoubleBooking.Checked ? 1 : 0);
		}

		private (string, string) GetClinicPrefToSave(long clinicNum, string prefName, string value)
		{
			//ClinicPref clinicPref = _listClinicPrefsWebSchedNewPats.FirstOrDefault(x => x.ClinicNum == clinicNum && x.PrefName == prefName)?.Clone();
			//if (clinicPref == null)
			//{
			//	return new ClinicPref(clinicNum, prefName, value);
			//}
			//clinicPref.ValueString = value;
			//return clinicPref;
			return default;
		}

		private void AuthorizeWebSchedNewPat(bool allowEdit)
		{
			butWebSchedNewPatBlockouts.Enabled = allowEdit;
			textWebSchedNewPatApptSearchDays.Enabled = allowEdit;
		}

		private void textWebSchedNewPatApptSearchDays_Leave(object sender, EventArgs e)
		{
			//Only refresh if the value of this preference changed.  _indexLastNewPatURL will be set to -1 if a refresh is needed.
			if (_indexLastNewPatURL == -1)
			{
				FillGridWebSchedNewPatApptTimeSlotsThreaded();
			}
		}

		private void comboWSNPClinics_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (comboWSNPClinics.SelectedIndex != _indexLastNewPatURL)
			{
				FillGridWebSchedNewPatApptTimeSlotsThreaded();
			}
		}

		private void FillGridWSNPAReasons()
		{
			List<Def> listDefs = Defs.GetDefsForCategory(DefCat.WebSchedNewPatApptTypes, true);
			List<DefLink> listDefLinks = DefLinks.GetDefLinksByType(DefLinkType.AppointmentType);
			List<AppointmentType> listApptTypes = AppointmentTypes.GetWhere(x => listDefLinks.Any(y => y.FKey == x.AppointmentTypeNum), true);
			//The combo box within the available times group box should always reflect the grid.
			comboWSNPADefApptType.Items.Clear();
			gridWSNPAReasons.BeginUpdate();
			gridWSNPAReasons.ListGridColumns.Clear();
			gridWSNPAReasons.ListGridColumns.Add(new GridColumn("Reason", 120));
			gridWSNPAReasons.ListGridColumns.Add(new GridColumn("Pattern", 80) { IsWidthDynamic = true });
			gridWSNPAReasons.ListGridRows.Clear();
			GridRow row;
			foreach (Def def in listDefs)
			{
				AppointmentType apptType = null;
				DefLink defLink = listDefLinks.FirstOrDefault(x => x.DefNum == def.DefNum);
				if (defLink == null)
				{
					continue;//Corruption?
				}
				apptType = listApptTypes.FirstOrDefault(x => x.AppointmentTypeNum == defLink.FKey);
				if (apptType == null)
				{
					continue;//Corruption?
				}
				row = new GridRow();
				row.Cells.Add(def.ItemName);
				row.Cells.Add((string.IsNullOrEmpty(apptType.Pattern) ? "(use procedure time pattern)" : Appointments.ConvertPatternFrom5(apptType.Pattern)));
				gridWSNPAReasons.ListGridRows.Add(row);
				comboWSNPADefApptType.Items.Add(def.ItemName, def);
			}
			gridWSNPAReasons.EndUpdate();
			if (comboWSNPADefApptType.Items.Count > 0)
			{
				comboWSNPADefApptType.SelectedIndex = 0;//Select Default.
			}
		}

		private void FillGridWebSchedNewPatApptHostedURLs()
		{
			//panelHostedURLs.Controls.Clear();
			//List<Clinic> clinicsAll = Clinics.GetDeepCopy();
			//var eServiceData = WebServiceMainHQProxy.GetSignups<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>(_signupOut, eServiceCode.WebSchedNewPatAppt)
			//	.Select(x => new
			//	{
			//		Signup = x,
			//		ClinicName = (clinicsAll.FirstOrDefault(y => y.ClinicNum == x.ClinicNum) ?? new Clinic() { Abbr = "N\\A" }).Abbr
			//	})
			//	.Where(x =>
			//		//When clinics off, only show headquarters
			//		(!PrefC.HasClinicsEnabled && x.Signup.ClinicNum == 0) ||
			//		//When clinics are on, only show if not hidden.
			//		(PrefC.HasClinicsEnabled && clinicsAll.Any(y => y.ClinicNum == x.Signup.ClinicNum && !y.IsHidden))
			//	)
			//	//Alpha sorted
			//	.OrderBy(x => x.ClinicName);
			//_listClinicPrefsWebSchedNewPats.Clear();
			//foreach (var clinic in eServiceData)
			//{
			//	ContrNewPatHostedURL contr = new ContrNewPatHostedURL(clinic.Signup);
			//	if (!PrefC.HasClinicsEnabled || eServiceData.Count() == 1)
			//	{
			//		contr.IsExpanded = true;
			//		contr.DoHideExpandButton = true;
			//	}
			//	Lan.C(this, contr);
			//	panelHostedURLs.Controls.Add(contr);
			//	comboWSNPClinics.Items.Add(new ODBoxItem<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>(clinic.ClinicName, clinic.Signup));
			//	if (clinic.Signup.ClinicNum == 0)
			//	{
			//		continue;
			//	}
			//	else
			//	{
			//		AddClinicPrefToList(PrefName.WebSchedNewPatAllowChildren, clinic.Signup.ClinicNum);
			//		AddClinicPrefToList(PrefName.WebSchedNewPatVerifyInfo, clinic.Signup.ClinicNum);
			//		AddClinicPrefToList(PrefName.WebSchedNewPatDoAuthEmail, clinic.Signup.ClinicNum);
			//		AddClinicPrefToList(PrefName.WebSchedNewPatDoAuthText, clinic.Signup.ClinicNum);
			//		AddClinicPrefToList(PrefName.WebSchedNewPatWebFormsURL, clinic.Signup.ClinicNum);
			//	}
			//}
		}

		private void AddClinicPrefToList(string prefName, long clinicNum)
		{
			//ClinicPref clinicPref = ClinicPrefs.GetPref(prefName, clinicNum);
			//if (clinicPref != null)
			//{
			//	_listClinicPrefsWebSchedNewPats.Add(clinicPref);
			//}
		}

		private void NavigateToURL(string URL)
		{
			try
			{
				Process.Start(URL);
			}
			catch (Exception)
			{
				MessageBox.Show("There was a problem launching the URL with a web browser.  Make sure a default browser has been set.");
			}
		}

		private void FillGridWebSchedNewPatApptOps()
		{
			int opNameWidth = 150;
			int clinicWidth = 150;
			if (!PrefC.HasClinicsEnabled)
			{
				opNameWidth += clinicWidth;
			}
			gridWebSchedNewPatApptOps.BeginUpdate();
			gridWebSchedNewPatApptOps.ListGridColumns.Clear();
			gridWebSchedNewPatApptOps.ListGridColumns.Add(new GridColumn("Op Name", opNameWidth));
			gridWebSchedNewPatApptOps.ListGridColumns.Add(new GridColumn("Abbrev", 60));
			if (PrefC.HasClinicsEnabled)
			{
				gridWebSchedNewPatApptOps.ListGridColumns.Add(new GridColumn("Clinic", clinicWidth));
			}
			gridWebSchedNewPatApptOps.ListGridColumns.Add(new GridColumn("Provider", 60));
			gridWebSchedNewPatApptOps.ListGridColumns.Add(new GridColumn("Hygienist", 60));
			gridWebSchedNewPatApptOps.ListGridColumns.Add(new GridColumn("ApptTypes", 40) { IsWidthDynamic = true });
			gridWebSchedNewPatApptOps.ListGridRows.Clear();
			//A list of all operatories that are considered for web sched new pat appt.
			List<Operatory> listWSNPAOps = Operatories.GetOpsForWebSchedNewPatAppts();
			List<long> listWSNPADefNums = listWSNPAOps.SelectMany(x => x.ListWSNPAOperatoryDefNums).Distinct().ToList();
			List<Def> listWSNPADefs = Defs.GetDefs(DefCat.WebSchedNewPatApptTypes, listWSNPADefNums);
			GridRow row;
			foreach (Operatory op in listWSNPAOps)
			{
				row = new GridRow();
				row.Cells.Add(op.OpName);
				row.Cells.Add(op.Abbrev);
				if (PrefC.HasClinicsEnabled)
				{
					row.Cells.Add(Clinics.GetAbbr(op.ClinicNum));
				}
				row.Cells.Add(Providers.GetAbbr(op.ProvDentist));
				row.Cells.Add(Providers.GetAbbr(op.ProvHygienist));
				//Display the name of all "appointment types" (definition.ItemName) that are associated with the current operatory.
				row.Cells.Add(string.Join(", ", listWSNPADefs.Where(x => op.ListWSNPAOperatoryDefNums.Any(y => y == x.DefNum)).Select(x => x.ItemName)));
				row.Tag = op;
				gridWebSchedNewPatApptOps.ListGridRows.Add(row);
			}
			gridWebSchedNewPatApptOps.EndUpdate();
		}

		private void FillGridWebSchedNewPatApptTimeSlotsThreaded()
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke((Action)delegate ()
				{
					FillGridWebSchedNewPatApptTimeSlotsThreaded();
				});
				return;
			}
			if (comboWSNPADefApptType.GetSelected<Def>() == null)
			{
				if (tabControlWebSched.SelectedTab == tabWebSchedNewPatAppts)
				{
					MessageBox.Show("Set a Web Sched New Pat Appt Type in Definitions to show appointment time slots.");
				}
				return;
			}
			//Clear the current grid rows before starting the thread below. This allows that thread to exit at any time without leaving old rows in the grid.
			gridWebSchedNewPatApptTimeSlots.BeginUpdate();
			gridWebSchedNewPatApptTimeSlots.ListGridRows.Clear();
			gridWebSchedNewPatApptTimeSlots.EndUpdate();
			//Validate time slot settings.
			if (textWebSchedNewPatApptsDateStart.errorProvider1.GetError(textWebSchedNewPatApptsDateStart) != "")
			{
				//Don't bother warning the user.  It will just be annoying.  The red indicator should be sufficient.
				return;
			}
			if (!PrefC.HasClinicsEnabled)
			{
				comboWSNPClinics.SelectedIndex = 0;//Not visible but this will set the combo box the "N/A" which is the non-clinic signup
			}
			if (comboWSNPClinics.SelectedIndex < 0)
			{
				return;//Nothing to do.
			}
			WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService signup = ((ODBoxItem<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>)comboWSNPClinics.SelectedItem).Tag;
			//Protect against re-entry
			if (_threadFillGridWebSchedNewPatApptTimeSlots != null)
			{
				//A thread is already refreshing the time slots grid so we simply need to queue up another refresh once the one thread has finished.
				_isWebSchedNewPatApptTimeSlotsOutdated = true;
				return;
			}
			_isWebSchedNewPatApptTimeSlotsOutdated = false;
			_indexLastNewPatURL = comboWSNPClinics.SelectedIndex;
			DateTime dateStart = PIn.Date(textWebSchedNewPatApptsDateStart.Text);
			DateTime dateEnd = dateStart.AddDays(30);
			if (!signup.IsEnabled)
			{
				return;//Do nothing, this clinic is excluded from New Pat Appts.
			}
			//Only get time slots for headquarters or clinics that are NOT excluded (aka included).
			var args = new
			{
				ClinicNum = signup.ClinicNum,
				DateStart = dateStart,
				DateEnd = dateStart.AddDays(30),
				DefApptType = comboWSNPADefApptType.GetSelected<Def>(),
			};
			_threadFillGridWebSchedNewPatApptTimeSlots = new ODThread(new ODThread.WorkerDelegate((th) =>
			{
				//The user might not have Web Sched ops set up correctly.  Don't warn them here because it is just annoying.  They'll figure it out.
				ODException.SwallowAnyException(() =>
				{
					//Get the next 30 days of open time schedules with the current settings
					List<TimeSlot> listTimeSlots = TimeSlots.GetAvailableNewPatApptTimeSlots(args.DateStart, args.DateEnd, args.ClinicNum
						, args.DefApptType.DefNum);
					FillGridWebSchedNewPatApptTimeSlots(listTimeSlots);
				});
			}))
			{ Name = "ThreadWebSchedNewPatApptTimeSlots" };
			_threadFillGridWebSchedNewPatApptTimeSlots.AddExitHandler(new ODThread.WorkerDelegate((th) =>
			{
				_threadFillGridWebSchedNewPatApptTimeSlots = null;
				//If something else wanted to refresh the grid while we were busy filling it then we need to refresh again.  A filter could have changed.
				if (_isWebSchedNewPatApptTimeSlotsOutdated)
				{
					FillGridWebSchedNewPatApptTimeSlotsThreaded();
				}
			}));
			_threadFillGridWebSchedNewPatApptTimeSlots.Start(true);
		}

		private void FillGridWebSchedNewPatApptTimeSlots(List<TimeSlot> listTimeSlots)
		{
			if (this.InvokeRequired)
			{
				this.Invoke((Action)delegate () { FillGridWebSchedNewPatApptTimeSlots(listTimeSlots); });
				return;
			}
			gridWebSchedNewPatApptTimeSlots.BeginUpdate();
			gridWebSchedNewPatApptTimeSlots.ListGridColumns.Clear();
			GridColumn col = new GridColumn("", 20) { IsWidthDynamic = true };
			col.TextAlign = HorizontalAlignment.Center;
			gridWebSchedNewPatApptTimeSlots.ListGridColumns.Add(col);
			gridWebSchedNewPatApptTimeSlots.ListGridRows.Clear();
			GridRow row;
			DateTime dateTimeSlotLast = DateTime.MinValue;
			foreach (TimeSlot timeSlot in listTimeSlots)
			{
				//Make a new row for every unique day.
				if (dateTimeSlotLast.Date != timeSlot.DateTimeStart.Date)
				{
					dateTimeSlotLast = timeSlot.DateTimeStart;
					row = new GridRow();
					row.ColorBackG = Color.LightBlue;
					row.Cells.Add(timeSlot.DateTimeStart.ToShortDateString());
					gridWebSchedNewPatApptTimeSlots.ListGridRows.Add(row);
				}
				row = new GridRow();
				row.Cells.Add(timeSlot.DateTimeStart.ToShortTimeString() + " - " + timeSlot.DateTimeStop.ToShortTimeString());
				gridWebSchedNewPatApptTimeSlots.ListGridRows.Add(row);
			}
			gridWebSchedNewPatApptTimeSlots.EndUpdate();
		}

		private void comboWebSchedNewPatApptsApptType_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillGridWebSchedNewPatApptTimeSlotsThreaded();
		}

		private void gridWebSchedNewPatApptOps_DoubleClick(object sender, EventArgs e)
		{
			ShowOperatoryEditAndRefreshGrids();
		}

		private void gridWSNPAReasons_DoubleClick(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormDefinitions FormD = new FormDefinitions(DefCat.WebSchedNewPatApptTypes);
			FormD.ShowDialog();
			FillGridWSNPAReasons();
			FillGridWebSchedNewPatApptTimeSlotsThreaded();
		}

		private void butWebSchedNewPatApptsToday_Click(object sender, EventArgs e)
		{
			textWebSchedNewPatApptsDateStart.Text = DateTime.Today.ToShortDateString();
		}

		private void textWebSchedNewPatApptSearchDays_Validated(object sender, EventArgs e)
		{
			if (textWebSchedNewPatApptSearchDays.errorProvider1.GetError(textWebSchedNewPatApptSearchDays) != "")
			{
				return;
			}
			int newPatApptDays = PIn.Int(textWebSchedNewPatApptSearchDays.Text);
			if (Prefs.Set(PrefName.WebSchedNewPatApptSearchAfterDays, newPatApptDays > 0 ? newPatApptDays : 0))
			{
				_indexLastNewPatURL = -1;//Force refresh of the grid in because this setting changed.
			}
		}

		private void textWebSchedNewPatApptsDateStart_TextChanged(object sender, EventArgs e)
		{
			//Only refresh the grid if the user has typed in a valid date.
			if (textWebSchedNewPatApptsDateStart.errorProvider1.GetError(textWebSchedNewPatApptsDateStart) == "")
			{
				FillGridWebSchedNewPatApptTimeSlotsThreaded();
			}
		}

		private void checkWebSchedNewPatForcePhoneFormatting_Click(object sender, EventArgs e)
		{
			Prefs.Set(PrefName.WebSchedNewPatApptForcePhoneFormatting, checkWebSchedNewPatForcePhoneFormatting.Checked);
		}

		private void butWebSchedNewPatBlockouts_Click(object sender, EventArgs e)
		{
			List<long> listBlockoutTypes = PrefC.GetWebSchedNewPatAllowedBlockouts;
			List<Def> listBlockoutTypeDefs = Defs.GetDefs(DefCat.BlockoutTypes, listBlockoutTypes);
			FormDefinitionPicker FormDP = new FormDefinitionPicker(DefCat.BlockoutTypes, listBlockoutTypeDefs);
			FormDP.HasShowHiddenOption = true;
			FormDP.IsMultiSelectionMode = true;
			FormDP.ShowDialog();
			if (FormDP.DialogResult == DialogResult.OK)
			{
				string strListWebSchedNewPatIgnoreBlockoutTypes = String.Join(",", FormDP.ListSelectedDefs.Select(x => x.DefNum));
				Prefs.Set(PrefName.WebSchedNewPatApptIgnoreBlockoutTypes, strListWebSchedNewPatIgnoreBlockoutTypes);
				FillWSNPABlockoutTypes();
			}
		}

		private void butWSNPRestrictedToReasonsEdit_Click(object sender, EventArgs e)
		{
			FormDefinitions formDefs = new FormDefinitions(DefCat.WebSchedNewPatApptTypes);
			formDefs.ShowDialog();
			FillWSNPABlockoutTypes();
		}
		#endregion Tab - New Patient

		#region Tab - Recall
		private void SaveTabWebSchedRecall()
		{
			WebSchedAutomaticSend sendType = WebSchedAutomaticSend.SendToEmailOnlyPreferred;
			if (radioDoNotSend.Checked)
			{
				sendType = WebSchedAutomaticSend.DoNotSend;
			}
			else if (radioSendToEmail.Checked)
			{
				sendType = WebSchedAutomaticSend.SendToEmail;
			}
			else if (radioSendToEmailNoPreferred.Checked)
			{
				sendType = WebSchedAutomaticSend.SendToEmailNoPreferred;
			}
			WebSchedAutomaticSend beforeEnum = (WebSchedAutomaticSend)PrefC.GetInt(PrefName.WebSchedAutomaticSendSetting);
			if (Prefs.Set(PrefName.WebSchedAutomaticSendSetting, (int)sendType))
			{
				SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "WebSched automated email preference changed from " + beforeEnum.ToString() + " to " + sendType.ToString() + ".");
			}
			WebSchedAutomaticSend sendTypeText = WebSchedAutomaticSend.SendToText;
			if (radioDoNotSendText.Checked)
			{
				sendTypeText = WebSchedAutomaticSend.DoNotSend;
			}
			beforeEnum = (WebSchedAutomaticSend)PrefC.GetInt(PrefName.WebSchedAutomaticSendTextSetting);
			if (Prefs.Set(PrefName.WebSchedAutomaticSendTextSetting, (int)sendTypeText))
			{
				SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "WebSched automated text preference changed from " + beforeEnum.ToString() + " to " + sendTypeText.ToString() + ".");
			}
			int beforeInt = PrefC.GetInt(PrefName.WebSchedTextsPerBatch);
			int afterInt = PIn.Int(textWebSchedPerBatch.Text);
			if (Prefs.Set(PrefName.WebSchedTextsPerBatch, afterInt))
			{
				SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "WebSched batch size preference changed from " + beforeInt.ToString() + " to " + afterInt.ToString() + ".");
			}
			Prefs.Set(PrefName.WebSchedRecallAllowProvSelection, checkRecallAllowProvSelection.Checked);
			if (comboWSRConfirmStatus.SelectedIndex != -1)
			{
				Prefs.Set(PrefName.WebSchedRecallConfirmStatus, comboWSRConfirmStatus.GetSelectedDefNum());
			}
			Prefs.Set(PrefName.WebSchedRecallDoubleBooking, checkWSRDoubleBooking.Checked ? 1 : 0);
		}

		private void FillTabWebSchedRecall()
		{
			int recallApptDays = PrefC.GetInt(PrefName.WebSchedRecallApptSearchAfterDays);
			textWebSchedRecallApptSearchDays.Text = recallApptDays > 0 ? recallApptDays.ToString() : "";
			switch (PrefC.GetInt(PrefName.WebSchedAutomaticSendSetting))
			{
				case (int)WebSchedAutomaticSend.DoNotSend:
					radioDoNotSend.Checked = true;
					break;
				case (int)WebSchedAutomaticSend.SendToEmail:
					radioSendToEmail.Checked = true;
					break;
				case (int)WebSchedAutomaticSend.SendToEmailNoPreferred:
					radioSendToEmailNoPreferred.Checked = true;
					break;
				case (int)WebSchedAutomaticSend.SendToEmailOnlyPreferred:
					radioSendToEmailOnlyPreferred.Checked = true;
					break;
			}
			switch (PrefC.GetInt(PrefName.WebSchedAutomaticSendTextSetting))
			{
				case (int)WebSchedAutomaticSend.DoNotSend:
					radioDoNotSendText.Checked = true;
					break;
				case (int)WebSchedAutomaticSend.SendToText:
					radioSendText.Checked = true;
					break;
			}
			textWebSchedPerBatch.Text = Prefs.GetString(PrefName.WebSchedTextsPerBatch);
			textWebSchedDateStart.Text = DateTime.Today.AddDays(recallApptDays).ToShortDateString();
			comboWebSchedClinic.Items.Clear();
			comboWebSchedClinic.Items.Add("Unassigned");
			_listWebSchedClinics = Clinics.GetDeepCopy();
			for (int i = 0; i < _listWebSchedClinics.Count; i++)
			{
				comboWebSchedClinic.Items.Add(_listWebSchedClinics[i].Abbr);
			}
			comboWebSchedClinic.SelectedIndex = 0;
			_listWebSchedProviders = Providers.GetDeepCopy(true);
			comboWebSchedProviders.Items.Clear();
			comboWebSchedProviders.Items.Add("All");
			for (int i = 0; i < _listWebSchedProviders.Count; i++)
			{
				comboWebSchedProviders.Items.Add(_listWebSchedProviders[i].GetLongDesc());
			}
			comboWebSchedProviders.SelectedIndex = 0;
			if (!PrefC.HasClinicsEnabled)
			{
				labelWebSchedClinic.Visible = false;
				comboWebSchedClinic.Visible = false;
				butWebSchedPickClinic.Visible = false;
			}
			FillAllowedBlockoutTypes();
			FillGridWebSchedRecallTypes();
			FillGridWebSchedOperatories();
			FillGridWebSchedTimeSlotsThreaded();
			FillWebSchedProviderRule();
			checkRecallAllowProvSelection.Checked = Prefs.GetBool(PrefName.WebSchedRecallAllowProvSelection);
			long defaultStatus = Prefs.GetLong(PrefName.WebSchedRecallConfirmStatus);
			comboWSRConfirmStatus.Items.AddDefs(Defs.GetDefsForCategory(DefCat.ApptConfirmed, true));
			comboWSRConfirmStatus.SetSelectedDefNum(defaultStatus);
			checkWSRDoubleBooking.Checked = PrefC.GetInt(PrefName.WebSchedRecallDoubleBooking) > 0;//0 = Allow double booking, 1 = prevent
		}

		private void AuthorizeWebSchedRecall(bool allowEdit)
		{
			textWebSchedRecallApptSearchDays.Enabled = allowEdit;
			butRecallSchedSetup.Enabled = allowEdit;
			butWebSchedRecallBlockouts.Enabled = allowEdit;
			groupWebSchedProvRule.Enabled = allowEdit;
			groupBoxWebSchedAutomation.Enabled = allowEdit;
			groupWebSchedPreview.Enabled = allowEdit;
			groupWebSchedText.Enabled = allowEdit;
		}

		private void FillAllowedBlockoutTypes()
		{
			List<Def> listBlockoutTypeDefs = Defs.GetDefs(DefCat.BlockoutTypes, PrefC.GetWebSchedRecallAllowedBlockouts);
			listboxWebSchedRecallIgnoreBlockoutTypes.Items.Clear();
			foreach (Def defCur in listBlockoutTypeDefs)
			{
				listboxWebSchedRecallIgnoreBlockoutTypes.Items.Add(defCur.ItemName);
			}
		}

		private void linkLabelAboutWebSched_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Use the Web Sched button in the Recall List window to send "
				+ "emails with a link that will allow patients to quickly schedule their recall appointments. "
				+ "You may also publish a link to the New Patient Appt URL on your web site to allow new patients to schedule "
				+ "their first appointment.  All appointments scheduled using Web Sched will instantly show "
				+ "up on the schedule.", "Web Sched Information", MessageBoxButtons.OK);
		}

		private void butWebSchedRecallBlockouts_Click(object sender, EventArgs e)
		{
			string[] arrayDefNums = Prefs.GetString(PrefName.WebSchedRecallIgnoreBlockoutTypes).Split(new char[] { ',' }); //comma-delimited list.
			List<long> listBlockoutTypes = new List<long>();
			foreach (string strDefNum in arrayDefNums)
			{
				listBlockoutTypes.Add(PIn.Long(strDefNum));
			}
			List<Def> listBlockoutTypeDefs = Defs.GetDefs(DefCat.BlockoutTypes, listBlockoutTypes);
			FormDefinitionPicker FormDP = new FormDefinitionPicker(DefCat.BlockoutTypes, listBlockoutTypeDefs);
			FormDP.HasShowHiddenOption = true;
			FormDP.IsMultiSelectionMode = true;
			FormDP.ShowDialog();
			if (FormDP.DialogResult == DialogResult.OK)
			{
				listboxWebSchedRecallIgnoreBlockoutTypes.Items.Clear();
				foreach (Def defCur in FormDP.ListSelectedDefs)
				{
					listboxWebSchedRecallIgnoreBlockoutTypes.Items.Add(defCur.ItemName);
				}
				string strListWebSChedRecallIgnoreBlockoutTypes = String.Join(",", FormDP.ListSelectedDefs.Select(x => x.DefNum));
				Prefs.Set(PrefName.WebSchedRecallIgnoreBlockoutTypes, strListWebSChedRecallIgnoreBlockoutTypes);
			}
		}

		///<summary>Shows the Operatories window and allows the user to edit them.  Does not show the window if user does not have Setup permission.
		///Refreshes all corresponding grids within the Web Sched tab that display Operatory information.  Feel free to add to this method.</summary>
		private void ShowOperatoryEditAndRefreshGrids()
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormOperatories FormO = new FormOperatories();
			FormO.ShowDialog();
			if (FormO.ListConflictingAppts.Count > 0)
			{
				FormApptConflicts FormAC = new FormApptConflicts(FormO.ListConflictingAppts);
				FormAC.Show();
				FormAC.BringToFront();
			}
			FillGridWebSchedOperatories();
			FillGridWebSchedTimeSlotsThreaded();
			FillGridWebSchedNewPatApptOps();
			FillGridWebSchedNewPatApptTimeSlotsThreaded();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Operatories accessed via EServices Setup window.");
		}

		#region Web Sched - Recalls
		private void textWebSchedRecallApptSearchDays_Leave(object sender, EventArgs e)
		{
			//Only refresh if the value of this preference changed.
			if (_doRefillTimeSlots)
			{
				FillGridWebSchedTimeSlotsThreaded();
				_doRefillTimeSlots = false;
			}
		}

		private void textWebSchedRecallApptSearchDays_Validated(object sender, EventArgs e)
		{
			if (!textWebSchedRecallApptSearchDays.IsValid)
			{
				return;
			}
			int recallApptDays = PIn.Int(textWebSchedRecallApptSearchDays.Text);
			if (Prefs.Set(PrefName.WebSchedRecallApptSearchAfterDays, recallApptDays > 0 ? recallApptDays : 0))
			{
				_doRefillTimeSlots = true;//Force refresh of the grid in because this setting changed.
			}
		}

		///<summary>Also refreshed the combo box of available recall types.</summary>
		private void FillGridWebSchedRecallTypes()
		{
			//Keep track of the previously selected recall type.
			long selectedRecallTypeNum = 0;
			if (comboWebSchedRecallTypes.SelectedIndex != -1)
			{
				selectedRecallTypeNum = _listRecallTypes[comboWebSchedRecallTypes.SelectedIndex].RecallTypeNum;
			}
			//Fill the combo boxes for the time slots preview.
			comboWebSchedRecallTypes.Items.Clear();
			_listRecallTypes = RecallTypes.GetDeepCopy();
			for (int i = 0; i < _listRecallTypes.Count; i++)
			{
				comboWebSchedRecallTypes.Items.Add(_listRecallTypes[i].Description);
				if (_listRecallTypes[i].RecallTypeNum == selectedRecallTypeNum)
				{
					comboWebSchedRecallTypes.SelectedIndex = i;
				}
			}
			if (selectedRecallTypeNum == 0 && comboWebSchedRecallTypes.Items.Count > 0)
			{
				comboWebSchedRecallTypes.SelectedIndex = 0;//Arbitrarily select the first recall type.
			}
			gridWebSchedRecallTypes.BeginUpdate();
			gridWebSchedRecallTypes.ListGridColumns.Clear();
			GridColumn col = new GridColumn("Description", 130);
			gridWebSchedRecallTypes.ListGridColumns.Add(col);
			col = new GridColumn("Time Pattern", 100);
			gridWebSchedRecallTypes.ListGridColumns.Add(col);
			col = new GridColumn("Time Length", 80) { IsWidthDynamic = true };
			col.TextAlign = HorizontalAlignment.Center;
			gridWebSchedRecallTypes.ListGridColumns.Add(col);
			gridWebSchedRecallTypes.ListGridRows.Clear();
			GridRow row;
			for (int i = 0; i < _listRecallTypes.Count; i++)
			{
				row = new GridRow();
				row.Cells.Add(_listRecallTypes[i].Description);
				row.Cells.Add(_listRecallTypes[i].TimePattern);
				int timeLength = RecallTypes.ConvertTimePattern(_listRecallTypes[i].TimePattern).Length * 5;
				if (timeLength == 0)
				{
					row.Cells.Add("");
				}
				else
				{
					row.Cells.Add(timeLength.ToString() + " " + "mins");
				}
				gridWebSchedRecallTypes.ListGridRows.Add(row);
			}
			gridWebSchedRecallTypes.EndUpdate();
		}

		private void FillGridWebSchedOperatories()
		{
			_listWebSchedRecallOps = Operatories.GetOpsForWebSched();
			int opNameWidth = 170;
			int clinicWidth = 80;
			if (!PrefC.HasClinicsEnabled)
			{
				opNameWidth += clinicWidth;
			}
			gridWebSchedOperatories.BeginUpdate();
			gridWebSchedOperatories.ListGridColumns.Clear();
			gridWebSchedOperatories.ListGridColumns.Add(new GridColumn("Op Name", opNameWidth));
			gridWebSchedOperatories.ListGridColumns.Add(new GridColumn("Abbrev", 70));
			if (PrefC.HasClinicsEnabled)
			{
				gridWebSchedOperatories.ListGridColumns.Add(new GridColumn("Clinic", clinicWidth));
			}
			gridWebSchedOperatories.ListGridColumns.Add(new GridColumn("Provider", 90));
			gridWebSchedOperatories.ListGridColumns.Add(new GridColumn("Hygienist", 90));
			gridWebSchedOperatories.ListGridRows.Clear();
			GridRow row;
			for (int i = 0; i < _listWebSchedRecallOps.Count; i++)
			{
				row = new GridRow();
				row.Cells.Add(_listWebSchedRecallOps[i].OpName);
				row.Cells.Add(_listWebSchedRecallOps[i].Abbrev);
				if (PrefC.HasClinicsEnabled)
				{
					row.Cells.Add(Clinics.GetAbbr(_listWebSchedRecallOps[i].ClinicNum));
				}
				row.Cells.Add(Providers.GetAbbr(_listWebSchedRecallOps[i].ProvDentist));
				row.Cells.Add(Providers.GetAbbr(_listWebSchedRecallOps[i].ProvHygienist));
				gridWebSchedOperatories.ListGridRows.Add(row);
			}
			gridWebSchedOperatories.EndUpdate();
		}

		private void FillGridWebSchedTimeSlotsThreaded()
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke((Action)delegate ()
				{
					FillGridWebSchedTimeSlotsThreaded();
				});
				return;
			}
			//Validate time slot settings.
			if (textWebSchedDateStart.errorProvider1.GetError(textWebSchedDateStart) != "")
			{
				//Don't bother warning the user.  It will just be annoying.  The red indicator should be sufficient.
				return;
			}
			if (comboWebSchedRecallTypes.SelectedIndex < 0
				|| comboWebSchedClinic.SelectedIndex < 0
				|| comboWebSchedProviders.SelectedIndex < 0)
			{
				return;
			}
			//Protect against re-entry
			if (_threadFillGridWebSchedTimeSlots != null)
			{
				//A thread is already refreshing the time slots grid so we simply need to queue up another refresh once the one thread has finished.
				_isWebSchedTimeSlotsOutdated = true;
				return;
			}
			_isWebSchedTimeSlotsOutdated = false;
			DateTime dateStart = PIn.Date(textWebSchedDateStart.Text);
			RecallType recallType = _listRecallTypes[comboWebSchedRecallTypes.SelectedIndex];
			Clinic clinic = _listWebSchedClinics.Find(x => x.ClinicNum == _webSchedClinicNum);//null clinic is treated as unassigned.
			List<Provider> listProviders = new List<Provider>(_listWebSchedProviders);//Use all providers by default.
			Provider provider = _listWebSchedProviders.Find(x => x.ProvNum == _webSchedProvNum);
			if (provider != null)
			{
				//Only use the provider that the user picked from the provider picker.
				listProviders = new List<Provider>() { provider };
			}
			WebSchedTimeSlotArgs webSchedTimeSlotArgs = new WebSchedTimeSlotArgs()
			{
				RecallTypeCur = recallType,
				ClinicCur = clinic,
				DateStart = dateStart,
				DateEnd = dateStart.AddDays(30),
				ListProviders = listProviders
			};
			_threadFillGridWebSchedTimeSlots = new ODThread(GetWebSchedTimeSlotsWorker, webSchedTimeSlotArgs);
			_threadFillGridWebSchedTimeSlots.Name = "ThreadWebSchedRecallTimeSlots";
			_threadFillGridWebSchedTimeSlots.AddExitHandler(GetWebSchedTimeSlotsThreadExitHandler);
			_threadFillGridWebSchedTimeSlots.AddExceptionHandler(GetWebSchedTimeSlotsExceptionHandler);
			_threadFillGridWebSchedTimeSlots.Start(true);
		}

		private void GetWebSchedTimeSlotsWorker(ODThread o)
		{
			WebSchedTimeSlotArgs w = (WebSchedTimeSlotArgs)o.Parameters[0];
			List<TimeSlot> listTimeSlots = new List<TimeSlot>();
			try
			{
				//Get the next 30 days of open time schedules with the current settings
				listTimeSlots = TimeSlots.GetAvailableWebSchedTimeSlots(w.RecallTypeCur, w.ListProviders, w.ClinicCur, w.DateStart, w.DateEnd);
			}
			catch (Exception)
			{
				//The user might not have Web Sched ops set up correctly.  Don't warn them here because it is just annoying.  They'll figure it out.
			}
			o.Tag = listTimeSlots;
		}

		private void GetWebSchedTimeSlotsThreadExitHandler(ODThread o)
		{
			ODException.SwallowAnyException(() =>
			{
				FillGridWebSchedTimeSlots((List<TimeSlot>)o.Tag);
			});
			_threadFillGridWebSchedTimeSlots = null;
			//If something else wanted to refresh the grid while we were busy filling it then we need to refresh again.  A filter could have changed.
			if (_isWebSchedTimeSlotsOutdated)
			{
				FillGridWebSchedTimeSlotsThreaded();
			}
		}

		private void GetWebSchedTimeSlotsExceptionHandler(Exception e)
		{
			_threadFillGridWebSchedTimeSlots = null;
		}

		private void FillGridWebSchedTimeSlots(List<TimeSlot> listTimeSlots)
		{
			if (this.InvokeRequired)
			{
				this.Invoke((Action)delegate () { FillGridWebSchedTimeSlots(listTimeSlots); });
				return;
			}
			gridWebSchedTimeSlots.BeginUpdate();
			gridWebSchedTimeSlots.ListGridColumns.Clear();
			GridColumn col = new GridColumn("", 20) { IsWidthDynamic = true };
			col.TextAlign = HorizontalAlignment.Center;
			gridWebSchedTimeSlots.ListGridColumns.Add(col);
			gridWebSchedTimeSlots.ListGridRows.Clear();
			GridRow row;
			DateTime dateTimeSlotLast = DateTime.MinValue;
			foreach (TimeSlot timeSlot in listTimeSlots)
			{
				//Make a new row for every unique day.
				if (dateTimeSlotLast.Date != timeSlot.DateTimeStart.Date)
				{
					dateTimeSlotLast = timeSlot.DateTimeStart;
					row = new GridRow();
					row.ColorBackG = Color.LightBlue;
					row.Cells.Add(timeSlot.DateTimeStart.ToShortDateString());
					gridWebSchedTimeSlots.ListGridRows.Add(row);
				}
				row = new GridRow();
				row.Cells.Add(timeSlot.DateTimeStart.ToShortTimeString() + " - " + timeSlot.DateTimeStop.ToShortTimeString());
				gridWebSchedTimeSlots.ListGridRows.Add(row);
			}
			gridWebSchedTimeSlots.EndUpdate();
		}

		private void gridWebSchedRecallTypes_DoubleClick(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormRecallTypes FormRT = new FormRecallTypes();
			FormRT.ShowDialog();
			FillGridWebSchedRecallTypes();
			FillGridWebSchedTimeSlotsThreaded();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Recall Types accessed via EServices Setup window.");
		}

		private void gridWebSchedOperatories_DoubleClick(object sender, EventArgs e)
		{
			ShowOperatoryEditAndRefreshGrids();
		}

		private void butProvRulePickClinic_Click(object sender, EventArgs e)
		{
			FormClinics FormC = new FormClinics();
			FormC.IsSelectionMode = true;
			FormC.ShowDialog();
			if (FormC.DialogResult != DialogResult.OK)
			{
				return;
			}
			comboClinicProvRule.SelectedClinicNum = FormC.SelectedClinicNum;
			FillWebSchedProviderRule();
		}

		private void comboClinicProvRule_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillWebSchedProviderRule();
		}

		private void FillWebSchedProviderRule()
		{
			//ClinicPref prefProviderRule = ClinicPrefs.GetPref(PrefName.WebSchedProviderRule, comboClinicProvRule.SelectedClinicNum, isDefaultIncluded: true);
			//checkUseDefaultProvRule.Visible = (!comboClinicProvRule.IsUnassignedSelected);//"Use Defaults" checkbox visible when actual clinic is selected.
			//checkUseDefaultProvRule.Checked = (prefProviderRule == null);
			//SetListBoxWebSchedProviderPref(prefProviderRule);//Select ClincPref's value, or default if no ClinicPref.
		}

		private void checkUseDefaultProvRule_Click(object sender, EventArgs e)
		{
			//listBoxWebSchedProviderPref.Enabled = (!checkUseDefaultProvRule.Checked);
			//ClinicPref prefProviderRule = ClinicPrefs.GetPref(PrefName.WebSchedProviderRule, comboClinicProvRule.SelectedClinicNum, isDefaultIncluded: true);
			//if (checkUseDefaultProvRule.Checked)
			//{
			//	if (ClinicPrefs.DeletePrefs(comboClinicProvRule.SelectedClinicNum, new List<PrefName>() { PrefName.WebSchedProviderRule }) > 0)
			//	{
			//		DataValid.SetInvalid(InvalidType.ClinicPrefs);//Checking "Use Defaults", delete ClinicPref is exists.
			//	}
			//}
			//else if (!comboClinicProvRule.IsUnassignedSelected)
			//{
			//	//Unchecking "Use Defaults" and on a clinic that doesn't have a ClinicPref yet, create new ClinicPref with default value
			//	if (prefProviderRule != null)
			//	{
			//		ClinicPrefs.DeletePrefs(comboClinicProvRule.SelectedClinicNum, new List<PrefName>() { PrefName.WebSchedProviderRule });
			//	}
			//	ClinicPrefs.InsertPref(PrefName.WebSchedProviderRule, comboClinicProvRule.SelectedClinicNum
			//		, POut.Int(PrefC.GetInt(PrefName.WebSchedProviderRule)));
			//	DataValid.SetInvalid(InvalidType.ClinicPrefs);
			//}
			//prefProviderRule = ClinicPrefs.GetPref(PrefName.WebSchedProviderRule, comboClinicProvRule.SelectedClinicNum, isDefaultIncluded: true);
			//SetListBoxWebSchedProviderPref(prefProviderRule);
		}

		private void SetListBoxWebSchedProviderPref((string, string) pref)
		{
			//if (pref == null)
			//{//Using defaults.
			//	listBoxWebSchedProviderPref.SelectedIndex = PrefC.GetInt(PrefName.WebSchedProviderRule);
			//}
			//else
			//{
			//	listBoxWebSchedProviderPref.SelectedIndex = PIn.Int(pref.ValueString);
			//}
		}

		private void listBoxWebSchedProviderPref_SelectedIndexChanged(object sender, EventArgs e)
		{
			//if (comboClinicProvRule.IsUnassignedSelected && listBoxWebSchedProviderPref.SelectedIndex != PrefC.GetInt(PrefName.WebSchedProviderRule))
			//{
			//	Prefs.Set(PrefName.WebSchedProviderRule, listBoxWebSchedProviderPref.SelectedIndex);//Update default preference.
			//	DataValid.SetInvalid(InvalidType.Prefs);
			//}
			//else if (!comboClinicProvRule.IsUnassignedSelected && !checkUseDefaultProvRule.Checked
			//	&& ClinicPrefs.Upsert(PrefName.WebSchedProviderRule, comboClinicProvRule.SelectedClinicNum, POut.Int(listBoxWebSchedProviderPref.SelectedIndex)))
			//{//Clinic not set to use defaults.
			//	DataValid.SetInvalid(InvalidType.ClinicPrefs);
			//}
		}

		private void comboWebSchedRecallTypes_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillGridWebSchedTimeSlotsThreaded();
		}

		private void comboWebSchedProviders_SelectionChangeCommitted(object sender, EventArgs e)
		{
			_webSchedProvNum = 0;
			if (comboWebSchedProviders.SelectedIndex > 0)
			{//Greater than 0 due to "All"
				_webSchedProvNum = _listWebSchedProviders[comboWebSchedProviders.SelectedIndex - 1].ProvNum;//-1 for 'All'
			}
			FillGridWebSchedTimeSlotsThreaded();
		}

		private void comboWebSchedClinic_SelectionChangeCommitted(object sender, EventArgs e)
		{
			_webSchedClinicNum = 0;
			if (comboWebSchedClinic.SelectedIndex > 0)
			{//Greater than 0 due to "Unassigned"
				_webSchedClinicNum = _listWebSchedClinics[comboWebSchedClinic.SelectedIndex - 1].ClinicNum;//-1 for 'Unassigned'
			}
			FillGridWebSchedTimeSlotsThreaded();
		}

		private void textWebSchedDateStart_TextChanged(object sender, EventArgs e)
		{
			//Only refresh the grid if the user has typed in a valid date.
			if (textWebSchedDateStart.errorProvider1.GetError(textWebSchedDateStart) == "")
			{
				FillGridWebSchedTimeSlotsThreaded();
			}
		}

		private void WebSchedRecallAutoSendRadioButtons_CheckedChanged(object sender, EventArgs e)
		{
			if (radioDoNotSend.Checked && radioDoNotSendText.Checked)
			{
				return;
			}
			//Validate the following recall setup preferences.  See task #880961 or #879613 for more details.
			//1. The Days Past field is not blank
			//2. The Initial Reminder field is greater than 0
			//3. The Second(or more) Reminder field is greater than 0
			//4. Integrated texting is enabled if Send Text is checked
			List<string> listSetupErrors = new List<string>();
			bool isEmailSendInvalid = false;
			bool isTextSendInvalid = false;
			if (Prefs.GetLong(PrefName.RecallDaysPast) == -1)
			{//Days Past field
				listSetupErrors.Add("- " + "Days Past (e.g. 1095, blank, etc) field cannot be blank.");
				isEmailSendInvalid = true;
				isTextSendInvalid = true;
			}
			if (Prefs.GetLong(PrefName.RecallShowIfDaysFirstReminder) < 1)
			{//Initial Reminder field
				listSetupErrors.Add("- " + "Initial Reminder field has to be greater than 0.");
				isEmailSendInvalid = true;
				isTextSendInvalid = true;
			}
			if (Prefs.GetLong(PrefName.RecallShowIfDaysSecondReminder) < 1)
			{//Second(or more) Reminder field
				listSetupErrors.Add("- " + "Second (or more) Reminder field has to be greater than 0.");
				isEmailSendInvalid = true;
				isTextSendInvalid = true;
			}
			if (radioSendText.Checked && !SmsPhones.IsIntegratedTextingEnabled())
			{
				listSetupErrors.Add("- " + "Integrated texting must be enabled.");
				isTextSendInvalid = true;
			}
			//Checking the "Do Not Send" radio button will automatically uncheck all the other radio buttons in the group box.
			if (isEmailSendInvalid)
			{
				radioDoNotSend.Checked = true;
			}
			if (isTextSendInvalid)
			{
				radioDoNotSendText.Checked = true;
			}
			if (listSetupErrors.Count > 0)
			{
				MessageBox.Show("Recall Setup settings are not correctly set in order to Send Messages Automatically to patients:"
						+ "\r\n" + string.Join("\r\n", listSetupErrors)
					, "Web Sched - Recall Setup Error");
			}
		}

		private void butWebSchedSetup_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}
			FormRecallSetup FormRS = new FormRecallSetup();
			FormRS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup, 0, "Recall Setup accessed via EServices Setup window.");
		}

		private void butWebSchedToday_Click(object sender, EventArgs e)
		{
			textWebSchedDateStart.Text = DateTime.Today.ToShortDateString();
			//Don't need to call FillTimeSlots because textChanged event already calls it.
		}

		private void butWebSchedPickProv_Click(object sender, EventArgs e)
		{
			FormProviderPick FormPP = new FormProviderPick();
			if (comboWebSchedProviders.SelectedIndex > 0)
			{
				FormPP.SelectedProvNum = _webSchedProvNum;
			}
			FormPP.ShowDialog();
			if (FormPP.DialogResult != DialogResult.OK)
			{
				return;
			}
			comboWebSchedProviders.SelectedIndex = _listWebSchedProviders.FindIndex(x => x.ProvNum == FormPP.SelectedProvNum) + 1;//+1 for 'All'
			_webSchedProvNum = FormPP.SelectedProvNum;
			FillGridWebSchedTimeSlotsThreaded();
		}

		private void butWebSchedPickClinic_Click(object sender, EventArgs e)
		{
			FormClinics FormC = new FormClinics();
			FormC.IsSelectionMode = true;
			FormC.ShowDialog();
			if (FormC.DialogResult != DialogResult.OK)
			{
				return;
			}
			comboWebSchedClinic.SelectedIndex = _listWebSchedClinics.FindIndex(x => x.ClinicNum == FormC.SelectedClinicNum) + 1;//+1 for 'Unassigned'
			_webSchedClinicNum = FormC.SelectedClinicNum;
			FillGridWebSchedTimeSlotsThreaded();
		}
		#endregion Web Sched - Recalls
		#endregion Tab - Recall

		#region Tab - Verify

		#region Load-in
		///<summary>Loading routine for the WebSchedVerify tab.</summary>
		private void FillTabWebSchedVerify()
		{
			////Load in an existing list of clinicprefs so we can keep in in-memory record of changes
			//ClinicPrefs.RefreshCache();
			//foreach (string prefName in _listWebSchedVerifyPrefNames)
			//{
			//	_listWebSchedVerifyClinicPrefs.AddRange(ClinicPrefs.GetPrefAllClinics(prefName));
			//	Pref pref = Prefs.GetString(prefName.ToString());
			//	_listWebSchedVerifyClinicPrefs.Add(new ClinicPref() { ClinicNum = CLINIC_NUM_DEFAULT, PrefName = prefName, ValueString = pref.Value });
			//}
			//_listWebSchedVerifyClinicPrefs_Old = _listWebSchedVerifyClinicPrefs.Select(x => x.Clone()).ToList();
			////Fill in the UI
			//WebSchedVerifyFillClinics();
			//WebSchedVerifyFillTemplates();
		}

		///<summary>Fill in the ClinicComboBox if applicable.</summary>
		private void WebSchedVerifyFillClinics()
		{
			if (PrefC.HasClinicsEnabled)
			{
				comboClinicVerify.Visible = true;
				checkUseDefaultsVerify.Visible = true;
			}
			else
			{
				comboClinicVerify.Visible = false;
				checkUseDefaultsVerify.Visible = false;
			}
		}

		///<summary>Fill in the template data for the current clinic.</summary>
		private void WebSchedVerifyFillTemplates()
		{
			//Load Recall values
			WebSchedVerify_SetRadioButtonVal(PrefName.WebSchedVerifyRecallType, groupBoxRadioRecall);
			textRecallTextTemplate.Text = WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyRecallText);
			textRecallEmailSubj.Text = WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyRecallEmailSubj);
			RefreshEmail(browserRecallEmailBody, WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyRecallEmailBody)
				, (EmailType)WebSchedVerify_GetInt(PrefName.WebSchedVerifyRecallEmailTemplateType) == EmailType.RawHtml);
			//Load NewPat values
			WebSchedVerify_SetRadioButtonVal(PrefName.WebSchedVerifyNewPatType, groupBoxRadioNewPat);
			textNewPatTextTemplate.Text = WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyNewPatText);
			textNewPatEmailSubj.Text = WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyNewPatEmailSubj);
			RefreshEmail(browserNewPatEmailBody, WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyNewPatEmailBody)
				, (EmailType)WebSchedVerify_GetInt(PrefName.WebSchedVerifyNewPatEmailTemplateType) == EmailType.RawHtml);
			//Load ASAP values
			WebSchedVerify_SetRadioButtonVal(PrefName.WebSchedVerifyASAPType, groupBoxRadioASAP);
			textASAPTextTemplate.Text = WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyASAPText);
			textASAPEmailSubj.Text = WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyASAPEmailSubj);
			RefreshEmail(browserAsapEmailBody, WebSchedVerify_GetTemplateVal(PrefName.WebSchedVerifyASAPEmailBody)
				, (EmailType)WebSchedVerify_GetInt(PrefName.WebSchedVerifyAsapEmailTemplateType) == EmailType.RawHtml);
		}

		private void RefreshEmail(WebBrowser emailBody, string emailText, bool isRawHtml)
		{
			if (isRawHtml)
			{
				emailBody.DocumentText = emailText;
				return;//text is already in HTML, it does not need to be translated. 
			}
			ODException.SwallowAnyException(() =>
			{
				string text = MarkupEdit.TranslateToXhtml(emailText, isPreviewOnly: true, hasWikiPageTitles: false, isEmail: true);
				emailBody.DocumentText = text;
			});
		}
		#endregion Load-in

		///<summary>Save template changes made in WebSchedVerify.</summary>
		private void SaveTabWebSchedVerify()
		{
			//List<long> listClinics = Clinics.GetForUserod(Security.CurrentUser).Select(x => x.ClinicNum).ToList();
			//foreach (PrefName prefName in _listWebSchedVerifyPrefNames)
			//{
			//	foreach (long clinicNum in listClinics)
			//	{
			//		ClinicPref newClinicPref = _listWebSchedVerifyClinicPrefs.FirstOrDefault(x => x.PrefName == prefName && x.ClinicNum == clinicNum);
			//		ClinicPref oldClinicPref = _listWebSchedVerifyClinicPrefs_Old.FirstOrDefault(x => x.PrefName == prefName && x.ClinicNum == clinicNum);
			//		if (oldClinicPref == null && newClinicPref == null)
			//		{ //skip items not in either list
			//			continue;
			//		}
			//		else if (oldClinicPref == null && newClinicPref != null)
			//		{ //insert items in the new list and not the old list
			//			ClinicPrefs.Insert(newClinicPref);
			//		}
			//		else if (oldClinicPref != null && newClinicPref == null)
			//		{ //delete items in the old list and not the new list
			//			ClinicPrefs.Delete(oldClinicPref.ClinicPrefNum);
			//		}
			//		else
			//		{ //update items that have changed
			//			ClinicPrefs.Update(newClinicPref, oldClinicPref);
			//		}
			//	}
			//	ClinicPref newPref = _listWebSchedVerifyClinicPrefs.FirstOrDefault(x => x.PrefName == prefName && x.ClinicNum == CLINIC_NUM_DEFAULT);
			//	if (newPref != null)
			//	{
			//		Prefs.Set(prefName, newPref.ValueString);
			//	}
			//}
		}

		#region Event handlers
		///<summary>Event handler for CheckUseDefaults check changed.</summary>
		private void WebSchedVerify_CheckUseDefaultsChanged(object sender, EventArgs e)
		{
			//if (checkUseDefaultsVerify.Checked)
			//{
			//	groupBoxRecall.Enabled = false;
			//	groupBoxNewPat.Enabled = false;
			//	groupBoxASAP.Enabled = false;
			//	_listWebSchedVerifyClinicPrefs.RemoveAll(x => x.ClinicNum == comboClinicVerify.SelectedClinicNum);
			//}
			//else
			//{
			//	groupBoxRecall.Enabled = true;
			//	groupBoxNewPat.Enabled = true;
			//	groupBoxASAP.Enabled = true;
			//	//Only do this logic if the check change result from the user manually checking the box, not from changing clinics
			//	if (!_listWebSchedVerifyClinicPrefs.Any(x => x.ClinicNum == comboClinicVerify.SelectedClinicNum))
			//	{
			//		foreach (PrefName prefName in _listWebSchedVerifyPrefNames)
			//		{
			//			WebSchedVerify_TryRestoreClinicPrefOld(prefName);
			//		}
			//	}
			//}
			//WebSchedVerifyFillTemplates();
		}

		///<summary>Event handler for ComboClinics index changed.</summary>
		private void WebSchedVerify_ComboClinicSelectedIndexChanged(object sender, EventArgs e)
		{
			//if (comboClinicVerify.SelectedClinicNum == CLINIC_NUM_DEFAULT)
			//{//'Default' is selected.
			//	checkUseDefaultsVerify.Visible = false;
			//	checkUseDefaultsVerify.Checked = false;
			//}
			//else
			//{
			//	checkUseDefaultsVerify.Visible = true;
			//	if (!_listWebSchedVerifyClinicPrefs.Exists(x => x.ClinicNum == comboClinicVerify.SelectedClinicNum))
			//	{
			//		checkUseDefaultsVerify.Checked = true;
			//	}
			//	else
			//	{
			//		checkUseDefaultsVerify.Checked = false;
			//	}
			//}
			//WebSchedVerifyFillTemplates();
		}

		///<summary>Event handler for RadioButtons check changed.</summary>
		private void WebSchedVerify_RadioButtonCheckChanged(object sender, EventArgs e)
		{
			//RadioButton buttonCur = (RadioButton)sender;
			//GroupBox groupBox = (GroupBox)buttonCur.Parent;
			//if (buttonCur.Checked)
			//{
			//	string prefName = (string)groupBox.Tag;
			//	WebSchedVerifyType verifyType = (WebSchedVerifyType)buttonCur.Tag;
			//	if (_listWebSchedVerifyClinicPrefs.Any(x => x.ClinicNum == comboClinicVerify.SelectedClinicNum))
			//	{
			//		//We only want to do this part when the user manually checked this, not when the check-defaults forced it to change
			//		WebSchedVerify_UpdateClinicPref(prefName, POut.Int((int)verifyType));
			//	}
			//}
		}

		///<summary>Event handler for TextBox leave.</summary>
		private void WebSchedVerify_TextLeave(object sender, EventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			string prefName = (string)textBox.Tag;
			WebSchedVerify_UpdateClinicPref(prefName, textBox.Text);
		}

		private void WebSchedVerify_RecallEditEmailClick(object sender, EventArgs e)
		{
			WebSchedNotify_EditEmail(browserRecallEmailBody, PrefName.WebSchedVerifyRecallEmailBody,
				PrefName.WebSchedVerifyRecallEmailTemplateType);
		}

		private void WebSchedVerify_NewPatEditEmailClick(object sender, EventArgs e)
		{
			WebSchedNotify_EditEmail(browserNewPatEmailBody, PrefName.WebSchedVerifyNewPatEmailBody,
				PrefName.WebSchedVerifyNewPatEmailTemplateType);
		}

		private void WebSchedVerify_AsapEditEmailClick(object sender, EventArgs e)
		{
			WebSchedNotify_EditEmail(browserAsapEmailBody, PrefName.WebSchedVerifyASAPEmailBody,
				PrefName.WebSchedVerifyAsapEmailTemplateType);
		}

		private void WebSchedNotify_EditEmail(WebBrowser emailBody, string prefName, string emailType)
		{
			FormEmailEdit formEmailEdit = new FormEmailEdit
			{
				MarkupText = WebSchedVerify_GetTemplateVal(prefName),
				DoCheckForDisclaimer = true,
				IsRawAllowed = true,
				IsRaw = (EmailType)WebSchedVerify_GetInt(emailType) == EmailType.RawHtml
			};
			formEmailEdit.ShowDialog();
			if (formEmailEdit.DialogResult != DialogResult.OK)
			{
				return;
			}
			WebSchedVerify_UpdateClinicPref(prefName, formEmailEdit.MarkupText);//update template text
			WebSchedVerify_UpdateClinicPref(emailType, (formEmailEdit.IsRaw ? (int)EmailType.RawHtml : (int)EmailType.Html).ToString());
			RefreshEmail(emailBody, formEmailEdit.MarkupText, formEmailEdit.IsRaw);
		}

		/// <summary>All the user to undo all changes they have made to the currently selected clinic.</summary>
		private void WebSchedVerify_butUndoClick(object sender, EventArgs e)
		{
			//bool isAccepted = MsgBox.Show(MsgBoxButtons.YesNo, "Undo all changes to templates in this clinic?");
			//if (isAccepted)
			//{
			//	foreach (PrefName prefName in _listWebSchedVerifyPrefNames)
			//	{
			//		WebSchedVerify_TryRestoreClinicPrefOld(prefName);
			//	}
			//	WebSchedVerifyFillTemplates();
			//}
		}

		/// <summary>This form uses a generic contextMenu for textboxes, so we need to use these event handlers to override the default menu.</summary>
		private void WebSchedVerify_ContextMenuUndoClick(object sender, EventArgs e)
		{
			ToolStripItem item = (ToolStripItem)sender;
			ContextMenuStrip menu = (ContextMenuStrip)item.Owner;
			((TextBox)menu.SourceControl).Undo();
		}

		/// <summary>This form uses a generic contextMenu for textboxes, so we need to use these event handlers to override the default menu.</summary>
		private void WebSchedVerify_ContextMenuCutClick(object sender, EventArgs e)
		{
			ToolStripItem item = (ToolStripItem)sender;
			ContextMenuStrip menu = (ContextMenuStrip)item.Owner;
			((TextBox)menu.SourceControl).Cut();
		}

		/// <summary>This form uses a generic contextMenu for textboxes, so we need to use these event handlers to override the default menu.</summary>
		private void WebSchedVerify_ContextMenuCopyClick(object sender, EventArgs e)
		{
			ToolStripItem item = (ToolStripItem)sender;
			ContextMenuStrip menu = (ContextMenuStrip)item.Owner;
			((TextBox)menu.SourceControl).Copy();
		}

		/// <summary>This form uses a generic contextMenu for textboxes, so we need to use these event handlers to override the default menu.</summary>
		private void WebSchedVerify_ContextMenuPasteClick(object sender, EventArgs e)
		{
			ToolStripItem item = (ToolStripItem)sender;
			ContextMenuStrip menu = (ContextMenuStrip)item.Owner;
			((TextBox)menu.SourceControl).Paste();
		}

		/// <summary>This form uses a generic contextMenu for textboxes, so we need to use these event handlers to override the default menu.</summary>
		private void WebSchedVerify_ContextMenuSelectAllClick(object sender, EventArgs e)
		{
			ToolStripItem item = (ToolStripItem)sender;
			ContextMenuStrip menu = (ContextMenuStrip)item.Owner;
			((TextBox)menu.SourceControl).SelectAll();
		}

		/// <summary>Opens FormMessageReplacements to allow the user to select from replaceable tags to include in the templates.</summary>
		private void WebSchedVerify_ContextMenuReplacementsClick(object sender, EventArgs e)
		{
			ToolStripItem item = (ToolStripItem)sender;
			ContextMenuStrip menu = (ContextMenuStrip)item.Owner;
			TextBox textBox = ((TextBox)menu.SourceControl);
			//PHI is not supposed to be communicated via text message.
			bool allowPHI = (!textBox.Name.In(textRecallTextTemplate.Name, textNewPatTextTemplate.Name, textASAPTextTemplate.Name));
			FormMessageReplacements FormMR = new FormMessageReplacements(
				MessageReplaceType.Appointment | MessageReplaceType.Office | MessageReplaceType.Patient, allowPHI);
			FormMR.IsSelectionMode = true;
			FormMR.ShowDialog();
			if (FormMR.DialogResult == DialogResult.OK)
			{
				textBox.SelectedText = FormMR.Replacement;
			}
		}
		#endregion Event handlers

		#region Helpers
		///<summary>Returns the clinic pref value for the currently selected clinic and provided PrefName, or the default pref if there is none.</summary>
		private string WebSchedVerify_GetTemplateVal(string prefName)
		{
			//ClinicPref clinicPref = _listWebSchedVerifyClinicPrefs.FirstOrDefault(x => x.ClinicNum == comboClinicVerify.SelectedClinicNum && x.PrefName == prefName);
			//ClinicPref defaultPref = _listWebSchedVerifyClinicPrefs.FirstOrDefault(x => x.ClinicNum == CLINIC_NUM_DEFAULT && x.PrefName == prefName);
			////ClinicPref won't be available if it has not been created previously.
			//return clinicPref != null ? clinicPref.ValueString : defaultPref.ValueString;
			return "";
		}

		///<summary>Checks the currently selected radio button for the given PrefName and groupBox, based on the radio button tags.</summary>
		private void WebSchedVerify_SetRadioButtonVal(string prefName, GroupBox groupBox)
		{
			WebSchedVerifyType type = (WebSchedVerifyType)PIn.Int(WebSchedVerify_GetTemplateVal(prefName));
			RadioButton buttonMatch = groupBox.Controls.OfType<RadioButton>().FirstOrDefault(x => (WebSchedVerifyType)x.Tag == type);
			buttonMatch.Checked = true;
		}

		private int WebSchedVerify_GetInt(string prefName)
		{
			return PIn.Int(WebSchedVerify_GetTemplateVal(prefName));
		}

		///<summary>Updates the in-memory clinic pref list with the given valueString for the provided prefName and currently selected clinic.</summary>
		private void WebSchedVerify_UpdateClinicPref(string prefName, string valueString)
		{
			//ClinicPref clinicPref = _listWebSchedVerifyClinicPrefs.FirstOrDefault(x => x.ClinicNum == comboClinicVerify.SelectedClinicNum && x.PrefName == prefName);
			//if (clinicPref == null)
			//{
			//	_listWebSchedVerifyClinicPrefs.Add(new ClinicPref() { PrefName = prefName, ClinicNum = comboClinicVerify.SelectedClinicNum, ValueString = valueString });
			//}
			//else
			//{
			//	clinicPref.ValueString = valueString;
			//}
		}

		/// <summary>Tries to get the original clinic pref that was loaded in when the form first opened, and reload it into the in-memory clinic pref 
		/// list. If there is no old pref, this loads the default pref value for that clinic into the in-memory list.</summary>
		private void WebSchedVerify_TryRestoreClinicPrefOld(string prefName)
		{
			//ClinicPref pref = _listWebSchedVerifyClinicPrefs_Old.FindAll(x => x.ClinicNum == comboClinicVerify.SelectedClinicNum && x.PrefName == prefName).FirstOrDefault();
			//if (pref == null)
			//{
			//	pref = _listWebSchedVerifyClinicPrefs.FindAll(x => x.ClinicNum == CLINIC_NUM_DEFAULT && x.PrefName == prefName).First();
			//}
			//WebSchedVerify_UpdateClinicPref(prefName, pref.ValueString);
		}
		#endregion Helpers

		#endregion Tab - Verify

		private void butOK_Click(object sender, EventArgs e)
		{
			if (Patients.DoesContainPHIField(textRecallTextTemplate.Text))
			{
				MessageBox.Show("Web Sched Verify Recall Text Template is not allowed to contain Protected Health Information.");
				return;
			}
			if (Patients.DoesContainPHIField(textNewPatTextTemplate.Text))
			{
				MessageBox.Show("Web Sched Verify New Patient Text Template is not allowed to contain Protected Health Information.");
				return;
			}
			if (Patients.DoesContainPHIField(textASAPTextTemplate.Text))
			{
				MessageBox.Show("Web Sched Verify ASAP Text Template is not allowed to contain Protected Health Information.");
				return;
			}
			SaveTabWebSchedNewPat();
			SaveTabWebSchedRecall();
			SaveTabWebSchedVerify();
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		///<summary>This is a helper struct which is set to the Tag of the Web Sched threads so that they don't have to access UI elements.</summary>
		private struct WebSchedTimeSlotArgs
		{
			///<summary>Only used for Web Sched Recall.</summary>
			public RecallType RecallTypeCur;
			///<summary>Only used for Web Sched Recall.</summary>
			public List<Provider> ListProviders;
			///<summary>Only trust ClinicNum from this object.</summary>
			public Clinic ClinicCur;
			///<summary></summary>
			public DateTime DateStart;
			///<summary></summary>
			public DateTime DateEnd;
		}

		public enum WebSchedTab
		{
			NewPat,
			Recall,
			Verify
		}
	}
}
