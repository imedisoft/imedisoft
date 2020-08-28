namespace Imedisoft.Forms
{
    partial class FormAccounting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccounting));
            this.imageListMain = new System.Windows.Forms.ImageList(this.components);
            this.inactiveCheckBox = new System.Windows.Forms.CheckBox();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.setupMenuItem = new System.Windows.Forms.MenuItem();
            this.lockMenuItem = new System.Windows.Forms.MenuItem();
            this.reportsMenuItem = new System.Windows.Forms.MenuItem();
            this.generalLedgerMenuItem = new System.Windows.Forms.MenuItem();
            this.balanceSheetMenuItem = new System.Windows.Forms.MenuItem();
            this.profitLossMenuItem = new System.Windows.Forms.MenuItem();
            this.dateLabel = new System.Windows.Forms.Label();
            this.todayButton = new OpenDental.UI.Button();
            this.refreshButton = new OpenDental.UI.Button();
            this.dateTextBox = new System.Windows.Forms.TextBox();
            this.accountsGrid = new OpenDental.UI.ODGrid();
            this.addButton = new OpenDental.UI.Button();
            this.editButton = new OpenDental.UI.Button();
            this.exportButton = new OpenDental.UI.Button();
            this.cancelButton = new OpenDental.UI.Button();
            this.deleteButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // imageListMain
            // 
            this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
            this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMain.Images.SetKeyName(0, "Add.gif");
            this.imageListMain.Images.SetKeyName(1, "editPencil.gif");
            this.imageListMain.Images.SetKeyName(2, "butExport.gif");
            // 
            // inactiveCheckBox
            // 
            this.inactiveCheckBox.AutoSize = true;
            this.inactiveCheckBox.Location = new System.Drawing.Point(305, 14);
            this.inactiveCheckBox.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.inactiveCheckBox.Name = "inactiveCheckBox";
            this.inactiveCheckBox.Size = new System.Drawing.Size(150, 17);
            this.inactiveCheckBox.TabIndex = 10;
            this.inactiveCheckBox.Text = "Include Inactive Accounts";
            this.inactiveCheckBox.UseVisualStyleBackColor = true;
            this.inactiveCheckBox.Click += new System.EventHandler(this.InactiveCheckBox_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.setupMenuItem,
            this.lockMenuItem,
            this.reportsMenuItem});
            // 
            // setupMenuItem
            // 
            this.setupMenuItem.Index = 0;
            this.setupMenuItem.Text = "Setup";
            this.setupMenuItem.Click += new System.EventHandler(this.SetupMenuItem_Click);
            // 
            // lockMenuItem
            // 
            this.lockMenuItem.Index = 1;
            this.lockMenuItem.Text = "Lock";
            this.lockMenuItem.Click += new System.EventHandler(this.LockMenuItem_Click);
            // 
            // reportsMenuItem
            // 
            this.reportsMenuItem.Index = 2;
            this.reportsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.generalLedgerMenuItem,
            this.balanceSheetMenuItem,
            this.profitLossMenuItem});
            this.reportsMenuItem.Text = "Reports";
            // 
            // generalLedgerMenuItem
            // 
            this.generalLedgerMenuItem.Index = 0;
            this.generalLedgerMenuItem.Text = "General Ledger Detail";
            this.generalLedgerMenuItem.Click += new System.EventHandler(this.GeneralLedgerMenuItem_Click);
            // 
            // balanceSheetMenuItem
            // 
            this.balanceSheetMenuItem.Index = 1;
            this.balanceSheetMenuItem.Text = "Balance Sheet";
            this.balanceSheetMenuItem.Click += new System.EventHandler(this.BalanceSheetMenuItem_Click);
            // 
            // profitLossMenuItem
            // 
            this.profitLossMenuItem.Index = 2;
            this.profitLossMenuItem.Text = "Profit and Loss";
            this.profitLossMenuItem.Click += new System.EventHandler(this.ProfitLossMenuItem_Click);
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(17, 15);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(57, 13);
            this.dateLabel.TabIndex = 6;
            this.dateLabel.Text = "As of date";
            // 
            // todayButton
            // 
            this.todayButton.Location = new System.Drawing.Point(232, 12);
            this.todayButton.Name = "todayButton";
            this.todayButton.Size = new System.Drawing.Size(60, 20);
            this.todayButton.TabIndex = 9;
            this.todayButton.Text = "Today";
            this.todayButton.UseVisualStyleBackColor = true;
            this.todayButton.Click += new System.EventHandler(this.TodayButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(166, 12);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(60, 20);
            this.refreshButton.TabIndex = 8;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // dateTextBox
            // 
            this.dateTextBox.Location = new System.Drawing.Point(80, 12);
            this.dateTextBox.Name = "dateTextBox";
            this.dateTextBox.Size = new System.Drawing.Size(80, 20);
            this.dateTextBox.TabIndex = 7;
            // 
            // accountsGrid
            // 
            this.accountsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountsGrid.Location = new System.Drawing.Point(12, 38);
            this.accountsGrid.Name = "accountsGrid";
            this.accountsGrid.Size = new System.Drawing.Size(500, 479);
            this.accountsGrid.TabIndex = 0;
            this.accountsGrid.Title = "Chart of Accounts";
            this.accountsGrid.TranslationName = "TableChartOfAccounts";
            this.accountsGrid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.AccountsGrid_CellDoubleClick);
            this.accountsGrid.SelectionCommitted += new System.EventHandler(this.AccountsGrid_SelectionCommitted);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Image = global::Imedisoft.Properties.Resources.IconPlusGreen;
            this.addButton.Location = new System.Drawing.Point(12, 523);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(30, 25);
            this.addButton.TabIndex = 1;
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editButton.Enabled = false;
            this.editButton.Image = global::Imedisoft.Properties.Resources.IconPencil;
            this.editButton.Location = new System.Drawing.Point(48, 523);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(30, 25);
            this.editButton.TabIndex = 2;
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportButton.Image = global::Imedisoft.Properties.Resources.IconFileExcel;
            this.exportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.exportButton.Location = new System.Drawing.Point(127, 523);
            this.exportButton.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(80, 25);
            this.exportButton.TabIndex = 4;
            this.exportButton.Text = "Export";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(432, 523);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Close";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::Imedisoft.Properties.Resources.IconTimesRed;
            this.deleteButton.Location = new System.Drawing.Point(84, 523);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(30, 25);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FormAccounting
            // 
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(524, 560);
            this.Controls.Add(this.todayButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.dateTextBox);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.inactiveCheckBox);
            this.Controls.Add(this.accountsGrid);
            this.Controls.Add(this.deleteButton);
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "FormAccounting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Accounting";
            this.Load += new System.EventHandler(this.FormAccounting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		private OpenDental.UI.ODGrid accountsGrid;
		private System.Windows.Forms.CheckBox inactiveCheckBox;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem setupMenuItem;
		private System.Windows.Forms.MenuItem reportsMenuItem;
		private System.Windows.Forms.MenuItem generalLedgerMenuItem;
		private System.Windows.Forms.MenuItem balanceSheetMenuItem;
		private System.Windows.Forms.ImageList imageListMain;
		private OpenDental.UI.Button refreshButton;
		private System.Windows.Forms.TextBox dateTextBox;
		private System.Windows.Forms.Label dateLabel;
		private OpenDental.UI.Button todayButton;
		private System.Windows.Forms.MenuItem lockMenuItem;
		private System.Windows.Forms.MenuItem profitLossMenuItem;
        private OpenDental.UI.Button addButton;
        private OpenDental.UI.Button editButton;
        private OpenDental.UI.Button exportButton;
        private OpenDental.UI.Button cancelButton;
        private OpenDental.UI.Button deleteButton;
    }
}
