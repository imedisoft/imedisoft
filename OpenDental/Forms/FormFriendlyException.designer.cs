namespace OpenDental
{
    partial class FormFriendlyException
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFriendlyException));
            this.closeButton = new OpenDental.UI.Button();
            this.detailsTabControl = new System.Windows.Forms.TabControl();
            this.stackTraceTabPage = new System.Windows.Forms.TabPage();
            this.stackTraceTextBox = new System.Windows.Forms.TextBox();
            this.queryTabPage = new System.Windows.Forms.TabPage();
            this.queryTextBox = new System.Windows.Forms.TextBox();
            this.printButton = new OpenDental.UI.Button();
            this.messageLabel = new System.Windows.Forms.Label();
            this.detailsLinkLabel = new System.Windows.Forms.Label();
            this.copyAllButton = new OpenDental.UI.Button();
            this.detailsTabControl.SuspendLayout();
            this.stackTraceTabPage.SuspendLayout();
            this.queryTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(392, 264);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // detailsTabControl
            // 
            this.detailsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.detailsTabControl.Controls.Add(this.stackTraceTabPage);
            this.detailsTabControl.Controls.Add(this.queryTabPage);
            this.detailsTabControl.Location = new System.Drawing.Point(12, 72);
            this.detailsTabControl.Name = "detailsTabControl";
            this.detailsTabControl.SelectedIndex = 0;
            this.detailsTabControl.Size = new System.Drawing.Size(460, 186);
            this.detailsTabControl.TabIndex = 2;
            // 
            // stackTraceTabPage
            // 
            this.stackTraceTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.stackTraceTabPage.Controls.Add(this.stackTraceTextBox);
            this.stackTraceTabPage.Location = new System.Drawing.Point(4, 22);
            this.stackTraceTabPage.Name = "stackTraceTabPage";
            this.stackTraceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.stackTraceTabPage.Size = new System.Drawing.Size(452, 160);
            this.stackTraceTabPage.TabIndex = 0;
            this.stackTraceTabPage.Text = "StackTrace";
            // 
            // stackTraceTextBox
            // 
            this.stackTraceTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stackTraceTextBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.stackTraceTextBox.Location = new System.Drawing.Point(3, 3);
            this.stackTraceTextBox.Multiline = true;
            this.stackTraceTextBox.Name = "stackTraceTextBox";
            this.stackTraceTextBox.ReadOnly = true;
            this.stackTraceTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.stackTraceTextBox.Size = new System.Drawing.Size(446, 154);
            this.stackTraceTextBox.TabIndex = 0;
            this.stackTraceTextBox.TabStop = false;
            // 
            // queryTabPage
            // 
            this.queryTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.queryTabPage.Controls.Add(this.queryTextBox);
            this.queryTabPage.Location = new System.Drawing.Point(4, 22);
            this.queryTabPage.Name = "queryTabPage";
            this.queryTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.queryTabPage.Size = new System.Drawing.Size(452, 160);
            this.queryTabPage.TabIndex = 1;
            this.queryTabPage.Text = "Query";
            // 
            // queryTextBox
            // 
            this.queryTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.queryTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryTextBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.queryTextBox.Location = new System.Drawing.Point(3, 3);
            this.queryTextBox.MaximumSize = new System.Drawing.Size(1200, 800);
            this.queryTextBox.Multiline = true;
            this.queryTextBox.Name = "queryTextBox";
            this.queryTextBox.ReadOnly = true;
            this.queryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.queryTextBox.Size = new System.Drawing.Size(446, 154);
            this.queryTextBox.TabIndex = 0;
            this.queryTextBox.TabStop = false;
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.printButton.Image = ((System.Drawing.Image)(resources.GetObject("printButton.Image")));
            this.printButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.printButton.Location = new System.Drawing.Point(166, 264);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(80, 25);
            this.printButton.TabIndex = 5;
            this.printButton.Text = "&Print";
            this.printButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // messageLabel
            // 
            this.messageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageLabel.Location = new System.Drawing.Point(12, 9);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(460, 60);
            this.messageLabel.TabIndex = 1;
            this.messageLabel.Text = "Friendly Error Message";
            this.messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // detailsLinkLabel
            // 
            this.detailsLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.detailsLinkLabel.AutoSize = true;
            this.detailsLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailsLinkLabel.ForeColor = System.Drawing.Color.Blue;
            this.detailsLinkLabel.Location = new System.Drawing.Point(16, 270);
            this.detailsLinkLabel.Name = "detailsLinkLabel";
            this.detailsLinkLabel.Size = new System.Drawing.Size(45, 15);
            this.detailsLinkLabel.TabIndex = 3;
            this.detailsLinkLabel.Text = "Details";
            this.detailsLinkLabel.Click += new System.EventHandler(this.DetailsLinkLabel_Click);
            // 
            // copyAllButton
            // 
            this.copyAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyAllButton.Location = new System.Drawing.Point(80, 264);
            this.copyAllButton.Name = "copyAllButton";
            this.copyAllButton.Size = new System.Drawing.Size(80, 25);
            this.copyAllButton.TabIndex = 4;
            this.copyAllButton.Text = "Copy All";
            this.copyAllButton.Click += new System.EventHandler(this.CopyAllButton_Click);
            // 
            // FormFriendlyException
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(484, 301);
            this.Controls.Add(this.detailsTabControl);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.copyAllButton);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.detailsLinkLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFriendlyException";
            this.Text = "Error Encountered";
            this.Load += new System.EventHandler(this.FormFriendlyException_Load);
            this.detailsTabControl.ResumeLayout(false);
            this.stackTraceTabPage.ResumeLayout(false);
            this.stackTraceTabPage.PerformLayout();
            this.queryTabPage.ResumeLayout(false);
            this.queryTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.Button closeButton;
        private System.Windows.Forms.Label detailsLinkLabel;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.TextBox stackTraceTextBox;
        private System.Windows.Forms.TabControl detailsTabControl;
        private System.Windows.Forms.TabPage stackTraceTabPage;
        private System.Windows.Forms.TabPage queryTabPage;
        private System.Windows.Forms.TextBox queryTextBox;
        private OpenDental.UI.Button printButton;
        private OpenDental.UI.Button copyAllButton;
    }
}
