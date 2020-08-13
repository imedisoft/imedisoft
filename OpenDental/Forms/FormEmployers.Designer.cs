namespace Imedisoft.Forms
{
    partial class FormEmployers
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
            this.components = new System.ComponentModel.Container();
            this.cancelButton = new OpenDental.UI.Button();
            this.acceptButton = new OpenDental.UI.Button();
            this.employersListBox = new System.Windows.Forms.ListBox();
            this.addButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.editButton = new OpenDental.UI.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.combineButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(342, 574);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Cancel";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(342, 543);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 6;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // employersListBox
            // 
            this.employersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.employersListBox.IntegralHeight = false;
            this.employersListBox.Location = new System.Drawing.Point(12, 12);
            this.employersListBox.Name = "employersListBox";
            this.employersListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.employersListBox.Size = new System.Drawing.Size(324, 587);
            this.employersListBox.TabIndex = 1;
            this.employersListBox.SelectedIndexChanged += new System.EventHandler(this.EmployersListBox_SelectedIndexChanged);
            this.employersListBox.DoubleClick += new System.EventHandler(this.EmployersListBox_DoubleClick);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlus;
            this.addButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addButton.Location = new System.Drawing.Point(342, 12);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 25);
            this.addButton.TabIndex = 2;
            this.addButton.Text = "&Add";
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimes;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(342, 43);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPencil;
            this.editButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editButton.Location = new System.Drawing.Point(342, 74);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(80, 25);
            this.editButton.TabIndex = 4;
            this.editButton.Text = "&Edit";
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // combineButton
            // 
            this.combineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.combineButton.Enabled = false;
            this.combineButton.Image = global::Imedisoft.Properties.Resources.IconObjectGroup;
            this.combineButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.combineButton.Location = new System.Drawing.Point(342, 105);
            this.combineButton.Name = "combineButton";
            this.combineButton.Size = new System.Drawing.Size(80, 25);
            this.combineButton.TabIndex = 5;
            this.combineButton.Text = "Co&mbine";
            this.toolTip1.SetToolTip(this.combineButton, "Combines multiple Employers");
            this.combineButton.Click += new System.EventHandler(this.CombineButton_Click);
            // 
            // FormEmployers
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(434, 611);
            this.Controls.Add(this.employersListBox);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.combineButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(340, 420);
            this.Name = "FormEmployers";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Employers";
            this.Load += new System.EventHandler(this.FormEmployers_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button acceptButton;
        private System.Windows.Forms.ListBox employersListBox;
        private OpenDental.UI.Button addButton;
        private OpenDental.UI.Button deleteButton;
        private OpenDental.UI.Button editButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private OpenDental.UI.Button combineButton;
    }
}
