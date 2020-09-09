using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using Imedisoft.UI;
using OpenDental;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormProgramLinkEdit : FormBase
	{
		private string pathOverrideOld;
		private bool _isLoading = false;

		public bool IsNew;
		public Program ProgramCur;

		/// <summary>
		/// Set to false if we do not want to allow assigning program link to toolbars.
		/// </summary>
		public bool AllowToolbarChanges = true;

		public FormProgramLinkEdit()
		{
			InitializeComponent();
		}

		private void FormProgramLinkEdit_Load(object sender, System.EventArgs e)
		{
			_isLoading = true;
			if (ProgramCur.Name != "")
			{
				//user not allowed to delete program links that we include, only their own.
				deleteButton.Enabled = false;
			}

			pathOverrideOld = ProgramProperties.GetLocalPathOverrideForProgram(ProgramCur.Id);
			overrideTextBox.Text = pathOverrideOld;
			FillForm();
			DisableUIElementsBasedOnClinicRestriction();//Disable the UI Elements if needed.
			HideClinicControls(PrefC.HasClinicsEnabled);//Hide the "Hide Button for Clinics" button based upon the user's clinics being on or off.
			ShowPLButHiddenLabel();//Display warning label for "Hide Button for Clinics" if needed.
			SetAdvertising();

			_isLoading = false;
		}

		/// <summary>
		/// Handles both visibility and checking of checkHideButtons.
		/// </summary>
		private void SetAdvertising()
		{
			hideButtonsCheckBox.Visible = true;

			ProgramProperty prop = ProgramProperties.GetForProgram(ProgramCur.Id).FirstOrDefault(x => x.Description == "Disable Advertising");
			if (enabledCheckBox.Checked || prop == null)
			{
				hideButtonsCheckBox.Visible = false;
			}

			if (prop != null)
			{
				hideButtonsCheckBox.Checked = (prop.Value == "1");
			}
		}

		private void checkEnabled_CheckedChanged(object sender, EventArgs e)
		{
			SetAdvertising();
		}

		private void FillForm()
		{
			//this is not refined enough to be called more than once on the form because it will not
			//remember the toolbars that were selected.
			ToolButItems.RefreshCache();
			ProgramProperties.RefreshCache();
			programNameTextBox.Text = ProgramCur.Name;
			descriptionTextBox.Text = ProgramCur.Description;
			enabledCheckBox.Checked = ProgramCur.Enabled;
			pathTextBox.Text = ProgramCur.Path;
			textCommandLine.Text = ProgramCur.CommandLine;
			notesTextBox.Text = ProgramCur.Note;
			buttonImagePictureBox.Image = PIn.Bitmap(ProgramCur.ButtonImage);
			List<ToolButItem> itemsForProgram = ToolButItems.GetForProgram(ProgramCur.Id);
			toolbarsListBox.Items.Clear();
			for (int i = 0; i < Enum.GetNames(typeof(ToolBarsAvail)).Length; i++)
			{
				toolbarsListBox.Items.Add(Enum.GetNames(typeof(ToolBarsAvail))[i]);
			}
			for (int i = 0; i < itemsForProgram.Count; i++)
			{
				toolbarsListBox.SetSelected((int)itemsForProgram[i].ToolBar, true);
			}
			if (!AllowToolbarChanges)
			{//As we add more static bridges, we will need to enhance this to show/hide controls as needed.
				toolbarsListBox.ClearSelected();
				toolbarsListBox.Enabled = false;
			}
			if (itemsForProgram.Count > 0)
			{//the text on all buttons will be the same for now
				buttonTextTextBox.Text = itemsForProgram[0].ButtonText;
			}
			FillGrid();
		}

		private void FillGrid()
		{
			var programProperties = ProgramProperties.GetForProgram(ProgramCur.Id);

			Plugins.HookAddCode(this, "FormProgramLinkEdit.FillGrid_GetProgramProperties", programProperties, ProgramCur);

			propertiesGrid.BeginUpdate();
			propertiesGrid.Columns.Clear();
			propertiesGrid.Columns.Add(new GridColumn("Property", 260));
			propertiesGrid.Columns.Add(new GridColumn("Value", 130));
			propertiesGrid.Rows.Clear();

			foreach (var property in programProperties)
			{
				if (property.Description.In("Disable Advertising", ProgramProperties.PropertyDescs.ClinicHideButton))
				{
					continue;
				}

				var gridRow = new GridRow();

				gridRow.Cells.Add(property.Description);
				if (ProgramCur.Name == ProgramName.XVWeb.ToString() && property.Description == XVWeb.ProgramProps.Password)
				{
                    //CDT.Class1.Decrypt(property.Value, out string decrypted);

                    gridRow.Cells.Add(new string('*', property.Value.Length));//Show the password as '*'

				}
				else if (ProgramCur.Name == ProgramName.XVWeb.ToString() && property.Description == XVWeb.ProgramProps.ImageCategory)
				{
					Definition imageCat = Definitions.GetDefsForCategory(DefinitionCategory.ImageCats).FirstOrDefault(x => x.Id == PIn.Long(property.Value));
					if (imageCat == null)
					{
						gridRow.Cells.Add("");
					}
					else if (imageCat.IsHidden)
					{
						gridRow.Cells.Add(imageCat.Name + " (hidden)");
					}
					else
					{
						gridRow.Cells.Add(imageCat.Name);
					}
				}
				else
				{
					gridRow.Cells.Add(property.Value);
				}

				gridRow.Tag = property;

				propertiesGrid.Rows.Add(gridRow);
			}

			propertiesGrid.EndUpdate();
		}

		/// <summary>
		/// This method hides (Visible=false) controls when the Clinics are turned off.
		/// </summary>
		private void HideClinicControls(bool hasClinicsEnabled)
		{
			if (!hasClinicsEnabled)
			{
				clinicsButton.Visible = false;
				clinicsWarningLabel.Visible = false;
			}
		}

		/// <summary>
		/// If Clinics are enabled, and the Program Link button is hidden for at least one clinic, display the warning label  labelClinicStateWarning.
		/// </summary>
		private void ShowPLButHiddenLabel()
		{
			var properties =
				ProgramProperties.GetForProgram(ProgramCur.Id)
					.Where(x => x.Description == ProgramProperties.PropertyDescs.ClinicHideButton).ToList();

			if (PrefC.HasClinicsEnabled && !properties.IsNullOrEmpty())
			{
				clinicsWarningLabel.Visible = true;
			}
			else
			{
				clinicsWarningLabel.Visible = false;
			}
		}

		/// <summary>
		/// If Clinics are enabled, and the user is clinic restricted, disable certain UI elements and turn on the warning that the user is restricted. 
		/// Any ProgramLink settings which would affect clinics to which the user does not have access are disabled.
		/// </summary>
		private void DisableUIElementsBasedOnClinicRestriction()
		{
			if (PrefC.HasClinicsEnabled && Security.CurrentUser.ClinicIsRestricted)
			{//Clinics are Enabled and the user is restricted.
			 //TODO: change this logic to be explicit instead of implicit (i.e get a list of all controls we want to explicitly disable.)
				List<Control> listEnabled = new List<Control>() {
					programNameLabel,programNameTextBox,descriptionLabel,descriptionTextBox,enabledCheckBox,hideButtonsCheckBox,pathLabel,pathTextBox,label4,textCommandLine,buttonTextLabel,buttonTextTextBox,
					propertiesGrid,notesLabel,notesTextBox,label6,toolbarsListBox,buttonImageLabel,buttonImagePictureBox,butClear,butImport,deleteButton
					};
				foreach (Control ctl in this.GetAllControls().Where(x => x.In(listEnabled)))
				{
					ctl.Enabled = false;//Turn off all but the specified controls above in ProgramLinkEdit window.
				}
				disableForClinicLabel.Visible = true;//Turn on the warning in the ProgramLinkEdit window that some controls are disabled for this user.
			}
			else
			{
				disableForClinicLabel.Visible = false;
			}
		}

		/// <summary>
		/// Chooses which type of form to open based on current program and selected property.
		/// </summary>
		private void PropertiesGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (propertiesGrid.Rows[e.Row].Tag is ProgramProperty programProperty)
			{
				switch (ProgramCur.Name)
				{
					case nameof(ProgramName.XVWeb):
						switch (programProperty.Description)
						{
							case XVWeb.ProgramProps.ImageCategory:
								List<string> listDefNums = Definitions.GetDefsForCategory(DefinitionCategory.ImageCats, true).Select(x => POut.Long(x.Id)).ToList();
								List<string> listItemNames = Definitions.GetDefsForCategory(DefinitionCategory.ImageCats, true).Select(x => x.Name).ToList();
								ShowComboBoxForProgramProperty(programProperty, listDefNums, listItemNames, "Choose an Image Category");
								return;

							case XVWeb.ProgramProps.ImageQuality:
								List<string> listOptions = Enum.GetValues(typeof(XVWebImageQuality)).Cast<XVWebImageQuality>().Select(x => x.ToString()).ToList();
								List<string> listDisplay = listOptions;
								ShowComboBoxForProgramProperty(programProperty, listOptions, listDisplay, "Choose an Image Quality");
								return;
						}
						break;
				}

				ShowFormProgramProperty(programProperty);
			}
		}

		private void ImportButton_Click(object sender, EventArgs e)
		{
            using var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                var importedImage = Image.FromFile(openFileDialog.FileName);

                if (importedImage.Size != new Size(22, 22))
                {
                    ShowError(
                        "Required image dimensions are 22x22.\r\n" +
                        "Selected image dimensions are: " +
                        importedImage.Size.Width + "x" + importedImage.Size.Height);

                    return;
                }

                buttonImagePictureBox.Image = importedImage;
            }
            catch
            {
                ShowError("Error loading file.");
            }
        }

		private void ClearButton_Click(object sender, EventArgs e) 
			=> buttonImagePictureBox.Image = null;

		private void checkHideButtons_CheckedChanged(object sender, EventArgs e)
		{
			if (_isLoading)
			{
				return;
			}

			ProgramProperty property = ProgramProperties.GetForProgram(ProgramCur.Id).FirstOrDefault(x => x.Description == "Disable Advertising");
			if (property == null)
			{
				return;//should never happen.
			}

			if (hideButtonsCheckBox.Checked)
			{
				property.Value = "1";
			}
			else
			{
				property.Value = "0";
			}

			ProgramProperties.Save(property);
		}

		private void ShowFormProgramProperty(ProgramProperty programProperty)
		{
			bool encrypted = ProgramCur.Name == ProgramName.XVWeb.ToString() && programProperty.Description == XVWeb.ProgramProps.Password;

			using (var formProgramProperty = new FormProgramProperty(programProperty, encrypted))
			{
				formProgramProperty.ShowDialog();
				if (formProgramProperty.DialogResult != DialogResult.OK)
				{
					return;
				}
			}

			ProgramProperties.RefreshCache();

			FillGrid();
		}

		///<summary>Opens a form where the user can select an option from a combo box for a program poperty.</summary>
		///<param name="listValuesForDb">The value that should be stored in the db for the corresponding display item that is selected. This list should
		///have the same number of items as listForDisplay.</param>
		///<param name="listForDisplay">The value that will be displayed to the user in the combo box. This list should have the same number of items 
		///as listValuesForDb.</param>
		private void ShowComboBoxForProgramProperty(ProgramProperty programProperty, List<string> listValuesForDb, List<string> listForDisplay
			, string prompt)
		{
			ProgramProperty programPropertyOld = programProperty.Copy();
			InputBox inputBox = new InputBox(prompt, listForDisplay, listValuesForDb.FindIndex(x => x == programProperty.Value));
			inputBox.ShowDialog();
			if (inputBox.DialogResult != DialogResult.OK || inputBox.SelectedIndex == -1 ||
				listValuesForDb[inputBox.SelectedIndex] == programPropertyOld.Value)
			{
				return;
			}
			programProperty.Value = listValuesForDb[inputBox.SelectedIndex];
			ProgramProperties.Save(programProperty);
			ProgramProperties.RefreshCache();
			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (ProgramCur.Name != "")
			{
				ShowError("Not allowed to delete a program link with an internal name.");

				return;
			}

			if (Prompt("Delete this program link?", MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				return;
			}

			if (!IsNew)
			{
				Programs.Delete(ProgramCur);
			}

			DialogResult = DialogResult.OK;
		}

		private void ClinicsButton_Click(object sender, EventArgs e)
		{
			var clinics = Clinics.GetByCurrentUser();
			var clinicIds = clinics.Select(c => c.Id).ToList();

			var properties = 
				ProgramProperties.GetForProgram(ProgramCur.Id)
					.Where(x => x.Description == ProgramProperties.PropertyDescs.ClinicHideButton && clinicIds.Contains(x.ClinicId))
					.ToList();

			using (var formProgramLinkHideClinics = new FormProgramLinkHideClinics(ProgramCur, properties, clinics))
			{
				if (formProgramLinkHideClinics.ShowDialog() == DialogResult.OK)
				{
					DataValid.SetInvalid(InvalidType.Programs, InvalidType.ToolButsAndMounts);
				}
			}

			ShowPLButHiddenLabel(); // Set the "Hide Button for Clinics" button based on the updated list.
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			bool mustReset = false;

			if (ProgramCur.Enabled != enabledCheckBox.Checked)
			{
				if (ProgramCur.Name == ProgramName.XVWeb.ToString() || descriptionTextBox.Text == "Suni")
				{
					if (Prompt("The entire program will now need to close to reset imaging.") == DialogResult.No)
					{
						return;
					}

					mustReset = true;
				}
			}

			ProgramCur.Name = programNameTextBox.Text;
			ProgramCur.Description = descriptionTextBox.Text;
			ProgramCur.Enabled = enabledCheckBox.Checked;
			ProgramCur.Path = pathTextBox.Text;
			if (pathOverrideOld != overrideTextBox.Text)
			{
				ProgramProperties.InsertOrUpdateLocalOverridePath(ProgramCur.Id, overrideTextBox.Text);
				ProgramProperties.RefreshCache();
			}
			ProgramCur.CommandLine = textCommandLine.Text;
			ProgramCur.Note = notesTextBox.Text;
			ProgramCur.ButtonImage = POut.Bitmap((Bitmap)buttonImagePictureBox.Image, System.Drawing.Imaging.ImageFormat.Png);

			if (IsNew)
			{
				Programs.Insert(ProgramCur);
			}
			else
			{
				Programs.Update(ProgramCur);
			}

			ToolButItems.DeleteAllForProgram(ProgramCur.Id);
			for (int i = 0; i < toolbarsListBox.SelectedIndices.Count; i++)
			{
                ToolButItems.Insert(new ToolButItem
				{
					ProgramNum = ProgramCur.Id,
					ButtonText = buttonTextTextBox.Text,
					ToolBar = (ToolBarsAvail)toolbarsListBox.SelectedIndices[i]
				});
			}

			DialogResult = DialogResult.OK;

			if (mustReset)
			{
				Prefs.Set(PrefName.ImagesModuleUsesOld2020, enabledCheckBox.Checked);

				Cursor = Cursors.WaitCursor;

				FormOpenDental.S_ProcessKillCommand();
			}
		}

		private void FormProgramLinkEdit_Closing(object sender, CancelEventArgs e)
		{
			if (DialogResult == DialogResult.OK) return;

			if (IsNew)
			{
				Programs.Delete(ProgramCur);
			}
		}
	}
}
