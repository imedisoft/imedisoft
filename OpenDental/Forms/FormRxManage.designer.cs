namespace Imedisoft.Forms
{
	partial class FormRxManage
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.rxPatGrid = new OpenDental.UI.ODGrid();
            this.newButton = new OpenDental.UI.Button();
            this.printSelectedButton = new OpenDental.UI.Button();
            this.closeButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // rxPatGrid
            // 
            this.rxPatGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rxPatGrid.Location = new System.Drawing.Point(12, 12);
            this.rxPatGrid.Name = "rxPatGrid";
            this.rxPatGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.rxPatGrid.Size = new System.Drawing.Size(1010, 436);
            this.rxPatGrid.TabIndex = 0;
            this.rxPatGrid.Title = "Prescriptions";
            this.rxPatGrid.TranslationName = "TableRxManage";
            this.rxPatGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.RxPatGrid_CellDoubleClick);
            // 
            // newButton
            // 
            this.newButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.newButton.Image = global::Imedisoft.Properties.Resources.IconPlus;
            this.newButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newButton.Location = new System.Drawing.Point(12, 454);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(80, 25);
            this.newButton.TabIndex = 1;
            this.newButton.Text = "&New Rx";
            this.newButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // printSelectedButton
            // 
            this.printSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.printSelectedButton.Image = global::Imedisoft.Properties.Resources.IconPrint;
            this.printSelectedButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.printSelectedButton.Location = new System.Drawing.Point(135, 454);
            this.printSelectedButton.Margin = new System.Windows.Forms.Padding(40, 3, 3, 3);
            this.printSelectedButton.Name = "printSelectedButton";
            this.printSelectedButton.Size = new System.Drawing.Size(120, 25);
            this.printSelectedButton.TabIndex = 2;
            this.printSelectedButton.Text = "&Print Selected";
            this.printSelectedButton.Click += new System.EventHandler(this.PrintSelectedButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(942, 454);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // FormRxManage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1034, 491);
            this.Controls.Add(this.rxPatGrid);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.printSelectedButton);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormRxManage";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rx Manage";
            this.Load += new System.EventHandler(this.FormRxManage_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.ODGrid rxPatGrid;
		private OpenDental.UI.Button printSelectedButton;
		private OpenDental.UI.Button closeButton;
		private OpenDental.UI.Button newButton;
	}
}
