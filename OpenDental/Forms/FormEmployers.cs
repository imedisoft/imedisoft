using Imedisoft.Data.Cache;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEmployers : FormBase
	{
		public FormEmployers() => InitializeComponent();
		
		private void FormEmployers_Load(object sender, EventArgs e) => FillGrid();

		private void FillGrid()
		{
			Employers.RefreshCache();

			var employers = new List<Employer>(Employers.GetAll());

			employers.Sort((x, y) => x.Name.CompareTo(y.Name));
			employersListBox.Items.Clear();
			foreach (var employer in employers)
            {
				employersListBox.Items.Add(employer);
			}
		}

		private void EmployersListBox_DoubleClick(object sender, EventArgs e) 
			=> EditButton_Click(sender, e);

		private void EmployersListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedItems = employersListBox.SelectedItems.Count;

			deleteButton.Enabled = editButton.Enabled
				= selectedItems > 0;

			combineButton.Enabled
				= selectedItems > 1;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			using var formEmployerEdit = new FormEmployerEdit(new Employer());

			formEmployerEdit.ShowDialog(this);

			FillGrid();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (!(employersListBox.SelectedItem is Employer employer))
			{
				ShowError("Please select one item first.");

				return;
			}

			// Make sure there are no dependent patients...
			string dependentNames = Employers.DependentPatients(employer);
			if (dependentNames != "")
			{
				ShowError(
					"Not allowed to delete this employer because it it attached to the following patients. " +
					"You should combine employers instead." +
					"\r\n\r\n" + dependentNames);

				return;
			}

			// Make sure there are no dependent insurance plans...
			dependentNames = Employers.DependentInsPlans(employer);
			if (string.IsNullOrEmpty(dependentNames))
			{
				ShowError(
					"Not allowed to delete this employer because it is attached to the following insurance plans. " +
					"You should combine employers instead." +
					"\r\n\r\n" + dependentNames);

				return;
			}

			if (Prompt("Delete Employer?") != DialogResult.Yes)
			{
				return;
			}

			Employers.Delete(employer);

			FillGrid();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			if (!(employersListBox.SelectedItem is Employer employer))
			{
				ShowError("Please select one item first.");

				return;
			}

			using var formEmployerEdit = new FormEmployerEdit(employer);

            if (formEmployerEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillGrid();
		}

		private void CombineButton_Click(object sender, EventArgs e)
		{
			if (employersListBox.SelectedIndices.Count < 2)
			{
				ShowError(
					"Please select multiple items first while holding down the control key.");

				return;
			}

			if (Prompt("Combine all these employers into a single employer? This will affect all patients using these employers.") != DialogResult.Yes)
			{
				return;
			}

			var employerIds = employersListBox.Items.OfType<Employer>().Select(e => e.Id).ToList();

			Employers.Combine(employerIds);

			FillGrid();
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			CacheManager.Refresh(nameof(InvalidType.Employers));

			DialogResult = DialogResult.OK;
		}
    }
}
