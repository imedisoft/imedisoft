using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using Imedisoft.Models.Dtos;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoNotes : FormBase
	{
		private readonly string expandedAutoNoteCategories;
		private TreeNode highlightTreeNode;

		/// <summary>
		/// Gets or sets a value indicating whether the form is in selection mode.
		/// </summary>
		public bool IsSelectionMode { get; set; }

		/// <summary>
		/// Gets the selected auto note.
		/// </summary>
		public AutoNote SelectedAutoNote => autoNotesTreeView.SelectedNode?.Tag as AutoNote;

		public FormAutoNotes()
		{
			InitializeComponent();

			imageListTree.Images.Add(Properties.Resources.IconFolderOpen);
			imageListTree.Images.Add(Properties.Resources.IconStickyNoteOpen);

			expandedAutoNoteCategories = UserPreference.GetString(UserPreferenceName.AutoNoteExpandedCats);
		}

		private void FormAutoNotes_Load(object sender, EventArgs e)
		{
			if (IsSelectionMode)
			{
				addButton.Visible = editButton.Visible = deleteButton.Visible 
					= false;
			}

			AutoNoteL.FillTreeView(autoNotesTreeView, expandedAutoNoteCategories);
		}

		private static OpenFileDialog InitializeOpenFileDialog()
		{
			return new OpenFileDialog
			{
				Multiselect = false,
				Filter = Translation.Common.JsonFiles + " (*.json)|*.json",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
			};
		}

		private static bool IsValidDestination(TreeNode treeNodeSource, TreeNode treeNodeDest)
		{
			if (treeNodeSource == null || treeNodeSource.Parent == treeNodeDest)
			{
				return false;
			}

			if (treeNodeSource.Tag is AutoNote)
			{
				return true;
			}

			if (treeNodeDest != null && treeNodeDest.FullPath.StartsWith(treeNodeSource.FullPath))
			{
				return false;
			}

			return true;
		}

		private void AutoNotesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			bool enabled = false;

			if (autoNotesTreeView.SelectedNode != null)
			{
				if (autoNotesTreeView.SelectedNode.Tag is AutoNote)
				{
					enabled = true;
				}
			}

			editButton.Enabled = deleteButton.Enabled = enabled;
		}

		private void AutoNotesTreeView_DragDrop(object sender, DragEventArgs e)
		{
			if (highlightTreeNode != null)
			{
				highlightTreeNode.BackColor = autoNotesTreeView.BackColor;
				highlightTreeNode = null;
			}

			if (!e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
			{
				return;
			}

			var treeNodeSource = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
			if (treeNodeSource == null)
			{
				return;
			}

			var treeNodePt = autoNotesTreeView.PointToClient(new Point(e.X, e.Y));
			var treeNodeDest = autoNotesTreeView.GetNodeAt(treeNodePt);

			var treeNodeTop = autoNotesTreeView.TopNode;
			if (treeNodeTop == treeNodeSource && treeNodeTop.PrevVisibleNode!=null)
            {
				treeNodeTop = treeNodeTop.PrevVisibleNode;
			}

			if (!IsValidDestination(treeNodeSource, treeNodeDest)) return;

			long? autoNoteCategoryId = null;
			if (treeNodeDest != null)
            {
				if (treeNodeDest.Tag is AutoNoteCategory autoNoteCategory)
                {
					autoNoteCategoryId = autoNoteCategory.Id;
                }
				else if (treeNodeDest.Tag is AutoNote autoNote)
                {
					autoNoteCategoryId = autoNote.AutoNoteCategoryId;
                }
            }

			{
				if (treeNodeSource.Tag is AutoNoteCategory autoNoteCategory)
				{
					if (autoNoteCategory.ParentId == autoNoteCategoryId)
                    {
						return;
                    }

					var prompt = autoNoteCategoryId.HasValue ?
						Translation.Common.ConfirmMoveSelectedCategory :
						Translation.Common.ConfirmMoveSelectedCategoryToRootLevel;

					if (!Confirm(prompt)) return;

					autoNoteCategory.ParentId = autoNoteCategoryId;

					AutoNoteCategories.Save(autoNoteCategory);
				}
				else if (treeNodeSource.Tag is AutoNote autoNote)
                {
					if (autoNote.AutoNoteCategoryId == autoNoteCategoryId)
                    {
						return;
                    }

					var prompt = autoNoteCategoryId.HasValue ?
						Translation.Common.ConfirmMoveSelectedNote :
						Translation.Common.ConfirmMoveSelectedNoteToRootLevel;

					if (!Confirm(prompt)) return;

					autoNote.AutoNoteCategoryId = autoNoteCategoryId;

					AutoNotes.Save(autoNote);
				}
                else
                {
					return;
                }
			}

			CacheManager.RefreshGlobal(nameof(InvalidType.AutoNotes));

			AutoNoteL.FillTreeView(autoNotesTreeView, expandedAutoNoteCategories, treeNodeTop);
		}

		private void AutoNotesTreeView_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}

		private void AutoNotesTreeView_DragOver(object sender, DragEventArgs e)
		{
			var treeNotePt = autoNotesTreeView.PointToClient(new Point(e.X, e.Y));
			var treeNode = autoNotesTreeView.GetNodeAt(treeNotePt);

			if (highlightTreeNode != null && highlightTreeNode != treeNode)
			{
				highlightTreeNode.BackColor = autoNotesTreeView.BackColor;
				highlightTreeNode = null;
			}

			if (treeNode != null && highlightTreeNode != treeNode)
            {
				highlightTreeNode = treeNode;
				highlightTreeNode.BackColor = Color.LightGray;
			}

			if (treeNotePt.Y < 25)
			{
				MiscUtils.SendMessage(autoNotesTreeView.Handle, 277, 0, 0);
			}
			else if (treeNotePt.Y > autoNotesTreeView.Height - 25)
			{
				MiscUtils.SendMessage(autoNotesTreeView.Handle, 277, 1, 0);
			}
		}

		private void AutoNotesTreeView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			autoNotesTreeView.SelectedNode = (TreeNode)e.Item;

			DoDragDrop(e.Item, DragDropEffects.Move);
		}

		private void AutoNotesTreeView_MouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			var autoNote = SelectedAutoNote;
			if (autoNote == null)
			{
				return;
			}

			if (IsSelectionMode)
			{
				DialogResult = DialogResult.OK;

				return;
			}

			EditButton_Click(this, EventArgs.Empty);
		}

		private void CollapseCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			AutoNoteL.SetCollapsed(autoNotesTreeView, collapseCheckBox.Checked);
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.AutoNoteQuickNoteEdit))
			{
				return;
			}

			long? category = null;
			if (autoNotesTreeView.SelectedNode != null)
			{
				if (autoNotesTreeView.SelectedNode.Tag is Definition definition)
				{
					category = definition.Id;
				}
				else if (autoNotesTreeView.SelectedNode.Tag is AutoNote autoNote)
				{
					category = autoNote.AutoNoteCategoryId;
				}
			}

			var newAutoNote = new AutoNote()
			{
				AutoNoteCategoryId = category
			};

			using var formAutoNoteEdit = new FormAutoNoteEdit(newAutoNote);
			if (formAutoNoteEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			autoNotesTreeView.SelectedNode?.Expand();

			AutoNoteL.FillTreeView(autoNotesTreeView, expandedAutoNoteCategories);

			static TreeNode FindTreeNode(TreeNodeCollection treeNodeCollection, AutoNote autoNote)
			{
				foreach (TreeNode treeNode in treeNodeCollection)
				{
					if (treeNode.Tag is AutoNote x && x.Id == autoNote.Id)
					{
						return treeNode;
					}

					return FindTreeNode(treeNode.Nodes, autoNote);
				}

				return default;
			}

			var autoNoteTreeNode = FindTreeNode(autoNotesTreeView.Nodes, newAutoNote);
			if (autoNoteTreeNode == null)
			{
				return;
			}

			autoNotesTreeView.SelectedNode = autoNoteTreeNode;
			autoNotesTreeView.SelectedNode.EnsureVisible();
			autoNotesTreeView.Focus();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var autoNote = SelectedAutoNote;
			if (autoNote == null)
			{
				return;
			}

			using var formAutoNoteEdit = new FormAutoNoteEdit(autoNote);
			if (formAutoNoteEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			AutoNoteL.FillTreeView(autoNotesTreeView, expandedAutoNoteCategories);
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var autoNote = SelectedAutoNote;
			if (autoNote == null)
			{
				return;
			}

			if (!Confirm(Translation.Common.ConfirmDeleteSelectedItem))
            {
				return;
            }

			AutoNotes.Delete(autoNote.Id);

			CacheManager.RefreshGlobal(nameof(InvalidType.AutoNotes));

			AutoNoteL.FillTreeView(autoNotesTreeView, expandedAutoNoteCategories);
		}

		private void ExportButton_Click(object sender, EventArgs e)
		{
			using var formAutoNoteExport = new FormAutoNoteExport();

			formAutoNoteExport.ShowDialog(this);
		}

		private static void RemoveDuplicates(List<AutoNotePromptDto> autoNotePromptDtos, List<AutoNoteDto> autoNoteDtos)
		{
			var duplicates = new List<string>();

			foreach (var autoNotePromptDto in autoNotePromptDtos)
			{
				var name = autoNotePromptDto.Description;
				var nameChanged = false;
				var nameCount = 0;

				var autoNotePrompt = AutoNotePrompts.GetByDescription(autoNotePromptDto.Description);

				while (autoNotePrompt != null)
                {
					if (autoNotePrompt.Type == autoNotePromptDto.Type && 
						autoNotePrompt.Options == autoNotePromptDto.Options)
                    {
						duplicates.Add(autoNotePrompt.Description);

						break;
					}

					nameCount++;
					nameChanged = true;

					autoNotePromptDto.Description = string.Concat(name, "_", nameCount);
					autoNotePrompt = AutoNotePrompts.GetByDescription(autoNotePromptDto.Description);
				}

				if (nameChanged)
                {
					foreach (var autoNoteDto in autoNoteDtos)
                    {
						autoNoteDto.Content = 
							autoNoteDto.Content.Replace(
								"[Prompt:\"" + name + "\"]", 
								"[Prompt:\"" + autoNotePromptDto.Description + "\"]");
					}
                }
			}

			autoNotePromptDtos.RemoveAll(autoNotePromptDto 
				=> duplicates.Contains(autoNotePromptDto.Description));
		}

		private async void ImportButton_Click(object sender, EventArgs e)
		{
			try
			{
				using var openFileDialog = InitializeOpenFileDialog();
				if (openFileDialog.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}

				Cursor = Cursors.WaitCursor;

				(int newAutoNotes, int newAutoNotePrompts) = await System.Threading.Tasks.Task.Run(() =>
				{
					var autoNoteExportJson = File.ReadAllText(openFileDialog.FileName);
					var autoNoteExportDto = JsonSerializer.Deserialize<AutoNoteExportDto>(autoNoteExportJson);

					RemoveDuplicates(autoNoteExportDto.AutoNotePrompts, autoNoteExportDto.AutoNotes);

					foreach (var autoNotePromptDto in autoNoteExportDto.AutoNotePrompts)
					{
						AutoNotePrompts.Save(new AutoNotePrompt
						{
							Description = autoNotePromptDto.Description,
							Type = autoNotePromptDto.Type,
							Label = autoNotePromptDto.Label,
							Options = autoNotePromptDto.Options
						});
					}

					foreach (var autoNoteDto in autoNoteExportDto.AutoNotes)
					{
						AutoNotes.Save(new AutoNote
						{
							Name = autoNoteDto.Name,
							Content = autoNoteDto.Content
						});
					}

					CacheManager.RefreshGlobal(nameof(InvalidType.AutoNotes));

					return (autoNoteExportDto.AutoNotes.Count, autoNoteExportDto.AutoNotePrompts.Count);
				});

				AutoNoteL.FillTreeView(autoNotesTreeView, expandedAutoNoteCategories);

				Cursor = Cursors.Default;

				SecurityLogs.Write(Permissions.AutoNoteQuickNoteEdit,
					string.Format(Translation.SecurityLog.AutoNoteImport, newAutoNotes, newAutoNotePrompts));

				if (Visible)
				{
					ShowInfo(string.Format(Translation.Common.AutoNoteImportSuccess, newAutoNotes, newAutoNotePrompts));
				}
			}
			catch (Exception exception)
			{
				Cursor = Cursors.Default;

				FriendlyException.Show(Translation.Common.AutoNoteImportFailed, exception);
			}
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void FormAutoNotes_FormClosing(object sender, FormClosingEventArgs e)
		{
			static IEnumerable<long> GetExpandedCategoryIds(TreeNodeCollection treeNodeCollection)
			{
				foreach (TreeNode treeNode in treeNodeCollection)
				{
					if (treeNode.IsExpanded && treeNode.Tag is Definition definition)
					{
						foreach (var categoryId in GetExpandedCategoryIds(treeNode.Nodes))
						{
							yield return categoryId;
						}

						yield return definition.Id;
					}
				}
			}

			UserPreference.Set(UserPreferenceName.AutoNoteExpandedCats, string.Join(",", GetExpandedCategoryIds(autoNotesTreeView.Nodes)));
		}
    }
}
