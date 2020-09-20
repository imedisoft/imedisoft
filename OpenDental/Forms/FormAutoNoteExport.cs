using Imedisoft.Data;
using Imedisoft.Data.Models;
using Imedisoft.Models.Dtos;
using OpenDental;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormAutoNoteExport : FormBase
	{
		private string expandedAutoNoteCategories;

		public FormAutoNoteExport()
		{
			InitializeComponent();

			imageListTree.Images.Add(Properties.Resources.IconFolderOpen);
			imageListTree.Images.Add(Properties.Resources.IconStickyNoteOpen);
		}

		private void FormAutoNoteExport_Load(object sender, EventArgs e)
		{
			expandedAutoNoteCategories = UserPreference.GetString(UserPreferenceName.AutoNoteExpandedCats);

			AutoNoteL.FillTreeView(autoNotesTreeView, expandedAutoNoteCategories);
		}

		private SaveFileDialog InitializeSaveFileDialog()
		{
			string exportPath = Preferences.GetString(PreferenceName.ExportPath);
            try
            {
				if (!Directory.Exists(exportPath))
                {
					Directory.CreateDirectory(exportPath);
                }
            }
            catch
            {
				exportPath = Path.GetTempPath();
			}

            return new SaveFileDialog
            {
                AddExtension = true,
                FileName = "autonotes.json",
                Filter = Translation.Common.JsonFiles + " (*.json)|*.json",
                InitialDirectory = exportPath
            };
		}

		private static void ToggleCheckboxes(TreeNode node, bool value)
		{
			if (node.Nodes != null)
			{
				ToggleCheckboxes(node.Nodes, value);
			}

			node.Checked = value;
		}

		private static void ToggleCheckboxes(TreeNodeCollection treeNodeCollection, bool value)
		{
			foreach (TreeNode treeNode in treeNodeCollection)
			{
				ToggleCheckboxes(treeNode.Nodes, value);

				treeNode.Checked = value;
			}
		}

		private static List<AutoNote> GetCheckedAutoNotes(TreeNodeCollection treeNodeCollection)
		{
			var autoNotes = new List<AutoNote>();

			foreach (TreeNode treeNode in treeNodeCollection)
			{
				autoNotes.AddRange(GetCheckedAutoNotes(treeNode.Nodes));
		
				if (treeNode.Checked && treeNode.Tag is AutoNote autoNote)
				{
					autoNotes.Add(autoNote);
				}
			}

			return autoNotes;
		}

		private void AutoNotesTreeView_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (e.Action != TreeViewAction.Unknown)
			{
				ToggleCheckboxes(e.Node, e.Node.Checked);
			}
		}

		private void CollapseCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			AutoNoteL.SetCollapsed(autoNotesTreeView, collapseCheckBox.Checked);
		}

		private void SelectAllButton_Click(object sender, EventArgs e)
		{
			ToggleCheckboxes(autoNotesTreeView.Nodes, true);
		}

		private void ClearButton_Click(object sender, EventArgs e)
		{
			ToggleCheckboxes(autoNotesTreeView.Nodes, false);
		}

		private void ExportButton_Click(object sender, EventArgs e)
		{
			var autoNotes = GetCheckedAutoNotes(autoNotesTreeView.Nodes);
			if (autoNotes.Count == 0)
			{
				ShowError(Translation.Common.PleaseSelectAtLeastOneNote);

				return;
			}

			var autoNotePrompts = AutoNotePrompts.GetByAutoNoteContent(autoNotes);

			try
			{
				using var saveDialog = InitializeSaveFileDialog();
				if (saveDialog.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}

				var fileName = saveDialog.FileName;

				var autoNoteExportDto = new AutoNoteExportDto
				{
					AutoNotes = autoNotes.Select(autoNote => new AutoNoteDto(autoNote)).ToList(),

					AutoNotePrompts = autoNotePrompts.Select(autoNotePrompt => new AutoNotePromptDto(autoNotePrompt)).ToList()
				};

				File.WriteAllText(fileName, JsonSerializer.Serialize(autoNoteExportDto));

				SecurityLogs.Write(Permissions.AutoNoteQuickNoteEdit, Translation.SecurityLog.AutoNoteExport);

				ShowInfo(Translation.Common.AutoNoteExportSuccess);

				DialogResult = DialogResult.OK;
			}
			catch (Exception exception)
			{
				FriendlyException.Show(Translation.Common.AutoNoteExportFailed, exception);
			}
		}
	}
}
