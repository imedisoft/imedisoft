namespace Imedisoft.Forms
{
    partial class FormRxDefEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRxDefEdit));
            this.drugLabel = new System.Windows.Forms.Label();
            this.notesLabel = new System.Windows.Forms.Label();
            this.refillsLabel = new System.Windows.Forms.Label();
            this.dispenseLabel = new System.Windows.Forms.Label();
            this.sigLabel = new System.Windows.Forms.Label();
            this.drugTextBox = new System.Windows.Forms.TextBox();
            this.notesTextBox = new System.Windows.Forms.TextBox();
            this.refillsTextBox = new System.Windows.Forms.TextBox();
            this.dispenseTextBox = new System.Windows.Forms.TextBox();
            this.sigTextBox = new System.Windows.Forms.TextBox();
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.alertsLabel = new System.Windows.Forms.Label();
            this.alertsListBox = new System.Windows.Forms.ListBox();
            this.controlledCheckBox = new System.Windows.Forms.CheckBox();
            this.addAllergyButton = new OpenDental.UI.Button();
            this.rxCuiLabel = new System.Windows.Forms.Label();
            this.rxCuiButton = new OpenDental.UI.Button();
            this.rxCuiTextBox = new System.Windows.Forms.TextBox();
            this.addProblemButton = new OpenDental.UI.Button();
            this.addMedicationButton = new OpenDental.UI.Button();
            this.procRequiredCheckBox = new System.Windows.Forms.CheckBox();
            this.patInstructionsTextBox = new OpenDental.ODtextBox();
            this.patInstructionsLabel = new System.Windows.Forms.Label();
            this.notesInfoLabel = new System.Windows.Forms.Label();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // drugLabel
            // 
            this.drugLabel.AutoSize = true;
            this.drugLabel.Location = new System.Drawing.Point(104, 15);
            this.drugLabel.Name = "drugLabel";
            this.drugLabel.Size = new System.Drawing.Size(30, 13);
            this.drugLabel.TabIndex = 0;
            this.drugLabel.Text = "Drug";
            // 
            // notesLabel
            // 
            this.notesLabel.AutoSize = true;
            this.notesLabel.Location = new System.Drawing.Point(99, 221);
            this.notesLabel.Name = "notesLabel";
            this.notesLabel.Size = new System.Drawing.Size(35, 13);
            this.notesLabel.TabIndex = 13;
            this.notesLabel.Text = "Notes";
            // 
            // refillsLabel
            // 
            this.refillsLabel.AutoSize = true;
            this.refillsLabel.Location = new System.Drawing.Point(99, 169);
            this.refillsLabel.Name = "refillsLabel";
            this.refillsLabel.Size = new System.Drawing.Size(35, 13);
            this.refillsLabel.TabIndex = 8;
            this.refillsLabel.Text = "Refills";
            // 
            // dispenseLabel
            // 
            this.dispenseLabel.AutoSize = true;
            this.dispenseLabel.Location = new System.Drawing.Point(84, 143);
            this.dispenseLabel.Name = "dispenseLabel";
            this.dispenseLabel.Size = new System.Drawing.Size(50, 13);
            this.dispenseLabel.TabIndex = 6;
            this.dispenseLabel.Text = "Dispense";
            // 
            // sigLabel
            // 
            this.sigLabel.AutoSize = true;
            this.sigLabel.Location = new System.Drawing.Point(113, 87);
            this.sigLabel.Name = "sigLabel";
            this.sigLabel.Size = new System.Drawing.Size(21, 13);
            this.sigLabel.TabIndex = 4;
            this.sigLabel.Text = "Sig";
            // 
            // drugTextBox
            // 
            this.drugTextBox.Location = new System.Drawing.Point(140, 12);
            this.drugTextBox.Name = "drugTextBox";
            this.drugTextBox.Size = new System.Drawing.Size(260, 20);
            this.drugTextBox.TabIndex = 1;
            // 
            // notesTextBox
            // 
            this.notesTextBox.AcceptsReturn = true;
            this.notesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.notesTextBox.Location = new System.Drawing.Point(140, 218);
            this.notesTextBox.Multiline = true;
            this.notesTextBox.Name = "notesTextBox";
            this.notesTextBox.Size = new System.Drawing.Size(482, 80);
            this.notesTextBox.TabIndex = 15;
            // 
            // refillsTextBox
            // 
            this.refillsTextBox.Location = new System.Drawing.Point(140, 166);
            this.refillsTextBox.Name = "refillsTextBox";
            this.refillsTextBox.Size = new System.Drawing.Size(80, 20);
            this.refillsTextBox.TabIndex = 9;
            // 
            // dispenseTextBox
            // 
            this.dispenseTextBox.Location = new System.Drawing.Point(140, 140);
            this.dispenseTextBox.Name = "dispenseTextBox";
            this.dispenseTextBox.Size = new System.Drawing.Size(80, 20);
            this.dispenseTextBox.TabIndex = 7;
            // 
            // sigTextBox
            // 
            this.sigTextBox.AcceptsReturn = true;
            this.sigTextBox.Location = new System.Drawing.Point(140, 84);
            this.sigTextBox.Multiline = true;
            this.sigTextBox.Name = "sigTextBox";
            this.sigTextBox.Size = new System.Drawing.Size(350, 50);
            this.sigTextBox.TabIndex = 5;
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(542, 513);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 25;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(542, 544);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 26;
            this.cancelButton.Text = "&Cancel";
            // 
            // alertsLabel
            // 
            this.alertsLabel.AutoSize = true;
            this.alertsLabel.Location = new System.Drawing.Point(99, 396);
            this.alertsLabel.Name = "alertsLabel";
            this.alertsLabel.Size = new System.Drawing.Size(35, 13);
            this.alertsLabel.TabIndex = 18;
            this.alertsLabel.Text = "Alerts";
            // 
            // alertsListBox
            // 
            this.alertsListBox.FormattingEnabled = true;
            this.alertsListBox.IntegralHeight = false;
            this.alertsListBox.Location = new System.Drawing.Point(140, 390);
            this.alertsListBox.Name = "alertsListBox";
            this.alertsListBox.Size = new System.Drawing.Size(160, 150);
            this.alertsListBox.Sorted = true;
            this.alertsListBox.TabIndex = 19;
            this.alertsListBox.SelectedIndexChanged += new System.EventHandler(this.AlertsListBox_SelectedIndexChanged);
            this.alertsListBox.DoubleClick += new System.EventHandler(this.AlertsListBox_DoubleClick);
            // 
            // controlledCheckBox
            // 
            this.controlledCheckBox.AutoSize = true;
            this.controlledCheckBox.Location = new System.Drawing.Point(140, 38);
            this.controlledCheckBox.Name = "controlledCheckBox";
            this.controlledCheckBox.Size = new System.Drawing.Size(128, 17);
            this.controlledCheckBox.TabIndex = 2;
            this.controlledCheckBox.Text = "Controlled Substance";
            this.controlledCheckBox.UseVisualStyleBackColor = true;
            // 
            // addAllergyButton
            // 
            this.addAllergyButton.Image = ((System.Drawing.Image)(resources.GetObject("addAllergyButton.Image")));
            this.addAllergyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addAllergyButton.Location = new System.Drawing.Point(306, 452);
            this.addAllergyButton.Name = "addAllergyButton";
            this.addAllergyButton.Size = new System.Drawing.Size(125, 25);
            this.addAllergyButton.TabIndex = 22;
            this.addAllergyButton.Text = "Add Allergy";
            this.addAllergyButton.Click += new System.EventHandler(this.AddAllergyButton_Click);
            // 
            // rxCuiLabel
            // 
            this.rxCuiLabel.AutoSize = true;
            this.rxCuiLabel.Location = new System.Drawing.Point(89, 195);
            this.rxCuiLabel.Name = "rxCuiLabel";
            this.rxCuiLabel.Size = new System.Drawing.Size(45, 13);
            this.rxCuiLabel.TabIndex = 10;
            this.rxCuiLabel.Text = "RxNorm";
            // 
            // rxCuiButton
            // 
            this.rxCuiButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rxCuiButton.Location = new System.Drawing.Point(597, 192);
            this.rxCuiButton.Name = "rxCuiButton";
            this.rxCuiButton.Size = new System.Drawing.Size(25, 20);
            this.rxCuiButton.TabIndex = 12;
            this.rxCuiButton.Text = "...";
            this.rxCuiButton.Click += new System.EventHandler(this.RxCuiButton_Click);
            // 
            // rxCuiTextBox
            // 
            this.rxCuiTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rxCuiTextBox.Location = new System.Drawing.Point(140, 192);
            this.rxCuiTextBox.Name = "rxCuiTextBox";
            this.rxCuiTextBox.ReadOnly = true;
            this.rxCuiTextBox.Size = new System.Drawing.Size(451, 20);
            this.rxCuiTextBox.TabIndex = 11;
            // 
            // addProblemButton
            // 
            this.addProblemButton.Image = ((System.Drawing.Image)(resources.GetObject("addProblemButton.Image")));
            this.addProblemButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addProblemButton.Location = new System.Drawing.Point(306, 390);
            this.addProblemButton.Name = "addProblemButton";
            this.addProblemButton.Size = new System.Drawing.Size(125, 25);
            this.addProblemButton.TabIndex = 20;
            this.addProblemButton.Text = "Add Problem";
            this.addProblemButton.Click += new System.EventHandler(this.AddProblemButton_Click);
            // 
            // addMedicationButton
            // 
            this.addMedicationButton.Image = ((System.Drawing.Image)(resources.GetObject("addMedicationButton.Image")));
            this.addMedicationButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addMedicationButton.Location = new System.Drawing.Point(306, 421);
            this.addMedicationButton.Name = "addMedicationButton";
            this.addMedicationButton.Size = new System.Drawing.Size(125, 25);
            this.addMedicationButton.TabIndex = 21;
            this.addMedicationButton.Text = "Add Medication";
            this.addMedicationButton.Click += new System.EventHandler(this.AddMedicationButton_Click);
            // 
            // procRequiredCheckBox
            // 
            this.procRequiredCheckBox.AutoSize = true;
            this.procRequiredCheckBox.Enabled = false;
            this.procRequiredCheckBox.Location = new System.Drawing.Point(140, 61);
            this.procRequiredCheckBox.Name = "procRequiredCheckBox";
            this.procRequiredCheckBox.Size = new System.Drawing.Size(105, 17);
            this.procRequiredCheckBox.TabIndex = 3;
            this.procRequiredCheckBox.Text = "Is Proc Required";
            this.procRequiredCheckBox.UseVisualStyleBackColor = true;
            // 
            // patInstructionsTextBox
            // 
            this.patInstructionsTextBox.AcceptsTab = true;
            this.patInstructionsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patInstructionsTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.patInstructionsTextBox.DetectLinksEnabled = false;
            this.patInstructionsTextBox.DetectUrls = false;
            this.patInstructionsTextBox.Location = new System.Drawing.Point(140, 304);
            this.patInstructionsTextBox.Name = "patInstructionsTextBox";
            this.patInstructionsTextBox.QuickPasteType = OpenDentBusiness.QuickPasteType.Rx;
            this.patInstructionsTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.patInstructionsTextBox.Size = new System.Drawing.Size(482, 80);
            this.patInstructionsTextBox.TabIndex = 17;
            this.patInstructionsTextBox.Text = "";
            // 
            // patInstructionsLabel
            // 
            this.patInstructionsLabel.AutoSize = true;
            this.patInstructionsLabel.Location = new System.Drawing.Point(33, 307);
            this.patInstructionsLabel.Name = "patInstructionsLabel";
            this.patInstructionsLabel.Size = new System.Drawing.Size(101, 13);
            this.patInstructionsLabel.TabIndex = 16;
            this.patInstructionsLabel.Text = "Patient Instructions";
            // 
            // notesInfoLabel
            // 
            this.notesInfoLabel.AutoSize = true;
            this.notesInfoLabel.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notesInfoLabel.Location = new System.Drawing.Point(21, 239);
            this.notesInfoLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.notesInfoLabel.Name = "notesInfoLabel";
            this.notesInfoLabel.Size = new System.Drawing.Size(113, 11);
            this.notesInfoLabel.TabIndex = 14;
            this.notesInfoLabel.Text = "(will not show on printout)";
            // 
            // deleteButton
            // 
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTrash;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(306, 483);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(125, 25);
            this.deleteButton.TabIndex = 27;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormRxDefEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(634, 581);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.patInstructionsTextBox);
            this.Controls.Add(this.patInstructionsLabel);
            this.Controls.Add(this.procRequiredCheckBox);
            this.Controls.Add(this.addMedicationButton);
            this.Controls.Add(this.addProblemButton);
            this.Controls.Add(this.rxCuiTextBox);
            this.Controls.Add(this.rxCuiButton);
            this.Controls.Add(this.rxCuiLabel);
            this.Controls.Add(this.addAllergyButton);
            this.Controls.Add(this.controlledCheckBox);
            this.Controls.Add(this.alertsListBox);
            this.Controls.Add(this.alertsLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.sigTextBox);
            this.Controls.Add(this.dispenseTextBox);
            this.Controls.Add(this.refillsTextBox);
            this.Controls.Add(this.notesTextBox);
            this.Controls.Add(this.drugTextBox);
            this.Controls.Add(this.sigLabel);
            this.Controls.Add(this.dispenseLabel);
            this.Controls.Add(this.refillsLabel);
            this.Controls.Add(this.notesInfoLabel);
            this.Controls.Add(this.notesLabel);
            this.Controls.Add(this.drugLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRxDefEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rx Template";
            this.Load += new System.EventHandler(this.FormRxDefEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Label drugLabel;
		private System.Windows.Forms.Label notesLabel;
		private System.Windows.Forms.Label refillsLabel;
		private System.Windows.Forms.Label dispenseLabel;
		private System.Windows.Forms.Label sigLabel;
		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.TextBox drugTextBox;
		private System.Windows.Forms.TextBox notesTextBox;
		private System.Windows.Forms.TextBox refillsTextBox;
		private System.Windows.Forms.TextBox dispenseTextBox;
		private System.Windows.Forms.TextBox sigTextBox;
		private System.Windows.Forms.Label alertsLabel;
		private System.Windows.Forms.ListBox alertsListBox;
		private System.Windows.Forms.CheckBox controlledCheckBox;
		private OpenDental.UI.Button addAllergyButton;
		private System.Windows.Forms.Label rxCuiLabel;
		private OpenDental.UI.Button rxCuiButton;
		private System.Windows.Forms.TextBox rxCuiTextBox;
		private OpenDental.UI.Button addProblemButton;
		private OpenDental.UI.Button addMedicationButton;
		private System.Windows.Forms.CheckBox procRequiredCheckBox;
		private OpenDental.ODtextBox patInstructionsTextBox;
		private System.Windows.Forms.Label patInstructionsLabel;
        private System.Windows.Forms.Label notesInfoLabel;
        private OpenDental.UI.Button deleteButton;
    }
}
