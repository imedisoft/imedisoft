using OpenDental;
using OpenDentBusiness;
using System;
using System.IO;
using System.Windows.Forms;
using MessageBox = OpenDental.MessageBox;

namespace Imedisoft.Forms
{
    public partial class FormPath : FormBase
	{
		private readonly bool notConfigured;

		/// <summary>
		///		<para>
		///			Initializes a new instance of the <see cref="FormPath"/> class.
		///		</para>
		/// </summary>
		/// <param name="notConfigured"></param>
		public FormPath(bool notConfigured = false)
		{
			InitializeComponent();

			this.notConfigured = notConfigured;
		}

		private void FormPath_Load(object sender, EventArgs e)
		{
			// Verify user has Setup permission to change paths, after user has logged in.
			if (!notConfigured && !Security.IsAuthorized(Permissions.Setup))
			{
				acceptButton.Enabled = false;
			}

			pathTextBox.Text = Prefs.GetString(PrefName.DocPath);

			localPathTextBox.Text = OpenDentBusiness.FileIO.FileAtoZ.LocalAtoZpath;//This was set on startup.  //compPref.AtoZpath;
			exportPathTextBox.Text = Prefs.GetString(PrefName.ExportPath);
			letterMergePathTextBox.Text = Prefs.GetString(PrefName.LetterMergePath);

			if (notConfigured)
			{
				Show();
				ShowInfo("Could not find the path for storing images and documents.");

				if (Security.CurrentUser == null || !Security.IsAuthorized(Permissions.Setup))
				{
					// The user is still allowed to set the "Path override for this computer", thus the user has a way to temporariliy get into OD in worst case.
					// For example, if the primary folder path is wrong or has changed, the user can set the path override for this computer to get into OD, then
					// can to to Setup | Data Paths to fix the primary path.
					DisableMostControls();

					localPathTextBox.ReadOnly = false;
					localPathButton.Enabled = true;

					ActiveControl = localPathTextBox;
				}
			}
		}

		private void DisableMostControls()
		{
			pathTextBox.ReadOnly = true;
			pathButton.Enabled = false;
			exportPathTextBox.ReadOnly = true;
			exportPathButton.Enabled = false;
			letterMergePathTextBox.ReadOnly = true;
			letterMergeButton.Enabled = false;
			localPathTextBox.ReadOnly = true;
			localPathButton.Enabled = false;
		}

		/// <summary>
		/// Displays the folder browser dialog and updates the <see cref="TextBox.Text"/> property 
		/// of the specified <see cref="TextBox"/> control with the selected path.
		/// </summary>
		/// <param name="textBox">The textbox to update.</param>
		private void ShowFolderBrowserForTextBox(TextBox textBox)
		{
			try
			{
				using (var folderBrowserDialog = new FolderBrowserDialog())
				{
					folderBrowserDialog.SelectedPath = "C:\\";

					if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
					{
						textBox.Text = folderBrowserDialog.SelectedPath;
					}
				}
			}
            catch
            {
				ShowError(
					"There was an error showing the browse window.\r\n" +
					"Try running as an Administrator or manually typing in a path.");
			}
		}

		private void PathButton_Click(object sender, EventArgs e) 
			=> ShowFolderBrowserForTextBox(pathTextBox);

		private void LocalPathButton_Click(object sender, EventArgs e) 
			=> ShowFolderBrowserForTextBox(localPathTextBox);

		private void ExportPathButton_Click(object sender, EventArgs e) 
			=> ShowFolderBrowserForTextBox(exportPathTextBox);
		
		private void LetterMergeButton_Click(object sender, EventArgs e) =>
			ShowFolderBrowserForTextBox(letterMergePathTextBox);

		private void AcceptButton_Click(object sender, EventArgs e)
		{
			bool TryCreateDirectory(string path)
            {
                try
                {
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}

					return true;
				}
                catch (Exception exception)
                {
					ShowError(exception.Message);
                }

				return false;
			}

			var path = localPathTextBox.Text.Trim();
			if (path.Length > 0)
            {
				if (!TryCreateDirectory(path))
                {
					return;
                }
            }
			else
            {
				path = pathTextBox.Text.Trim();
				if (path.Length > 0)
                {
					if (!TryCreateDirectory(path))
                    {
						return;
                    }
                }
            }

			if (Prefs.Set(PrefName.DocPath, pathTextBox.Text) |
				Prefs.Set(PrefName.ExportPath, exportPathTextBox.Text) |
				Prefs.Set(PrefName.LetterMergePath, letterMergePathTextBox.Text))
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}

			if (OpenDentBusiness.FileIO.FileAtoZ.LocalAtoZpath != path)
			{
				ComputerPrefs.LocalComputer.AtoZpath = 
					OpenDentBusiness.FileIO.FileAtoZ.LocalAtoZpath = path;

				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			}

			DialogResult = DialogResult.OK;
		}
	}
}
