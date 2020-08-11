using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormTaskListSelect : FormBase
	{
		/// <summary>
		/// Gets the selected task list.
		/// </summary>
		public TaskList SelectedList 
			=> taskListsTreeView.SelectedNode?.Tag as TaskList;

		/// <summary>
		/// Initializes a new instance of the <see cref="FormTaskListSelect"/> class.
		/// </summary>
		public FormTaskListSelect() 
			=> InitializeComponent();

		private void FormTaskListSelect_Load(object sender, EventArgs e)
		{
			var taskLists = TaskLists.GetAll();

            static TreeNode CreateTreeNode(TaskList taskList)
            {
				return new TreeNode(taskList.Description)
				{
					Tag = taskList
				};
            }

			void CreateTreeNodes(TreeNodeCollection treeNodeCollection, IEnumerable<TaskList> taskLists)
            {
				foreach (var taskList in taskLists)
                {
					var treeNode = CreateTreeNode(taskList);

					CreateTreeNodes(treeNode.Nodes, taskLists.Where(tl => tl.ParentId == taskList.Id));

					if (Security.CurrentUser.InboxTaskListId == taskList.Id)
                    {
						taskListsTreeView.SelectedNode = treeNode;
					}
                }
            }

			CreateTreeNodes(taskListsTreeView.Nodes, taskLists.Where(tl => !tl.ParentId.HasValue));

			taskListsTreeView.ExpandAll();
        }

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (SelectedList == null)
			{
				ShowError("Please select a task list first.");

				return;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
