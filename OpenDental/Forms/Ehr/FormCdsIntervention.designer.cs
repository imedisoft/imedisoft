namespace Imedisoft.Forms
{
	partial class FormCdsIntervention
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCdsIntervention));
            this.imageListInfoButton = new System.Windows.Forms.ImageList(this.components);
            this.cdsInterventionsGrid = new OpenDental.UI.ODGrid();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // imageListInfoButton
            // 
            this.imageListInfoButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListInfoButton.ImageStream")));
            this.imageListInfoButton.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListInfoButton.Images.SetKeyName(0, "iButton_16px.png");
            // 
            // cdsInterventionsGrid
            // 
            this.cdsInterventionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cdsInterventionsGrid.Location = new System.Drawing.Point(12, 12);
            this.cdsInterventionsGrid.Name = "cdsInterventionsGrid";
            this.cdsInterventionsGrid.Size = new System.Drawing.Size(920, 306);
            this.cdsInterventionsGrid.TabIndex = 1;
            this.cdsInterventionsGrid.Title = "Interventions";
            this.cdsInterventionsGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.CdsInterventionsGrid_CellClick);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.acceptButton.Location = new System.Drawing.Point(706, 324);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "&OK";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(792, 324);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(140, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel Current Action";
            // 
            // FormCdsIntervention
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(944, 361);
            this.Controls.Add(this.cdsInterventionsGrid);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "FormCdsIntervention";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clinical Decision Support Intervention";
            this.Load += new System.EventHandler(this.FormCDSIntervention_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.ODGrid cdsInterventionsGrid;
		private System.Windows.Forms.ImageList imageListInfoButton;
	}
}
