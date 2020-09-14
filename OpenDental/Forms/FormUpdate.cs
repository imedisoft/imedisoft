using Imedisoft.Data;
using OpenDental;
using OpenDentBusiness;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;

namespace Imedisoft.Forms
{
    public partial class FormUpdate : FormBase
	{
		public FormUpdate()
		{
			InitializeComponent();
		}

		class UpdateDto
		{
			public bool UpdateAvailable { get; set; }

			public string Version { get; set; }

			public string UpdateUrl { get; set; }

			public string ReleaseNotesUrl { get; set; }
		}

		private void TryInstallUpdate(UpdateDto updateDto)
		{
			if (updateDto == null || string.IsNullOrEmpty(updateDto.UpdateUrl))
			{
				return;
			}

			if (!string.IsNullOrEmpty(updateDto.ReleaseNotesUrl))
			{
				using var formUpdateInstallMsg = new FormUpdateReleaseNotes(updateDto.ReleaseNotesUrl);
				if (formUpdateInstallMsg.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
			}

			try
			{
				var tempPath = Path.Combine(Path.GetTempPath(), "Imedisoft");
				if (!Directory.Exists(tempPath))
				{
					Directory.CreateDirectory(tempPath);
				}

				tempPath = Path.Combine(tempPath, "update_" + updateDto.Version.Replace('.', '_') + ".exe");

				PrefL.DownloadInstallPatchFromURI(
					updateDto.UpdateUrl, tempPath, true, true);
			}
            catch
            {
            }
		}

		private void FormUpdate_Load(object sender, EventArgs e)
		{
			var updateHistory = UpdateHistories.GetForVersion(Application.ProductVersion);
			if (updateHistory != null)
			{
				versionLabel.Text = string.Format(Translation.Common.UsingVersionSince,
					Application.ProductVersion, updateHistory.InstalledOn.ToShortDateString());
			}
            else
            {
				versionLabel.Text = string.Format(Translation.Common.UsingVersion, 
					Application.ProductVersion);
			}

			if (!Security.IsAuthorized(Permissions.Setup, true))
			{
				checkForUpdatesButton.Enabled = setupButton.Enabled = false;
			}
		}

		private async void CheckForUpdatesButton_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;

			checkForUpdatesButton.Enabled = false;

			try
            {
				using var httpClient = new HttpClient
				{
					BaseAddress = new Uri("https://localhost:5001/api/v1/")
				};

				var updateDto = JsonSerializer.Deserialize<UpdateDto>(
					await httpClient.GetStringAsync(
						"update?version=" + Application.ProductVersion));

				Cursor = Cursors.Default;

				if (updateDto.UpdateAvailable)
                {
					if (!Confirm(Translation.Common.ConfirmInstallUpdate))
                    {
						return;
                    }

					TryInstallUpdate(updateDto);

				}
                else
                {

					ShowInfo(Translation.Common.ThereIsNoUpdateAvailable);
                }
			}
			catch (Exception exception)
            {
				Cursor = Cursors.Default;

				FriendlyException.Show(Translation.Common.ErrorCheckingForUpdates, exception);
			}
            finally
            {
				checkForUpdatesButton.Enabled = true;
			}
		}

		private void PreviousVersionsButton_Click(object sender, EventArgs e)
		{
			using var formPreviousVersions = new FormPreviousVersions();

			formPreviousVersions.ShowDialog(this);
		}

		private void SetupButton_Click(object sender, EventArgs e)
		{
			if (!Security.IsAuthorized(Permissions.Setup))
			{
				return;
			}

			using var formUpdateSetup = new FormUpdateSetup();

			formUpdateSetup.ShowDialog();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
