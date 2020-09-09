using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormDefinitionPicker : FormBase
	{
		private readonly List<Definition> definitions;
		private readonly string definitionCategory;
		private readonly List<Definition> selectedDefinitions;

		// TODO: AutoNotes are storing the autonotecategory definitions as a tree (using the value column
		//       of a definition to hold the parent def ID). This should be reworked so that auto note cats have
		//       their own table. The definition table should only store flat lists.

		/// <summary>
		/// Gets the selected definitions.
		/// </summary>
		public List<Definition> SelectedDefinitions 
			=> definitionsGrid.SelectedTags<Definition>();

		/// <summary>
		/// Gets the selected definition.
		/// </summary>
		public Definition SelectedDefinition 
			=> definitionsGrid.SelectedTag<Definition>();

		/// <summary>
		/// Gets or sets a value indicating whether the "Show Hidden" checkbox is available.
		/// </summary>
		public bool AllowShowHidden { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether multiple definitions may be selected.
		/// </summary>
		public bool AllowMultiSelect { get; set; }

		///<summary>Passing in a list of Defs will make those defs pre-selected and highlighted when this window loads.  For AutoNoteCategories this is
		///used to select a parent category for the current category, but we want to prevent an infinite loop so defNumCur is used to exclude the
		///currently selected def and any direct-line descendants from being selected as the category's parent.</summary>
		public FormDefinitionPicker(string definitionCategory, List<Definition> selectedDefinitions = null, long? excludeDefinitionId = null)
		{
			InitializeComponent();

			this.definitionCategory = definitionCategory;
			this.selectedDefinitions = selectedDefinitions;

			definitions = Definitions.GetByCategory(definitionCategory);

			RemoveParentDefinitions(excludeDefinitionId);
		}

		private void FormDefinitionPicker_Load(object sender, EventArgs e)
		{
			definitionsGrid.Title = DefinitionCategory.GetDescription(definitionCategory);

			if (!AllowMultiSelect)
			{
				definitionsGrid.SelectionMode = GridSelectionMode.One;
			}

			showHiddenCheckBox.Visible = AllowShowHidden;

			FillGrid(
				selectedDefinitions?.Select(definition 
					=> definition.Id).ToList());
		}

		/// <summary>
		/// Removes the definition with the specified ID and all direct-line descendants from the list
		/// of definitions. This is required for AutoNoteCats to prevent potential infinite loops.
		/// </summary>
		/// <param name="excludeDefinitionId">The ID of the (parent) definition to exclude.</param>
		private void RemoveParentDefinitions(long? excludeDefinitionId)
		{
			if (!excludeDefinitionId.HasValue) return;

			var excludeDefinition = definitions.FirstOrDefault(definition => definition.Id == excludeDefinitionId);
			if (excludeDefinition == null)
            {
				return;
            }

			var invalidDefinitionIds = new List<long> { excludeDefinition.Id };
			for (int i = 0; i < invalidDefinitionIds.Count; i++)
            {
				invalidDefinitionIds.AddRange(
					definitions
						.Where(definition 
							=> definition.Value == invalidDefinitionIds[i].ToString())
						.Select(definition 
							=> definition.Id));
            }

			definitions.RemoveAll(definition => invalidDefinitionIds.Contains(definition.Id));
		}

		private void FillGrid(List<long> selectedDefinitionIds = null)
		{
			selectedDefinitionIds ??= definitionsGrid.SelectedTags<Definition>().Select(x => x.Id).ToList();

			var selectedDefinitionIndices = new List<int>();

			var definitionNames = new Dictionary<string, string>();
			if (definitionCategory == DefinitionCategory.AutoNoteCats)
			{
				definitionNames = definitions.ToDictionary(
					x => x.Id.ToString(), 
					x => x.Name);
			}

			definitionsGrid.BeginUpdate();
			definitionsGrid.Columns.Clear();
			definitionsGrid.Columns.Add(new GridColumn(Translation.Common.Definition, 200));
			definitionsGrid.Columns.Add(new GridColumn(Translation.Common.Value, 70));

			if (AllowShowHidden)
			{
				definitionsGrid.Columns.Add(new GridColumn(Translation.Common.Hidden, 20) { IsWidthDynamic = true });
			}

			definitionsGrid.Rows.Clear();

			foreach (var definition in definitions)
			{
				if (definition.IsHidden && !showHiddenCheckBox.Checked && !selectedDefinitions.Any(x => definition.Id == x.Id))
				{
					continue;
				}

				var gridRow = new GridRow();
				gridRow.Cells.Add(definition.Name);

				// for auto note categories, the ItemValue is stored as the long of the parent 
				// DefNum, so we have to get the name from the list.  But always default to the 
				// ItemValue if the num cannot be found in the list.
				if (definition.Category == DefinitionCategory.AutoNoteCats && !string.IsNullOrWhiteSpace(definition.Value))
				{
                    gridRow.Cells.Add(definitionNames.TryGetValue(definition.Value, out string name) ? name : definition.Value);
                }
				else
				{
					gridRow.Cells.Add(definition.Value);
				}

				if (AllowShowHidden)
				{
					gridRow.Cells.Add(definition.IsHidden ? "X" : "");
				}

				gridRow.Tag = definition;

				definitionsGrid.Rows.Add(gridRow);
				if (selectedDefinitionIds.Contains(definition.Id))
                {
					selectedDefinitionIndices.Add(definitionsGrid.Rows.Count - 1);
                }
			}

			definitionsGrid.EndUpdate();

			selectedDefinitionIndices.ForEach(index 
				=> definitionsGrid.SetSelected(index, true));
		}

		private void DefinitionsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e) 
			=> AcceptButton_Click(this, EventArgs.Empty);

		private void ShowHiddenCheckBox_CheckedChanged(object sender, EventArgs e) 
			=> FillGrid();

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (definitionsGrid.SelectedRows.Count == 0)
            {
				return;
            }

			DialogResult = DialogResult.OK;
		}
	}
}
