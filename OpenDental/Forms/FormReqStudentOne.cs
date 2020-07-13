using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormReqStudentOne : ODForm {
		private OpenDental.UI.Button butCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public long ProvNum;
		private OpenDental.UI.ODGrid gridMain;
		private Provider prov;
		private DataTable table;
		//public bool IsSelectionMode;
		//<summary>If IsSelectionMode and DialogResult.OK, then this will contain the selected req.</summary>
		//public int SelectedReqStudentNum;

		///<summary></summary>
		public FormReqStudentOne()
		{
			//
			// Required for Windows Form Designer support
			//
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReqStudentOne));
			this.butCancel = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(738,621);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// gridMain
			// 
			this.gridMain.Location = new System.Drawing.Point(19,12);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(698,635);
			this.gridMain.TabIndex = 2;
			this.gridMain.Title = "Student Requirements";
			this.gridMain.TranslationName = "TableReqStudentOne";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// FormReqStudentOne
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(825,665);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormReqStudentOne";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Student Requirements - One";
			this.Load += new System.EventHandler(this.FormReqStudentOne_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormReqStudentOne_Load(object sender,EventArgs e) {
			//if(IsSelectionMode){
				
			//}
			//else{
				//labelSelection.Visible=false;
				//butOK.Visible=false;
				//butCancel.Text=Lan.g(this,"Close");
			//}
			prov=Providers.GetProv(ProvNum);
			Text=Lan.G(this,"Student Requirements - ")+Providers.GetLongDesc(ProvNum);
			FillGrid();
		}

		private void FillGrid(){
			table=ReqStudents.RefreshOneStudent(ProvNum);
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.G("TableReqStudentOne","Course"),100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("TableReqStudentOne","Requirement"),200);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("TableReqStudentOne","Done"),40);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("TableReqStudentOne","Patient"),140);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("TableReqStudentOne","Appointment"),190);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<table.Rows.Count;i++){
				row=new GridRow();
				row.Cells.Add(table.Rows[i]["course"].ToString());
				row.Cells.Add(table.Rows[i]["requirement"].ToString());
				row.Cells.Add(table.Rows[i]["done"].ToString());
				row.Cells.Add(table.Rows[i]["patient"].ToString());
				row.Cells.Add(table.Rows[i]["appointment"].ToString());
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//if(IsSelectionMode){
			//	if(table.Rows[e.Row]["appointment"].ToString()!=""){
			//		MessageBox.Show("Already attached to an appointment.");
			//		return;
			//	}
			//	SelectedReqStudentNum=PIn.PInt(table.Rows[e.Row]["ReqStudentNum"].ToString());
			//	DialogResult=DialogResult.OK;
			//}
			//else{
				FormReqStudentEdit FormRSE=new FormReqStudentEdit();
				FormRSE.ReqCur=ReqStudents.GetOne(PIn.Long(table.Rows[e.Row]["ReqStudentNum"].ToString()));
				FormRSE.ShowDialog();
				if(FormRSE.DialogResult!=DialogResult.OK) {
					return;
				}
				FillGrid();
			//}
		}

		//private void butOK_Click(object sender, System.EventArgs e) {
			//not accessible
			/*if(IsSelectionMode){
				if(gridMain.GetSelectedIndex()==-1){
					MessageBox.Show("Please select a requirement first.");
					return;
				}
				if(table.Rows[gridMain.GetSelectedIndex()]["appointment"].ToString()!="") {
					MessageBox.Show("Selected requirement is already attached to an appointment.");
					return;
				}
				SelectedReqStudentNum=PIn.PInt(table.Rows[gridMain.GetSelectedIndex()]["ReqStudentNum"].ToString());
				DialogResult=DialogResult.OK;
			}*/
			//should never get to here.
		//}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		


	}
}





















