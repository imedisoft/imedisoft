using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using OpenDental.UI;
using OpenDental;
using OpenDentBusiness;
using System.Globalization;

namespace OpenDental.User_Controls.SetupWizard {
	public partial class UserControlSetupWizPractice:SetupWizControl {
		///<summary>Deep copy of the short providers cache.  Refilled every time FillControls() is invoked.</summary>
		private List<Provider> _listProviders;

		public UserControlSetupWizPractice() {
			InitializeComponent();
			this.OnControlDone += ControlDone;
		}

		private void UserControlSetupWizPractice_Load(object sender,EventArgs e) {
			FillControls();
		}

		private void FillControls() {
			_listProviders=Providers.GetDeepCopy(true);
			textPracticeTitle.Text=Prefs.GetString(PrefName.PracticeTitle);
			textAddress.Text=Prefs.GetString(PrefName.PracticeAddress);
			textAddress2.Text=Prefs.GetString(PrefName.PracticeAddress2);
			textCity.Text=Prefs.GetString(PrefName.PracticeCity);
			textST.Text=Prefs.GetString(PrefName.PracticeST);
			textZip.Text=Prefs.GetString(PrefName.PracticeZip);
			textPhone.Text=TelephoneNumbers.ReFormat(Prefs.GetString(PrefName.PracticePhone));
			textFax.Text=TelephoneNumbers.ReFormat(Prefs.GetString(PrefName.PracticeFax));
			listProvider.Items.Clear();
			for(int i = 0;i<_listProviders.Count;i++) {
				listProvider.Items.Add(_listProviders[i].GetLongDesc());
				if(_listProviders[i].ProvNum==Prefs.GetLong(PrefName.PracticeDefaultProv)) {
					listProvider.SelectedIndex=i;
				}
			}
			CheckIsDone();
		}

		private void butAdvanced_Click(object sender,EventArgs e) {
			new FormPractice().ShowDialog();
			FillControls();
		}

		private void CheckIsDone() {
			IsDone=true;
			StrIncomplete = Lan.G("FormSetupWizard","The following fields need to be corrected: ");
			string phone = textPhone.Text;//Auto formatting turned off on purpose.
			if(!TelephoneNumbers.IsNumberValidTenDigit(ref phone)) {
				IsDone=false;
				StrIncomplete+="\r\n "+Lan.G("FormSetupWizard","-Practice Phone is invalid.  Must contain exactly ten digits.");
			}
			string fax = textFax.Text;//Auto formatting turned off on purpose.
			if(!TelephoneNumbers.IsNumberValidTenDigit(ref fax)) {
				IsDone=false;
				StrIncomplete+="\r\n "+Lan.G("FormSetupWizard","-Practice Fax is invalid.  Must contain exactly ten digits.");
			}
			if(listProvider.SelectedIndex==-1//practice really needs a default prov
				&& _listProviders.Count > 0) 
			{
				listProvider.SelectedIndex=0;
			}
			if(_listProviders.Count > 0
				&& _listProviders[listProvider.SelectedIndex].FeeSched==0)//Default provider must have a fee schedule set.
			{
				//listProvider.BackColor = OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
				IsDone=false;
				StrIncomplete+="\r\n "+Lan.G("FormSetupWizard","-Practice Provider must have a default fee schedule.");
			}
		}

		private void Control_Validated(object sender,EventArgs e) {
			CheckIsDone();
		}


		private void ControlDone(object sender,EventArgs e) {
			string phone = textPhone.Text;
			if(Application.CurrentCulture.Name=="en-US"
				|| CultureInfo.CurrentCulture.Name.EndsWith("CA")) //Canadian. en-CA or fr-CA)
			{
				phone=phone.Replace("(","");
				phone=phone.Replace(")","");
				phone=phone.Replace(" ","");
				phone=phone.Replace("-","");
			}
			string fax = textFax.Text;
			if(Application.CurrentCulture.Name=="en-US"
				|| CultureInfo.CurrentCulture.Name.EndsWith("CA")) //Canadian. en-CA or fr-CA)
			{
				fax=fax.Replace("(","");
				fax=fax.Replace(")","");
				fax=fax.Replace("-","");
				if(fax.Length!=0 && fax.Length!=10) {
					textFax.BackColor = OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
					IsDone=false;
				}
			}
			bool changed = false;
			if(Prefs.Set(PrefName.PracticeTitle,textPracticeTitle.Text)
				| Prefs.Set(PrefName.PracticeAddress,textAddress.Text)
				| Prefs.Set(PrefName.PracticeAddress2,textAddress2.Text)
				| Prefs.Set(PrefName.PracticeCity,textCity.Text)
				| Prefs.Set(PrefName.PracticeST,textST.Text)
				| Prefs.Set(PrefName.PracticeZip,textZip.Text)
				| Prefs.Set(PrefName.PracticePhone,phone)
				| Prefs.Set(PrefName.PracticeFax,fax))
			{
				changed=true;
			}
			if(listProvider.SelectedIndex!=-1) {
				if(Prefs.Set(PrefName.PracticeDefaultProv,_listProviders[listProvider.SelectedIndex].ProvNum)) {
					changed=true;
				}
			}
			if(changed) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FormEServicesMobileSynch.UploadPreference(PrefName.PracticeTitle);
		}
	}
}
