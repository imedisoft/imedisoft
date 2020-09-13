using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormCounties : FormBase
	{
		public FormCounties() 
			=> InitializeComponent();

		private void FormCounties_Load(object sender, EventArgs e) 
			=> FillList();

		private void FillList()
		{
			countiesListBox.Items.Clear();

			foreach (var county in Counties.GetAll())
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

			try
			{
				Counties.Delete(county);
			}
			catch (Exception exception)
            {
				ShowError(exception.Message);

				return;
            }

			FillList();
		}
    }
}
