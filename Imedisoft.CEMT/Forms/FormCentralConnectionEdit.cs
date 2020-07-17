using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralConnectionEdit : FormBase
	{
		private readonly CentralConnection centralConnection;

		public int LastItemOrder { get; set; }

		public FormCentralConnectionEdit(CentralConnection centralConnection)
        {
			InitializeComponent();

			this.centralConnection = centralConnection;
		}

		private void FormCentralConnectionEdit_Load(object sender, EventArgs e)
		{
			databaseServerTextBox.Text = centralConnection.ServerName;
			databaseNameTextBox.Text = centralConnection.DatabaseName;
			databaseUserTextBox.Text = centralConnection.MySqlUser;
			databasePasswordTextBox.Text = centralConnection.MySqlPassword;
			noteTextBox.Text = centralConnection.Note;
			showBreakdownCheckBox.Checked = centralConnection.IsNew || centralConnection.HasClinicBreakdownReports;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (centralConnection.IsNew)
			{
				DialogResult = DialogResult.Cancel;

				return;
			}

			if (Confirm("Are you sure you want to delete this connection?") == DialogResult.Yes)
			{
				CentralConnections.Delete(centralConnection.CentralConnectionNum);

				DialogResult = DialogResult.OK;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			centralConnection.ServerName = databaseServerTextBox.Text;
			centralConnection.DatabaseName = databaseNameTextBox.Text;
			centralConnection.MySqlUser = databaseUserTextBox.Text;
			centralConnection.MySqlPassword = databasePasswordTextBox.Text;
			centralConnection.Note = noteTextBox.Text;
			centralConnection.HasClinicBreakdownReports = showBreakdownCheckBox.Checked;

			if (centralConnection.IsNew)
			{
				centralConnection.ItemOrder = LastItemOrder + 1;
				CentralConnections.Insert(centralConnection);

				centralConnection.IsNew = false;
			}
			else
			{
				CentralConnections.Update(centralConnection);
			}

			DialogResult = DialogResult.OK;
		}
	}
}
