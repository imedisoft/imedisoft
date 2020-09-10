using System;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Drawing.Printing;
using CodeBase;
using Imedisoft.Data;

namespace OpenDental.User_Controls.SetupWizard {
	public partial class UserControlSetupWizPrinter:SetupWizControl {

		public UserControlSetupWizPrinter() {
			InitializeComponent();
			OnControlDone+=ControlDone;
			OnControlValidated+=ControlValidated;
		}

		private void UserControlSetupWizPrinter_Load(object sender,EventArgs e) {
			FillControls();
		}

		private void FillControls() {
			checkSimple.Checked=Preferences.GetBool(PreferenceName.EasyHidePrinters);
			IsDone=true;
			SetSimple();
			SetControls(groupPrinter);
			if(Clinics.IsMedicalClinic(Clinics.ClinicId)) {
				labelTPandPerio.Text="Treatment Plans";
			}
			checkScanDocSelectSource.Checked=ComputerPrefs.LocalComputer.ScanDocSelectSource;
			if(ComputerPrefs.LocalComputer.ScanDocShowOptions) {
				radioScanDocShowOptions.Checked=true;
				radioScanDocUseOptionsBelow.Checked=false;
				groupScanningOptions.Enabled=false;
			}
			else {
				radioScanDocShowOptions.Checked=false;
				radioScanDocUseOptionsBelow.Checked=true;
				groupScanningOptions.Enabled=true;
			}
			checkScanDocDuplex.Checked=ComputerPrefs.LocalComputer.ScanDocDuplex;
			checkScanDocGrayscale.Checked=ComputerPrefs.LocalComputer.ScanDocGrayscale;
			textScanDocResolution.Text=ComputerPrefs.LocalComputer.ScanDocResolution.ToString();
			textScanDocQuality.Text=ComputerPrefs.LocalComputer.ScanDocQuality.ToString();
		}

		///<summary>recursive</summary>
		private void SetControls(Control c) {
			foreach(Control control in c.Controls) {
				SetControls(control);
				if(control==checkSimple) {
					continue;
				}
				if(control.GetType()==typeof(ComboBox)) {
					FillCombo((ComboBox)control);
				}
				if(control.GetType()==typeof(CheckBox)) {
					FillCheck((CheckBox)control);
				}
			}
		}

		private void FillCombo(ComboBox combo) {
			PrintSituation sit = GetSit(combo);
			Printer printerForSit = Printers.GetForSit(sit);
			string printerName = "";
			if(printerForSit!=null) {
				printerName=printerForSit.PrinterName;
			}
			combo.Items.Clear();
			if(combo==comboDefault) {
				combo.Items.Add("Windows default");
			}
			else {
				combo.Items.Add("default");
			}
			for(int i = 0;i<PrinterSettings.InstalledPrinters.Count;i++) {
				combo.Items.Add(PrinterSettings.InstalledPrinters[i]);
				if(printerName==PrinterSettings.InstalledPrinters[i]) {
					combo.SelectedIndex=i+1;
				}
			}
			if(combo.SelectedIndex==-1) {
				combo.SelectedIndex=0;
			}
		}

		private void FillCheck(CheckBox check) {
			PrintSituation sit = GetSit(check);
			Printer printerForSit = Printers.GetForSit(sit);
			if(printerForSit==null) {
				check.Checked=false;
				return;
			}
			check.Checked=printerForSit.DisplayPrompt;
		}

		private PrintSituation GetSit(Control contr) {
			PrintSituation sit = PrintSituation.Default;
			switch(contr.Name) {
				default:
					MessageBox.Show("error. "+contr.Name);
					break;
				case "comboDefault":
				case "checkDefault":
					sit=PrintSituation.Default;
					break;
				case "comboAppointments":
				case "checkAppointments":
					sit=PrintSituation.Appointments;
					break;
				case "comboClaim":
				case "checkClaim":
					sit=PrintSituation.Claim;
					break;
				case "comboLabelSheet":
				case "checkLabelSheet":
					sit=PrintSituation.LabelSheet;
					break;
				case "comboLabelSingle":
				case "checkLabelSingle":
					sit=PrintSituation.LabelSingle;
					break;
				case "comboPostcard":
				case "checkPostcard":
					sit=PrintSituation.Postcard;
					break;
				case "comboRx":
				case "checkRx":
					sit=PrintSituation.Rx;
					break;
				case "comboRxControlled":
				case "checkRxControlled":
					sit=PrintSituation.RxControlled;
					break;
				case "comboRxMulti":
				case "checkRxMulti":
					sit=PrintSituation.RxMulti;
					break;
				case "comboStatement":
				case "checkStatement":
					sit=PrintSituation.Statement;
					break;
				case "comboTPPerio":
				case "checkTPPerio":
					sit=PrintSituation.TPPerio;
					break;
				case "comboReceipt":
				case "checkReceipt":
					sit=PrintSituation.Receipt;
					break;
			}
			return sit;
		}

		///<summary>Sets the simple hide based on the status of the checkbox</summary>
		private void SetSimple() {
			panelSimple.Visible=!checkSimple.Checked;
		}

		private void checkSimple_Click(object sender,System.EventArgs e) {
			SetSimple();
		}

		private void labelInfoCompression_Click(object sender,EventArgs e) {
			MsgBox.Show("Enter a percentage between 0 and 100. Enter 100 for no compression. A typical setting is 40%.");
		}

		private void labelInfoDuplex_Click(object sender,EventArgs e) {
			MsgBox.Show("If this setting causes your scanner to malfunction, use the 'Show Scanner Options Window' instead.");
		}

		private void labelInfoResolution_Click(object sender,EventArgs e) {
			MsgBox.Show("Enter a dpi value between 50 and 1000. A typical setting is 150 dpi.");
		}

		private void butSetScanner_Click(object sender,EventArgs e) {
			xImageDeviceManager.Obfuscator.ActivateEZTwain();
			EZTwain.SelectImageSource(this.Handle);
		}

		private void radioScanDocShowOptions_CheckedChanged(object sender,EventArgs e) {
			if(radioScanDocShowOptions.Checked) {
				groupScanningOptions.Enabled=false;
			}
			else {
				groupScanningOptions.Enabled=true;
			}
		}

		private void butAdvanced_Click(object sender,EventArgs e) {
			new FormImagingSetup().ShowDialog();
			FillControls();
		}

		private new bool ControlValidated(object sender,EventArgs e) {
			if(textScanDocQuality.errorProvider1.GetError(textScanDocQuality)!=""
				|| textScanDocResolution.errorProvider1.GetError(textScanDocResolution)!="") 
			{
				MsgBox.Show("Please fix data entry errors first.");
				return false;
			}
			if(textScanDocQuality.Text=="100"
				|| (radioScanDocUseOptionsBelow.Checked && PIn.Int(textScanDocResolution.Text)>300)) 
			{
				if(!MsgBox.Show("FormSetupWizard",MsgBoxButtons.YesNo,"With the provided scanner compression settings, the file created may be extremely large. "
					+" Would you like to continue?")) {
					return false;
				}
			}
			if(checkSimple.Checked && !Preferences.GetBool(PreferenceName.EasyHidePrinters)) {
				//if user clicked the simple option
				if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Warning! You have selected the easy view option for printers.  This will clear all printing preferences for all computers.  Are you sure you wish to continue?")) {
					return false;
				}
			}
			return true;
		}

		private void ControlDone(object sender,EventArgs e) {
			string compName = SystemInformation.ComputerName;
			if(checkSimple.Checked && !Preferences.GetBool(PreferenceName.EasyHidePrinters)) {
				Printers.ClearAll();
				Printers.RefreshCache();
                string printerName;
                if (comboDefault.SelectedIndex==0) {
					printerName="";
				}
				else {
					printerName=PrinterSettings.InstalledPrinters[comboDefault.SelectedIndex-1];
				}
				Printers.PutForSit(PrintSituation.Default,compName,printerName,true);
			}
			else for(int i = 0;i<Enum.GetValues(typeof(PrintSituation)).Length;i++) {
				//loop through each printSituation
				string printerName = "";
				bool isChecked = false;
				//PrintSituation sit=PrintSituation.Default;
				//first: main Default, since not in panel Simple
				if(i==0) {//printSituation.Default
					if(comboDefault.SelectedIndex==0) {
						printerName="";
					}
					else {
						printerName=PrinterSettings.InstalledPrinters[comboDefault.SelectedIndex-1];
					}
				}
				foreach(Control control in panelSimple.Controls) {
					if(control.GetType()!=typeof(ComboBox)//skip anything but comboBoxes and CheckBoxes
						&& control.GetType()!=typeof(CheckBox)) {
						continue;
					}
					//so only two controls out of all will be used in each Enum loop
					if(GetSit(control)!=(PrintSituation)i) {
						continue;
					}
					if(control.GetType()==typeof(ComboBox)) {
						if(((ComboBox)control).SelectedIndex==0) {
							printerName="";
						}
						else {
							printerName=PrinterSettings.InstalledPrinters[((ComboBox)control).SelectedIndex-1];
						}
					}
					else {//checkBox
						isChecked=((CheckBox)control).Checked;
					}
				}
				Printers.PutForSit((PrintSituation)i,compName,printerName,isChecked);
			}
			DataValid.SetInvalid(InvalidType.Computers);
			if(checkSimple.Checked!=Preferences.GetBool(PreferenceName.EasyHidePrinters)) {
				Preferences.Set(PreferenceName.EasyHidePrinters,checkSimple.Checked);
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			Printers.RefreshCache();//the other computers don't care
			ComputerPrefs.LocalComputer.ScanDocSelectSource=checkScanDocSelectSource.Checked;
			ComputerPrefs.LocalComputer.ScanDocShowOptions=radioScanDocShowOptions.Checked;
			ComputerPrefs.LocalComputer.ScanDocDuplex=checkScanDocDuplex.Checked;
			ComputerPrefs.LocalComputer.ScanDocGrayscale=checkScanDocGrayscale.Checked;
			ComputerPrefs.LocalComputer.ScanDocResolution=PIn.Int(textScanDocResolution.Text);
			ComputerPrefs.LocalComputer.ScanDocQuality=PIn.Byte(textScanDocQuality.Text);
			ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			DataValid.SetInvalid(InvalidType.Prefs);
		}
	}
}
