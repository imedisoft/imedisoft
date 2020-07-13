using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental.Forms
{
    public partial class FormPath : ODForm
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

			pathTextBox.Text = PrefC.GetString(PrefName.DocPath);

			localPathTextBox.Text = OpenDentBusiness.FileIO.FileAtoZ.LocalAtoZpath;//This was set on startup.  //compPref.AtoZpath;
			exportPathTextBox.Text = PrefC.GetString(PrefName.ExportPath);
			letterMergePathTextBox.Text = PrefC.GetString(PrefName.LetterMergePath);

			if (notConfigured)
			{
				MessageBox.Show("Could not find the path for the AtoZ folder.");

				if (Security.CurUser == null || !Security.IsAuthorized(Permissions.Setup))
				{
					//The user is still allowed to set the "Path override for this computer", thus the user has a way to temporariliy get into OD in worst case.
					//For example, if the primary folder path is wrong or has changed, the user can set the path override for this computer to get into OD, then
					//can to to Setup | Data Paths to fix the primary path.
					DisableMostControls();
					localPathTextBox.ReadOnly = false;
					localPathButton.Enabled = true;
					ActiveControl = localPathTextBox;//Focus on textLocalPath, since this is the only textbox the user can edit in this case.
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
				using (var f = new FolderBrowserDialog())
				{
					f.SelectedPath = "C:\\";

					if (f.ShowDialog(this) == DialogResult.OK)
					{
						textBox.Text = f.SelectedPath;
					}
				}
			}
            catch
            {
				MessageBox.Show(
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
			if (localPathTextBox.Text != "")
			{
				if (OpenDentBusiness.FileIO.FileAtoZ.GetValidPathFromString(localPathTextBox.Text) == null)
				{
					MessageBox.Show(
						"The path override for this computer is invalid. " +
						"The folder must exist and must contain all 26 A through Z folders.");

					return;
				}
			}
			else
			{
				if (OpenDentBusiness.FileIO.FileAtoZ.GetValidPathFromString(pathTextBox.Text) == null)
				{
					MessageBox.Show(
						"The path is invalid. " +
						"The folder must exist and must contain all 26 A through Z folders.");

					return;
				}
			}

			if (Prefs.UpdateString(PrefName.DocPath, pathTextBox.Text) |
				Prefs.UpdateString(PrefName.ExportPath, exportPathTextBox.Text) |
				Prefs.UpdateString(PrefName.LetterMergePath, letterMergePathTextBox.Text))
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}

			if (OpenDentBusiness.FileIO.FileAtoZ.LocalAtoZpath != localPathTextBox.Text)
			{//if local path changed
				OpenDentBusiness.FileIO.FileAtoZ.LocalAtoZpath = localPathTextBox.Text;
				//ComputerPref compPref=ComputerPrefs.GetForLocalComputer();
				ComputerPrefs.LocalComputer.AtoZpath = OpenDentBusiness.FileIO.FileAtoZ.LocalAtoZpath;
				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			}

			DialogResult = DialogResult.OK;
		}
	}
}
