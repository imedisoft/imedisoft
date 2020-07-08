using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormApptReminderRuleAggEdit:ODForm {
		public ApptReminderRule ApptReminderRuleCur;
		public List<ApptReminderRule> ListRulesNonDefault;
		///<summary>Langauge of the tab that was selected in the parent form. Used for picking the tab index of this form. </summary>
		private string _selectedLanguageLoading;

		public FormApptReminderRuleAggEdit(ApptReminderRule apptReminderCur,List<ApptReminderRule> listRulesNonDefault,string selectedLanguage) {
			InitializeComponent();
			Lan.F(this);
			//This needs to remain a shallow copy because FormEServicesECR is expecting shallow copy changes only. Making a new instance would break that.
			ApptReminderRuleCur=apptReminderCur;
			ListRulesNonDefault=listRulesNonDefault;
			_selectedLanguageLoading=selectedLanguage;
		}

		private void FormApptReminderRuleEdit_Load(object sender,EventArgs e) {
			UserControlReminderAgg defaultAggControl=new UserControlReminderAgg(ApptReminderRuleCur);
			defaultAggControl.Dock=DockStyle.Fill;
			defaultAggControl.Anchor=((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom);
			if(ListRulesNonDefault.Count==0) {
				tabControl1.Visible=false;
				Controls.Add(defaultAggControl);
				defaultAggControl.Location=new System.Drawing.Point(12,2);
			}
			else {
				tabPageDefault.Controls.Add(defaultAggControl);
				defaultAggControl.Dock=DockStyle.Fill;
			}
			foreach(ApptReminderRule languageRule in ListRulesNonDefault) {
				TabPage languageTab=new TabPage();
				CultureInfo culture=MiscUtils.GetCultureFromThreeLetter(languageRule.Language);
				if(culture==null) {
					languageTab.Text=languageRule.Language;
				}
				else {
					languageTab.Text=culture.DisplayName;
				}
				UserControlReminderAgg languageAggControl=new UserControlReminderAgg(languageRule);
				languageAggControl.Anchor=defaultAggControl.Anchor;
				languageAggControl.Dock=DockStyle.Fill;
				languageTab.Controls.Add(languageAggControl);
				tabControl1.TabPages.Add(languageTab);
				if(languageRule.Language==_selectedLanguageLoading) {
					tabControl1.SelectedTab=languageTab;
				}
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(ListRulesNonDefault.Count==0) {
				List<string> listErrors=Controls.OfType<UserControlReminderAgg>().First().ValidateTemplates();
				if(listErrors.Count!=0) {
					MessageBox.Show(Lan.g(this,"You must fix the following errors before continuing.")+"\r\n\r\n-"+string.Join("\r\n-",listErrors));
					return;
				}
				Controls.OfType<UserControlReminderAgg>().First().SaveControlTemplates();
			}
			else {
				foreach(TabPage page in tabControl1.TabPages) {
					UserControlReminderAgg aggControl=(UserControlReminderAgg)page.Controls[0];
					List<string> listErrors=aggControl.ValidateTemplates();
					if(listErrors.Count!=0) {
						MessageBox.Show(Lan.g(this,"You must fix the following errors before continuing.")+"\r\n\r\n-"+string.Join("\r\n-",listErrors));
						return;
					}
					aggControl.SaveControlTemplates();
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}