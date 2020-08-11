using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormTaskListBlocks : FormBase
	{
		///<summary>A List of the task lists that the current user wants to block pop ups for.  Filled on load.</summary>
		//private List<UserOdPref> _listUserOdPrefTaskListBlocks;
		//private List<UserOdPref> _listUserDBPrefs;
		///<summary>Dictionary to hold changed task list subscriptions.</summary>
		//private Dictionary<long,UserOdPref> _dictBlockedTaskPrefs=new Dictionary<long, UserOdPref>();
		private Dictionary<long, TaskList> _dictAllTaskLists = new Dictionary<long, TaskList>();

		/// <summary>
		/// Set to true when settings the checkmarks of parents so we don't roll back down through children for each parent recursivly.
		/// </summary>
		private bool _isCheckingParents = false;

		/// <summary>
		/// Loads up a list of task lists that the currently logged in user is subscribed to.
		/// Allows the user to selectivly block task lists that they are subscribed to.
		/// </summary>
		public FormTaskListBlocks()
		{
			InitializeComponent();
		}

		private void FormTaskListBlock_Load(object sender, EventArgs e)
		{
			_dictAllTaskLists = TaskLists.GetAll().ToDictionary(x => x.Id);

			InitializeTree();
		}

		/// <summary>
		/// Fetches the subscriptions that the user is currently subscribed too ad adds them to the treeview.
		/// </summary>
		private void InitializeTree()
		{
			subscriptionsTreeView.Nodes.Clear();
			subscriptionsTreeView.ExpandAll();
		}

		private void SetTreeNodeCollectionChecked(TreeNodeCollection treeNodeCollection, bool isChecked)
        {
			foreach (TreeNode treeNode in treeNodeCollection)
            {
				treeNode.Checked = isChecked;

				SetTreeNodeCollectionChecked(treeNode.Nodes, isChecked);
            }
        }

		/// <summary>
		/// Start from the roots of the tree, and work toward leaves.
		/// Sets the node to checked if needed.
		/// </summary>
		private void SetCheckBoxes(TreeNode node)
		{
			node.Checked = false;//Unchecked if no block exists yet.
								 //if(_listUserOdPrefTaskListBlocks.Exists(x => x.Fkey==(long)node.Tag && PIn.Bool(x.ValueString))) {
								 //	node.Checked=true;
								 //}
								 //Deal with children

			foreach (TreeNode child in node.Nodes)
			{
				SetCheckBoxes(child);
			}
		}

		/// <summary>
		/// Handles the user clicking on the node text to activate the checkbox
		/// </summary>
		private void treeSubscriptions_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.X - e.Node.Bounds.X > 0 && e.X < e.Node.Bounds.Width + e.Node.Bounds.X)
			{
				e.Node.Checked = !e.Node.Checked;
			}
		}

		/// <summary>
		/// Is activated when a node is checked or unchecked.
		/// </summary>
		private void treeSubscriptions_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (_isCheckingParents)
			{
				return;
			}

			bool nodeChecked = e.Node.Checked;
			foreach (TreeNode branch in e.Node.Nodes)
			{
				if (branch.Checked != nodeChecked)
				{
					branch.Checked = nodeChecked;   //Activates their own AfterCheck event
				}
			}

			//If all children are checked, the parent should be checked too.
			//We'll work our way up the list.
			if (e.Node.Parent == null)
			{
				return;
			}

			//Lock children from being iterated through because of parent changes
			_isCheckingParents = true;
			nodesCheckUp(e.Node.Parent);
			_isCheckingParents = false;
		}

		/// <summary>
		/// Works it's way up from a given node, checking the parent if all it's children are checked.
		///	Be sure to set isCheckingParents to true so the treeSubscriptions_AfterCheck isn't triggered
		/// </summary>
		private void nodesCheckUp(TreeNode curNode)
		{
			bool allChildrenChecked = true;

			foreach (TreeNode child in curNode.Nodes)
			{
				if (child.Checked == false)
				{
					allChildrenChecked = false;
					break;
				}
			}

			//isCheckingParents should be true, so AfterCheck shouldn't be triggered for children
			curNode.Checked = allChildrenChecked;
			if (curNode.Parent != null)
			{
				nodesCheckUp(curNode.Parent);   //Recursion
			}
		}

		private void SelectAllButton_Click(object sender, EventArgs e)
		{
			SetTreeNodeCollectionChecked(subscriptionsTreeView.Nodes, true);
		}

		private void SelectNoneButton_Click(object sender, EventArgs e)
		{
			SetTreeNodeCollectionChecked(subscriptionsTreeView.Nodes, false);
		}

		/// <summary>
		/// Goes through tree and sets up changes to the TaskList block preferences dictionary.
		/// </summary>
		private void SetDictPrefsRecursive(TreeNode node)
		{
			//foreach(TreeNode child in node.Nodes) {
			//	SetDictPrefsRecursive(child);	//Recursion
			//}
			////Create preference
			//UserOdPref pref=new UserOdPref();
			//pref.Fkey=(long)node.Tag;
			//pref.FkeyType=UserOdFkeyType.TaskListBlock;
			//pref.UserNum=Security.CurrentUser.Id;
			//pref.ValueString=POut.Bool(node.Checked);
			////Add preference to dictionary of preferences
			//_dictBlockedTaskPrefs[(long)node.Tag]=pref;
		}

		/// <summary>
		/// Gets the changed preferences for the tree, then updates the database with the changes.
		/// </summary>
		private void AcceptButton_Click(object sender, EventArgs e)
		{
			foreach (TreeNode node in subscriptionsTreeView.Nodes)
			{
				SetDictPrefsRecursive(node);
			}

			//Add new preferences and changes to database
			//foreach(UserOdPref editPref in _dictBlockedTaskPrefs.Values) {
			//	if(_listUserOdPrefTaskListBlocks.Exists(x => x.Fkey==editPref.Fkey)) {
			//		editPref.UserOdPrefNum=_listUserOdPrefTaskListBlocks.Find(x => x.Fkey==editPref.Fkey).UserOdPrefNum;
			//	}
			//}
			// TODO: UserOdPrefs.Sync(_dictBlockedTaskPrefs.Select(x => x.Value).ToList(),_listUserDBPrefs);

			DialogResult = DialogResult.OK;

			Close();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;

			Close();
		}
	}
}
