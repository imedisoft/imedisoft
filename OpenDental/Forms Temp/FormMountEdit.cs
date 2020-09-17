using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormMountEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private TextBox textDescription;
		private TextBox textNote;
		private Label label1;
		private Label label2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Label label3;
		private ListBox listCategory;
		private TextBox textTime;
		private Label label4;
		private ValidDate textDate;
		private Label label5;
		private Label label6;
		private Mount _mountCur=null;
		List<Definition> _listDefNumsImageCats;

		///<summary></summary>
		public FormMountEdit(Mount mount)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			_mountCur=mount;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMountEdit));
			this.textDescription = new System.Windows.Forms.TextBox();
			this.textNote = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.listCategory = new System.Windows.Forms.ListBox();
			this.textTime = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textDate = new OpenDental.ValidDate();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(232, 49);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(339, 20);
			this.textDescription.TabIndex = 2;
			// 
			// textNote
			// 
			this.textNote.Location = new System.Drawing.Point(232, 118);
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.Size = new System.Drawing.Size(339, 44);
			this.textNote.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(148, 49);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 18);
			this.label1.TabIndex = 4;
			this.label1.Text = "Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(148, 118);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(82, 18);
			this.label2.TabIndex = 5;
			this.label2.Text = "Note";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(438, 296);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(530, 296);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "Category";
			// 
			// listCategory
			// 
			this.listCategory.Location = new System.Drawing.Point(12, 26);
			this.listCategory.Name = "listCategory";
			this.listCategory.Size = new System.Drawing.Size(104, 290);
			this.listCategory.TabIndex = 6;
			// 
			// textTime
			// 
			this.textTime.Location = new System.Drawing.Point(232, 95);
			this.textTime.Name = "textTime";
			this.textTime.Size = new System.Drawing.Size(104, 20);
			this.textTime.TabIndex = 133;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(154, 98);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(77, 16);
			this.label4.TabIndex = 132;
			this.label4.Text = "Time";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(232, 72);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(104, 20);
			this.textDate.TabIndex = 130;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(149, 75);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(82, 18);
			this.label5.TabIndex = 131;
			this.label5.Text = "Date";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(154, 267);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(271, 49);
			this.label6.TabIndex = 134;
			this.label6.Text = "This information is only for the mount itself.  The individual images on the moun" +
    "t have their own information windows.";
			// 
			// FormMountEdit
			// 
			this.ClientSize = new System.Drawing.Size(617, 334);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textTime);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.listCategory);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormMountEdit";
			this.ShowInTaskbar = false;
			this.Text = "Mount Information";
			this.Load += new System.EventHandler(this.FormMountEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormMountEdit_Load(object sender,EventArgs e) {
			_listDefNumsImageCats=Definitions.GetDefsForCategory(DefinitionCategory.ImageCats,true);
			for(int i=0;i<_listDefNumsImageCats.Count;i++){
				listCategory.Items.Add(_listDefNumsImageCats[i].Name);
				if(_listDefNumsImageCats[i].Id==_mountCur.Category){
					listCategory.SelectedIndex=i;
				}
			}
			textDescription.Text=_mountCur.Description;
			textDate.Text=_mountCur.AddedOn.ToShortDateString();
			textTime.Text=_mountCur.AddedOn.ToShortTimeString();
			textNote.Text=_mountCur.Note;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(listCategory.SelectedIndex==-1){
				MessageBox.Show("Please select a category.");
				return;
			}
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
			if(textDate.Text=="") {
				MessageBox.Show("Please enter a date.");
				return;
			}
			if(textTime.Text=="") {
				MessageBox.Show("Please enter a time.");
				return;
			}
			DateTime time;
			if(!DateTime.TryParse(textTime.Text,out time)) {
				MessageBox.Show("Please enter a valid time.");
				return;
			}
			_mountCur.Category=_listDefNumsImageCats[listCategory.SelectedIndex].Id;
			_mountCur.Description=textDescription.Text;
			DateTime dateTimeEntered=PIn.Date(textDate.Text+" "+textTime.Text);
			_mountCur.AddedOn=dateTimeEntered;	
			_mountCur.Note=textNote.Text;
			Mounts.Update(_mountCur);
			//new mounts are never added here, so it's never an insert
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}





















