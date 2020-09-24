namespace Imedisoft.Forms
{
	partial class FormEhrSummaryOfCareEdit
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
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.printButton = new OpenDental.UI.Button();
            this.reconcileMedicationsButton = new OpenDental.UI.Button();
            this.reconcileLabel = new System.Windows.Forms.Label();
            this.reconcileProblemsButton = new OpenDental.UI.Button();
            this.reconcileAllergiesButton = new OpenDental.UI.Button();
            this.showXmlButton = new OpenDental.UI.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(792, 602);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 25);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "&Close";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // webBrowser
            // 
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.Location = new System.Drawing.Point(1, 1);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(882, 595);
            this.webBrowser.TabIndex = 1;
            // 
            // printButton
            // 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.printButton.Image = global::Imedisoft.Properties.Resources.IconPrint;
            this.printButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.printButton.Location = new System.Drawing.Point(679, 602);
            this.printButton.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(80, 25);
            this.printButton.TabIndex = 7;
            this.printButton.Text = "&Print";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // reconcileMedicationsButton
            // 
            this.reconcileMedicationsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reconcileMedicationsButton.Location = new System.Drawing.Point(100, 602);
            this.reconcileMedicationsButton.Name = "reconcileMedicationsButton";
            this.reconcileMedicationsButton.Size = new System.Drawing.Size(80, 25);
            this.reconcileMedicationsButton.TabIndex = 3;
            this.reconcileMedicationsButton.Text = "Medications";
            this.reconcileMedicationsButton.UseVisualStyleBackColor = true;
            this.reconcileMedicationsButton.Click += new System.EventHandler(this.ReconcileMedicationsButton_Click);
            // 
            // reconcileLabel
            // 
            this.reconcileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reconcileLabel.AutoSize = true;
            this.reconcileLabel.Location = new System.Drawing.Point(39, 608);
            this.reconcileLabel.Name = "reconcileLabel";
            this.reconcileLabel.Size = new System.Drawing.Size(52, 13);
            this.reconcileLabel.TabIndex = 2;
            this.reconcileLabel.Text = "Reconcile";
            this.reconcileLabel.Visible = false;
            // 
            // reconcileProblemsButton
            // 
            this.reconcileProblemsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reconcileProblemsButton.Location = new System.Drawing.Point(186, 602);
            this.reconcileProblemsButton.Name = "reconcileProblemsButton";
            this.reconcileProblemsButton.Size = new System.Drawing.Size(80, 25);
            this.reconcileProblemsButton.TabIndex = 4;
            this.reconcileProblemsButton.Text = "Problems";
            this.reconcileProblemsButton.UseVisualStyleBackColor = true;
            this.reconcileProblemsButton.Click += new System.EventHandler(this.ReconcileProblemsButton_Click);
            // 
            // reconcileAllergiesButton
            // 
            this.reconcileAllergiesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reconcileAllergiesButton.Location = new System.Drawing.Point(272, 602);
            this.reconcileAllergiesButton.Name = "reconcileAllergiesButton";
            this.reconcileAllergiesButton.Size = new System.Drawing.Size(80, 25);
            this.reconcileAllergiesButton.TabIndex = 5;
            this.reconcileAllergiesButton.Text = "Allergies";
            this.reconcileAllergiesButton.UseVisualStyleBackColor = true;
            this.reconcileAllergiesButton.Click += new System.EventHandler(this.ReconcileAllergiesButton_Click);
            // 
            // showXmlButton
            // 
            this.showXmlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.showXmlButton.Location = new System.Drawing.Point(566, 602);
            this.showXmlButton.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.showXmlButton.Name = "showXmlButton";
            this.showXmlButton.Size = new System.Drawing.Size(80, 25);
            this.showXmlButton.TabIndex = 6;
            this.showXmlButton.Text = "&Show XML";
            this.showXmlButton.UseVisualStyleBackColor = true;
            this.showXmlButton.Click += new System.EventHandler(this.ShowXmlButton_Click);
            // 
            // FormEhrSummaryOfCareEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(884, 639);
            this.Controls.Add(this.showXmlButton);
            this.Controls.Add(this.reconcileAllergiesButton);
            this.Controls.Add(this.reconcileProblemsButton);
            this.Controls.Add(this.reconcileLabel);
            this.Controls.Add(this.reconcileMedicationsButton);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.cancelButton);
            this.MinimumSize = new System.Drawing.Size(650, 200);
            this.Name = "FormEhrSummaryOfCareEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Summary of Care";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormEhrSummaryOfCareEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button cancelButton;
		private System.Windows.Forms.WebBrowser webBrowser;
		private OpenDental.UI.Button printButton;
		private OpenDental.UI.Button reconcileMedicationsButton;
		private System.Windows.Forms.Label reconcileLabel;
		private OpenDental.UI.Button reconcileProblemsButton;
		private OpenDental.UI.Button reconcileAllergiesButton;
		private OpenDental.UI.Button showXmlButton;
	}
}
