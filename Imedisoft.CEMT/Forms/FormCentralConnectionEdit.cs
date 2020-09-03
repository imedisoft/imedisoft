using Imedisoft.Data.Cemt;
using Imedisoft.Data.Models.Cemt;
using System;
using System.Windows.Forms;

namespace Imedisoft.CEMT.Forms
{
    public partial class FormCentralConnectionEdit : FormBase
	{
		private readonly Connection connection;

		public int LastItemOrder { get; set; }

		public FormCentralConnectionEdit(Connection connection)
        {
			InitializeComponent();

			this.connection = connection;
		}

		private void FormCentralConnectionEdit_Load(object sender, EventArgs e)
		{
			databaseServerTextBox.Text = connection.DatabaseServer;
			databaseNameTextBox.Text = connection.DatabaseName;
			databaseUserTextBox.Text = connection.DatabaseUser;
			databasePasswordTextBox.Text = connection.DatabasePassword;
			noteTextBox.Text = connection.Note;
			showBreakdownCheckBox.Checked = connection.Id == 0 || connection.HasClinicBreakdownReports;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			if (connection.Id == 0)
			{
				DialogResult = DialogResult.Cancel;

				return;
			}

			if (Confirm("Are you sure you want to delete this connection?") == DialogResult.Yes)
			{
				Connections.Delete(connection.Id);

				DialogResult = DialogResult.OK;
			}
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			connection.DatabaseServer = databaseServerTextBox.Text;
			connection.DatabaseName = databaseNameTextBox.Text;
			connection.DatabaseUser = databaseUserTextBox.Text;
			connection.DatabasePassword = databasePasswordTextBox.Text;
			connection.Note = noteTextBox.Text;
			connection.HasClinicBreakdownReports = showBreakdownCheckBox.Checked;

			if (connection.Id == 0)
			{
				connection.ItemOrder = LastItemOrder + 1;

				Connections.Insert(connection);
			}
			else
			{
				Connections.Update(connection);
			}

			DialogResult = DialogResult.OK;
		}
	}
}
