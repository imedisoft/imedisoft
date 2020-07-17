namespace Imedisoft.CEMT.Forms
{
    partial class FormCentralPatientSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCentralPatientSearch));
            this.patientsGrid = new OpenDental.UI.ODGrid();
            this.searchGroupBox = new System.Windows.Forms.GroupBox();
            this.guarantorsCheckBox = new System.Windows.Forms.CheckBox();
            this.connectionTextBox = new System.Windows.Forms.TextBox();
            this.connectionLabel = new System.Windows.Forms.Label();
            this.limitCheckBox = new System.Windows.Forms.CheckBox();
            this.countryTextBox = new System.Windows.Forms.TextBox();
            this.countryLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.subscriberIdTextBox = new System.Windows.Forms.TextBox();
            this.subscriberIdLabel = new System.Windows.Forms.Label();
            this.birthdateTextBox = new System.Windows.Forms.TextBox();
            this.birthdateLabel = new System.Windows.Forms.Label();
            this.hideArchivedCheckBox = new System.Windows.Forms.CheckBox();
            this.chartNumberTextBox = new System.Windows.Forms.TextBox();
            this.ssnTextBox = new System.Windows.Forms.TextBox();
            this.ssnLabel = new System.Windows.Forms.Label();
            this.chartNumberLabel = new System.Windows.Forms.Label();
            this.patientNumberTextBox = new System.Windows.Forms.TextBox();
            this.patientNumberLabel = new System.Windows.Forms.Label();
            this.stateTextBox = new System.Windows.Forms.TextBox();
            this.stateLabel = new System.Windows.Forms.Label();
            this.cityTextBox = new System.Windows.Forms.TextBox();
            this.cityLabel = new System.Windows.Forms.Label();
            this.hideInactiveCheckBox = new System.Windows.Forms.CheckBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.addressLabel = new System.Windows.Forms.Label();
            this.phoneTextBox = new OpenDental.ValidPhone();
            this.phoneLabel = new System.Windows.Forms.Label();
            this.firstNameTextBox = new System.Windows.Forms.TextBox();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.lastNameTextBox = new System.Windows.Forms.TextBox();
            this.lastNameLabel = new System.Windows.Forms.Label();
            this.searchButton = new OpenDental.UI.Button();
            this.fetchLabel = new System.Windows.Forms.Label();
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.searchGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // patientsGrid
            // 
            this.patientsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patientsGrid.HScrollVisible = true;
            this.patientsGrid.Location = new System.Drawing.Point(13, 13);
            this.patientsGrid.Name = "patientsGrid";
            this.patientsGrid.Size = new System.Drawing.Size(522, 555);
            this.patientsGrid.TabIndex = 0;
            this.patientsGrid.Title = "Patients - Double Click to Launch Connection";
            this.patientsGrid.TranslationName = "FormPatientSelect";
            this.patientsGrid.WrapText = false;
            this.patientsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.PatientsGrid_CellDoubleClick);
            // 
            // searchGroupBox
            // 
            this.searchGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchGroupBox.Controls.Add(this.guarantorsCheckBox);
            this.searchGroupBox.Controls.Add(this.connectionTextBox);
            this.searchGroupBox.Controls.Add(this.connectionLabel);
            this.searchGroupBox.Controls.Add(this.limitCheckBox);
            this.searchGroupBox.Controls.Add(this.countryTextBox);
            this.searchGroupBox.Controls.Add(this.countryLabel);
            this.searchGroupBox.Controls.Add(this.emailTextBox);
            this.searchGroupBox.Controls.Add(this.emailLabel);
            this.searchGroupBox.Controls.Add(this.subscriberIdTextBox);
            this.searchGroupBox.Controls.Add(this.subscriberIdLabel);
            this.searchGroupBox.Controls.Add(this.birthdateTextBox);
            this.searchGroupBox.Controls.Add(this.birthdateLabel);
            this.searchGroupBox.Controls.Add(this.hideArchivedCheckBox);
            this.searchGroupBox.Controls.Add(this.chartNumberTextBox);
            this.searchGroupBox.Controls.Add(this.ssnTextBox);
            this.searchGroupBox.Controls.Add(this.ssnLabel);
            this.searchGroupBox.Controls.Add(this.chartNumberLabel);
            this.searchGroupBox.Controls.Add(this.patientNumberTextBox);
            this.searchGroupBox.Controls.Add(this.patientNumberLabel);
            this.searchGroupBox.Controls.Add(this.stateTextBox);
            this.searchGroupBox.Controls.Add(this.stateLabel);
            this.searchGroupBox.Controls.Add(this.cityTextBox);
            this.searchGroupBox.Controls.Add(this.cityLabel);
            this.searchGroupBox.Controls.Add(this.hideInactiveCheckBox);
            this.searchGroupBox.Controls.Add(this.infoLabel);
            this.searchGroupBox.Controls.Add(this.addressTextBox);
            this.searchGroupBox.Controls.Add(this.addressLabel);
            this.searchGroupBox.Controls.Add(this.phoneTextBox);
            this.searchGroupBox.Controls.Add(this.phoneLabel);
            this.searchGroupBox.Controls.Add(this.firstNameTextBox);
            this.searchGroupBox.Controls.Add(this.firstNameLabel);
            this.searchGroupBox.Controls.Add(this.lastNameTextBox);
            this.searchGroupBox.Controls.Add(this.lastNameLabel);
            this.searchGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.searchGroupBox.Location = new System.Drawing.Point(541, 13);
            this.searchGroupBox.Name = "searchGroupBox";
            this.searchGroupBox.Size = new System.Drawing.Size(230, 440);
            this.searchGroupBox.TabIndex = 1;
            this.searchGroupBox.TabStop = false;
            this.searchGroupBox.Text = "Search Criteria";
            // 
            // guarantorsCheckBox
            // 
            this.guarantorsCheckBox.AutoSize = true;
            this.guarantorsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.guarantorsCheckBox.Location = new System.Drawing.Point(6, 344);
            this.guarantorsCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.guarantorsCheckBox.Name = "guarantorsCheckBox";
            this.guarantorsCheckBox.Size = new System.Drawing.Size(140, 18);
            this.guarantorsCheckBox.TabIndex = 29;
            this.guarantorsCheckBox.Text = "Show Guarantors Only";
            // 
            // connectionTextBox
            // 
            this.connectionTextBox.Location = new System.Drawing.Point(120, 311);
            this.connectionTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 10);
            this.connectionTextBox.Name = "connectionTextBox";
            this.connectionTextBox.Size = new System.Drawing.Size(100, 20);
            this.connectionTextBox.TabIndex = 28;
            // 
            // connectionLabel
            // 
            this.connectionLabel.AutoSize = true;
            this.connectionLabel.Location = new System.Drawing.Point(53, 314);
            this.connectionLabel.Name = "connectionLabel";
            this.connectionLabel.Size = new System.Drawing.Size(61, 13);
            this.connectionLabel.TabIndex = 27;
            this.connectionLabel.Text = "Connection";
            // 
            // limitCheckBox
            // 
            this.limitCheckBox.AutoSize = true;
            this.limitCheckBox.Checked = true;
            this.limitCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.limitCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.limitCheckBox.Location = new System.Drawing.Point(6, 407);
            this.limitCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.limitCheckBox.Name = "limitCheckBox";
            this.limitCheckBox.Size = new System.Drawing.Size(184, 18);
            this.limitCheckBox.TabIndex = 32;
            this.limitCheckBox.Text = "Limit 30 patients per connection";
            // 
            // countryTextBox
            // 
            this.countryTextBox.Location = new System.Drawing.Point(120, 290);
            this.countryTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.countryTextBox.Name = "countryTextBox";
            this.countryTextBox.Size = new System.Drawing.Size(100, 20);
            this.countryTextBox.TabIndex = 26;
            // 
            // countryLabel
            // 
            this.countryLabel.AutoSize = true;
            this.countryLabel.Location = new System.Drawing.Point(68, 293);
            this.countryLabel.Name = "countryLabel";
            this.countryLabel.Size = new System.Drawing.Size(46, 13);
            this.countryLabel.TabIndex = 25;
            this.countryLabel.Text = "Country";
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(120, 269);
            this.emailTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(100, 20);
            this.emailTextBox.TabIndex = 24;
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(79, 272);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(35, 13);
            this.emailLabel.TabIndex = 23;
            this.emailLabel.Text = "E-mail";
            // 
            // subscriberIdTextBox
            // 
            this.subscriberIdTextBox.Location = new System.Drawing.Point(120, 248);
            this.subscriberIdTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.subscriberIdTextBox.Name = "subscriberIdTextBox";
            this.subscriberIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.subscriberIdTextBox.TabIndex = 22;
            // 
            // subscriberIdLabel
            // 
            this.subscriberIdLabel.AutoSize = true;
            this.subscriberIdLabel.Location = new System.Drawing.Point(43, 251);
            this.subscriberIdLabel.Name = "subscriberIdLabel";
            this.subscriberIdLabel.Size = new System.Drawing.Size(71, 13);
            this.subscriberIdLabel.TabIndex = 21;
            this.subscriberIdLabel.Text = "Subscriber ID";
            // 
            // birthdateTextBox
            // 
            this.birthdateTextBox.Location = new System.Drawing.Point(120, 227);
            this.birthdateTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.birthdateTextBox.Name = "birthdateTextBox";
            this.birthdateTextBox.Size = new System.Drawing.Size(100, 20);
            this.birthdateTextBox.TabIndex = 20;
            // 
            // birthdateLabel
            // 
            this.birthdateLabel.AutoSize = true;
            this.birthdateLabel.Location = new System.Drawing.Point(63, 230);
            this.birthdateLabel.Name = "birthdateLabel";
            this.birthdateLabel.Size = new System.Drawing.Size(51, 13);
            this.birthdateLabel.TabIndex = 19;
            this.birthdateLabel.Text = "Birthdate";
            // 
            // hideArchivedCheckBox
            // 
            this.hideArchivedCheckBox.AutoSize = true;
            this.hideArchivedCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.hideArchivedCheckBox.Location = new System.Drawing.Point(6, 386);
            this.hideArchivedCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.hideArchivedCheckBox.Name = "hideArchivedCheckBox";
            this.hideArchivedCheckBox.Size = new System.Drawing.Size(149, 18);
            this.hideArchivedCheckBox.TabIndex = 31;
            this.hideArchivedCheckBox.Text = "Hide Archived/Deceased";
            // 
            // chartNumberTextBox
            // 
            this.chartNumberTextBox.Location = new System.Drawing.Point(120, 206);
            this.chartNumberTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.chartNumberTextBox.Name = "chartNumberTextBox";
            this.chartNumberTextBox.Size = new System.Drawing.Size(100, 20);
            this.chartNumberTextBox.TabIndex = 18;
            // 
            // ssnTextBox
            // 
            this.ssnTextBox.Location = new System.Drawing.Point(120, 164);
            this.ssnTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.ssnTextBox.Name = "ssnTextBox";
            this.ssnTextBox.Size = new System.Drawing.Size(100, 20);
            this.ssnTextBox.TabIndex = 14;
            // 
            // ssnLabel
            // 
            this.ssnLabel.AutoSize = true;
            this.ssnLabel.Location = new System.Drawing.Point(88, 167);
            this.ssnLabel.Name = "ssnLabel";
            this.ssnLabel.Size = new System.Drawing.Size(26, 13);
            this.ssnLabel.TabIndex = 13;
            this.ssnLabel.Text = "SSN";
            // 
            // chartNumberLabel
            // 
            this.chartNumberLabel.AutoSize = true;
            this.chartNumberLabel.Location = new System.Drawing.Point(40, 209);
            this.chartNumberLabel.Name = "chartNumberLabel";
            this.chartNumberLabel.Size = new System.Drawing.Size(74, 13);
            this.chartNumberLabel.TabIndex = 17;
            this.chartNumberLabel.Text = "Chart Number";
            // 
            // patientNumberTextBox
            // 
            this.patientNumberTextBox.Location = new System.Drawing.Point(120, 185);
            this.patientNumberTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.patientNumberTextBox.Name = "patientNumberTextBox";
            this.patientNumberTextBox.Size = new System.Drawing.Size(100, 20);
            this.patientNumberTextBox.TabIndex = 16;
            // 
            // patientNumberLabel
            // 
            this.patientNumberLabel.AutoSize = true;
            this.patientNumberLabel.Location = new System.Drawing.Point(33, 188);
            this.patientNumberLabel.Name = "patientNumberLabel";
            this.patientNumberLabel.Size = new System.Drawing.Size(81, 13);
            this.patientNumberLabel.TabIndex = 15;
            this.patientNumberLabel.Text = "Patient Number";
            // 
            // stateTextBox
            // 
            this.stateTextBox.Location = new System.Drawing.Point(120, 143);
            this.stateTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.stateTextBox.Name = "stateTextBox";
            this.stateTextBox.Size = new System.Drawing.Size(100, 20);
            this.stateTextBox.TabIndex = 12;
            // 
            // stateLabel
            // 
            this.stateLabel.AutoSize = true;
            this.stateLabel.Location = new System.Drawing.Point(81, 146);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(33, 13);
            this.stateLabel.TabIndex = 11;
            this.stateLabel.Text = "State";
            // 
            // cityTextBox
            // 
            this.cityTextBox.Location = new System.Drawing.Point(120, 122);
            this.cityTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.cityTextBox.Name = "cityTextBox";
            this.cityTextBox.Size = new System.Drawing.Size(100, 20);
            this.cityTextBox.TabIndex = 10;
            // 
            // cityLabel
            // 
            this.cityLabel.AutoSize = true;
            this.cityLabel.Location = new System.Drawing.Point(88, 125);
            this.cityLabel.Name = "cityLabel";
            this.cityLabel.Size = new System.Drawing.Size(26, 13);
            this.cityLabel.TabIndex = 9;
            this.cityLabel.Text = "City";
            // 
            // hideInactiveCheckBox
            // 
            this.hideInactiveCheckBox.AutoSize = true;
            this.hideInactiveCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.hideInactiveCheckBox.Location = new System.Drawing.Point(6, 365);
            this.hideInactiveCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.hideInactiveCheckBox.Name = "hideInactiveCheckBox";
            this.hideInactiveCheckBox.Size = new System.Drawing.Size(137, 18);
            this.hideInactiveCheckBox.TabIndex = 30;
            this.hideInactiveCheckBox.Text = "Hide Inactive Patients";
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(6, 19);
            this.infoLabel.Margin = new System.Windows.Forms.Padding(3);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(179, 13);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "Hint: enter values in multiple boxes.";
            // 
            // addressTextBox
            // 
            this.addressTextBox.Location = new System.Drawing.Point(120, 101);
            this.addressTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(100, 20);
            this.addressTextBox.TabIndex = 8;
            // 
            // addressLabel
            // 
            this.addressLabel.AutoSize = true;
            this.addressLabel.Location = new System.Drawing.Point(68, 104);
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(46, 13);
            this.addressLabel.TabIndex = 7;
            this.addressLabel.Text = "Address";
            // 
            // phoneTextBox
            // 
            this.phoneTextBox.Location = new System.Drawing.Point(120, 80);
            this.phoneTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.Size = new System.Drawing.Size(100, 20);
            this.phoneTextBox.TabIndex = 6;
            // 
            // phoneLabel
            // 
            this.phoneLabel.AutoSize = true;
            this.phoneLabel.Location = new System.Drawing.Point(48, 83);
            this.phoneLabel.Name = "phoneLabel";
            this.phoneLabel.Size = new System.Drawing.Size(66, 13);
            this.phoneLabel.TabIndex = 5;
            this.phoneLabel.Text = "Phone (any)";
            // 
            // firstNameTextBox
            // 
            this.firstNameTextBox.Location = new System.Drawing.Point(120, 59);
            this.firstNameTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.firstNameTextBox.Name = "firstNameTextBox";
            this.firstNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.firstNameTextBox.TabIndex = 4;
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Location = new System.Drawing.Point(56, 62);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(58, 13);
            this.firstNameLabel.TabIndex = 3;
            this.firstNameLabel.Text = "First Name";
            // 
            // lastNameTextBox
            // 
            this.lastNameTextBox.Location = new System.Drawing.Point(120, 38);
            this.lastNameTextBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.lastNameTextBox.Name = "lastNameTextBox";
            this.lastNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.lastNameTextBox.TabIndex = 2;
            // 
            // lastNameLabel
            // 
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Location = new System.Drawing.Point(57, 41);
            this.lastNameLabel.Name = "lastNameLabel";
            this.lastNameLabel.Size = new System.Drawing.Size(57, 13);
            this.lastNameLabel.TabIndex = 1;
            this.lastNameLabel.Text = "Last Name";
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(691, 459);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(80, 25);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "Search";
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // fetchLabel
            // 
            this.fetchLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fetchLabel.AutoSize = true;
            this.fetchLabel.BackColor = System.Drawing.Color.Transparent;
            this.fetchLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fetchLabel.ForeColor = System.Drawing.Color.Red;
            this.fetchLabel.Location = new System.Drawing.Point(547, 465);
            this.fetchLabel.Name = "fetchLabel";
            this.fetchLabel.Size = new System.Drawing.Size(109, 13);
            this.fetchLabel.TabIndex = 2;
            this.fetchLabel.Text = "Fetching Results...";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(697, 543);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(697, 512);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // FormCentralPatientSearch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(784, 581);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.fetchLabel);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchGroupBox);
            this.Controls.Add(this.patientsGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCentralPatientSearch";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search Patients";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCentralPatientSearch_FormClosing);
            this.Load += new System.EventHandler(this.FormCentralPatientSearch_Load);
            this.searchGroupBox.ResumeLayout(false);
            this.searchGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.ODGrid patientsGrid;
        private System.Windows.Forms.GroupBox searchGroupBox;
        private System.Windows.Forms.TextBox countryTextBox;
        private System.Windows.Forms.Label countryLabel;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox subscriberIdTextBox;
        private System.Windows.Forms.Label subscriberIdLabel;
        private System.Windows.Forms.TextBox birthdateTextBox;
        private System.Windows.Forms.Label birthdateLabel;
        private System.Windows.Forms.CheckBox hideArchivedCheckBox;
        private System.Windows.Forms.TextBox chartNumberTextBox;
        private System.Windows.Forms.TextBox ssnTextBox;
        private System.Windows.Forms.Label ssnLabel;
        private System.Windows.Forms.Label chartNumberLabel;
        private System.Windows.Forms.TextBox patientNumberTextBox;
        private System.Windows.Forms.Label patientNumberLabel;
        private System.Windows.Forms.TextBox stateTextBox;
        private System.Windows.Forms.Label stateLabel;
        private System.Windows.Forms.TextBox cityTextBox;
        private System.Windows.Forms.Label cityLabel;
        private System.Windows.Forms.CheckBox hideInactiveCheckBox;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.Label addressLabel;
        private OpenDental.ValidPhone phoneTextBox;
        private System.Windows.Forms.Label phoneLabel;
        private System.Windows.Forms.TextBox firstNameTextBox;
        private System.Windows.Forms.Label firstNameLabel;
        private System.Windows.Forms.TextBox lastNameTextBox;
        private System.Windows.Forms.Label lastNameLabel;
        private OpenDental.UI.Button searchButton;
        private System.Windows.Forms.TextBox connectionTextBox;
        private System.Windows.Forms.Label connectionLabel;
        private System.Windows.Forms.CheckBox guarantorsCheckBox;
        private System.Windows.Forms.CheckBox limitCheckBox;
        private System.Windows.Forms.Label fetchLabel;
        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button acceptButton;
    }
}
