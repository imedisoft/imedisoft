using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormProviderPick : FormBase
	{
		private readonly List<Provider> providers;

		/// <summary>
		/// Gets or sets a value indicating whether the form should display students.
		/// </summary>
		public bool IsStudentPicker { get; set; }

		/// <summary>
		/// Gets or sets the ID of the selected provider.
		/// </summary>
		public long SelectedProviderId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the "None" button is visible.
		/// </summary>
		public bool IsNoneAvailable { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the "Show All" checkbox is visible.
		/// </summary>
		public bool IsShowAllAvailable { get; set; }

		public FormProviderPick(List<Provider> providers = null)
		{
			InitializeComponent();

			this.providers = providers;
		}

		private void FormProviderSelect_Load(object sender, EventArgs e)
		{
			SetFilterControlsAndAction(() => FillGrid(),
				(int)TimeSpan.FromSeconds(0.5).TotalMilliseconds,
				firstNameTextBox, lastNameTextBox, providerIdTextBox);

			showAllCheckBox.Visible = IsShowAllAvailable;
			if (Prefs.GetBool(PrefName.EasyHideDentalSchools))
			{
				dentalSchoolGroupBox.Visible = false;
			}

			if (IsStudentPicker)
			{
				Text = Translation.Common.StudentPicker;
				providersGrid.Title = Translation.Common.Students;

				foreach (var schoolClass in SchoolClasses.GetDeepCopy())
                {
					classComboBox.Items.Add(schoolClass);
                }

				if (classComboBox.Items.Count > 0)
				{
					classComboBox.SelectedIndex = 0;
				}
			}
			else
			{
				classComboBox.Visible = false;
				classLabel.Visible = false;
			}

			FillGrid();

			if (providers != null)
			{
				for (int i = 0; i < providers.Count; i++)
				{
					if (providers[i].ProvNum == SelectedProviderId)
					{
						providersGrid.SetSelected(i, true);

						break;
					}
				}
			}
			else if (SelectedProviderId != 0)
			{
				providersGrid.SetSelected(Providers.GetIndex(SelectedProviderId), true);
			}

			noneButton.Visible = IsNoneAvailable;
			if (IsNoneAvailable)
			{
				//Default value for the selected provider when none is an option is always -1
				SelectedProviderId = -1;
			}
		}

		private void FillGrid()
		{
            if (!long.TryParse(providerIdTextBox.Text, out long providerId))
            {
                providerId = 0;
            }

            long classId = 0;
			if (IsStudentPicker)
			{
				var schoolClass = classComboBox.SelectedItem as SchoolClass;
				if (schoolClass != null)
                {
					classId = schoolClass.Id;
                }
			}

			providersGrid.BeginUpdate();
			providersGrid.ListGridColumns.Clear();
			providersGrid.ListGridColumns.Add(new GridColumn(Translation.Common.ID, 60));
			providersGrid.ListGridColumns.Add(new GridColumn(Translation.Common.Abbr, 80));
			providersGrid.ListGridColumns.Add(new GridColumn(Translation.Common.FirstName, 100));
			providersGrid.ListGridColumns.Add(new GridColumn(Translation.Common.LastName, 100));
			providersGrid.ListGridRows.Clear();

			var filteredProviders = providers != null && !showAllCheckBox.Checked ?
				providers : Providers.GetFilteredProviderList(providerId, lastNameTextBox.Text, firstNameTextBox.Text, classId);

			foreach (var provider in filteredProviders)
			{
				if (IsStudentPicker && provider.SchoolClassNum == 0)
				{
					continue;
				}

				var gridRow = new GridRow();
				gridRow.Cells.Add(provider.ProvNum.ToString());
				gridRow.Cells.Add(provider.Abbr);
				gridRow.Cells.Add(provider.LName);
				gridRow.Cells.Add(provider.FName);
				gridRow.Tag = provider;

				providersGrid.ListGridRows.Add(gridRow);
			}

			providersGrid.EndUpdate();
		}

		private void ProvidersGrid_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			AcceptButton_Click(this, EventArgs.Empty);
		}

		private void ClassComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void ShowAllCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			FillGrid();
		}

		private void NoneButton_Click(object sender, EventArgs e)
		{
			providersGrid.SetSelected(false);

			AcceptButton_Click(this, EventArgs.Empty);
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var provider = providersGrid.SelectedTag<Provider>();
			if (provider == null)
			{
				ShowError(Translation.Common.PleaseSelectProvider);

				return;
			}

			SelectedProviderId = provider.ProvNum;

			DialogResult = DialogResult.OK;
		}
	}
}
