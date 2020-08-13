using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormEmployerEdit : FormBase
	{
		private readonly Employer employer;

		public FormEmployerEdit(Employer employer)
		{
			InitializeComponent();

			this.employer = employer;
		}

		private void FormEmployerEdit_Load(object sender, EventArgs e)
		{
			nameTextBox.Text = employer.Name;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			var name = nameTextBox.Text.Trim();
			if (name.Length == 0)
			{
				ShowError("Please enter an employer name.");

				return;
			}

			Employer empOld = employer.Copy();

			employer.Name = name;
			if (employer.Id == 0)
			{
				Employers.Insert(employer);
			}
			else
			{
				Employers.Update(employer, empOld);
			}

			DialogResult = DialogResult.OK;
		}
	}
}
