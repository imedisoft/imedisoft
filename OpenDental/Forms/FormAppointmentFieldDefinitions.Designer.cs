namespace Imedisoft.Forms
{
    partial class FormAppointmentFieldDefinitions
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
            this.appointmentFieldDefinitionsListBox = new System.Windows.Forms.ListBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.closeButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.setupButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // appointmentFieldDefinitionsListBox
            // 
            this.appointmentFieldDefinitionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.appointmentFieldDefinitionsListBox.IntegralHeight = false;
            this.appointmentFieldDefinitionsListBox.Location = new System.Drawing.Point(12, 110);
            this.appointmentFieldDefinitionsListBox.Name = "appointmentFieldDefinitionsListBox";
            this.appointmentFieldDefinitionsListBox.Size = new System.Drawing.Size(360, 258);
            this.appointmentFieldDefinitionsListBox.TabIndex = 3;
            this.appointmentFieldDefinitionsListBox.SelectedIndexChanged += new System.EventHandler(this.ApptFieldDefinitionsListBox_SelectedIndexChanged);
            this.appointmentFieldDefinitionsListBox.DoubleClick += new System.EventHandler(this.ApptFieldDefinitionsListBox_DoubleClick);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoLabel.Location = new System.Drawing.Point(12, 47);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(360, 60);
            this.infoLabel.TabIndex = 2;
            this.infoLabel.Text = "This is only for advanced users. This is a list of extra fields that you can set " +
    "up for appointments. After adding fields to this list, you can set the values in" +
    " an appointment edit window.";
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(292, 374);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(12, 375);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 4;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // setupButton
            // 
            this.setupButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.setupButton.Location = new System.Drawing.Point(12, 12);
            this.setupButton.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(80, 25);
            this.setupButton.TabIndex = 1;
            this.setupButton.Text = "&Setup";
            this.setupButton.Click += new System.EventHandler(this.SetupButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(48, 374);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormAppointmentFieldDefinitions
            // 
            this.ClientSize = new System.Drawing.Size(384, 411);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.setupButton);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.appointmentFieldDefinitionsListBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.addButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAppointmentFieldDefinitions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Appointment Field Definitions";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApptFieldDefs_FormClosing);
            this.Load += new System.EventHandler(this.FormApptFieldDefs_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private OpenDental.UI.Button closeButton;
		private System.Windows.Forms.ListBox appointmentFieldDefinitionsListBox;
		private OpenDental.UI.Button addButton;
		private System.Windows.Forms.Label infoLabel;
        private OpenDental.UI.Button setupButton;
        private OpenDental.UI.Button deleteButton;
    }
}
