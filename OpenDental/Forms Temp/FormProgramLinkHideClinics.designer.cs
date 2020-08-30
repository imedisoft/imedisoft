namespace Imedisoft.Forms
{
    partial class FormProgramLinkHideClinics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgramLinkHideClinics));
            this.hiddenLabel = new System.Windows.Forms.Label();
            this.hiddenListBox = new System.Windows.Forms.ListBox();
            this.visibleListBox = new System.Windows.Forms.ListBox();
            this.visibleLabel = new System.Windows.Forms.Label();
            this.leftButton = new OpenDental.UI.Button();
            this.rightButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.restrictionLabel = new System.Windows.Forms.Label();
            this.clinicsLabel = new System.Windows.Forms.Label();
            this.acceptButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // hiddenLabel
            // 
            this.hiddenLabel.AutoSize = true;
            this.hiddenLabel.Location = new System.Drawing.Point(12, 72);
            this.hiddenLabel.Name = "hiddenLabel";
            this.hiddenLabel.Size = new System.Drawing.Size(40, 13);
            this.hiddenLabel.TabIndex = 2;
            this.hiddenLabel.Text = "Hidden";
            // 
            // hiddenListBox
            // 
            this.hiddenListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.hiddenListBox.FormattingEnabled = true;
            this.hiddenListBox.IntegralHeight = false;
            this.hiddenListBox.Location = new System.Drawing.Point(12, 88);
            this.hiddenListBox.Name = "hiddenListBox";
            this.hiddenListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.hiddenListBox.Size = new System.Drawing.Size(180, 350);
            this.hiddenListBox.TabIndex = 3;
            this.hiddenListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListboxHiddenClinics_MouseClick);
            // 
            // visibleListBox
            // 
            this.visibleListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.visibleListBox.FormattingEnabled = true;
            this.visibleListBox.IntegralHeight = false;
            this.visibleListBox.Location = new System.Drawing.Point(252, 88);
            this.visibleListBox.Name = "visibleListBox";
            this.visibleListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.visibleListBox.Size = new System.Drawing.Size(180, 350);
            this.visibleListBox.TabIndex = 7;
            this.visibleListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListboxVisibleClinics_MouseClick);
            // 
            // visibleLabel
            // 
            this.visibleLabel.AutoSize = true;
            this.visibleLabel.Location = new System.Drawing.Point(249, 72);
            this.visibleLabel.Name = "visibleLabel";
            this.visibleLabel.Size = new System.Drawing.Size(36, 13);
            this.visibleLabel.TabIndex = 6;
            this.visibleLabel.Text = "Visible";
            // 
            // leftButton
            // 
            this.leftButton.Image = global::Imedisoft.Properties.Resources.IconArrowLeft;
            this.leftButton.Location = new System.Drawing.Point(202, 251);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(40, 25);
            this.leftButton.TabIndex = 5;
            this.leftButton.Click += new System.EventHandler(this.butLeft_Click);
            // 
            // rightButton
            // 
            this.rightButton.Image = global::Imedisoft.Properties.Resources.IconArrowRight;
            this.rightButton.Location = new System.Drawing.Point(202, 220);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(40, 25);
            this.rightButton.TabIndex = 4;
            this.rightButton.Click += new System.EventHandler(this.butRight_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(352, 464);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "&Cancel";
            // 
            // restrictionLabel
            // 
            this.restrictionLabel.AutoSize = true;
            this.restrictionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(140)))));
            this.restrictionLabel.Location = new System.Drawing.Point(12, 49);
            this.restrictionLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.restrictionLabel.Name = "restrictionLabel";
            this.restrictionLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.restrictionLabel.Size = new System.Drawing.Size(254, 13);
            this.restrictionLabel.TabIndex = 1;
            this.restrictionLabel.Text = "Some clinics not shown due to user clinic restriction.";
            // 
            // clinicsLabel
            // 
            this.clinicsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clinicsLabel.ForeColor = System.Drawing.Color.Black;
            this.clinicsLabel.Location = new System.Drawing.Point(12, 9);
            this.clinicsLabel.Name = "clinicsLabel";
            this.clinicsLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.clinicsLabel.Size = new System.Drawing.Size(420, 40);
            this.clinicsLabel.TabIndex = 0;
            this.clinicsLabel.Text = "Program Link button will be hidden for clinics on the left, and visible for clini" +
    "cs on the right.";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(266, 464);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 9;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.butOK_Click);
            // 
            // FormProgramLinkHideClinics
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(444, 501);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.clinicsLabel);
            this.Controls.Add(this.restrictionLabel);
            this.Controls.Add(this.visibleListBox);
            this.Controls.Add(this.visibleLabel);
            this.Controls.Add(this.leftButton);
            this.Controls.Add(this.rightButton);
            this.Controls.Add(this.hiddenListBox);
            this.Controls.Add(this.hiddenLabel);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProgramLinkHideClinics";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Hide Program Link Button by Clinic";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.Button cancelButton;
        private System.Windows.Forms.Label hiddenLabel;
        private System.Windows.Forms.ListBox hiddenListBox;
        private OpenDental.UI.Button rightButton;
        private OpenDental.UI.Button leftButton;
        private System.Windows.Forms.ListBox visibleListBox;
        private System.Windows.Forms.Label visibleLabel;
        private System.Windows.Forms.Label restrictionLabel;
        private System.Windows.Forms.Label clinicsLabel;
        private OpenDental.UI.Button acceptButton;
    }
}
