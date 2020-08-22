using CodeBase;
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
			archiveMakeBackupCheckBox.Checked = Prefs.GetBool(PrefName.ArchiveDoBackupFirst);
			backupDestTextBox.Text = Prefs.GetString(PrefName.BackupToPath);
			backupSourceTextBox.Text = Prefs.GetString(PrefName.BackupRestoreFromPath);
			restoreTargetTextBox.Text = Prefs.GetString(PrefName.BackupRestoreToPath);
			archiveDateTime.Value = Prefs.GetDateTime(PrefName.ArchiveDate, DateTime.Today.AddYears(-3));
		}

		private bool IsBackupTabValid()
		{
			if (backupDestTextBox.Text != "" && !backupDestTextBox.Text.EndsWith("" + Path.DirectorySeparatorChar))
			{
				ShowError("Paths must end with " + Path.DirectorySeparatorChar + ".");
				return false;
			}

			if (backupSourceTextBox.Text != "" && !backupSourceTextBox.Text.EndsWith("" + Path.DirectorySeparatorChar))
			{
				ShowError("Paths must end with " + Path.DirectorySeparatorChar + ".");
				return false;
			}
			if (restoreTargetTextBox.Text != "" && !restoreTargetTextBox.Text.EndsWith("" + Path.DirectorySeparatorChar))
			{
				ShowError("Paths must end with " + Path.DirectorySeparatorChar + ".");
				return false;
			}

			return true;
		}

		private bool SaveTabPrefs() =>
			Prefs.Set(PrefName.ArchiveDoBackupFirst, archiveMakeBackupCheckBox.Checked) | 
			Prefs.Set(PrefName.BackupToPath, backupDestTextBox.Text) | 
			Prefs.Set(PrefName.BackupRestoreFromPath, backupSourceTextBox.Text) |
			Prefs.Set(PrefName.BackupRestoreToPath, restoreTargetTextBox.Text) |
			Prefs.Set(PrefName.ArchiveDate, archiveDateTime.Value);

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
			if (!IsBackupTabValid())
			{
				return;
			}

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

			string databaseName = MiscData.GetCurrentDatabase();

			var backupFileName = string.Concat(databaseName, "_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmm"));
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

				var versionDatabase = new Version(Prefs.GetStringNoCache(PrefName.ProgramVersion));
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
			if (!IsBackupTabValid())
			{
				return;
			}

			if (SaveTabPrefs())
			{
				DataValid.SetInvalid(InvalidType.Prefs);

				ShowInfo("Saved");
			}

			Prefs.Set(PrefName.ArchiveDate, archiveDateTime.Value);
		}

		private void ArchiveButton_Click(object sender, EventArgs e)
		{
			if (archiveMakeBackupCheckBox.Checked)
			{ 
				if (!Confirm("To make a backup of the database, ensure no other machines are currently using OpenDental. Proceed?"))
				{
					return;
				}
			}

			ODProgress.ShowAction(() =>
				{
					if (archiveMakeBackupCheckBox.Checked)
					{
						try
						{
							MiscData.MakeABackup();
						}
						catch (Exception exception)
						{
							FriendlyException.Show(
								"An error occurred backing up the old database. " +
								"Old data was not removed from the database. " +
								"Ensure no other machines are currently using OpenDental and try again.", 
								exception);

							return;
						}
					}

					SecurityLogs.DeleteBeforeDateInclusive(archiveDateTime.Value);
					SecurityLogs.MakeLogEntry(Permissions.Backup, 0, $"SecurityLog and SecurityLogHashes on/before {archiveDateTime.Value} deleted.");
				},
				eventType: typeof(MiscDataEvent),
				odEventType: EventCategory.MiscData);
		}
	}

	/// <summary>
	/// Backing up can fail at two points, when backing up the database or the A to Z images. 
	/// This delegate lets the backup thread manipulate a local variable so that we can let the user know at what point the backup failed.
	/// </summary>
	public delegate void ErrorMessageDelegate(string errorMessage);
}
