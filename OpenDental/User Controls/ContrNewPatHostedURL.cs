﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using Imedisoft.Data;

namespace OpenDental.User_Controls {
	public partial class ContrNewPatHostedURL:UserControl {

		private bool _isExpanded;
		private WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService _signup;
		private const int LAUNCHWF_COL=4;//The launch webforms column of the options grid is auto-filled, keep track of its index
		private const int VERIFYTXT_COL=3;//The verify text column of the options grid can only be checked if texting is enabled, keep track of its index

		public bool IsExpanded
		{
			get { return _isExpanded; }
			set
			{
				_isExpanded=value;
				butExpander.Text=_isExpanded ? "-" : "+";
				Height=_isExpanded ? 175 : 25;
			}
		}

		///<summary>Set to true to hide the expander button.</summary>
		public bool DoHideExpandButton {
			set {
				butExpander.Visible=!value;
			}
		}

		private bool IsTextingEnabled {
			get {
			return (!PrefC.HasClinicsEnabled && SmsPhones.IsIntegratedTextingEnabled()) ||  
				(PrefC.HasClinicsEnabled && Clinics.IsTextingEnabled(Signup.ClinicNum));
			}
		}

		public WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService Signup
		{
			get { return _signup; }
			set { _signup=value; }
		}

		public ContrNewPatHostedURL(WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService signup) {
			InitializeComponent();
			IsExpanded=false;
			AddContextMenu(textWebFormToLaunch);
			AddContextMenu(textSchedulingURL);
			Signup=signup;
			FillControl();
		}

		public string GetPrefValue(string prefName) {
			switch(prefName) {
				case PreferenceName.WebSchedNewPatAllowChildren:
					return FromGridCell(0);
				case PreferenceName.WebSchedNewPatVerifyInfo:
					return FromGridCell(1);
				case PreferenceName.WebSchedNewPatDoAuthEmail:
					return FromGridCell(2);
				case PreferenceName.WebSchedNewPatDoAuthText:
					return FromGridCell(3);
				case PreferenceName.WebSchedNewPatWebFormsURL:
					return textWebFormToLaunch.Text;
				default: return "";
			}
		}

		public long GetClinicNum() {
			return Signup.ClinicNum;
		}

		private void FillControl() {
			labelClinicName.Text=Signup.ClinicNum!=0 ? Clinics.GetDescription(Signup.ClinicNum) : "Headquarters";
			labelEnabled.Text=Signup.IsEnabled ? "Enabled" : "Disabled";
			FillGrid();
		}

		private void FillGrid() {
			gridOptions.BeginUpdate();
			//Columns
			gridOptions.Columns.Clear();
			GridColumn col=new GridColumn("Allow Children",95,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			col=new GridColumn("Show Pre-Screen Questions",180,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			col=new GridColumn("Verify Email",85,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			col=new GridColumn("Verify Text",85,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			col=new GridColumn("Launch WebForm on Complete",200,HorizontalAlignment.Center);
			gridOptions.Columns.Add(col);
			//Rows
			gridOptions.Rows.Clear();
			GridRow row=new GridRow();
			row.Cells.Add(ToGridStr(ClinicPrefs.GetBool(Signup.ClinicNum, PreferenceName.WebSchedNewPatAllowChildren)));
			row.Cells.Add(ToGridStr(ClinicPrefs.GetBool(Signup.ClinicNum, PreferenceName.WebSchedNewPatVerifyInfo)));
			row.Cells.Add(ToGridStr(ClinicPrefs.GetBool(Signup.ClinicNum, PreferenceName.WebSchedNewPatDoAuthEmail)));
			row.Cells.Add(IsTextingEnabled?ToGridStr(ClinicPrefs.GetBool(Signup.ClinicNum, PreferenceName.WebSchedNewPatDoAuthText)):"");
			string url="";
			if(Signup.ClinicNum==0) { //HQ always uses pref.
				url=Preferences.GetString(PreferenceName.WebSchedNewPatWebFormsURL);
			}
			else { //Clinic should not default back to HQ version of URL. This is unlike typical ClinicPref behavior.
				var pref=ClinicPrefs.GetString(Signup.ClinicNum, PreferenceName.WebSchedNewPatWebFormsURL);			
				if(pref!=null) {
					url=pref;
				}
			}
			row.Cells.Add(ToGridStr(!string.IsNullOrWhiteSpace(url)));
			gridOptions.Rows.Add(row);
			gridOptions.EndUpdate();
			SetFormToLaunch(url);			
		}

		private string ToGridStr(bool value) {
			return value ? "X" : "";
		}

		private string FromGridCell(int cellIdx) {
			return gridOptions.Rows[0].Cells[cellIdx].Text=="X" ? "1" : "0";
		}

		private static void AddContextMenu(TextBox text) {
			if(text.ContextMenuStrip==null) {
				ContextMenuStrip menu=new ContextMenuStrip();
				ToolStripMenuItem browse = new ToolStripMenuItem("Browse");
        browse.Click += (sender, e) => {
					if(!string.IsNullOrWhiteSpace(text.Text)) {
						System.Diagnostics.Process.Start(text.Text);
					}
				};
        menu.Items.Add(browse);
        ToolStripMenuItem copy = new ToolStripMenuItem("Copy");
        copy.Click += (sender, e) => text.Copy();
        menu.Items.Add(copy);
        text.ContextMenuStrip = menu;
			}
		}

		private void SetFormToLaunch(string formURL) {
			textWebFormToLaunch.Text=formURL;
			string extraParams="";
			if(!string.IsNullOrWhiteSpace(formURL)) {
				gridOptions.Rows[0].Cells[LAUNCHWF_COL].Text="X";
				extraParams+="&WF=Y&ReturnURL="+formURL.Replace("&","%26");//encode &s so they aren't misinterpreted as separate parameters
			}
			else {
				gridOptions.Rows[0].Cells[LAUNCHWF_COL].Text="";
			}
			gridOptions.Refresh();
			textSchedulingURL.Text=Signup.HostedUrl+extraParams;
		}

		private void butExpander_Click(object sender,EventArgs e) {
			IsExpanded=!IsExpanded;
		}

		private void butEdit_Click(object sender,EventArgs e) {
			FormWebFormSetup formWFS;
			formWFS=new FormWebFormSetup(Signup.ClinicNum,textSchedulingURL.Text,true);
			formWFS.ShowDialog();
			if(formWFS.DialogResult==DialogResult.OK) {
				SetFormToLaunch(formWFS.SheetURLs);
			}
		}

		private void butCopy_Click(object sender,EventArgs e) {
			try {
				ODClipboard.Text = textSchedulingURL.Text;
			}
			catch(Exception ex) {
				FriendlyException.Show("Unable to copy to clipboard.",ex);
			}
		}

		private void gridOptions_CellClick(object sender,ODGridClickEventArgs e) {
			//Cell coordinates are [e.Row][e.Col]
			switch(e.Col) {
				case LAUNCHWF_COL: //This column is not checkable so just return and don't allow anything.
					return;
				case VERIFYTXT_COL:
					if(!IsTextingEnabled) {
						MessageBox.Show("Texting not enabled"+(PrefC.HasClinicsEnabled?" for this clinic":""));
						return;
					}
					break;
				default: break;
			}
			string cellTextCur=gridOptions.Rows[e.Row].Cells[e.Col].Text;
			string cellTextNew=(cellTextCur=="X" ? "" : "X");
			gridOptions.Rows[e.Row].Cells[e.Col].Text=cellTextNew;
			gridOptions.Refresh();
		}

		private void butClear_Click(object sender, EventArgs e)
		{
			var result = MessageBox.Show(this,
				"This will clear the formed URL and you will have to click Edit to create a new one. Continue?", 
				"Clear Webform URL",
				MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
            {
				SetFormToLaunch("");
			}
		}
	}
}
