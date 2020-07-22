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
            this.addButton = new OpenDental.UI.Button();
            this.programsLabel = new System.Windows.Forms.Label();
            this.addLabel = new System.Windows.Forms.Label();
            this.programsGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(337, 523);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlus;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(12, 523);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 25);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "&Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // programsLabel
            // 
            this.programsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.programsLabel.Location = new System.Drawing.Point(12, 9);
            this.programsLabel.Name = "programsLabel";
            this.programsLabel.Size = new System.Drawing.Size(400, 40);
            this.programsLabel.TabIndex = 1;
            this.programsLabel.Text = "Double click on one of the programs in the\r\nlist below to enable it or change its" +
    " settings";
            // 
            // addLabel
            // 
            this.addLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addLabel.Location = new System.Drawing.Point(98, 520);
            this.addLabel.Name = "addLabel";
            this.addLabel.Size = new System.Drawing.Size(233, 32);
            this.addLabel.TabIndex = 4;
            this.addLabel.Text = "Do not Add unless you have a totally\r\ncustom bridge which we don\'t support.";
            this.addLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // programsGrid
            // 
            this.programsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.programsGrid.Location = new System.Drawing.Point(12, 52);
            this.programsGrid.Name = "programsGrid";
            this.programsGrid.Size = new System.Drawing.Size(400, 465);
            this.programsGrid.TabIndex = 2;
            this.programsGrid.Title = "Programs";
            this.programsGrid.TranslationName = "TablePrograms";
            this.programsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.ProgramsGrid_CellDoubleClick);
            // 
            // FormProgramLinks
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(424, 561);
            this.Controls.Add(this.addLabel);
            this.Controls.Add(this.programsLabel);
            this.Controls.Add(this.addButton);
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
		private OpenDental.UI.Button addButton;
		private System.Windows.Forms.Label programsLabel;
		private System.Windows.Forms.Label addLabel;
		private OpenDental.UI.ODGrid programsGrid;
	}
}
