using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormPlansForFamily : ODForm {
		private OpenDental.UI.Button butClose;
		private ODGrid gridMain;
		private Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary>Set this externally.</summary>
		public Family FamCur;
		private List<InsurancePlan> PlanList;
		private List<InsSub> SubList;

		///<summary></summary>
		public FormPlansForFamily()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPlansForFamily));
			this.butClose = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(475,257);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75,26);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridMain
			// 
			this.gridMain.Location = new System.Drawing.Point(34,57);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(516,181);
			this.gridMain.TabIndex = 1;
			this.gridMain.Title = "Insurance Plans for Family";
			this.gridMain.TranslationName = "TableInsPlans";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(31,9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(462,35);
			this.label1.TabIndex = 2;
			this.label1.Text = "This is a list of all insurance plans for the family.  The main purpose is to vie" +
    "w inactive plans that have been dropped.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// FormPlansForFamily
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(585,300);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPlansForFamily";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Insurance Plans for Family";
			this.Load += new System.EventHandler(this.FormPlansForFamily_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormPlansForFamily_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			SubList=InsSubs.RefreshForFam(FamCur);
			if(!InsSubs.ValidatePlanNumForList(SubList.Select(x => x.InsSubNum).ToList())) {
				SubList=InsSubs.RefreshForFam(FamCur);
			}
			PlanList=InsPlans.RefreshForSubList(SubList);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			GridColumn col;
			//=new ODGridColumn("#",20);
			//gridMain.Columns.Add(col);
			col=new GridColumn("Subscriber",140);
			gridMain.Columns.Add(col);
			col=new GridColumn("Ins Carrier",100);
			gridMain.Columns.Add(col);
			col=new GridColumn("Date Effect.",90);
			gridMain.Columns.Add(col);
			col=new GridColumn("Date Term.",90);
			gridMain.Columns.Add(col);
			col=new GridColumn("Used By",90);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			GridRow row;
			//PatPlan[] patPlanArray;
			InsurancePlan plan;
			for(int i=0;i<SubList.Count;i++){
				plan=InsPlans.GetPlan(SubList[i].PlanNum,PlanList);
				row=new GridRow();
				row.Cells.Add(FamCur.GetNameInFamLF(SubList[i].Subscriber));
				row.Cells.Add(Carriers.GetName(plan.CarrierId));
				if(SubList[i].DateEffective.Year<1880)
					row.Cells.Add("");
				else
					row.Cells.Add(SubList[i].DateEffective.ToString("d"));
				if(SubList[i].DateTerm.Year<1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(SubList[i].DateTerm.ToString("d"));
				}
				int countPatPlans=PatPlans.GetCountBySubNum(SubList[i].InsSubNum);
				row.Cells.Add(countPatPlans.ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			InsurancePlan plan=InsPlans.GetPlan(SubList[e.Row].PlanNum,PlanList);
			FormInsPlan FormIP=new FormInsPlan(plan,null,SubList[e.Row]);
			FormIP.ShowDialog();
			FillGrid();
		}
		
		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		

		


		


	}
}





















