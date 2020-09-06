namespace Imedisoft.Forms
{
    partial class FormAdjustmentPicker
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
            this.acceptButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.unattachedCheckBox = new System.Windows.Forms.CheckBox();
            this.adjustmentsGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(326, 444);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 3;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(412, 444);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // unattachedCheckBox
            // 
            this.unattachedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.unattachedCheckBox.AutoSize = true;
            this.unattachedCheckBox.Location = new System.Drawing.Point(12, 449);
            this.unattachedCheckBox.Name = "unattachedCheckBox";
            this.unattachedCheckBox.Size = new System.Drawing.Size(136, 17);
            this.unattachedCheckBox.TabIndex = 2;
            this.unattachedCheckBox.Text = "Show Unattached Only";
            this.unattachedCheckBox.UseVisualStyleBackColor = true;
            this.unattachedCheckBox.Click += new System.EventHandler(this.UnattachedCheckBox_Click);
            // 
            // adjustmentsGrid
            // 
            this.adjustmentsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adjustmentsGrid.Location = new System.Drawing.Point(12, 12);
            this.adjustmentsGrid.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.adjustmentsGrid.Name = "adjustmentsGrid";
            this.adjustmentsGrid.Size = new System.Drawing.Size(480, 419);
            this.adjustmentsGrid.TabIndex = 1;
            this.adjustmentsGrid.Title = "Adjustments";
            this.adjustmentsGrid.TranslationName = "TableAjdustmentPicker";
            this.adjustmentsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.AdjustmentsGrid_CellDoubleClick);
            // 
            // FormAdjustmentPicker
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(504, 481);
            this.Controls.Add(this.unattachedCheckBox);
            this.Controls.Add(this.adjustmentsGrid);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(420, 330);
            this.Name = "FormAdjustmentPicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Adjustment Picker";
            this.Load += new System.EventHandler(this.FormAdjustmentPicker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.Button acceptButton;
        private OpenDental.UI.Button cancelButton;
        private System.Windows.Forms.CheckBox unattachedCheckBox;
        private OpenDental.UI.ODGrid adjustmentsGrid;
    }
}
