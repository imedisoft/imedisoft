namespace Imedisoft.Forms
{
	partial class FormAdjustSelect
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
            this.breakdownGroupBox = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.amountEndLabel = new System.Windows.Forms.Label();
            this.amountCurrentSplitLabel = new System.Windows.Forms.Label();
            this.amountAvailableLabel = new System.Windows.Forms.Label();
            this.amountUsedLabel = new System.Windows.Forms.Label();
            this.amountOriginalLabel = new System.Windows.Forms.Label();
            this.adjustmentsGrid = new OpenDental.UI.ODGrid();
            this.infoLabel = new System.Windows.Forms.Label();
            this.breakdownGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(526, 424);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 4;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(612, 424);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // breakdownGroupBox
            // 
            this.breakdownGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.breakdownGroupBox.Controls.Add(this.label9);
            this.breakdownGroupBox.Controls.Add(this.label12);
            this.breakdownGroupBox.Controls.Add(this.label6);
            this.breakdownGroupBox.Controls.Add(this.label3);
            this.breakdownGroupBox.Controls.Add(this.label2);
            this.breakdownGroupBox.Controls.Add(this.amountEndLabel);
            this.breakdownGroupBox.Controls.Add(this.amountCurrentSplitLabel);
            this.breakdownGroupBox.Controls.Add(this.amountAvailableLabel);
            this.breakdownGroupBox.Controls.Add(this.amountUsedLabel);
            this.breakdownGroupBox.Controls.Add(this.amountOriginalLabel);
            this.breakdownGroupBox.Location = new System.Drawing.Point(502, 12);
            this.breakdownGroupBox.Name = "breakdownGroupBox";
            this.breakdownGroupBox.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.breakdownGroupBox.Size = new System.Drawing.Size(190, 150);
            this.breakdownGroupBox.TabIndex = 3;
            this.breakdownGroupBox.TabStop = false;
            this.breakdownGroupBox.Text = "Breakdown";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(47, 115);
            this.label9.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Amt End:";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(33, 92);
            this.label12.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Current Split:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(39, 69);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Amt Avail:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Already Used:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 23);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Amt Original:";
            // 
            // amountEndLabel
            // 
            this.amountEndLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.amountEndLabel.AutoSize = true;
            this.amountEndLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amountEndLabel.Location = new System.Drawing.Point(110, 115);
            this.amountEndLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.amountEndLabel.Name = "amountEndLabel";
            this.amountEndLabel.Size = new System.Drawing.Size(31, 13);
            this.amountEndLabel.TabIndex = 9;
            this.amountEndLabel.Text = "0.00";
            // 
            // amountCurrentSplitLabel
            // 
            this.amountCurrentSplitLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.amountCurrentSplitLabel.AutoSize = true;
            this.amountCurrentSplitLabel.Location = new System.Drawing.Point(110, 92);
            this.amountCurrentSplitLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.amountCurrentSplitLabel.Name = "amountCurrentSplitLabel";
            this.amountCurrentSplitLabel.Size = new System.Drawing.Size(29, 13);
            this.amountCurrentSplitLabel.TabIndex = 7;
            this.amountCurrentSplitLabel.Text = "0.00";
            // 
            // amountAvailableLabel
            // 
            this.amountAvailableLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.amountAvailableLabel.AutoSize = true;
            this.amountAvailableLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amountAvailableLabel.Location = new System.Drawing.Point(110, 69);
            this.amountAvailableLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.amountAvailableLabel.Name = "amountAvailableLabel";
            this.amountAvailableLabel.Size = new System.Drawing.Size(31, 13);
            this.amountAvailableLabel.TabIndex = 5;
            this.amountAvailableLabel.Text = "0.00";
            // 
            // amountUsedLabel
            // 
            this.amountUsedLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.amountUsedLabel.AutoSize = true;
            this.amountUsedLabel.Location = new System.Drawing.Point(110, 46);
            this.amountUsedLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.amountUsedLabel.Name = "amountUsedLabel";
            this.amountUsedLabel.Size = new System.Drawing.Size(29, 13);
            this.amountUsedLabel.TabIndex = 3;
            this.amountUsedLabel.Text = "0.00";
            // 
            // amountOriginalLabel
            // 
            this.amountOriginalLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.amountOriginalLabel.AutoSize = true;
            this.amountOriginalLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.amountOriginalLabel.Location = new System.Drawing.Point(110, 23);
            this.amountOriginalLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.amountOriginalLabel.Name = "amountOriginalLabel";
            this.amountOriginalLabel.Size = new System.Drawing.Size(31, 13);
            this.amountOriginalLabel.TabIndex = 1;
            this.amountOriginalLabel.Text = "0.00";
            // 
            // adjustmentsGrid
            // 
            this.adjustmentsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adjustmentsGrid.Location = new System.Drawing.Point(12, 12);
            this.adjustmentsGrid.Name = "adjustmentsGrid";
            this.adjustmentsGrid.Size = new System.Drawing.Size(484, 410);
            this.adjustmentsGrid.TabIndex = 1;
            this.adjustmentsGrid.Title = "Unattached Adjustments";
            this.adjustmentsGrid.TranslationName = "TableAdjustSelect";
            this.adjustmentsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.AdjustmentsGrid_CellDoubleClick);
            this.adjustmentsGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.AdjustmentsGrid_CellClick);
            // 
            // infoLabel
            // 
            this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(13, 430);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(434, 13);
            this.infoLabel.TabIndex = 2;
            this.infoLabel.Text = "Unattached adjustments are not attached to a procedure and have an amount availab" +
    "le.";
            // 
            // FormAdjustSelect
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(704, 461);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.adjustmentsGrid);
            this.Controls.Add(this.breakdownGroupBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(690, 300);
            this.Name = "FormAdjustSelect";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Adjustment Select";
            this.Load += new System.EventHandler(this.FormAdjustSelect_Load);
            this.breakdownGroupBox.ResumeLayout(false);
            this.breakdownGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button acceptButton;
		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.GroupBox breakdownGroupBox;
		private OpenDental.UI.ODGrid adjustmentsGrid;
		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.Label amountAvailableLabel;
		private System.Windows.Forms.Label amountUsedLabel;
		private System.Windows.Forms.Label amountOriginalLabel;
		private System.Windows.Forms.Label amountEndLabel;
		private System.Windows.Forms.Label amountCurrentSplitLabel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label9;
	}
}
