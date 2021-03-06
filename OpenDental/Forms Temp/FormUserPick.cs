using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormUserPick:ODForm {
		///<summary>The filtered list of Users to pick from.</summary>
		public List<User> ListUserodsFiltered;
		public List<User> ListUserodsShowing;
		///<summary>If this form closes with OK, then this value will be filled.</summary>
		public long SelectedUserNum;
		///<summary>When IsMultiSelect, this is the list of selected users after the OK click.</summary>
		public List<long> ListSelectedUserNums=new List<long>();
		///<summary>If provided, this usernum will be preselected if it is also in the list of available usernums.</summary>
		public long SuggestedUserNum=0;
		///<summary>When IsMultiSelect, these usernums will be preselected if it is also in the list of available usernums.</summary>
		public List<long> ListSuggestedUserNums=new List<long>();
		public bool IsSelectionmode;
		public bool IsShowAllAllowed;
		///<summary>Will return 0 for SelectedUserNum if the None 
		public bool IsPickNoneAllowed;
		///<summary>Will return -1 for SelectedUserNum if the All 
		public bool IsPickAllAllowed;
		///<summary>Will return the currently logged in user as the SelectedUserNum
		public bool IsPickMeAllowed;
		///<summary>Set true when we want to allow multiple user selections.  When true uses ListSelectedUsers</summary>
		private bool _isMultiSelect;

		public FormUserPick(bool isMultiSelect=false) {
			InitializeComponent();
			
			_isMultiSelect=isMultiSelect;
		}

		private void FormUserPick_Load(object sender,EventArgs e) {
			if(IsShowAllAllowed && ListUserodsFiltered!=null && ListUserodsFiltered.Count>0) {
				butShow.Visible=true;
			}
			if(IsPickAllAllowed) {
				butAll.Visible=true;
			}
			if(IsPickNoneAllowed) {
				butNone.Visible=true;
			}
			if(IsPickMeAllowed) {
				butMe.Visible=true;
			}
			if(!butNone.Visible && !butAll.Visible) {
				groupSelect.Visible=false;
			}
			if(_isMultiSelect) {
				listUser.SelectionMode=SelectionMode.MultiExtended;
				Text="Pick Users";
			}
			FillList(ListUserodsFiltered);
		}

		private void FillList(List<User> listUserods) {
			if(listUserods==null) {
				listUserods=Users.GetAll(true);
			}
			ListUserodsShowing=listUserods.Select(x => x.Copy()).ToList();
			listUserods.ForEach(x => listUser.Items.Add(x));
			if(_isMultiSelect) {
				foreach(long userNum in ListSuggestedUserNums) {
					int index=listUserods.FindIndex(x => x.Id==userNum);
					listUser.SetSelected(index,true);
				}
			}
			else { 
				listUser.SelectedIndex=listUserods.FindIndex(x => x.Id==SuggestedUserNum);
			}
		}

		private void listUser_DoubleClick(object sender,EventArgs e) {
			if(listUser.SelectedIndex==-1) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.TaskEdit,true) && Users.GetInboxTaskList(ListUserodsShowing[listUser.SelectedIndex].Id)!=0 && !IsSelectionmode) {
				MessageBox.Show("Please select a user that does not have an inbox.");
				return;
			}
			SelectedUserNum=ListUserodsShowing[listUser.SelectedIndex].Id;
			foreach(int index in listUser.SelectedIndices) {
				ListSelectedUserNums.Add(ListUserodsShowing[index].Id);
			}
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(listUser.SelectedIndex==-1) {
				MessageBox.Show("Please pick a user first.");
				return;
			}
			if(!IsSelectionmode && !Security.IsAuthorized(Permissions.TaskEdit,true) && Users.GetInboxTaskList(ListUserodsShowing[listUser.SelectedIndex].Id)!=0) {
				MessageBox.Show("Please select a user that does not have an inbox.");
				return;
			}
			SelectedUserNum=ListUserodsShowing[listUser.SelectedIndex].Id;
			foreach(int index in listUser.SelectedIndices) {
				ListSelectedUserNums.Add(ListUserodsShowing[index].Id);
			}
			DialogResult=DialogResult.OK;
		}

		private void butAll_Click(object sender,EventArgs e) {
			SelectedUserNum=-1;
			ListSelectedUserNums=ListUserodsShowing.Select(x => x.Id).ToList();
			DialogResult=DialogResult.OK;
		}

		private void butNone_Click(object sender,EventArgs e) {
			SelectedUserNum=0;
			ListSelectedUserNums=new List<long>() { };
			DialogResult=DialogResult.OK;
		}

		private void butMe_Click(object sender,EventArgs e) {
			SelectedUserNum=Security.CurrentUser.Id;
			ListSelectedUserNums=new List<long>() { };
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butShow_Click(object sender,EventArgs e) {
			SelectedUserNum=0;
			if(Text=="Show All") {
				Text="Show Filtered";
				FillList(null);
			}
			else {
				Text="Show All";
				FillList(ListUserodsFiltered);
			}
		}
	}
}