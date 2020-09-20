using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental
{
    class AutoNoteL
	{
		public static void FillTreeView(TreeView autoNotesTreeView, string expandedAutoNoteCategories, TreeNode prevTopTreeNode = null)
		{
			var autoNoteCategories = AutoNoteCategories.GetAll();
			var autoNotes = AutoNotes.GetAll();

			var expandedAutoNoteCategoryIds = new List<long>();
            try
            {
				expandedAutoNoteCategoryIds.AddRange(
					expandedAutoNoteCategories.Split(',').Select(str => long.Parse(str)));
			}
            catch
            {
            }

			static TreeNode CreateAutoNoteCategoryTreeNode(AutoNoteCategory autoNoteCategory)
			{
				return new TreeNode(autoNoteCategory.Description)
				{
					Tag = autoNoteCategory,
					ImageIndex = 0,
					SelectedImageIndex = 0
				};
			}

			static TreeNode CreateAutoNoteTreeNode(AutoNote autoNote)
            {
				return new TreeNode(autoNote.Name)
				{
					Tag = autoNote,
					ImageIndex = 1,
					SelectedImageIndex = 1
				};
			}

			void CreateAutoNoteCategoryTreeNodes(TreeNodeCollection treeNodeCollection, AutoNoteCategory parentCategory)
            {
				foreach (var autoNoteCategory in autoNoteCategories.Where(anc => anc.ParentId == parentCategory.Id))
				{
					var treeNode = CreateAutoNoteCategoryTreeNode(autoNoteCategory);

					treeNodeCollection.Add(treeNode);

					CreateAutoNoteCategoryTreeNodes(treeNode.Nodes, autoNoteCategory);

					if (expandedAutoNoteCategoryIds.Contains(autoNoteCategory.Id))
					{
						treeNode.Expand();
					}
				}

				foreach (var autoNote in autoNotes.Where(an => an.AutoNoteCategoryId == parentCategory.Id))
				{
					treeNodeCollection.Add(CreateAutoNoteTreeNode(autoNote));
				}
			}

			autoNotesTreeView.BeginUpdate();
			autoNotesTreeView.Nodes.Clear();

			foreach (var autoNoteCategory in autoNoteCategories.Where(anc => anc.ParentId == null))
            {
				var treeNode = CreateAutoNoteCategoryTreeNode(autoNoteCategory);

				CreateAutoNoteCategoryTreeNodes(treeNode.Nodes, autoNoteCategory);

				if (expandedAutoNoteCategoryIds.Contains(autoNoteCategory.Id))
                {
					treeNode.Expand();
                }

				autoNotesTreeView.Nodes.Add(treeNode);
            }

			foreach (var autoNote in autoNotes.Where(an => an.AutoNoteCategoryId == null))
            {
				autoNotesTreeView.Nodes.Add(CreateAutoNoteTreeNode(autoNote));
            }

			autoNotesTreeView.EndUpdate();
			autoNotesTreeView.Focus();

			if (prevTopTreeNode != null)
            {
				static TreeNode FindTreeNode(TreeNodeCollection treeNodeCollection, Func<TreeNode, bool> predicate)
                {
					foreach (TreeNode treeNode in treeNodeCollection)
                    {
						if (predicate(treeNode)) return treeNode;

						var result = FindTreeNode(treeNode.Nodes, predicate);
						if (result != null)
                        {
							return result;
                        }
                    }

					return null;
                }

				TreeNode treeNode = null;
				if (prevTopTreeNode.Tag is AutoNoteCategory autoNoteCategory)
                {
					treeNode = FindTreeNode(autoNotesTreeView.Nodes, tn => tn.Tag is AutoNoteCategory anc && anc.Id == autoNoteCategory.Id);
                }
				else if (prevTopTreeNode.Tag is AutoNote autoNote)
                {
					treeNode = FindTreeNode(autoNotesTreeView.Nodes, tn => tn.Tag is AutoNote an && an.Id == autoNote.Id);
				}

				if (treeNode != null)
                {
					autoNotesTreeView.TopNode = treeNode;
                }
            }
		}

		public static void SetCollapsed(TreeView treeNotes, bool value)
		{
			TreeNode topNode = treeNotes.TopNode;
			TreeNode selectedNode = treeNotes.SelectedNode;

			treeNotes.BeginUpdate();

			if (value)
			{
				while (topNode.Parent != null)
				{//store the topNode's root to set the topNode after collapsing all nodes
					topNode = topNode.Parent;
				}
				while (selectedNode != null && selectedNode.Parent != null)
				{//store the selectedNode's root to select after collapsing
					selectedNode = selectedNode.Parent;
				}
				treeNotes.CollapseAll();
			}
			else
			{
				treeNotes.ExpandAll();
			}

			treeNotes.EndUpdate();
			if (selectedNode == null)
			{
				treeNotes.TopNode = topNode;//set TopNode if there is no SelectedNode
			}
			else
			{//reselect the node and ensure that it is visible after expanding or collapsing
				treeNotes.SelectedNode = selectedNode;
				treeNotes.SelectedNode.EnsureVisible();
				treeNotes.Focus();
			}
		}
	}
}
