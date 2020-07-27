using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	/// <summary></summary>
	public class FormImageSelect : ODForm{
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.ODGrid gridMain;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public bool OnlyShowImages;
		public long PatNum;
		private List<Document> _listDocuments;

		///<summary>If DialogResult==OK, then this will contain the new DocNum of the image we want.</summary>
		public long SelectedDocNum;

		///<summary></summary>
		public FormImageSelect()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImageSelect));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(505, 472);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(505, 513);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// gridMain
			// 
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(451, 527);
			this.gridMain.TabIndex = 2;
			this.gridMain.Title = "Images";
			this.gridMain.TranslationName = "FormImageSelect";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// FormImageSelect
			// 
			this.ClientSize = new System.Drawing.Size(632, 564);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormImageSelect";
			this.ShowInTaskbar = false;
			this.Text = "Select Image";
			this.Load += new System.EventHandler(this.FormImageSelect_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormImageSelect_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.G(this,"Date"),100);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G(this,"Category"),120);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G(this,"Description"),300);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			_listDocuments=Documents.GetAllWithPat(PatNum).ToList();
			if(OnlyShowImages){
				List<Document> listDocuments=new List<Document>();
				for(int i=0;i<_listDocuments.Count;i++) {
					if(Path.GetExtension(_listDocuments[i].FileName).ToLower()!=".pdf") {
						listDocuments.Add(_listDocuments[i]);
					}
				}
				_listDocuments=listDocuments;
			}
			for(int i=0;i<_listDocuments.Count;i++){
				row=new GridRow();
				row.Cells.Add(_listDocuments[i].DateCreated.ToString());
				row.Cells.Add(Defs.GetName(DefCat.ImageCats,_listDocuments[i].DocCategory));
			  row.Cells.Add(_listDocuments[i].Description);
				gridMain.ListGridRows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			SelectedDocNum=_listDocuments[e.Row].DocNum;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MessageBox.Show("Please select an image first.");
				return;
			}
			SelectedDocNum=_listDocuments[gridMain.GetSelectedIndex()].DocNum;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		


	}
}





















