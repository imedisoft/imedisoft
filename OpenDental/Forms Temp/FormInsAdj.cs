using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormInsAdj : ODForm {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private OpenDental.ValidDate textDate;
		private OpenDental.ValidDouble textInsUsed;
		private OpenDental.ValidDouble textDedUsed;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label4;
		///<summary></summary>
		public bool IsNew;
		private OpenDental.UI.Button butDelete;
		private ClaimProc ClaimProcCur;
		private ClaimProc _claimProcOld;

		///<summary></summary>
		public FormInsAdj(ClaimProc claimProcCur){
			ClaimProcCur=claimProcCur;
			_claimProcOld=ClaimProcCur.Copy();
			InitializeComponent();
			
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsAdj));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textDate = new OpenDental.ValidDate();
			this.textInsUsed = new OpenDental.ValidDouble();
			this.textDedUsed = new OpenDental.ValidDouble();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(29,74);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100,14);
			this.label1.TabIndex = 0;
			this.label1.Text = "Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(15,130);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(114,14);
			this.label2.TabIndex = 1;
			this.label2.Text = "Deductible Used";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(17,102);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112,14);
			this.label3.TabIndex = 2;
			this.label3.Text = "Insurance Used";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(131,71);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(75,20);
			this.textDate.TabIndex = 0;
			// 
			// textInsUsed
			// 
			this.textInsUsed.Location = new System.Drawing.Point(131,99);
			this.textInsUsed.Name = "textInsUsed";
			this.textInsUsed.Size = new System.Drawing.Size(69,20);
			this.textInsUsed.TabIndex = 1;
			// 
			// textDedUsed
			// 
			this.textDedUsed.Location = new System.Drawing.Point(131,127);
			this.textDedUsed.Name = "textDedUsed";
			this.textDedUsed.Size = new System.Drawing.Size(69,20);
			this.textDedUsed.TabIndex = 2;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(260,148);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(260,180);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,24);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(32,12);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(259,41);
			this.label4.TabIndex = 5;
			this.label4.Text = "Make sure the date you use falls within the correct benefit year.";
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image = global::Imedisoft.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(18,180);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75,24);
			this.butDelete.TabIndex = 6;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// FormInsAdj
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(354,217);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.textDedUsed);
			this.Controls.Add(this.textInsUsed);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormInsAdj";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Insurance Adjustments";
			this.Load += new System.EventHandler(this.FormInsAdj_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormInsAdj_Load(object sender, System.EventArgs e) {
			textDate.Text=ClaimProcCur.ProcDate.ToShortDateString();
			textInsUsed.Text=ClaimProcCur.InsPayAmt.ToString("F");
			textDedUsed.Text=ClaimProcCur.DedApplied.ToString("F");
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show("All",MsgBoxButtons.OKCancel,"Delete?")) {
				return;
			}
			ClaimProcs.Delete(ClaimProcCur);
			InsEditPatLogs.MakeLogEntry(null,ClaimProcCur,InsEditPatLogType.Adjustment);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(  textDate.errorProvider1.GetError(textDate)!=""
				|| textInsUsed.errorProvider1.GetError(textInsUsed)!=""
				|| textDedUsed.errorProvider1.GetError(textDedUsed)!=""
				){
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
			ClaimProcCur.ProcDate=PIn.Date(textDate.Text);
			ClaimProcCur.InsPayAmt=PIn.Double(textInsUsed.Text);
			ClaimProcCur.DedApplied=PIn.Double(textDedUsed.Text);
			if(IsNew){
				ClaimProcs.Insert(ClaimProcCur);
				InsEditPatLogs.MakeLogEntry(ClaimProcCur,null,InsEditPatLogType.Adjustment);
			}
			else{
				ClaimProcs.Update(ClaimProcCur);
				InsEditPatLogs.MakeLogEntry(ClaimProcCur,_claimProcOld,InsEditPatLogType.Adjustment);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		
		
	}
}
