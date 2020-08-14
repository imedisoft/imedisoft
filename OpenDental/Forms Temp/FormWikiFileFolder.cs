using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;

namespace OpenDental {
	public partial class FormWikiFileFolder:ODForm {
		public bool IsFolderMode;
		public string SelectedLink;

		public FormWikiFileFolder() {
			InitializeComponent();
			
		}

		private void FormWikiFileFolder_Load(object sender,EventArgs e) {
			if(IsFolderMode) {
				Text="Insert Folder Link";
			}
		}

		private void butBrowse_Click(object sender,EventArgs e) {
			if(IsFolderMode) {
				FolderBrowserDialog folderBD=new FolderBrowserDialog();
				if(folderBD.ShowDialog()!=DialogResult.OK) {
					return;
				}
				textLink.Text=folderBD.SelectedPath;
				return;
			}
			OpenFileDialog openFileD=new OpenFileDialog();
			if(openFileD.ShowDialog()!=DialogResult.OK) {
				return;
			}
			textLink.Text=openFileD.FileName;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(IsFolderMode){
				if(!Directory.Exists(textLink.Text)) {
					if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Folder does not exist. Continue anyway?")) {
						return;
					}
				}
			}
			else{//file mode
				if(!File.Exists(textLink.Text)) {
					if(!MsgBox.Show(MsgBoxButtons.OKCancel,"File does not exist. Continue anyway?")) {
						return;
					}
				}
			}
			SelectedLink=textLink.Text;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}