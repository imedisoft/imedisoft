namespace Imedisoft.Forms
{
	partial class FormQueryMonitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQueryMonitor));
            this.closeButton = new OpenDental.UI.Button();
            this.toggleButton = new OpenDental.UI.Button();
            this.logButton = new OpenDental.UI.Button();
            this.queryGrid = new OpenDental.UI.ODGrid();
            this.timerProcessQueue = new System.Windows.Forms.Timer(this.components);
            this.queryGroupBox = new System.Windows.Forms.GroupBox();
            this.commandTextBox = new System.Windows.Forms.TextBox();
            this.elapsedLabel = new System.Windows.Forms.Label();
            this.stopLabel = new System.Windows.Forms.Label();
            this.startLabel = new System.Windows.Forms.Label();
            this.elapsedTextBox = new System.Windows.Forms.TextBox();
            this.stopTextBox = new System.Windows.Forms.TextBox();
            this.startTextBox = new System.Windows.Forms.TextBox();
            this.commandLabel = new System.Windows.Forms.Label();
            this.queryMonitorSplitContainer = new System.Windows.Forms.SplitContainer();
            this.copyButton = new OpenDental.UI.Button();
            this.alwaysOnTopCheckBox = new System.Windows.Forms.CheckBox();
            this.queryGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.queryMonitorSplitContainer)).BeginInit();
            this.queryMonitorSplitContainer.Panel1.SuspendLayout();
            this.queryMonitorSplitContainer.Panel2.SuspendLayout();
            this.queryMonitorSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(892, 484);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(80, 25);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // toggleButton
            // 
            this.toggleButton.Image = global::Imedisoft.Properties.Resources.IconMediaPlay;
            this.toggleButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toggleButton.Location = new System.Drawing.Point(12, 12);
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.Size = new System.Drawing.Size(80, 25);
            this.toggleButton.TabIndex = 0;
            this.toggleButton.Text = "Start";
            this.toggleButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toggleButton.Click += new System.EventHandler(this.ToggleButton_Click);
            // 
            // logButton
            // 
            this.logButton.Location = new System.Drawing.Point(145, 12);
            this.logButton.Margin = new System.Windows.Forms.Padding(50, 3, 3, 3);
            this.logButton.Name = "logButton";
            this.logButton.Size = new System.Drawing.Size(80, 25);
            this.logButton.TabIndex = 1;
            this.logButton.Text = "Log";
            this.logButton.Click += new System.EventHandler(this.LogButton_Click);
            // 
            // queryGrid
            // 
            this.queryGrid.AllowSortingByColumn = true;
            this.queryGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryGrid.Location = new System.Drawing.Point(0, 0);
            this.queryGrid.Name = "queryGrid";
            this.queryGrid.Size = new System.Drawing.Size(960, 225);
            this.queryGrid.TabIndex = 0;
            this.queryGrid.Title = "Query Feed";
            this.queryGrid.TranslationName = "TableQueryFeed";
            this.queryGrid.WrapText = false;
            this.queryGrid.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.QueryGrid_CellClick);
            // 
            // timerProcessQueue
            // 
            this.timerProcessQueue.Interval = 5;
            this.timerProcessQueue.Tick += new System.EventHandler(this.TimerProcessQueue_Tick);
            // 
            // queryGroupBox
            // 
            this.queryGroupBox.Controls.Add(this.commandTextBox);
            this.queryGroupBox.Controls.Add(this.elapsedLabel);
            this.queryGroupBox.Controls.Add(this.stopLabel);
            this.queryGroupBox.Controls.Add(this.startLabel);
            this.queryGroupBox.Controls.Add(this.elapsedTextBox);
            this.queryGroupBox.Controls.Add(this.stopTextBox);
            this.queryGroupBox.Controls.Add(this.startTextBox);
            this.queryGroupBox.Controls.Add(this.commandLabel);
            this.queryGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryGroupBox.Location = new System.Drawing.Point(0, 0);
            this.queryGroupBox.Name = "queryGroupBox";
            this.queryGroupBox.Size = new System.Drawing.Size(960, 200);
            this.queryGroupBox.TabIndex = 0;
            this.queryGroupBox.TabStop = false;
            this.queryGroupBox.Text = "Query Details";
            // 
            // commandTextBox
            // 
            this.commandTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commandTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commandTextBox.Location = new System.Drawing.Point(6, 52);
            this.commandTextBox.Multiline = true;
            this.commandTextBox.Name = "commandTextBox";
            this.commandTextBox.ReadOnly = true;
            this.commandTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.commandTextBox.Size = new System.Drawing.Size(948, 142);
            this.commandTextBox.TabIndex = 7;
            // 
            // elapsedLabel
            // 
            this.elapsedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.elapsedLabel.AutoSize = true;
            this.elapsedLabel.Location = new System.Drawing.Point(764, 29);
            this.elapsedLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.elapsedLabel.Name = "elapsedLabel";
            this.elapsedLabel.Size = new System.Drawing.Size(44, 13);
            this.elapsedLabel.TabIndex = 5;
            this.elapsedLabel.Text = "Elapsed";
            // 
            // stopLabel
            // 
            this.stopLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopLabel.AutoSize = true;
            this.stopLabel.Location = new System.Drawing.Point(566, 29);
            this.stopLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.stopLabel.Name = "stopLabel";
            this.stopLabel.Size = new System.Drawing.Size(29, 13);
            this.stopLabel.TabIndex = 3;
            this.stopLabel.Text = "Stop";
            // 
            // startLabel
            // 
            this.startLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startLabel.AutoSize = true;
            this.startLabel.Location = new System.Drawing.Point(366, 29);
            this.startLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(31, 13);
            this.startLabel.TabIndex = 1;
            this.startLabel.Text = "Start";
            // 
            // elapsedTextBox
            // 
            this.elapsedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.elapsedTextBox.Location = new System.Drawing.Point(814, 26);
            this.elapsedTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.elapsedTextBox.Name = "elapsedTextBox";
            this.elapsedTextBox.ReadOnly = true;
            this.elapsedTextBox.Size = new System.Drawing.Size(140, 20);
            this.elapsedTextBox.TabIndex = 6;
            this.elapsedTextBox.TabStop = false;
            // 
            // stopTextBox
            // 
            this.stopTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopTextBox.Location = new System.Drawing.Point(601, 26);
            this.stopTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 20, 3);
            this.stopTextBox.Name = "stopTextBox";
            this.stopTextBox.ReadOnly = true;
            this.stopTextBox.Size = new System.Drawing.Size(140, 20);
            this.stopTextBox.TabIndex = 4;
            this.stopTextBox.TabStop = false;
            // 
            // startTextBox
            // 
            this.startTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startTextBox.Location = new System.Drawing.Point(403, 26);
            this.startTextBox.Margin = new System.Windows.Forms.Padding(3, 10, 20, 3);
            this.startTextBox.Name = "startTextBox";
            this.startTextBox.ReadOnly = true;
            this.startTextBox.Size = new System.Drawing.Size(140, 20);
            this.startTextBox.TabIndex = 2;
            this.startTextBox.TabStop = false;
            // 
            // commandLabel
            // 
            this.commandLabel.AutoSize = true;
            this.commandLabel.Location = new System.Drawing.Point(6, 29);
            this.commandLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.commandLabel.Name = "commandLabel";
            this.commandLabel.Size = new System.Drawing.Size(54, 13);
            this.commandLabel.TabIndex = 0;
            this.commandLabel.Text = "Command";
            // 
            // queryMonitorSplitContainer
            // 
            this.queryMonitorSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queryMonitorSplitContainer.Location = new System.Drawing.Point(12, 43);
            this.queryMonitorSplitContainer.Name = "queryMonitorSplitContainer";
            this.queryMonitorSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // queryMonitorSplitContainer.Panel1
            // 
            this.queryMonitorSplitContainer.Panel1.Controls.Add(this.queryGrid);
            this.queryMonitorSplitContainer.Panel1MinSize = 200;
            // 
            // queryMonitorSplitContainer.Panel2
            // 
            this.queryMonitorSplitContainer.Panel2.Controls.Add(this.queryGroupBox);
            this.queryMonitorSplitContainer.Panel2MinSize = 200;
            this.queryMonitorSplitContainer.Size = new System.Drawing.Size(960, 435);
            this.queryMonitorSplitContainer.SplitterDistance = 225;
            this.queryMonitorSplitContainer.SplitterWidth = 10;
            this.queryMonitorSplitContainer.TabIndex = 4;
            // 
            // copyButton
            // 
            this.copyButton.Enabled = false;
            this.copyButton.Location = new System.Drawing.Point(231, 12);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(80, 25);
            this.copyButton.TabIndex = 2;
            this.copyButton.Text = "Copy";
            this.copyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // alwaysOnTopCheckBox
            // 
            this.alwaysOnTopCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.alwaysOnTopCheckBox.AutoSize = true;
            this.alwaysOnTopCheckBox.Checked = true;
            this.alwaysOnTopCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.alwaysOnTopCheckBox.Location = new System.Drawing.Point(878, 17);
            this.alwaysOnTopCheckBox.Name = "alwaysOnTopCheckBox";
            this.alwaysOnTopCheckBox.Size = new System.Drawing.Size(94, 17);
            this.alwaysOnTopCheckBox.TabIndex = 3;
            this.alwaysOnTopCheckBox.Text = "Always on top";
            this.alwaysOnTopCheckBox.UseVisualStyleBackColor = true;
            this.alwaysOnTopCheckBox.CheckedChanged += new System.EventHandler(this.AlwaysOnTopCheckBox_CheckedChanged);
            // 
            // FormQueryMonitor
            // 
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(984, 521);
            this.Controls.Add(this.alwaysOnTopCheckBox);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.queryMonitorSplitContainer);
            this.Controls.Add(this.logButton);
            this.Controls.Add(this.toggleButton);
            this.Controls.Add(this.closeButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "FormQueryMonitor";
            this.Text = "Query Monitor";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormQueryMonitor_FormClosing);
            this.Load += new System.EventHandler(this.FormQueryMonitor_Load);
            this.queryGroupBox.ResumeLayout(false);
            this.queryGroupBox.PerformLayout();
            this.queryMonitorSplitContainer.Panel1.ResumeLayout(false);
            this.queryMonitorSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.queryMonitorSplitContainer)).EndInit();
            this.queryMonitorSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button closeButton;
		private OpenDental.UI.Button toggleButton;
		private OpenDental.UI.Button logButton;
		private OpenDental.UI.ODGrid queryGrid;
		private System.Windows.Forms.Timer timerProcessQueue;
		private System.Windows.Forms.GroupBox queryGroupBox;
		private System.Windows.Forms.Label elapsedLabel;
		private System.Windows.Forms.Label stopLabel;
		private System.Windows.Forms.Label startLabel;
		private System.Windows.Forms.TextBox elapsedTextBox;
		private System.Windows.Forms.TextBox stopTextBox;
		private System.Windows.Forms.TextBox startTextBox;
		private System.Windows.Forms.Label commandLabel;
		private System.Windows.Forms.SplitContainer queryMonitorSplitContainer;
		private OpenDental.UI.Button copyButton;
        private System.Windows.Forms.TextBox commandTextBox;
        private System.Windows.Forms.CheckBox alwaysOnTopCheckBox;
    }
}
