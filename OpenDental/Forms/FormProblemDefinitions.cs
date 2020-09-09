using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormProblemDefinitions : FormBase
	{
		private readonly List<long> coloredProblemDefinitionIds = new List<long>();
		private List<ProblemDefinition> problemDefinitions;
		private bool hasChanges;

		/// <summary>
		/// Gets or sets a value indicating whether the form is in selection mode.
		/// </summary>
		public bool IsSelectionMode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether multiple items may be selected.
		/// </summary>
		public bool IsMultiSelect { get; set; }

		/// <summary>
		/// Gets the selected problem definition.
		/// </summary>
		public ProblemDefinition SelectedProblemDefinition 
			=> problemsGrid.SelectedTag<ProblemDefinition>();

		/// <summary>
		/// Gets the list of selected disease definitions.
		/// </summary>
		public List<ProblemDefinition> SelectedProblemDefinitions 
			=> problemsGrid.SelectedTags<ProblemDefinition>();

		/// <summary>
		/// Initializes a new instance of the <see cref="FormProblemDefinitions"/> class.
		/// </summary>
		/// <param name="problemDefinitionIds">The ID's of the problems that should be displayed highlighted.</param>
		public FormProblemDefinitions(IEnumerable<long> problemDefinitionIds = null)
		{
			InitializeComponent();

			if (problemDefinitionIds != null)
			{
				coloredProblemDefinitionIds.AddRange(problemDefinitionIds);
			}
		}

		private void FormProblemDefinitions_Load(object sender, EventArgs e)
		{
			SetFilterControlsAndAction(FillGrid,
				(int)TimeSpan.FromSeconds(0.5).TotalMilliseconds,
				icd9TextBox, icd10TextBox, snomedTextBox, descriptionTextBox);

			if (IsSelectionMode)
			{
				cancelButton.Text = Translation.Common.CancelWithMnemonic;

				showHiddenCheckBox.Visible = false;
				if (IsMultiSelect)
				{
					problemsGrid.SelectionMode = GridSelectionMode.MultiExtended;
				}

				problemDefinitions = ProblemDefinitions.GetAll(false);
			}
			else
			{
				acceptButton.Visible = false;

				problemDefinitions = ProblemDefinitions.GetAll();
			}

			FillGrid();
		}

		private void FillGrid()
		{
			var problems = FilterList(problemDefinitions);

			problemsGrid.BeginUpdate();
			problemsGrid.Columns.Clear();
			problemsGrid.Columns.Add(new GridColumn("ICD-9", 50));
			problemsGrid.Columns.Add(new GridColumn("ICD-10", 50));
			problemsGrid.Columns.Add(new GridColumn("SNOMED CT", 100));
			problemsGrid.Columns.Add(new GridColumn(Translation.Common.Description, 250));
			if (!IsSelectionMode)
			{
				problemsGrid.Columns.Add(new GridColumn(Translation.Common.Hidden, 50, HorizontalAlignment.Center));
			}
			problemsGrid.Rows.Clear();

			foreach (var problem in problems)
			{
				var row = new GridRow();
				row.Cells.Add(problem.CodeIcd9);
				row.Cells.Add(problem.CodeIcd10);
				row.Cells.Add(problem.CodeSnomed);
				row.Cells.Add(problem.Description);
				if (!IsSelectionMode)
				{
					row.Cells.Add(problem.IsHidden ? "X" : "");
				}
				row.Tag = problem;

				if (coloredProblemDefinitionIds.Contains(problem.Id))
				{
					row.BackColor = Color.LightCyan;
				}

				problemsGrid.Rows.Add(row);
			}

			problemsGrid.EndUpdate();
		}

		private List<ProblemDefinition> FilterList(List<ProblemDefinition> problemDefinitions)
		{
			static IEnumerable<ProblemDefinition> Filter(IEnumerable<ProblemDefinition> problems, string criteria, Func<ProblemDefinition, string> selector)
            {
				if (string.IsNullOrEmpty(criteria))
				{
					var terms = criteria.ToLower().Split(' ');

					foreach (var problem in problems)
					{
						var field = selector(problem);
						if (string.IsNullOrEmpty(field))
						{
							continue;
						}

						if (terms.Any(term => field.ToLower().Contains(term)))
                        {
							yield return problem;
                        }
					}

					yield break;
				}

				foreach (var disease in problems) yield return disease;
            }

			IEnumerable<ProblemDefinition> results = problemDefinitions;
			results = Filter(results, icd9TextBox.Text.Trim(), disease => disease.CodeIcd9);
			results = Filter(results, icd10TextBox.Text.Trim(), disease => disease.CodeIcd10);
			results = Filter(results, snomedTextBox.Text.Trim(), disease => disease.CodeSnomed);
			results = Filter(results, descriptionTextBox.Text.Trim(), disease => disease.Description);

			if (!showHiddenCheckBox.Checked)
            {
				results = results.Where(problem => !problem.IsHidden);
            }

			return results.ToList();
		}

		private void ProblemsGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			if (!IsSelectionMode && !Security.IsAuthorized(Permissions.ProblemEdit))
			{
				return;
			}

			var problemDefinition = problemsGrid.SelectedTag<ProblemDefinition>();
			if (problemDefinition == null)
			{
				return;
			}

			if (IsSelectionMode)
			{
				AcceptButton_Click(this, EventArgs.Empty);

				return;
			}

			using var formProblemDefinitionEdit = new FormProblemDefinitionEdit(problemDefinition);
			if (formProblemDefinitionEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			ProblemDefinitions.Save(problemDefinition);

			hasChanges = true;

			FillGrid();
		}

		private void ProblemsGrid_SelectionCommitted(object sender, EventArgs e)
		{
			var problemDefinition = problemsGrid.SelectedTag<ProblemDefinition>();

			if (!Security.IsAuthorized(Permissions.ProblemEdit) || problemDefinition == null)
			{
				deleteButton.Enabled = false;

				return;
			}

			deleteButton.Enabled = true;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.ProblemEdit)) return;

			var problemDefinition = new ProblemDefinition();

			using var formProblemDefinitionEdit = new FormProblemDefinitionEdit(problemDefinition);
			if (formProblemDefinitionEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			ProblemDefinitions.Save(problemDefinition);

			hasChanges = true;

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.ProblemEdit)) return;

			var problemDefinition = problemsGrid.SelectedTag<ProblemDefinition>();
			if (problemDefinition == null)
			{
				return;
			}

			try
			{
				ProblemDefinitions.Delete(problemDefinition);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return;
			}

			problemDefinitions.Remove(problemDefinition);

			hasChanges = true;

			FillGrid();
		}

		private void ShowHiddenCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var problemDefinition = SelectedProblemDefinition;
			if (problemDefinition == null)
			{
				ShowError(Translation.Common.PleaseSelectItemFirst);

				return;
			}

			if (IsSelectionMode && !IsMultiSelect)
			{
				if (Snomeds.GetByCode(problemDefinition.CodeSnomed) == null)
				{
					ShowError(Translation.Common.YouHaveSelectedProblemWithUnofficialSnomedCode);

					return;
				}
			}

			DialogResult = DialogResult.OK;
		}

		private void FormProblemDefinitions_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (hasChanges)
			{
				CacheManager.RefreshGlobal(nameof(InvalidType.Diseases));
			}
		}
    }
}