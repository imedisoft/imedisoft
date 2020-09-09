namespace Imedisoft.Forms
{
    partial class FormStudentResultsOne
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
            this.cancelButton = new OpenDental.UI.Button();
            this.requirementSummaryGrid = new OpenDental.UI.ODGrid();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(652, 544);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Close";
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // requirementSummaryGrid
            // 
            this.requirementSummaryGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.requirementSummaryGrid.Location = new System.Drawing.Point(12, 12);
            this.requirementSummaryGrid.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.requirementSummaryGrid.Name = "requirementSummaryGrid";
            this.requirementSummaryGrid.Size = new System.Drawing.Size(720, 519);
            this.requirementSummaryGrid.TabIndex = 1;
            this.requirementSummaryGrid.Title = "Student Results";
            this.requirementSummaryGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.RequirementSummaryGrid_CellDoubleClick);
            // 
            // FormStudentResultsOne
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(744, 581);
            this.Controls.Add(this.requirementSummaryGrid);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStudentResultsOne";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Student Result Summary";
            this.Load += new System.EventHandler(this.FormStudentResultsOne_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private OpenDental.UI.Button cancelButton;
		private OpenDental.UI.ODGrid requirementSummaryGrid;
	}
}
