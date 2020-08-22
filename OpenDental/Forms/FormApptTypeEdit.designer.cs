namespace Imedisoft.Forms
{
    partial class FormApptTypeEdit
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
            this.colorLabel = new System.Windows.Forms.Label();
            this.colorClearButton = new OpenDental.UI.Button();
            this.colorButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.hiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteButton = new OpenDental.UI.Button();
            this.sliderButton = new System.Windows.Forms.Button();
            this.timeTable = new OpenDental.TableTimeBar();
            this.timeLabel = new System.Windows.Forms.Label();
            this.procedureCodesListBox = new System.Windows.Forms.ListBox();
            this.addButton = new OpenDental.UI.Button();
            this.removeButton = new OpenDental.UI.Button();
            this.proceduresLabel = new System.Windows.Forms.Label();
            this.timeClearButton = new OpenDental.UI.Button();
            this.timeTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.Location = new System.Drawing.Point(246, 548);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(80, 25);
            this.acceptButton.TabIndex = 16;
            this.acceptButton.Text = "&OK";
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(332, 548);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 17;
            this.cancelButton.Text = "&Cancel";
            // 
            // colorLabel
            // 
            this.colorLabel.AutoSize = true;
            this.colorLabel.Location = new System.Drawing.Point(92, 70);
            this.colorLabel.Name = "colorLabel";
            this.colorLabel.Size = new System.Drawing.Size(32, 13);
            this.colorLabel.TabIndex = 4;
            this.colorLabel.Text = "Color";
            // 
            // colorClearButton
            // 
            this.colorClearButton.Location = new System.Drawing.Point(156, 66);
            this.colorClearButton.Name = "colorClearButton";
            this.colorClearButton.Size = new System.Drawing.Size(50, 20);
            this.colorClearButton.TabIndex = 6;
            this.colorClearButton.Text = "none";
            this.colorClearButton.Click += new System.EventHandler(this.ColorClearButton_Click);
            // 
            // colorButton
            // 
            this.colorButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colorButton.Location = new System.Drawing.Point(130, 66);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(20, 20);
            this.colorButton.TabIndex = 5;
            this.colorButton.Click += new System.EventHandler(this.ColorButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(90, 43);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(34, 13);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(130, 40);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(260, 20);
            this.nameTextBox.TabIndex = 3;
            // 
            // hiddenCheckBox
            // 
            this.hiddenCheckBox.AutoSize = true;
            this.hiddenCheckBox.Checked = true;
            this.hiddenCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hiddenCheckBox.Location = new System.Drawing.Point(130, 324);
            this.hiddenCheckBox.Name = "hiddenCheckBox";
            this.hiddenCheckBox.Size = new System.Drawing.Size(59, 17);
            this.hiddenCheckBox.TabIndex = 14;
            this.hiddenCheckBox.Text = "Hidden";
            this.hiddenCheckBox.UseVisualStyleBackColor = true;
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTrash;
            this.deleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteButton.Location = new System.Drawing.Point(33, 548);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(80, 25);
            this.deleteButton.TabIndex = 15;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // sliderButton
            // 
            this.sliderButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.sliderButton.Location = new System.Drawing.Point(14, 13);
            this.sliderButton.Name = "sliderButton";
            this.sliderButton.Size = new System.Drawing.Size(11, 15);
            this.sliderButton.TabIndex = 1;
            this.sliderButton.UseVisualStyleBackColor = false;
            this.sliderButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SliderButton_MouseDown);
            this.sliderButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SliderButton_MouseMove);
            this.sliderButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SliderButton_MouseUp);
            // 
            // timeTable
            // 
            this.timeTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.timeTable.BackColor = System.Drawing.SystemColors.Window;
            this.timeTable.Location = new System.Drawing.Point(12, 12);
            this.timeTable.Name = "timeTable";
            this.timeTable.ScrollValue = 150;
            this.timeTable.SelectedIndices = new int[0];
            this.timeTable.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.timeTable.Size = new System.Drawing.Size(15, 561);
            this.timeTable.TabIndex = 0;
            this.timeTable.CellClicked += new OpenDental.ContrTable.CellEventHandler(this.TimeTable_CellClicked);
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(95, 95);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(29, 13);
            this.timeLabel.TabIndex = 7;
            this.timeLabel.Text = "Time";
            // 
            // procedureCodesListBox
            // 
            this.procedureCodesListBox.FormattingEnabled = true;
            this.procedureCodesListBox.IntegralHeight = false;
            this.procedureCodesListBox.Location = new System.Drawing.Point(130, 118);
            this.procedureCodesListBox.Name = "procedureCodesListBox";
            this.procedureCodesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.procedureCodesListBox.Size = new System.Drawing.Size(120, 200);
            this.procedureCodesListBox.TabIndex = 11;
            // 
            // addButton
            // 
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlus;
            this.addButton.Location = new System.Drawing.Point(256, 118);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 12;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Image = global::Imedisoft.Properties.Resources.IconMinus;
            this.removeButton.Location = new System.Drawing.Point(256, 149);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(30, 25);
            this.removeButton.TabIndex = 13;
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // proceduresLabel
            // 
            this.proceduresLabel.AutoSize = true;
            this.proceduresLabel.Location = new System.Drawing.Point(63, 121);
            this.proceduresLabel.Name = "proceduresLabel";
            this.proceduresLabel.Size = new System.Drawing.Size(61, 13);
            this.proceduresLabel.TabIndex = 10;
            this.proceduresLabel.Text = "Procedures";
            // 
            // timeClearButton
            // 
            this.timeClearButton.Location = new System.Drawing.Point(283, 92);
            this.timeClearButton.Name = "timeClearButton";
            this.timeClearButton.Size = new System.Drawing.Size(50, 20);
            this.timeClearButton.TabIndex = 9;
            this.timeClearButton.Text = "Clear";
            this.timeClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // timeTextBox
            // 
            this.timeTextBox.Location = new System.Drawing.Point(130, 92);
            this.timeTextBox.Name = "timeTextBox";
            this.timeTextBox.ReadOnly = true;
            this.timeTextBox.Size = new System.Drawing.Size(147, 20);
            this.timeTextBox.TabIndex = 8;
            // 
            // FormApptTypeEdit
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(424, 585);
            this.Controls.Add(this.timeClearButton);
            this.Controls.Add(this.timeTextBox);
            this.Controls.Add(this.proceduresLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.procedureCodesListBox);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.sliderButton);
            this.Controls.Add(this.timeTable);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.hiddenCheckBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.colorLabel);
            this.Controls.Add(this.colorClearButton);
            this.Controls.Add(this.colorButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormApptTypeEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Appointment Type";
            this.Load += new System.EventHandler(this.FormApptTypeEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenDental.UI.Button acceptButton;
        private OpenDental.UI.Button cancelButton;
        private System.Windows.Forms.Label colorLabel;
        private OpenDental.UI.Button colorClearButton;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.CheckBox hiddenCheckBox;
        private OpenDental.UI.Button deleteButton;
        private System.Windows.Forms.Button sliderButton;
        private OpenDental.TableTimeBar timeTable;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.ListBox procedureCodesListBox;
        private OpenDental.UI.Button addButton;
        private OpenDental.UI.Button removeButton;
        private System.Windows.Forms.Label proceduresLabel;
        private OpenDental.UI.Button timeClearButton;
        private System.Windows.Forms.TextBox timeTextBox;
    }
}
