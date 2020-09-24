namespace Imedisoft.Forms
{
	partial class FormEhrElectronicCopy
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
            this.cancelButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.requestButton = new OpenDental.UI.Button();
            this.measureEventsGrid = new OpenDental.UI.ODGrid();
            this.requestLabel = new System.Windows.Forms.Label();
            this.showGroupBox = new System.Windows.Forms.GroupBox();
            this.showXhtmlButton = new OpenDental.UI.Button();
            this.showXmlButton = new OpenDental.UI.Button();
            this.providerGroupBox = new System.Windows.Forms.GroupBox();
            this.provideLabel = new System.Windows.Forms.Label();
            this.exportButton = new OpenDental.UI.Button();
            this.emailButton = new OpenDental.UI.Button();
            this.showGroupBox.SuspendLayout();
            this.providerGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(352, 444);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Close";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(12, 444);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 6;
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // requestButton
            // 
            this.requestButton.Location = new System.Drawing.Point(12, 12);
            this.requestButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.requestButton.Name = "requestButton";
            this.requestButton.Size = new System.Drawing.Size(100, 25);
            this.requestButton.TabIndex = 1;
            this.requestButton.Text = "Requested";
            this.requestButton.UseVisualStyleBackColor = true;
            this.requestButton.Click += new System.EventHandler(this.RequestButton_Click);
            // 
            // measureEventsGrid
            // 
            this.measureEventsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.measureEventsGrid.ColorSelectedRow = System.Drawing.SystemColors.Highlight;
            this.measureEventsGrid.Location = new System.Drawing.Point(12, 163);
            this.measureEventsGrid.Name = "measureEventsGrid";
            this.measureEventsGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.measureEventsGrid.Size = new System.Drawing.Size(420, 275);
            this.measureEventsGrid.TabIndex = 5;
            this.measureEventsGrid.Title = "History";
            this.measureEventsGrid.TranslationName = "TableHistory";
            this.measureEventsGrid.SelectionCommitted += new System.EventHandler(this.MeasureEventsGrid_SelectionCommitted);
            // 
            // requestLabel
            // 
            this.requestLabel.AutoSize = true;
            this.requestLabel.Location = new System.Drawing.Point(118, 18);
            this.requestLabel.Name = "requestLabel";
            this.requestLabel.Size = new System.Drawing.Size(53, 13);
            this.requestLabel.TabIndex = 2;
            this.requestLabel.Text = "(optional)";
            // 
            // showGroupBox
            // 
            this.showGroupBox.Controls.Add(this.showXhtmlButton);
            this.showGroupBox.Controls.Add(this.showXmlButton);
            this.showGroupBox.Location = new System.Drawing.Point(248, 50);
            this.showGroupBox.Name = "showGroupBox";
            this.showGroupBox.Size = new System.Drawing.Size(120, 100);
            this.showGroupBox.TabIndex = 4;
            this.showGroupBox.TabStop = false;
            this.showGroupBox.Text = "Show";
            // 
            // showXhtmlButton
            // 
            this.showXhtmlButton.Location = new System.Drawing.Point(15, 27);
            this.showXhtmlButton.Name = "showXhtmlButton";
            this.showXhtmlButton.Size = new System.Drawing.Size(90, 25);
            this.showXhtmlButton.TabIndex = 0;
            this.showXhtmlButton.Text = "Show XHTML";
            this.showXhtmlButton.UseVisualStyleBackColor = true;
            this.showXhtmlButton.Click += new System.EventHandler(this.ShowXhtml_Click);
            // 
            // showXmlButton
            // 
            this.showXmlButton.Location = new System.Drawing.Point(15, 58);
            this.showXmlButton.Name = "showXmlButton";
            this.showXmlButton.Size = new System.Drawing.Size(90, 25);
            this.showXmlButton.TabIndex = 1;
            this.showXmlButton.Text = "Show XML";
            this.showXmlButton.UseVisualStyleBackColor = true;
            this.showXmlButton.Click += new System.EventHandler(this.ShowXml_Click);
            // 
            // providerGroupBox
            // 
            this.providerGroupBox.Controls.Add(this.provideLabel);
            this.providerGroupBox.Controls.Add(this.exportButton);
            this.providerGroupBox.Controls.Add(this.emailButton);
            this.providerGroupBox.Location = new System.Drawing.Point(12, 50);
            this.providerGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.providerGroupBox.Name = "providerGroupBox";
            this.providerGroupBox.Size = new System.Drawing.Size(230, 100);
            this.providerGroupBox.TabIndex = 3;
            this.providerGroupBox.TabStop = false;
            this.providerGroupBox.Text = "Provide Electronic Copy to Patient";
            // 
            // provideLabel
            // 
            this.provideLabel.Location = new System.Drawing.Point(6, 47);
            this.provideLabel.Name = "provideLabel";
            this.provideLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.provideLabel.Size = new System.Drawing.Size(218, 50);
            this.provideLabel.TabIndex = 2;
            this.provideLabel.Text = "includes 2 files:\r\nccd.xml - the data\r\nccd.xsl - for human readable viewing";
            // 
            // exportButton
            // 
            this.exportButton.Image = global::Imedisoft.Properties.Resources.IconExport;
            this.exportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.exportButton.Location = new System.Drawing.Point(32, 19);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(80, 25);
            this.exportButton.TabIndex = 0;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // emailButton
            // 
            this.emailButton.Image = global::Imedisoft.Properties.Resources.IconEnvelope;
            this.emailButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.emailButton.Location = new System.Drawing.Point(118, 19);
            this.emailButton.Name = "emailButton";
            this.emailButton.Size = new System.Drawing.Size(80, 25);
            this.emailButton.TabIndex = 1;
            this.emailButton.Text = "E-mail";
            this.emailButton.UseVisualStyleBackColor = true;
            this.emailButton.Click += new System.EventHandler(this.EmailButton_Click);
            // 
            // FormEhrElectronicCopy
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(444, 481);
            this.Controls.Add(this.providerGroupBox);
            this.Controls.Add(this.showGroupBox);
            this.Controls.Add(this.requestButton);
            this.Controls.Add(this.requestLabel);
            this.Controls.Add(this.measureEventsGrid);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEhrElectronicCopy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Electronic Copy for Patient";
            this.Load += new System.EventHandler(this.FormElectronicCopy_Load);
            this.showGroupBox.ResumeLayout(false);
            this.providerGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.ODGrid measureEventsGrid;
		private OpenDental.UI.Button deleteButton;
		private OpenDental.UI.Button requestButton;
		private System.Windows.Forms.Label requestLabel;
		private System.Windows.Forms.GroupBox showGroupBox;
		private OpenDental.UI.Button showXhtmlButton;
		private OpenDental.UI.Button showXmlButton;
		private System.Windows.Forms.GroupBox providerGroupBox;
		private System.Windows.Forms.Label provideLabel;
		private OpenDental.UI.Button exportButton;
		private OpenDental.UI.Button emailButton;
	}
}
