using Imedisoft.Data;
using Imedisoft.Data.Cache;
using MySql.Data.MySqlClient;
using OpenDental;
using OpenDentBusiness;
using System;
using System.IO;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormBackup : FormBase
	{
		public FormBackup() => InitializeComponent();

		private void FormBackup_Load(object sender, EventArgs e)
		{
			backupDestTextBox.Text = Preferences.GetString(PreferenceName.BackupToPath);
			backupSourceTextBox.Text = Preferences.GetString(PreferenceName.BackupRestoreFromPath);
			restoreTargetTextBox.Text = Preferences.GetString(PreferenceName.BackupRestoreToPath);

			saveButton.Enabled = false;
		}

		private bool SaveTabPrefs() =>
			Preferences.Set(PreferenceName.BackupToPath, backupDestTextBox.Text) | 
			Preferences.Set(PreferenceName.BackupRestoreFromPath, backupSourceTextBox.Text) |
			Preferences.Set(PreferenceName.BackupRestoreToPath, restoreTargetTextBox.Text);

		private void BackupDestButton_Click(object sender, EventArgs e)
		{
            using var folderBrowserDialog = new FolderBrowserDialog
            {
                SelectedPath = backupDestTextBox.Text
            };

            if (folderBrowserDialog.ShowDialog(this) != DialogResult.Cancel)
			{
				backupDestTextBox.Text = folderBrowserDialog.SelectedPath;
			}
		}

		private void BackupSourceButton_Click(object sender, EventArgs e)
		{
            using var openFileDialog = new OpenFileDialog
            {
                FileName = backupSourceTextBox.Text,
				Filter = "Backup Files (*.backup)|*.backup"
            };

            if (openFileDialog.ShowDialog(this) != DialogResult.Cancel)
			{
				backupSourceTextBox.Text = openFileDialog.FileName;
			}
		}

		private void BackupButton_Click(object sender, EventArgs e)
		{
			var backupDest = backupDestTextBox.Text.Trim();
			if (string.IsNullOrEmpty(backupDest))
            {
				ShowError("Please specify the backup location.");

				return;
            }

			if (!Directory.Exists(backupDest))
            {
				if (!Confirm("The backup location does not exist. Create?"))
                {
					return;
                }

                try
                {
					Directory.CreateDirectory(backupDest);
                }
				catch (Exception exception)
                {
					ShowError(exception.Message);

					return;
                }
            }

			var backupFileName = string.Concat(Database.CurrentDatabase, "_" + DateTime.Now.ToString("yyyyMMdd_HHmm"));
			var backupFile = Path.Combine(backupDest, backupFileName + ".backup");
			if (File.Exists(backupFile))
            {
				for (int i = 0; i < 100; i++)
                {
					backupFile = Path.Combine(backupDest, backupFileName + i.ToString().PadLeft(2, '0') + ".backup");
					if (!File.Exists(backupFile))
                    {
						break;
                    }
                }
            }

			using var connection = new MySqlConnection(DataConnection.ConnectionString);

			connection.Open();

			using var backup = new MySqlBackup(connection.CreateCommand());

			backup.ExportCompleted += (s, e) =>
			{
				if (e.HasError && e.LastError != null)
				{
					ShowError(e.LastError.Message);
				}
				else
				{
					ShowInfo("The backup has been created succesfully.");
				}

				Close();
			};

			backup.ExportProgressChanged += (s, e) =>
			{
			};

			backup.ExportToFile(backupFile);
		}

		private void RestoreButton_Click(object sender, EventArgs e)
		{
			var backupFile = backupSourceTextBox.Text;
			if (string.IsNullOrEmpty(backupFile))
            {
				ShowError("Please specify the backup file to restore.");

				return;
            }

			if (!File.Exists(backupFile))
            {
				ShowError("The specified backup file does not exist.");

				return;
            }

			using var connection = new MySqlConnection(DataConnection.ConnectionString);

			connection.Open();

			using var backup = new MySqlBackup(connection.CreateCommand());

			backup.ImportCompleted += (s, e) =>
			{
				if (e.HasErrors && e.LastError != null)
                {
					ShowError(e.LastError.Message);
                }
                else
                {
					ShowInfo("The backup has been succesfully restored.");
                }

				Close();

				var versionDatabase = new Version(Preferences.GetStringNoCache(PreferenceName.ProgramVersion));
				var versionCurrent = new Version(Application.ProductVersion);

				if (versionDatabase != versionCurrent)
				{
					ShowError("The restored database version is different than the version installed and requires a restart. The program will now close.");

					FormOpenDental.S_ProcessKillCommand();

					return;
				}
				else
				{
					CacheManager.RefreshAll();
				}
			};

			backup.ImportProgressChanged += (s, e) =>
			{
			};

			backup.ImportFromFile(backupFile);
		}

		private void SaveButton_Click(object sender, EventArgs e)
		{
			if (SaveTabPrefs())
			{
				CacheManager.RefreshGlobal(nameof(InvalidType.Prefs));
			}

			saveButton.Enabled = false;
		}

        private void InputTextChanged_TextChanged(object sender, EventArgs e)
        {
			saveButton.Enabled = true;
        }
    }
}
