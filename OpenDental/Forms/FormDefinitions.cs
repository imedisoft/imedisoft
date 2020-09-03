using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Imedisoft.Forms
{
    public partial class FormDefinitions : FormBase
	{
		private readonly string initialDefinitionCategory;
		private List<Definition> definitions;
		private bool hasChanges;

		/// <summary>
		/// Gets the options for the currently selected category.
		/// </summary>
		private DefinitionCategoryOptions SelectedCategoryOptions =>
			categoryListBox.SelectedItem as DefinitionCategoryOptions;

		/// <summary>
		/// Gets the currently selected definition.
		/// </summary>
		private Definition SelectedDefinition
			=> definitionsGrid.SelectedTag<Definition>();

		private List<Definition> CurrentDefinitions 
			=> definitions
				.Where(
					x => x.Category == SelectedCategoryOptions.Category)
				.OrderBy(
					x => x.SortOrder)
				.ToList();

		/// <summary>
		/// Initializes a new instance of the <see cref="FormDefinitions"/> class.
		/// </summary>
		public FormDefinitions() : this(DefinitionCategory.AccountColors)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="FormDefinitions"/> class.
		/// </summary>
		public FormDefinitions(string initialDefinitionCategory)
		{
			InitializeComponent();

			this.initialDefinitionCategory = initialDefinitionCategory;
		}

		private void FormDefinitions_Load(object sender, EventArgs e) 
			=> LoadDefinitionCategories();

		private void LoadDefinitionCategories()
		{
            var definitionCategoryOptions = DefL.GetOptionsForDefCats(DefinitionCategory.All);

            foreach (var categoryOptions in definitionCategoryOptions)
			{
				categoryListBox.Items.Add(categoryOptions);

				if (initialDefinitionCategory == categoryOptions.Category)
				{
					categoryListBox.SelectedItem = categoryOptions;
				}
			}
		}

		private void CategoryListBox_SelectedIndexChanged(object sender, EventArgs e) 
			=> FillGrid();

		private void RefreshDefinitions()
		{
            Definitions.RefreshCache();

            definitions = Definitions.GetAll();
		}

		private void FillGrid()
		{
			if (SelectedCategoryOptions == null)
            {
				return;
            }

			if (definitions == null)
			{
				RefreshDefinitions();
			}

			DefL.FillGrid(definitionsGrid, SelectedCategoryOptions, CurrentDefinitions);

			hideButton.Visible = SelectedCategoryOptions.CanHide;

			if (SelectedCategoryOptions.CanEditName)
			{
				editGroupBox.Enabled = true;
				editGroupBox.Text = "Edit Items";
			}
			else
			{
				editGroupBox.Enabled = false;
				editGroupBox.Text = "Edit Items (Not Allowed)";
			}

			helpTextBox.Text = SelectedCategoryOptions.HelpText;
		}

		private void DefinitionsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var definition = SelectedDefinition;
			if (definition == null)
			{
				return;
			}

			if (DefL.TryEditDefinition(definition, SelectedCategoryOptions, CurrentDefinitions))
			{
				hasChanges = true;

				RefreshDefinitions();

				FillGrid();
			}
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			if (DefL.CreateDefinition(definitionsGrid, SelectedCategoryOptions))
			{
				RefreshDefinitions();

				FillGrid();

				hasChanges = true;
			}
		}

		private void HideButton_Click(object sender, EventArgs e)
		{
			if (DefL.TryHideSelectedDefinition(definitionsGrid, SelectedCategoryOptions))
			{
				RefreshDefinitions();

				FillGrid();

				hasChanges = true;
			}
		}

		private void UpButton_Click(object sender, EventArgs e)
		{
			if (DefL.MoveSelectedUp(definitionsGrid))
			{
				hasChanges = true;

				FillGrid();
			}
		}

		private void DownButton_Click(object sender, EventArgs e)
		{
			if (DefL.MoveSelectedDown(definitionsGrid))
			{
				hasChanges = true;

				FillGrid();
			}
		}

		private void AlphabetizeButton_Click(object sender, EventArgs e)
		{
			if (!Confirm("Alphabetizing does not have an 'undo' button. Continue?"))
			{
				return;
			}

			var definitions = definitionsGrid.GetTags<Definition>().OrderBy(x => x.Name).ToList();
			for (int i = 0; i < definitions.Count; i++)
            {
				if (definitions[i].SortOrder!= i)
                {
					definitions[i].SortOrder = i;

					Definitions.Update(definitions[i]);

					hasChanges = true;
                }
            }

			RefreshDefinitions();

			FillGrid();
		}

		private void CloseButton_Click(object sender, EventArgs e) 
			=> Close();

		private void FormDefinitions_Closing(object sender, CancelEventArgs e)
		{
			if (hasChanges)
			{
				// A specialty could have been renamed, invalidate the specialty associated to the currently selected patient just in case.
				PatientL.InvalidateSelectedPatSpecialty();

				CacheManager.RefreshGlobal(nameof(InvalidType.Defs));
			}
		}
	}
}
