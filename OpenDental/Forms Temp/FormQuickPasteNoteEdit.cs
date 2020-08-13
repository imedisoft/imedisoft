using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormQuickPasteNoteEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox textNote;
		private OpenDental.UI.Button butDelete;
		public QuickPasteNote QuickNote;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textAbbreviation;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;

		///<summary></summary>
		public FormQuickPasteNoteEdit(QuickPasteNote quickNote){
			//
			// Required for Windows Form Designer support
			//
			QuickNote=quickNote.Copy();
			InitializeComponent();
			Lan.F(this);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQuickPasteNoteEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.textNote = new System.Windows.Forms.TextBox();
			this.butDelete = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textAbbreviation = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(592, 514);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(497, 514);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 2;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// textNote
			// 
			this.textNote.Location = new System.Drawing.Point(38, 62);
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.Size = new System.Drawing.Size(628, 431);
			this.textNote.TabIndex = 1;
			this.textNote.TextChanged += new System.EventHandler(this.textNote_TextChanged);
			// 
			// butDelete
			// 
			this.butDelete.Image = global::Imedisoft.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(40, 514);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(79, 26);
			this.butDelete.TabIndex = 4;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(37, 44);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 15;
			this.label1.Text = "Note";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textAbbreviation
			// 
			this.textAbbreviation.Location = new System.Drawing.Point(38, 22);
			this.textAbbreviation.Name = "textAbbreviation";
			this.textAbbreviation.Size = new System.Drawing.Size(252, 20);
			this.textAbbreviation.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(37, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(157, 16);
			this.label2.TabIndex = 17;
			this.label2.Text = "Abbreviation";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(293, 13);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(365, 33);
			this.label3.TabIndex = 18;
			this.label3.Text = "If you type a ? immediately followed by the abbreviation, your note will be inser" +
    "ted";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// FormQuickPasteNoteEdit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(719, 564);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textAbbreviation);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormQuickPasteNoteEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Quick Paste Note";
			this.Load += new System.EventHandler(this.FormQuickPasteNoteEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormQuickPasteNoteEdit_Load(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AutoNoteQuickNoteEdit,true)) {
				textAbbreviation.ReadOnly=true;
				textAbbreviation.BackColor=SystemColors.Window;
				textNote.ReadOnly=true;
				textNote.BackColor=SystemColors.Window;
				butDelete.Enabled=false;
				butOK.Enabled=false;
			}
			textAbbreviation.Text=QuickNote.Abbreviation;
			textNote.Text=QuickNote.Note;
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(MessageBox.Show(Lan.G(this,"Delete note?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
				return;
			}
			QuickNote=null;//triggers an action in the calling form
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textAbbreviation.Text.Contains("?")) {
				MessageBox.Show("Question mark not allowed in abbreviation.  Use the question mark later when trying to insert a quick note.");
				return;
			}
			if(textAbbreviation.Text.Contains("*")) {
				MessageBox.Show("Asterisk character not allowed in abbreviation.  Use the asterisk later when trying to insert a quick note.");
				return;
			}
			QuickNote.Abbreviation=textAbbreviation.Text;
			if(QuickNote.Abbreviation!=""){
				string msgText=QuickPasteNotes.AbbrAlreadyInUse(QuickNote);
				if(!String.IsNullOrEmpty(msgText) && MessageBox.Show(msgText,Lan.G(this,"Warning"),MessageBoxButtons.YesNo)==DialogResult.No) {
					return;
				}
			}
			QuickNote.Note=textNote.Text;
			DialogResult=DialogResult.OK;
		}

		private void textNote_TextChanged(object sender,EventArgs e) {
			//We do not normally use TextChanged but we need to perform maintenance of the data when it loads, is saved, or is pasted to.
			textNote.Text=textNote.Text.Replace("\r\n","\n")//convert windows to \n
				.Replace("\r","\n")									//replace linux or other \n
																			//Mac is already \n
				.Replace("\n","\r\n");								//reset to Windows newline character		
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
			
	}
}




















