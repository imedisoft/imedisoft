using CentralManager;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralConnectionEdit : FormBase
	{
		// TODO: Implement the port field

		// TODO: Add a dropdown to set SSL mode...

		// TODO: Change the database field into a dropdown...

		public int LastItemOrder;

		private readonly CentralConnection centralConnection;

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
			databasePasswordTextBox.Text = CentralConnections.Decrypt(centralConnection.MySqlPassword, FormCentralManager.EncryptionKey);
			noteTextBox.Text = centralConnection.Note;
			showBreakdownCheckBox.Checked = centralConnection.IsNew ? true : centralConnection.HasClinicBreakdownReports;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (centralConnection.IsNew)
			{
				DialogResult = DialogResult.Cancel;

				return;
			}

			// TODO: Confirm...

			CentralConnections.Delete(centralConnection.CentralConnectionNum);

			DialogResult = DialogResult.OK;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			centralConnection.ServerName = databaseServerTextBox.Text;
			centralConnection.DatabaseName = databaseNameTextBox.Text;
			centralConnection.MySqlUser = databaseUserTextBox.Text;
			centralConnection.MySqlPassword = CentralConnections.Encrypt(databasePasswordTextBox.Text, FormCentralManager.EncryptionKey);
			centralConnection.Note = noteTextBox.Text;
			centralConnection.HasClinicBreakdownReports = showBreakdownCheckBox.Checked;

			if (centralConnection.IsNew)
			{
				centralConnection.ItemOrder = LastItemOrder + 1;
				CentralConnections.Insert(centralConnection);

				centralConnection.IsNew = false;//so a double-click immediately in FormCentralConnections doesn't insert again
			}
			else
			{
				CentralConnections.Update(centralConnection);
			}

			DialogResult = DialogResult.OK;
		}
	}
}
