using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormCounties : FormBase
	{
		private List<County> counties;

		public FormCounties() 
			=> InitializeComponent();

		private void FormCounties_Load(object sender, EventArgs e) 
			=> FillList();

		private void FillList()
		{
			counties = Counties.GetAll();

			countiesListBox.Items.Clear();

			foreach (var county in counties)
            {
				countiesListBox.Items.Add(county);
            }
		}

		private void CountiesListBox_DoubleClick(object sender, EventArgs e)
		{
            if (!(countiesListBox.SelectedItem is County county))
            {
                return;
            }

            using var formCountyEdit = new FormCountyEdit(county);

			if (formCountyEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillList();
		}

		private void CountiesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			deleteButton.Enabled = countiesListBox.SelectedItem != null;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var county = new County();

			using var formCountyEdit = new FormCountyEdit(county);

			if (formCountyEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			FillList();
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
            if (!(countiesListBox.SelectedItem is County county))
            {
                ShowError(Translation.Common.PleaseSelectItemFirst);
                return;
            }

            string usedBy = Counties.UsedBy(county.Name);
			if (usedBy != "")
			{
				ShowError("Cannot delete County because it is already in use by the following patients: \r" + usedBy);

				return;
			}

			Counties.Delete(county);

			FillList();
		}
    }
}
