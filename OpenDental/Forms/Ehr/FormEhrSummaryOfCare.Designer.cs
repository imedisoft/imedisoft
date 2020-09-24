namespace Imedisoft.Forms
{
	partial class FormEhrSummaryOfCare
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
            this.receiveEmailButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.showXhtmlButton = new OpenDental.UI.Button();
            this.showXmlButton = new OpenDental.UI.Button();
            this.showGroupBox = new System.Windows.Forms.GroupBox();
            this.receivedGrid = new OpenDental.UI.ODGrid();
            this.sentGrid = new OpenDental.UI.ODGrid();
            this.receiveGroupBox = new System.Windows.Forms.GroupBox();
            this.importButton = new OpenDental.UI.Button();
            this.sendGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.exportButton = new OpenDental.UI.Button();
            this.sendEmailButton = new OpenDental.UI.Button();
            this.showGroupBox.SuspendLayout();
            this.receiveGroupBox.SuspendLayout();
            this.sendGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(412, 484);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Close";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // receiveEmailButton
            // 
            this.receiveEmailButton.Image = global::Imedisoft.Properties.Resources.IconEnvelope;
            this.receiveEmailButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.receiveEmailButton.Location = new System.Drawing.Point(113, 19);
            this.receiveEmailButton.Name = "receiveEmailButton";
            this.receiveEmailButton.Size = new System.Drawing.Size(90, 25);
            this.receiveEmailButton.TabIndex = 1;
            this.receiveEmailButton.Text = "E-mail";
            this.receiveEmailButton.UseVisualStyleBackColor = true;
            this.receiveEmailButton.Click += new System.EventHandler(this.ReceiveEmailButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(12, 484);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // showXhtmlButton
            // 
            this.showXhtmlButton.Location = new System.Drawing.Point(27, 19);
            this.showXhtmlButton.Name = "showXhtmlButton";
            this.showXhtmlButton.Size = new System.Drawing.Size(80, 25);
            this.showXhtmlButton.TabIndex = 0;
            this.showXhtmlButton.Text = "Show XHTML";
            this.showXhtmlButton.UseVisualStyleBackColor = true;
            this.showXhtmlButton.Click += new System.EventHandler(this.ShowXhtmlButton_Click);
            // 
            // showXmlButton
            // 
            this.showXmlButton.Location = new System.Drawing.Point(113, 19);
            this.showXmlButton.Name = "showXmlButton";
            this.showXmlButton.Size = new System.Drawing.Size(80, 25);
            this.showXmlButton.TabIndex = 1;
            this.showXmlButton.Text = "Show XML";
            this.showXmlButton.UseVisualStyleBackColor = true;
            this.showXmlButton.Click += new System.EventHandler(this.ShowXmlButton_Click);
            // 
            // showGroupBox
            // 
            this.showGroupBox.Controls.Add(this.showXhtmlButton);
            this.showGroupBox.Controls.Add(this.showXmlButton);
            this.showGroupBox.Location = new System.Drawing.Point(12, 118);
            this.showGroupBox.Name = "showGroupBox";
            this.showGroupBox.Size = new System.Drawing.Size(220, 60);
            this.showGroupBox.TabIndex = 2;
            this.showGroupBox.TabStop = false;
            this.showGroupBox.Text = "Show";
            // 
            // receivedGrid
            // 
            this.receivedGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.receivedGrid.ColorSelectedRow = System.Drawing.SystemColors.Highlight;
            this.receivedGrid.Location = new System.Drawing.Point(272, 78);
            this.receivedGrid.Name = "receivedGrid";
            this.receivedGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.receivedGrid.Size = new System.Drawing.Size(220, 400);
            this.receivedGrid.TabIndex = 6;
            this.receivedGrid.Title = "Received";
            this.receivedGrid.TranslationName = "TableReceived";
            this.receivedGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.ReceivedGrid_CellDoubleClick);
            // 
            // sentGrid
            // 
            this.sentGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.sentGrid.ColorSelectedRow = System.Drawing.SystemColors.Highlight;
            this.sentGrid.Location = new System.Drawing.Point(12, 184);
            this.sentGrid.Name = "sentGrid";
            this.sentGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.sentGrid.Size = new System.Drawing.Size(220, 294);
            this.sentGrid.TabIndex = 3;
            this.sentGrid.Title = "Sent";
            this.sentGrid.TranslationName = "TableSent";
            this.sentGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.SentGrid_CellDoubleClick);
            this.sentGrid.SelectionCommitted += new System.EventHandler(this.SentGrid_SelectionCommitted);
            // 
            // receiveGroupBox
            // 
            this.receiveGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.receiveGroupBox.Controls.Add(this.importButton);
            this.receiveGroupBox.Controls.Add(this.receiveEmailButton);
            this.receiveGroupBox.Location = new System.Drawing.Point(272, 12);
            this.receiveGroupBox.Name = "receiveGroupBox";
            this.receiveGroupBox.Size = new System.Drawing.Size(220, 60);
            this.receiveGroupBox.TabIndex = 5;
            this.receiveGroupBox.TabStop = false;
            this.receiveGroupBox.Text = "Receive by";
            // 
            // importButton
            // 
            this.importButton.Image = global::Imedisoft.Properties.Resources.IconFileImport;
            this.importButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.importButton.Location = new System.Drawing.Point(17, 19);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(90, 25);
            this.importButton.TabIndex = 0;
            this.importButton.Text = "File Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // sendGroupBox
            // 
            this.sendGroupBox.Controls.Add(this.label1);
            this.sendGroupBox.Controls.Add(this.exportButton);
            this.sendGroupBox.Controls.Add(this.sendEmailButton);
            this.sendGroupBox.Location = new System.Drawing.Point(12, 12);
            this.sendGroupBox.Name = "sendGroupBox";
            this.sendGroupBox.Size = new System.Drawing.Size(220, 100);
            this.sendGroupBox.TabIndex = 1;
            this.sendGroupBox.TabStop = false;
            this.sendGroupBox.Text = "Send Summary of Care to Doctor";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 50);
            this.label1.TabIndex = 2;
            this.label1.Text = "includes 2 files:\r\nccd.xml - the data\r\nccd.xsl - for human readable viewing";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // exportButton
            // 
            this.exportButton.Image = global::Imedisoft.Properties.Resources.IconExport;
            this.exportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.exportButton.Location = new System.Drawing.Point(17, 19);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(90, 25);
            this.exportButton.TabIndex = 0;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // sendEmailButton
            // 
            this.sendEmailButton.Image = global::Imedisoft.Properties.Resources.IconEnvelope;
            this.sendEmailButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sendEmailButton.Location = new System.Drawing.Point(113, 19);
            this.sendEmailButton.Name = "sendEmailButton";
            this.sendEmailButton.Size = new System.Drawing.Size(90, 25);
            this.sendEmailButton.TabIndex = 1;
            this.sendEmailButton.Text = "E-mail";
            this.sendEmailButton.UseVisualStyleBackColor = true;
            this.sendEmailButton.Click += new System.EventHandler(this.SendEmailButton_Click);
            // 
            // FormEhrSummaryOfCare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 521);
            this.Controls.Add(this.sendGroupBox);
            this.Controls.Add(this.receiveGroupBox);
            this.Controls.Add(this.showGroupBox);
            this.Controls.Add(this.receivedGrid);
            this.Controls.Add(this.sentGrid);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEhrSummaryOfCare";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Summary of Care";
            this.Load += new System.EventHandler(this.FormSummaryOfCare_Load);
            this.showGroupBox.ResumeLayout(false);
            this.receiveGroupBox.ResumeLayout(false);
            this.sendGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.Button receiveEmailButton;
		private OpenDental.UI.ODGrid sentGrid;
		private OpenDental.UI.Button deleteButton;
		private OpenDental.UI.ODGrid receivedGrid;
		private OpenDental.UI.Button showXhtmlButton;
		private OpenDental.UI.Button showXmlButton;
		private System.Windows.Forms.GroupBox showGroupBox;
		private System.Windows.Forms.GroupBox receiveGroupBox;
		private OpenDental.UI.Button importButton;
		private System.Windows.Forms.GroupBox sendGroupBox;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button exportButton;
		private OpenDental.UI.Button sendEmailButton;
	}
}
