using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Linq;

namespace OpenDental {
	public partial class FormSecurity:ODForm {

		public FormSecurity() {
			InitializeComponent();
			Lan.F(this);
		}
		
		private void FormSecurityEdit_Load(object sender,EventArgs e) {
		}

		private void globalSecuritySettingsToolStripMenuItem_Click(object sender,EventArgs e) {
			FormGlobalSecurity FormGS = new FormGlobalSecurity();
			FormGS.ShowDialog();//no refresh needed; settings changed in FormGlobalSecurity have no bearing on what displays in this form.
		}

		private void userControlSecurityTabs_AddUserClick(object sender,SecurityEventArgs e) {
			Userod user = new Userod();
			FormUserEdit FormU = new FormUserEdit(user);
			FormU.IsNew=true;
			FormU.ShowDialog();
			if(FormU.DialogResult == DialogResult.OK) {//update to reflect changes that were made in FormUserEdit.
				userControlSecurityTabs.FillGridUsers();//New user is not in grid yet, add them.
				userControlSecurityTabs.SelectedUser=FormU.UserCur;//Selects the user that was just added in the grid.
				userControlSecurityTabs.RefreshUserTabGroups();//Previously selected users User Groups are still selected, refresh for UserCur.
			}
		}		

		private void UserControlSecurityTabs_CopyUserClick(object sender,SecurityEventArgs e) {
			//validation is handled here in GetUniqueUsername(...) 
			Userod user=e.User;
			if (user is null) {
				MsgBox.Show(Lan.G(this,"Please select a user."));
				return;
			}		
			if(!Userods.TryGetUniqueUsername(user.UserName+"(Copy)",0,false,false,out string newUserName)){//This should really never fail.
				MessageBox.Show("Could not generate a unique username.");
				return;
			}
			//New username format; user.UserName(copy)(X)
			FormUserPassword formPassword=new FormUserPassword(false,newUserName,isCopiedUser:true);
			formPassword.IsInSecurityWindow=true;//Do not show or validate current password UI since this is a new user.
			if(formPassword.ShowDialog()!=DialogResult.OK) {
				return;
			}
			Userod newUser=Userods.CopyUser(user,formPassword.LoginDetails,formPassword.PasswordIsStrong,newUserName);
			DataValid.SetInvalid(InvalidType.Security,InvalidType.UserClinics);//Must be called after Userods.CopyUser(...)
			userControlSecurityTabs.FillGridUsers();
			userControlSecurityTabs.SelectedUser=newUser;
			userControlSecurityTabs.RefreshUserTabGroups();
		}

		private void userControlSecurityTabs_EditUserClick(object sender,SecurityEventArgs e) {
			FormUserEdit FormUE = new FormUserEdit(e.User);
			FormUE.ShowDialog();
			if(FormUE.DialogResult == DialogResult.OK) {//update to reflect changes that were made in FormUserEdit.
				userControlSecurityTabs.FillGridUsers();
				userControlSecurityTabs.RefreshUserTabGroups();
			}
		}

		private void userControlSecurityTabs_AddUserGroupClick(object sender,SecurityEventArgs e) {
			UserGroup group = new UserGroup();
			FormUserGroupEdit FormU = new FormUserGroupEdit(group);
			FormU.IsNew=true;
			FormU.ShowDialog();
			if(FormU.DialogResult == DialogResult.OK) {
				userControlSecurityTabs.FillListUserGroupTabUserGroups();//update to reflect changes that were made in FormUserGroupEdit.
				userControlSecurityTabs.SelectedUserGroup=group;
			}
		}

		private void userControlSecurityTabs_EditUserGroupClick(object sender,SecurityEventArgs e) {
			FormUserGroupEdit FormU = new FormUserGroupEdit(e.Group);
			FormU.ShowDialog();
			if(FormU.DialogResult == DialogResult.OK) {
				userControlSecurityTabs.FillListUserGroupTabUserGroups();
			}
		}

		private DialogResult userControlSecurityTabs_ReportPermissionChecked(object sender,SecurityEventArgs e) {
			GroupPermission perm = e.Perm;
			FormReportSetup FormRS = new FormReportSetup(perm.UserGroupNum,true);
			FormRS.ShowDialog();
			if(FormRS.DialogResult==DialogResult.Cancel) {
				return FormRS.DialogResult;
			}
			if(!FormRS.HasReportPerms) {//Only insert base Reports permission if the user actually has any reports allowed
				return FormRS.DialogResult;
			}
			try {
				GroupPermissions.Insert(perm);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return DialogResult.Cancel;
			}
			return FormRS.DialogResult;
		}

		private DialogResult userControlSecurityTabs_GroupPermissionChecked(object sender,SecurityEventArgs e) {
			FormGroupPermEdit FormG = new FormGroupPermEdit(e.Perm);
			FormG.IsNew=true;
			FormG.ShowDialog();
			return FormG.DialogResult;
		}

		private void butOK_Click(object sender,EventArgs e) {
			Close();
		}

		private void FormSecurityEdit_FormClosing(object sender,FormClosingEventArgs e) {
			DataValid.SetInvalid(InvalidType.Security);
		}
	}
}