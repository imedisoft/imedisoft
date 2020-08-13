using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormRecallTypes:ODForm {
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button butSynch;
		private Label label1;
		private bool changed;
		private List<RecallType> _listRecallTypes;

		//public bool IsSelectionMode;
		//<summary>Only used if IsSelectionMode.  On OK, contains selected pharmacyNum.  Can be 0.  Can also be set ahead of time externally.</summary>
		//public int SelectedPharmacyNum;

		///<summary></summary>
		public FormRecallTypes()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecallTypes));
			this.label1 = new System.Windows.Forms.Label();
			this.butSynch = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(290,298);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(337,23);
			this.label1.TabIndex = 17;
			this.label1.Text = "Forces a resynchronization of recall for all patients.";
			// 
			// butSynch
			// 
			this.butSynch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSynch.Location = new System.Drawing.Point(211,292);
			this.butSynch.Name = "butSynch";
			this.butSynch.Size = new System.Drawing.Size(75,24);
			this.butSynch.TabIndex = 16;
			this.butSynch.Text = "Synch";
			this.butSynch.Click += new System.EventHandler(this.butSynch_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(17,12);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(842,262);
			this.gridMain.TabIndex = 11;
			this.gridMain.Title = "RecallTypes";
			this.gridMain.TranslationName = "TableRecallTypes";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butAdd
			// 
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Image = global::Imedisoft.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(17,292);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(80,24);
			this.butAdd.TabIndex = 10;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Location = new System.Drawing.Point(784,292);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75,24);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormRecallTypes
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(887,332);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butSynch);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRecallTypes";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Recall Types";
			this.Load += new System.EventHandler(this.FormRecallTypes_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRecallTypes_FormClosing);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRecallTypes_Load(object sender, System.EventArgs e) {
			/*if(IsSelectionMode){
				butClose.Text=Lan.g(this,"Cancel");
			}
			else{
				butOK.Visible=false;
				butNone.Visible=false;
			}*/
			FillGrid();
			/*if(SelectedPharmacyNum!=0){
				for(int i=0;i<PharmacyC.Listt.Count;i++){
					if(PharmacyC.Listt[i].PharmacyNum==SelectedPharmacyNum){
						gridMain.SetSelected(i,true);
						break;
					}
				}
			}*/
		}

		private void FillGrid(){
			RecallTypes.RefreshCache();
			_listRecallTypes=RecallTypes.GetDeepCopy();
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.G("TableRecallTypes","Description"),110);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("TableRecallTypes","Special Type"),110);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("TableRecallTypes","Triggers"),190);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("TableRecallTypes","Interval"),60);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("TableRecallTypes","Time Pattern"),90);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("TableRecallTypes","Procedures"),190);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			//string txt;
			for(int i=0;i<_listRecallTypes.Count;i++){
				row=new GridRow();
				row.Cells.Add(_listRecallTypes[i].Description);
				row.Cells.Add(RecallTypes.GetSpecialTypeStr(_listRecallTypes[i].RecallTypeNum));
				row.Cells.Add(GetStringForType(_listRecallTypes[i].RecallTypeNum));
				row.Cells.Add(_listRecallTypes[i].DefaultInterval.ToString());
				row.Cells.Add(_listRecallTypes[i].TimePattern);
				row.Cells.Add(_listRecallTypes[i].Procedures);
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private string GetStringForType(long recallTypeNum) {
			if(recallTypeNum==0){
				return "";
			}
			List<RecallTrigger> triggerList=RecallTriggers.GetForType(recallTypeNum);
			string retVal="";
			for(int i=0;i<triggerList.Count;i++){
				if(i>0){
					retVal+=",";
				}
				retVal+=ProcedureCodes.GetStringProcCode(triggerList[i].CodeNum);
			}
			return retVal;
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormRecallTypeEdit FormRE=new FormRecallTypeEdit();
			FormRE.RecallTypeCur=new RecallType();
			FormRE.RecallTypeCur.IsNew=true;
			FormRE.ShowDialog();
			FillGrid();
			changed=true;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			/*if(IsSelectionMode){
				SelectedPharmacyNum=PharmacyC.Listt[e.Row].PharmacyNum;
				DialogResult=DialogResult.OK;
				return;
			}
			else{*/
			FormRecallTypeEdit FormR=new FormRecallTypeEdit();
			FormR.RecallTypeCur=_listRecallTypes[e.Row].Copy();
			FormR.ShowDialog();
			FillGrid();
			changed=true;
			//}*/
		}

		private void butSynch_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			GC.Collect();//free up resources since this could take a lot of memory
			DataValid.SetInvalid(InvalidType.RecallTypes);
			Action actionCloseRecallSyncProgress=ODProgress.Show(EventCategory.RecallSync,typeof(RecallSyncEvent),Lan.G(this,"Running Prep Queries")+"...",false,true);
			bool isSyncCompleted=Recalls.SynchAllPatients();
			actionCloseRecallSyncProgress?.Invoke();
			GC.Collect();//clean up resources, force the garbage collector to collect since resources may remain tied-up
			Cursor=Cursors.Default;
			if(isSyncCompleted) {
				changed=false;
				MessageBox.Show("Done.");
			}
			else {
				MessageBox.Show("Synch is currently running from a different workstation.");
			}
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormRecallTypes_FormClosing(object sender,FormClosingEventArgs e) {
			if(changed){
				DataValid.SetInvalid(InvalidType.RecallTypes);
				if(MessageBox.Show(Lan.G(this,"Recalls for all patients should be synchronized.  Synchronize now?"),"",MessageBoxButtons.YesNo)
					==DialogResult.Yes)
				{
					Cursor=Cursors.WaitCursor;
					GC.Collect();//free up resources since this could take a lot of memory
					Action actionCloseRecallSyncProgress=ODProgress.Show(EventCategory.RecallSync,typeof(RecallSyncEvent),Lan.G(this,"Running Prep Queries")+"...");
					bool isSyncSuccessful=Recalls.SynchAllPatients();
					actionCloseRecallSyncProgress?.Invoke();
					GC.Collect();//clean up resources, force the garbage collector to collect since resources may remain tied-up
					Cursor=Cursors.Default;
					if(!isSyncSuccessful) {
						MessageBox.Show("Synch is currently running from a different workstation.  Recalls should be synchronized again later.");
					}
				}
			}
		}

	

	

		

		

		



		
	}
}




















