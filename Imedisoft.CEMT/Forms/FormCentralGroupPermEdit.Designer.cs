namespace Imedisoft.CEMT.Forms
{
    partial class FormCentralGroupPermEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralGroupPermEdit));
            this.dateGroupBox = new System.Windows.Forms.GroupBox();
            this.dateTextBox = new System.Windows.Forms.TextBox();
            this.daysTextBox = new System.Windows.Forms.TextBox();
            this.dateLabel = new System.Windows.Forms.Label();
            this.daysLabel = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.dateGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateGroupBox
            // 
            this.dateGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateGroupBox.Controls.Add(this.dateTextBox);
            this.dateGroupBox.Controls.Add(this.daysTextBox);
            this.dateGroupBox.Controls.Add(this.dateLabel);
            this.dateGroupBox.Controls.Add(this.daysLabel);
            this.dateGroupBox.Controls.Add(this.infoLabel);
            this.dateGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dateGroupBox.Location = new System.Drawing.Point(13, 46);
            this.dateGroupBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 20);
            this.dateGroupBox.Name = "dateGroupBox";
            this.dateGroupBox.Size = new System.Drawing.Size(408, 114);
            this.dateGroupBox.TabIndex = 2;
            this.dateGroupBox.TabStop = false;
            this.dateGroupBox.Text = "Only if newer than";
            // 
            // dateTextBox
            // 
            this.dateTextBox.Location = new System.Drawing.Point(127, 19);
            this.dateTextBox.Name = "dateTextBox";
            this.dateTextBox.Size = new System.Drawing.Size(100, 20);
            this.dateTextBox.TabIndex = 1;
            this.dateTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DateTextBox_KeyDown);
            this.dateTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DateTextBox_Validating);
            // 
            // daysTextBox
            // 
            this.daysTextBox.Location = new System.Drawing.Point(127, 45);
            this.daysTextBox.Name = "daysTextBox";
            this.daysTextBox.Size = new System.Drawing.Size(50, 20);
            this.daysTextBox.TabIndex = 3;
            this.daysTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DaysTextBox_KeyDown);
            this.daysTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DaysTextBox_KeyPress);
            this.daysTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DaysTextBox_Validating);
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(91, 22);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(30, 13);
            this.dateLabel.TabIndex = 0;
            this.dateLabel.Text = "Date";
            // 
            // daysLabel
            // 
            this.daysLabel.AutoSize = true;
            this.daysLabel.Location = new System.Drawing.Point(90, 48);
            this.daysLabel.Name = "daysLabel";
            this.daysLabel.Size = new System.Drawing.Size(31, 13);
            this.daysLabel.TabIndex = 2;
            this.daysLabel.Text = "Days";
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(129, 68);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(273, 43);
            this.infoLabel.TabIndex = 4;
            this.infoLabel.Text = "For instance, if you set days to 1, then user will only have permission if the da" +
    "te of the item is today";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(140, 13);
            this.nameTextBox.MaxLength = 100;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ReadOnly = true;
            this.nameTextBox.Size = new System.Drawing.Size(260, 20);
            this.nameTextBox.TabIndex = 1;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(103, 16);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(31, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Type";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(341, 183);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(255, 183);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // FormCentralGroupPermEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(434, 221);
            this.Controls.Add(this.dateGroupBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCentralGroupPermEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Group Permission";
            this.Load += new System.EventHandler(this.FormCentralGroupPermEdit_Load);
            this.dateGroupBox.ResumeLayout(false);
            this.dateGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox dateGroupBox;
        private System.Windows.Forms.TextBox dateTextBox;
        private System.Windows.Forms.TextBox daysTextBox;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Label daysLabel;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label nameLabel;
        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button acceptButton;
    }
}
