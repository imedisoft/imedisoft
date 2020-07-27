namespace OpenDental{
	partial class FormIncomeTransferManage {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIncomeTransferManage));
			this.gridImbalances = new OpenDental.UI.ODGrid();
			this.gridTransfers = new OpenDental.UI.ODGrid();
			this.butTransfer = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// gridImbalances
			// 
			this.gridImbalances.AllowSortingByColumn = true;
			this.gridImbalances.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridImbalances.HasDropDowns = true;
			this.gridImbalances.Location = new System.Drawing.Point(426, 12);
			this.gridImbalances.Name = "gridImbalances";
			this.gridImbalances.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridImbalances.Size = new System.Drawing.Size(657, 552);
			this.gridImbalances.TabIndex = 13;
			this.gridImbalances.Title = "Provider/Family Balances";
			this.gridImbalances.TranslationName = "TableImbalances";
			// 
			// gridTransfers
			// 
			this.gridTransfers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridTransfers.Location = new System.Drawing.Point(12, 12);
			this.gridTransfers.Name = "gridTransfers";
			this.gridTransfers.Size = new System.Drawing.Size(408, 552);
			this.gridTransfers.TabIndex = 12;
			this.gridTransfers.Title = "Existing Transfers (editable)";
			this.gridTransfers.TranslationName = "TableTransfers";
			this.gridTransfers.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTransfers_CellDoubleClick);
			// 
			// butTransfer
			// 
			this.butTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butTransfer.Image = global::Imedisoft.Properties.Resources.Left;
			this.butTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butTransfer.Location = new System.Drawing.Point(426, 570);
			this.butTransfer.Name = "butTransfer";
			this.butTransfer.Size = new System.Drawing.Size(90, 24);
			this.butTransfer.TabIndex = 18;
			this.butTransfer.Text = "Transfer";
			this.butTransfer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.butTransfer.Click += new System.EventHandler(this.butTransfer_Click);
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(1008, 570);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormIncomeTransferManage
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(1095, 606);
			this.Controls.Add(this.butTransfer);
			this.Controls.Add(this.gridImbalances);
			this.Controls.Add(this.gridTransfers);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(794, 500);
			this.Name = "FormIncomeTransferManage";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Income Transfer Manager";
			this.Load += new System.EventHandler(this.FormIncomeTransferManage_Load);
			this.ResumeLayout(false);

		}

		#endregion
		private OpenDental.UI.Button butClose;
		private UI.ODGrid gridTransfers;
		private UI.ODGrid gridImbalances;
		private UI.Button butTransfer;
	}
}