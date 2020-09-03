using Imedisoft.Data;
using Imedisoft.Data.Cemt;
using Imedisoft.Data.Models.Cemt;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormChooseDatabase : FormBase
	{
		private readonly bool fromMainMenu;

		private readonly Connection centralConnection = new Connection
		{
			DatabaseServer = DataConnection.ServerName,
			DatabaseName = DataConnection.DatabaseName,
			DatabaseUser = DataConnection.User,
			DatabasePassword = DataConnection.Password
        };

		protected override bool HasHelpKey => false;

		public FormChooseDatabase(bool fromMainMenu)
		{
			InitializeComponent();

			this.fromMainMenu = fromMainMenu;
		}

		private void FormChooseDatabase_Load(object sender, EventArgs e)
		{
			connectionGroupBox.Enabled = true;

			Connections.GetChooseDatabaseConnectionSettings(
				out var connectionString,
				out var autoConnect);

			userTextBox.Text = centralConnection.DatabaseUser;
			passwordTextBox.Text = centralConnection.DatabasePassword;
			autoConnectComboBox.Checked = autoConnect;

			if (fromMainMenu)
			{
				serverTextBox.Enabled = false;
				serverTextBox.Text = DataConnection.ServerName;
				userTextBox.Text = DataConnection.User;
				passwordTextBox.Text = DataConnection.Password;
				databaseComboBox.Enabled = false;
				databaseComboBox.Text = DataConnection.DatabaseName;
				autoConnectComboBox.Checked = autoConnect;
			}
			else
			{
				var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

				serverTextBox.Text = connectionStringBuilder.Server;
				userTextBox.Text = connectionStringBuilder.UserID;
				passwordTextBox.Text = connectionStringBuilder.Password;
				databaseComboBox.Text = connectionStringBuilder.Database;
			}
		}

		private void DatabaseComboBox_DropDown(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;

			centralConnection.DatabaseServer = serverTextBox.Text;
			centralConnection.DatabaseName = databaseComboBox.Text;
			centralConnection.DatabaseUser = userTextBox.Text;
			centralConnection.DatabasePassword = passwordTextBox.Text;

			databaseComboBox.Items.Clear();
			databaseComboBox.Items.AddRange(Connections.EnumerateDatabases(centralConnection).ToArray());

			Cursor = Cursors.Default;
		}

		/// <summary>
		/// Attempts to connect to the database.
		/// Returns true if connection settings are valid. Otherwise, false.
		/// </summary>
		private bool IsValidConnection()
		{
			try
			{
				Connections.TryToConnect(
					serverTextBox.Text,
					userTextBox.Text, 
					passwordTextBox.Text, 
					databaseComboBox.Text, 
					autoConnectComboBox.Checked, 
					true);
			}
			catch (Exception exception)
			{
				ShowError(exception.Message);

				return false;
			}

			return true;
		}

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			if (!IsValidConnection())
			{
				return;
			}

			DialogResult = DialogResult.OK;
		}
	}
}
