namespace Imedisoft.Forms
{
    partial class FormEvaluationCriterionDefEdit
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
            this.isCategoryNameCheckBox = new System.Windows.Forms.CheckBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.gradingScaleLabel = new System.Windows.Forms.Label();
            this.gradingScaleTextBox = new System.Windows.Forms.TextBox();
            this.gradingScaleButton = new OpenDental.UI.Button();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.pointsLabel = new System.Windows.Forms.Label();
            this.pointsTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(236, 144);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 8;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(322, 144);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "&Cancel";
            // 
            // isCategoryNameCheckBox
            // 
            this.isCategoryNameCheckBox.AutoSize = true;
            this.isCategoryNameCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.isCategoryNameCheckBox.Location = new System.Drawing.Point(150, 97);
            this.isCategoryNameCheckBox.Name = "isCategoryNameCheckBox";
            this.isCategoryNameCheckBox.Size = new System.Drawing.Size(251, 18);
            this.isCategoryNameCheckBox.TabIndex = 7;
            this.isCategoryNameCheckBox.Text = "Is Category Name (this will not show a grade)";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(150, 19);
            this.descriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.descriptionTextBox.MaxLength = 255;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(180, 20);
            this.descriptionTextBox.TabIndex = 1;
            // 
            // gradingScaleLabel
            // 
            this.gradingScaleLabel.AutoSize = true;
            this.gradingScaleLabel.Location = new System.Drawing.Point(72, 48);
            this.gradingScaleLabel.Name = "gradingScaleLabel";
            this.gradingScaleLabel.Size = new System.Drawing.Size(72, 13);
            this.gradingScaleLabel.TabIndex = 2;
            this.gradingScaleLabel.Text = "Grading Scale";
            // 
            // gradingScaleTextBox
            // 
            this.gradingScaleTextBox.Location = new System.Drawing.Point(150, 45);
            this.gradingScaleTextBox.MaxLength = 255;
            this.gradingScaleTextBox.Name = "gradingScaleTextBox";
            this.gradingScaleTextBox.ReadOnly = true;
            this.gradingScaleTextBox.Size = new System.Drawing.Size(180, 20);
            this.gradingScaleTextBox.TabIndex = 3;
            // 
            // gradingScaleButton
            // 
            this.gradingScaleButton.Image = global::Imedisoft.Properties.Resources.IconEllipsisSmall;
            this.gradingScaleButton.Location = new System.Drawing.Point(336, 45);
            this.gradingScaleButton.Name = "gradingScaleButton";
            this.gradingScaleButton.Size = new System.Drawing.Size(25, 20);
            this.gradingScaleButton.TabIndex = 4;
            this.gradingScaleButton.Click += new System.EventHandler(this.GradingScaleButton_Click);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(84, 22);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Description";
            // 
            // pointsLabel
            // 
            this.pointsLabel.AutoSize = true;
            this.pointsLabel.Location = new System.Drawing.Point(108, 74);
            this.pointsLabel.Name = "pointsLabel";
            this.pointsLabel.Size = new System.Drawing.Size(36, 13);
            this.pointsLabel.TabIndex = 5;
            this.pointsLabel.Text = "Points";
            // 
            // pointsTextBox
            // 
            this.pointsTextBox.Location = new System.Drawing.Point(150, 71);
            this.pointsTextBox.MaxLength = 255;
            this.pointsTextBox.Name = "pointsTextBox";
            this.pointsTextBox.ReadOnly = true;
            this.pointsTextBox.Size = new System.Drawing.Size(60, 20);
            this.pointsTextBox.TabIndex = 6;
            // 
            // FormEvaluationCriterionDefEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(414, 181);
            this.Controls.Add(this.pointsLabel);
            this.Controls.Add(this.pointsTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.gradingScaleLabel);
            this.Controls.Add(this.gradingScaleTextBox);
            this.Controls.Add(this.gradingScaleButton);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.isCategoryNameCheckBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(417, 197);
            this.Name = "FormEvaluationCriterionDefEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Evaluation Criterion Definition";
            this.Load += new System.EventHandler(this.FormEvaluationCriterionDefEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.Button acceptButton;
        private OpenDental.UI.Button cancelButton;
        private System.Windows.Forms.CheckBox isCategoryNameCheckBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label gradingScaleLabel;
        private System.Windows.Forms.TextBox gradingScaleTextBox;
        private OpenDental.UI.Button gradingScaleButton;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label pointsLabel;
        private System.Windows.Forms.TextBox pointsTextBox;
    }
}
