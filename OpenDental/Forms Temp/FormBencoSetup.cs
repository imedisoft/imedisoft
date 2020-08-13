﻿using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormBencoSetup:Form {
		private Program _prog;
		private List<ToolButItem> _listToolButs;

		public FormBencoSetup() {
			InitializeComponent();
			_prog=Programs.GetCur(ProgramName.BencoPracticeManagement);
			_listToolButs=ToolButItems.GetForProgram(_prog.Id); //Only set up for Main Toolbar
		}

		private void FormBencoSetup_Load(object sender,EventArgs e) {
			checkEnable.Checked=_prog.Enabled;
			textProgDesc.Text=_prog.Description;
			textPath.Text=_prog.Path;
			textButText.Text=_listToolButs.FirstOrDefault()?.ButtonText??"Benco";
			FillToolBars();
		}

		private void FillToolBars() {
			listToolBars.Items.Clear();
			foreach(ToolBarsAvail toolbar in Enum.GetValues(typeof(ToolBarsAvail))) {
				int index=listToolBars.Items.Add(toolbar);
				if(_listToolButs.Any(x => x.ToolBar==toolbar)) {
					listToolBars.SetSelected(index,true);
				}
			}
		}

		/// <summary>Updates some preferences in Open Dental according to the enabled state of the Benco bridge</summary>
		private void UpdateBencoSettings() {
			bool hasPrefChanged=false;
			string odTitle="Open Dental";
			string odSoftware="Open Dental Software";
			string bencoTitle="Benco Practice Management powered by Open Dental";
			string bencoSoftware="Benco Practice Management";
			if(_prog.Enabled) {
				if(Prefs.GetString(PrefName.MainWindowTitle)==odTitle) {
					hasPrefChanged|=Prefs.Set(PrefName.MainWindowTitle,bencoTitle);
				}
				if(Prefs.GetString(PrefName.SoftwareName)!=bencoSoftware) {
					hasPrefChanged|=Prefs.Set(PrefName.SoftwareName,bencoSoftware);
				}
			}
			else {
				if(Prefs.GetString(PrefName.MainWindowTitle)==bencoTitle) {
					hasPrefChanged|=Prefs.Set(PrefName.MainWindowTitle,odTitle);
				}
				if(Prefs.GetString(PrefName.SoftwareName)!=odSoftware) {
					hasPrefChanged|=Prefs.Set(PrefName.SoftwareName,odSoftware);
				}
			}
			if(hasPrefChanged) {
				Signalods.SetInvalid(InvalidType.Prefs);
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			//Program
			_prog.Enabled=checkEnable.Checked;
			_prog.Description=textProgDesc.Text;
			_prog.Path=textPath.Text;
			Programs.Update(_prog);
			//Toolbar button
			ToolButItems.DeleteAllForProgram(_prog.Id);
			foreach(ToolBarsAvail toolbar in listToolBars.SelectedItems) {
				ToolButItem newBut=new ToolButItem();
				newBut.ProgramNum=_prog.Id;
				newBut.ToolBar=toolbar;
				newBut.ButtonText=textButText.Text;
				ToolButItems.Insert(newBut);
			}
			//Update settings as necessary
			UpdateBencoSettings();
			MessageBox.Show("You will need to restart Open Dental for these changes to take effect.");
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}