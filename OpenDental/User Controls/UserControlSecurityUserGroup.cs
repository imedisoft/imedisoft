using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.ComponentModel;
using CodeBase;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental {
	///<summary>This control contains the User and User Group edit tabs for use in FormSecurity 
	///and FormCentralSecurity so that any changes need not be made in multiple places.
	///The implementing class should handle all the "Security Events" listed in the designer.</summary>
	public partial class UserControlSecurityUserGroup:UserControl {
		#region Private Variables
		///<summary>When true, various selection and click methods will not be executed. 
		///Set to true when loading/filling lists/grids.
		/// If this is set to true, ALWAYS set it back to false when you are done.</summary>
		private bool _isFillingList;
		///<summary>Used to filter the list of users shown in the "Users" tab.</summary>
		private Dictionary<long,Provider> _dictProvNumProvs;
		///<summary>The currently selected user. Should not be used except for in the selected user property.</summary>
		private Userod _selectedUser;
		///<summary>This context menu makes it so you can right click a user in the userGrid and select 'Copy User'</summary>
		private ContextMenu contextMenuUsers;
		#endregion
		#region Public Variables/Events
		///<summary>The form that implements this control should use their own Add and Edit User/UserGroup forms.</summary>
		public delegate void SecurityTabsEventHandler(object sender,SecurityEventArgs e);
		///<summary>An eventhandler that returns a DialogResult, so that the form that implements this security tree 
		///can use their own Report Permission and Group Permission forms. The result of this event is passed into the Security Tree.</summary>
		public delegate DialogResult SecurityTreeEventHandler(object sender,SecurityEventArgs e);
		[Category("Security Events"), Description("Occurs when the Add User button is clicked.")]
		public event SecurityTabsEventHandler AddUserClick = null;
		[Category("Security Events"), Description("Occurs when the Copy User button is clicked.")]
		public event SecurityTabsEventHandler CopyUserClick = null;
		[Category("Security Events"), Description("Occurs when the Edit User button is clicked.")]
		public event SecurityTabsEventHandler EditUserClick = null;
		[Category("Security Events"), Description("Occurs when the Add User Group button is clicked.")]
		public event SecurityTabsEventHandler AddUserGroupClick = null;
		[Category("Security Events"), Description("Occurs when the Edit User Group button is clicked.")]
		public event SecurityTabsEventHandler EditUserGroupClick = null;
		[Category("Security Events"), Description("Occurs when the Report Permission is checked.")]
		public event SecurityTreeEventHandler ReportPermissionChecked = null;
		[Category("Security Events"), Description("Occurs when a date-editable Group Permission is checked.")]
		public event SecurityTreeEventHandler GroupPermissionChecked = null;
		#endregion
		#region Properties
		#region Public
		[Category("Security Properties"), Description("Set to true when this user control is used in the CEMT tool. "+
			"When true, includes CEMT users and user groups and hides the User Filters.")]
		public bool IsForCEMT {
			get;
			set;
		}

		///<summary>The user selected in the "Users" tab. Setting the SelectedUser to null or a user that does not exist in the listbox does nothing.</summary>
		public Userod SelectedUser {
			get {
				return _selectedUser;
			}
			set {
				_selectedUser=value;
				gridUsers.SetSelected(false);
				butCopyUser.Enabled=(_selectedUser!=null);
				if(_selectedUser==null) {
					labelUserCurr.Text="No User Selected";
					return;
				}
				labelUserCurr.Text=_selectedUser.UserName;
				for(int i=0;i<gridUsers.Rows.Count;i++){
					if(((Userod)(gridUsers.Rows[i].Tag)).Id==_selectedUser.Id) {
						gridUsers.SetSelected(i,true);
						break;
					}
				}
			}
		}

		///<summary>The usergroup selected in the "User Groups" tab. 
		///Setting the SelectedUserGroup to null or a usergroup that does not exist in the listbox does nothing.</summary>
		public UserGroup SelectedUserGroup {
			get { return ((ODBoxItem<UserGroup>)listUserGroupTabUserGroups.SelectedItem)?.Tag; }
			set {
				if(value == null) {
					return;
				}
				foreach(ODBoxItem<UserGroup> boxItemUserGroupCur in listUserGroupTabUserGroups.Items) {
					if(boxItemUserGroupCur.Tag.Id == value.Id) {
						listUserGroupTabUserGroups.SelectedItem=boxItemUserGroupCur;
						break;
					}
				}
			}
		}
		#endregion
		#endregion

		public UserControlSecurityUserGroup() {
			InitializeComponent();
		}

		private void UserControlUserGroupSecurity_Load(object sender,EventArgs e) {
			if(CopyUserClick!=null) {//CEMT currently does not define/allow a user to be copied.
				contextMenuUsers.MenuItems.Add("Copy User",new EventHandler(butCopyUser_Click));
				gridUsers.ContextMenu=contextMenuUsers;
				butCopyUser.Visible=true;//False by default
			}
			if(IsForCEMT) {
				groupBox2.Visible=false;
				gridUsers.Bounds=new Rectangle(gridUsers.Bounds.X,securityTreeUser.Bounds.Y,gridUsers.Bounds.Width,securityTreeUser.Bounds.Height);
				int gridUsersRight=gridUsers.Bounds.X+gridUsers.Width;
				panelUserGroups.Bounds=new Rectangle(gridUsersRight,gridUsers.Bounds.Y,securityTreeUser.Bounds.X-gridUsersRight,gridUsers.Bounds.Height);
			}
			if(!this.DesignMode) {
				securityTreeUser.FillTreePermissionsInitial();
				#region Load Users Tab
				FillFilters();
				FillGridUsers();
				RefreshUserTabGroups();
				#endregion
				#region Load UserGroups Tab
				securityTreeUserGroup.FillTreePermissionsInitial();
				FillListUserGroupTabUserGroups();
				securityTreeUserGroup.FillForUserGroup(SelectedUserGroup.Id);
				FillAssociatedUsers();
				#endregion
			}
		}

		#region User Tab Methods
		///<summary>Fills the filter comboboxes on the "Users" tab.</summary>
		private void FillFilters() {
			foreach(UserFilters filterCur in Enum.GetValues(typeof(UserFilters))) {
				if(Prefs.GetBool(PrefName.EasyHideDentalSchools) && (filterCur == UserFilters.Students || filterCur == UserFilters.Instructors)) {
					continue;
				}
				comboShowOnly.Items.Add(new ODBoxItem<UserFilters>(filterCur.GetDescription(),filterCur));
			}
			comboShowOnly.SelectedIndex=0;
			comboSchoolClass.Items.Add(new ODBoxItem<SchoolClass>("All"));
			comboSchoolClass.SelectedIndex=0;
			foreach(SchoolClass schoolClassCur in SchoolClasses.GetAll()) {
				comboSchoolClass.Items.Add(new ODBoxItem<SchoolClass>(SchoolClasses.GetDescription(schoolClassCur),schoolClassCur));
			}
			if(PrefC.HasClinicsEnabled) {
				comboClinic.Visible=true;
				labelClinic.Visible=true;
				comboClinic.Items.Clear();
				comboClinic.Items.Add(new ODBoxItem<Clinic>("All Clinics"));
				comboClinic.SelectedIndex=0;
				foreach(Clinic clinicCur in Clinics.GetAll(false)) {
					comboClinic.Items.Add(new ODBoxItem<Clinic>(clinicCur.Abbr,clinicCur));
				}
			}
			comboGroups.Items.Clear();
			comboGroups.Items.Add(new ODBoxItem<UserGroup>("All Groups"));
			comboGroups.SelectedIndex=0;
			foreach(UserGroup groupCur in UserGroups.GetList(IsForCEMT)) {
				comboGroups.Items.Add(new ODBoxItem<UserGroup>(groupCur.Description,groupCur));
			}
		}

		///<summary>Returns a filtered list of userods that should be displayed. Returns all users when IsCEMT is true.</summary>
		private List<Userod> GetFilteredUsersHelper() {
			List<Userod> retVal = Userods.GetAll();
			if(IsForCEMT) {
				return retVal;
			}
			if(_dictProvNumProvs == null) { //fill the dictionary if needed
				_dictProvNumProvs=Providers.GetMultProviders(Userods.GetAll().Select(x => x.ProviderId ?? 0).ToList()).ToDictionary(x => x.Id,x => x);
			}
			retVal.RemoveAll(x => x.UserNumCEMT>0);//NEVER show CEMT users when not in the CEMT tool.
			if(!checkShowHidden.Checked) {
				retVal.RemoveAll(x => x.IsHidden);
			}
			long classNum = 0;
			if(comboSchoolClass.Visible && comboSchoolClass.SelectedIndex>0) {
				classNum=((ODBoxItem<SchoolClass>)comboSchoolClass.SelectedItem).Tag.Id;
			}
			switch(((ODBoxItem<UserFilters>)comboShowOnly.SelectedItem).Tag) {
				case UserFilters.Employees:
					retVal.RemoveAll(x => x.EmployeeId==0);
					break;
				case UserFilters.Providers:
					retVal.RemoveAll(x => x.ProviderId==0);
					break;
				case UserFilters.Students:
					//might not count user as student if attached to invalid providers.
					retVal.RemoveAll(x => !_dictProvNumProvs.ContainsKey(x.ProviderId ?? 0) || _dictProvNumProvs[x.ProviderId ?? 0].IsInstructor);
					if(classNum>0) {
						retVal.RemoveAll(x => _dictProvNumProvs[x.ProviderId ?? 0].SchoolClassId!=classNum);
					}
					break;
				case UserFilters.Instructors:
					retVal.RemoveAll(x => !_dictProvNumProvs.ContainsKey(x.ProviderId ?? 0) || !_dictProvNumProvs[x.ProviderId ?? 0].IsInstructor);
					if(classNum>0) {
						retVal.RemoveAll(x => _dictProvNumProvs[x.ProviderId ?? 0].SchoolClassId!=classNum);
					}
					break;
				case UserFilters.Other:
					retVal.RemoveAll(x => x.EmployeeId!=0 || x.ProviderId!=0);
					break;
				case UserFilters.AllUsers:
				default:
					break;
			}
			if(comboClinic.SelectedIndex>0) {
				retVal.RemoveAll(x => x.ClinicId!=((ODBoxItem<Clinic>)comboClinic.SelectedItem).Tag.Id);
			}
			if(comboGroups.SelectedIndex>0) {
				retVal.RemoveAll(x => !x.IsInUserGroup(((ODBoxItem<UserGroup>)comboGroups.SelectedItem).Tag.Id));
			}
			if(!string.IsNullOrWhiteSpace(textPowerSearch.Text)) {
				switch(((ODBoxItem<UserFilters>)comboShowOnly.SelectedItem).Tag) {
					case UserFilters.Employees:
						retVal.RemoveAll(x => !Employees.GetNameFL(x.EmployeeId ?? 0).ToLower().Contains(textPowerSearch.Text.ToLower()));
						break;
					case UserFilters.Providers:
					case UserFilters.Students:
					case UserFilters.Instructors:
						retVal.RemoveAll(x => !_dictProvNumProvs[x.ProviderId ?? 0].GetLongDesc().ToLower().Contains(textPowerSearch.Text.ToLower()));
						break;
					case UserFilters.AllUsers:
					case UserFilters.Other:
					default:
						retVal.RemoveAll(x => !x.UserName.ToLower().Contains(textPowerSearch.Text.ToLower()));
						break;
				}
			}
			return retVal;
		}

		///<summary>Refreshes the security tree in the "Users" tab.</summary>
		private void RefreshUserTree() {
			securityTreeUser.FillForUserGroup(listUserTabUserGroups.SelectedItems.OfType<ODBoxItem<UserGroup>>().Select(x => x.Tag.Id).ToList());
		}

		///<summary>Refreshes the UserGroups list box on the "User" tab. Also refreshes the security tree. 
		///Public so that it can be called from the Form that implements this control.</summary>
		public void RefreshUserTabGroups() {
			List<UserGroup> listUserGroups=(SelectedUser==null) ? UserGroups.GetList(IsForCEMT) : SelectedUser.GetGroups(IsForCEMT);
			_isFillingList=true;
			listUserTabUserGroups.Items.Clear();
			for(int i=0;i<listUserGroups.Count;i++) {
				listUserTabUserGroups.Items.Add(new ODBoxItem<UserGroup>(listUserGroups[i].Description,listUserGroups[i]));
				if(SelectedUser!=null) {
					listUserTabUserGroups.SetSelected(i,true);
				}
			}
			_isFillingList=false;
			//RefreshTree takes a while (it has to draw many images) so this is to show the usergroup selections before loading the tree.
			Application.DoEvents();
			RefreshUserTree();
		}

		private void listUserTabUserGroups_SelectedIndexChanged(object sender,EventArgs e) {
			if(_isFillingList) {
				return;
			}
			RefreshUserTree();
		}

		private void comboShowOnly_SelectionIndexChanged(object sender,EventArgs e) {
			string filterType;
			switch(((ODBoxItem<UserFilters>)comboShowOnly.SelectedItem).Tag) {
				case UserFilters.Employees:
					filterType="Employee Name";
					break;
				case UserFilters.Providers:
				case UserFilters.Students:
				case UserFilters.Instructors:
					filterType="Provider Name";
					break;
				case UserFilters.AllUsers:
				case UserFilters.Other:
				default:
					filterType="Username";
					break;
			}
			if(((ODBoxItem<UserFilters>)comboShowOnly.SelectedItem).Tag==UserFilters.Students) {
				labelSchoolClass.Visible=true;
				comboSchoolClass.Visible=true;
			}
			else {
				labelSchoolClass.Visible=false;
				comboSchoolClass.Visible=false;
			}
			labelFilterType.Text=filterType;
			textPowerSearch.Text=string.Empty;
			FillGridUsers();
		}

		private void Filter_Changed(object sender,EventArgs e) {
			FillGridUsers();
		}

		///<summary>Fills gridUsers. Public so that it can be called from the Form that implements this control.</summary>
		public void FillGridUsers() {
			_isFillingList=true;
			Userod selectedUser=SelectedUser;//preserve user selection.
			gridUsers.BeginUpdate();
			gridUsers.Columns.Clear();
			gridUsers.Columns.Add(new GridColumn("Username",90));
			gridUsers.Columns.Add(new GridColumn("Employee",90));
			gridUsers.Columns.Add(new GridColumn("Provider",90));
			if(PrefC.HasClinicsEnabled) {
				gridUsers.Columns.Add(new GridColumn("Clinic",80));
				gridUsers.Columns.Add(new GridColumn("Clinic\r\nRestr",38,HorizontalAlignment.Center));
			}
			gridUsers.Columns.Add(new GridColumn("Strong\r\nPwd",45,HorizontalAlignment.Center));
			gridUsers.Rows.Clear();
			List<Userod> listFilteredUsers=GetFilteredUsersHelper();
			foreach(Userod user in listFilteredUsers) {
				GridRow row=new GridRow();
				row.Cells.Add(user.UserName);
				row.Cells.Add(Employees.GetNameFL(user.EmployeeId ?? 0));
				row.Cells.Add(Providers.GetLongDesc(user.ProviderId ?? 0));
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(user.ClinicId ?? 0));
					row.Cells.Add(user.ClinicIsRestricted?"X":"");
				}
				row.Cells.Add(user.PasswordIsStrong?"X":"");
				row.Tag=user;
				gridUsers.Rows.Add(row);
			}
			gridUsers.EndUpdate();
			_isFillingList=false;//Done filling the grid.
			//Selection logic has to occur after ODGrid.EndUpdate().
			if(selectedUser!=null) {
				//Reselect previously selected user.  SelectedUser is allowed to be null (ex. on load).
				SelectedUser=listFilteredUsers.FirstOrDefault(x => x.Id==selectedUser.Id);
			}
			RefreshUserTabGroups();
		}

		private void butAddUser_Click(object sender,EventArgs e) {
			if(_isFillingList) {
				return;
			}
			//Call an event that bubbles back up to the calling Form.
			AddUserClick?.Invoke(this,new SecurityEventArgs(new Userod()));
		}

		private void butCopyUser_Click(object sender,EventArgs e) {//Not visible by default
			if(_isFillingList) {
				return;
			}
			//Call an event that bubbles back up to the calling Form.
			CopyUserClick?.Invoke(sender,new SecurityEventArgs(SelectedUser));
		}

		private void gridUsers_CellClick(object sender,ODGridClickEventArgs e) {
			if(_isFillingList) {
				return;
			}
			SelectedUser=gridUsers.SelectedTag<Userod>();
			//Refresh the selected groups and the security tree
			RefreshUserTabGroups();
		}

		private void gridUsers_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(_isFillingList) {
				return;
			}
			//Call an event that bubbles back up to the calling Form.
			EditUserClick?.Invoke(this,new SecurityEventArgs(SelectedUser));
		}

		#endregion

		#region UserGroup Tab Methods
		///<summary>Fills listUserGroupTabUserGroups. Public so that it can be called from the Form that implements this control.</summary>
		public void FillListUserGroupTabUserGroups() {
			_isFillingList=true;
			UserGroup selectedGroup = SelectedUserGroup; //Preserve Usergroup selection.
			listUserGroupTabUserGroups.Items.Clear();
			foreach(UserGroup groupCur in UserGroups.GetList(IsForCEMT)) {
				ODBoxItem<UserGroup> boxItemCur = new ODBoxItem<UserGroup>(groupCur.Description,groupCur);
				listUserGroupTabUserGroups.Items.Add(boxItemCur);
				if(selectedGroup != null && groupCur.Id == selectedGroup.Id) {
					listUserGroupTabUserGroups.SelectedItem = boxItemCur;
				}
			}
			_isFillingList=false;
			if(listUserGroupTabUserGroups.SelectedItem == null) {
				listUserGroupTabUserGroups.SetSelected(0,true);
			}
		}

		///<summary>Fills listAssociatedUsers, which displays the users that are currently associated to the selected usergroup.
		///This also dynamically sets the height of the control.</summary>
		private void FillAssociatedUsers() {
			listAssociatedUsers.Items.Clear();
			List<Userod> listUsers = Userods.GetForGroup(SelectedUserGroup.Id);
			foreach(Userod userCur in listUsers) {
				listAssociatedUsers.Items.Add(new ODBoxItem<Userod>(userCur.UserName,userCur));
			}
			if(listAssociatedUsers.Items.Count == 0) {
				listAssociatedUsers.Items.Add(new ODBoxItem<Userod>("None"));
			}
		}

		private void listUserGroupTabUserGroups_SelectedIndexChanged(object sender,EventArgs e) {
			if(_isFillingList) {
				return;
			}
			securityTreeUserGroup.FillForUserGroup(((ODBoxItem<UserGroup>)listUserGroupTabUserGroups.SelectedItem).Tag.Id);
			FillAssociatedUsers();
		}

		private void butAddGroup_Click(object sender,EventArgs e) {
			//Call an event that bubbles back up to the calling Form.
			AddUserGroupClick?.Invoke(this,new SecurityEventArgs(new UserGroup()));
		}

		private void butEditGroup_Click(object sender,EventArgs e) {
			if(listUserGroupTabUserGroups.SelectedIndex==-1) {
				MessageBox.Show("Please select a User Group to edit.");
				return;
			}
			//Call an event that bubbles back up to the calling Form.
			EditUserGroupClick?.Invoke(this,new SecurityEventArgs(SelectedUserGroup));
		}

		private void listUserGroupTabUserGroups_DoubleClick(object sender,EventArgs e) {
			//Call an event that bubbles back up to the calling Form.
			EditUserGroupClick?.Invoke(this,new SecurityEventArgs(SelectedUserGroup));
		}

		private void butSetAll_Click(object sender,EventArgs e) {
			securityTreeUserGroup.SetAll();
		}
		#endregion

		///<summary>We need to refresh the selected tab to display updated information.</summary>
		private void tabControlMain_SelectedIndexChanged(object sender,EventArgs e) {
			if(tabControlMain.SelectedTab == tabPageUsers) {
				FillGridUsers();
				RefreshUserTabGroups(); //a usergroup could have been added, so refresh.
			}
			else if(tabControlMain.SelectedTab == tabPageUserGroups) {
				FillAssociatedUsers(); //the only thing that could have changed are the users associated to the user groups.
			}
		}

		private DialogResult securityTreeUserGroup_ReportPermissionChecked(object sender,SecurityEventArgs e) {
			return ReportPermissionChecked?.Invoke(sender,e)??DialogResult.Cancel;
		}

		private enum UserFilters{
			[Description("All Users")]
			AllUsers=0,
			Providers,
			Employees,
			Students,
			Instructors,
			Other,
		}

		private DialogResult securityTreeUserGroup_GroupPermissionChecked(object sender,SecurityEventArgs e) {
			return GroupPermissionChecked?.Invoke(sender,e)??DialogResult.Cancel;
		}
	}

	///<summary>A rather generic EventArgs class that can contain specific Security Object types (Userod, UserGroup, or GroupPermission).</summary>
	public class SecurityEventArgs {
		public Userod User {
			get;
		}
		public UserGroup Group {
			get;
		}
		public GroupPermission Perm {
			get;
		}

		public SecurityEventArgs(Userod user) {
			User=user;
		}

		public SecurityEventArgs(UserGroup userGroup) {
			Group=userGroup;
		}

		public SecurityEventArgs(GroupPermission perm) {
			Perm=perm;
		}

	}
}
