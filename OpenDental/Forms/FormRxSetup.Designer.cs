namespace Imedisoft.Forms
{
	partial class FormRxSetup
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
            this.closeButton = new OpenDental.UI.Button();
            this.addButton = new OpenDental.UI.Button();
            this.duplicateButton = new OpenDental.UI.Button();
            this.rxDefsGrid = new OpenDental.UI.ODGrid();
            this.procCodeRequiredCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(912, 564);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.Add;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(108, 564);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(90, 25);
            this.addButton.TabIndex = 2;
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // duplicateButton
            // 
            this.duplicateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.duplicateButton.Image = global::Imedisoft.Properties.Resources.Add;
            this.duplicateButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.duplicateButton.Location = new System.Drawing.Point(12, 564);
            this.duplicateButton.Name = "duplicateButton";
            this.duplicateButton.Size = new System.Drawing.Size(90, 25);
            this.duplicateButton.TabIndex = 1;
            this.duplicateButton.Text = "Duplicate";
            this.duplicateButton.Click += new System.EventHandler(this.DuplicateButton_Click);
            // 
            // rxDefsGrid
            // 
            this.rxDefsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rxDefsGrid.Location = new System.Drawing.Point(12, 35);
            this.rxDefsGrid.Name = "rxDefsGrid";
            this.rxDefsGrid.Size = new System.Drawing.Size(980, 523);
            this.rxDefsGrid.TabIndex = 0;
            this.rxDefsGrid.Title = "Prescriptions";
            this.rxDefsGrid.TranslationName = "TableRxSetup";
            this.rxDefsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.RxDefsGrid_CellDoubleClick);
            // 
            // procCodeRequiredCheckBox
            // 
            this.procCodeRequiredCheckBox.AutoSize = true;
            this.procCodeRequiredCheckBox.Location = new System.Drawing.Point(12, 12);
            this.procCodeRequiredCheckBox.Name = "procCodeRequiredCheckBox";
            this.procCodeRequiredCheckBox.Size = new System.Drawing.Size(251, 17);
            this.procCodeRequiredCheckBox.TabIndex = 5;
            this.procCodeRequiredCheckBox.Text = "Procedure code required on some prescriptions";
            this.procCodeRequiredCheckBox.UseVisualStyleBackColor = true;
            this.procCodeRequiredCheckBox.Click += new System.EventHandler(this.ProcCodeRequiredCheckBox_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTrash;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(231, 564);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(90, 25);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Text = "Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormRxSetup
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(1004, 601);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.procCodeRequiredCheckBox);
            this.Controls.Add(this.rxDefsGrid);
            this.Controls.Add(this.duplicateButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.closeButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(820, 450);
            this.Name = "FormRxSetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rx Setup";
            this.Load += new System.EventHandler(this.FormRxSetup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private OpenDental.UI.Button addButton;
		private OpenDental.UI.Button duplicateButton;
		private OpenDental.UI.Button closeButton;
		private OpenDental.UI.ODGrid rxDefsGrid;
		private System.Windows.Forms.CheckBox procCodeRequiredCheckBox;
        private OpenDental.UI.Button deleteButton;
    }
}
