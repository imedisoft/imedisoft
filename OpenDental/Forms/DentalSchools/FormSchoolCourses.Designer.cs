namespace Imedisoft.Forms
{
    partial class FormSchoolCourses
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
            this.addButton = new OpenDental.UI.Button();
            this.schoolCoursesGrid = new OpenDental.UI.ODGrid();
            this.acceptButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.editButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(392, 404);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Close";
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(12, 404);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 2;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // schoolCoursesGrid
            // 
            this.schoolCoursesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schoolCoursesGrid.Location = new System.Drawing.Point(12, 12);
            this.schoolCoursesGrid.Name = "schoolCoursesGrid";
            this.schoolCoursesGrid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
            this.schoolCoursesGrid.Size = new System.Drawing.Size(460, 386);
            this.schoolCoursesGrid.TabIndex = 1;
            this.schoolCoursesGrid.Title = "Courses";
            this.schoolCoursesGrid.TranslationName = "FormEvaluationDefEdit";
            this.schoolCoursesGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.SchoolCoursesGrid_CellDoubleClick);
            this.schoolCoursesGrid.SelectionCommitted += new System.EventHandler(this.SchoolCoursesGrid_SelectionCommitted);
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.acceptButton.Location = new System.Drawing.Point(306, 404);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 5;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Visible = false;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(84, 404);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPenBlack;
            this.editButton.Location = new System.Drawing.Point(48, 404);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 3;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // FormSchoolCourses
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(484, 441);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.schoolCoursesGrid);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 300);
            this.Name = "FormSchoolCourses";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "School Courses";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FormSchoolCourses_Closing);
            this.Load += new System.EventHandler(this.FormSchoolCourses_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button addButton;
        private OpenDental.UI.ODGrid schoolCoursesGrid;
        private OpenDental.UI.Button acceptButton;
        private OpenDental.UI.Button deleteButton;
        private OpenDental.UI.Button editButton;
    }
}
