namespace Imedisoft.Forms
{
    partial class FormProgramLinks
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

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgramLinks));
            this.closeButton = new OpenDental.UI.Button();
            this.programsLabel = new System.Windows.Forms.Label();
            this.programsGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(352, 524);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // programsLabel
            // 
            this.programsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.programsLabel.Location = new System.Drawing.Point(12, 9);
            this.programsLabel.Name = "programsLabel";
            this.programsLabel.Size = new System.Drawing.Size(420, 40);
            this.programsLabel.TabIndex = 1;
            this.programsLabel.Text = "Double click on one of the programs in the list below to enable it or change its " +
    "settings";
            // 
            // programsGrid
            // 
            this.programsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.programsGrid.Location = new System.Drawing.Point(12, 52);
            this.programsGrid.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.programsGrid.Name = "programsGrid";
            this.programsGrid.Size = new System.Drawing.Size(420, 459);
            this.programsGrid.TabIndex = 2;
            this.programsGrid.Title = "Programs";
            this.programsGrid.TranslationName = "TablePrograms";
            this.programsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.ProgramsGrid_CellDoubleClick);
            // 
            // FormProgramLinks
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(444, 561);
            this.Controls.Add(this.programsLabel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.programsGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 460);
            this.Name = "FormProgramLinks";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Program Links";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProgramLinks_Closing);
            this.Load += new System.EventHandler(this.FormProgramLinks_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button closeButton;
		private System.Windows.Forms.Label programsLabel;
		private OpenDental.UI.ODGrid programsGrid;
	}
}
