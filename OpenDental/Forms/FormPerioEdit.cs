using Imedisoft.Data;
using Imedisoft.Data.Models;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormPerioEdit : FormBase
	{
		private readonly PerioExam perioExam;

		public FormPerioEdit(PerioExam perioExam)
		{
			InitializeComponent();

			this.perioExam = perioExam;
		}

		private void FormPerioEdit_Load(object sender, EventArgs e)
		{
			examDateTextBox.Text = perioExam.ExamDate.ToShortDateString();

			providerListBox.Items.Clear();

			foreach (var provider in Providers.GetAll(true))
            {
				providerListBox.Items.Add(provider);
				if (provider.Id == perioExam.ProviderId)
                {
					providerListBox.SelectedItem = provider;
                }
            }

			if (providerListBox.SelectedIndex == -1)
				providerListBox.SelectedIndex = 0;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(examDateTextBox.Text) || !DateTime.TryParse(examDateTextBox.Text, out var examDate))
			{
				ShowError(Translation.Common.PleaseEnterValidDate);

				return;
			}

			if (!(providerListBox.SelectedItem is Provider provider))
            {
				ShowError(Translation.Common.PleaseSelectProvider);

				return;
            }

			perioExam.ExamDate = examDate;
			perioExam.ProviderId = provider.Id;

			PerioExams.Update(perioExam);

			DialogResult = DialogResult.OK;
		}
	}
}
