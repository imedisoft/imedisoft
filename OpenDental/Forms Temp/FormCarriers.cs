using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDental.Bridges;
using System.Linq;
using System.Text;
using OpenDentBusiness.Eclaims;
using Imedisoft.Data;
using Imedisoft.Forms;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormCarriers : ODForm {
		private OpenDental.UI.Button butAdd;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolTip toolTip1;
		private OpenDental.UI.Button butCombine;
		///<summary>Set to true if using this dialog to select a carrier.</summary>
		public bool IsSelectMode;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.ODGrid gridMain;
		private CheckBox checkCDAnet;
		private CheckBox checkShowHidden;
		public TextBox textCarrier;
		private Label label2;
		private UI.Button butOK;//keeps track of whether an update is necessary.
		private DataTable table;
		public ValidPhone textPhone;
		private Label labelPhone;
		private UI.Button butRefresh;
		private UI.Button butItransUpdateCarriers;
		private GroupBox groupItrans;
		public Carrier SelectedCarrier;
		private CheckBox checkITransPhone;
		private CheckBox checkItransName;
		private CheckBox checkItransAddress;
		private CheckBox checkItransMissing;
		private List<ItransImportFields> _listShownUpdateFields=new List<ItransImportFields>();

		///<summary></summary>
		public FormCarriers()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCarriers));
			this.butAdd = new OpenDental.UI.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.butCombine = new OpenDental.UI.Button();
			this.butItransUpdateCarriers = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.checkCDAnet = new System.Windows.Forms.CheckBox();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.textCarrier = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.textPhone = new ValidPhone();
			this.labelPhone = new System.Windows.Forms.Label();
			this.butRefresh = new OpenDental.UI.Button();
			this.groupItrans = new System.Windows.Forms.GroupBox();
			this.checkItransName = new System.Windows.Forms.CheckBox();
			this.checkItransAddress = new System.Windows.Forms.CheckBox();
			this.checkITransPhone = new System.Windows.Forms.CheckBox();
			this.checkItransMissing = new System.Windows.Forms.CheckBox();
			this.groupItrans.SuspendLayout();
			this.SuspendLayout();
			// 
			// butAdd
			// 
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Image = global::Imedisoft.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(830, 435);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(90, 26);
			this.butAdd.TabIndex = 7;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butCombine
			// 
			this.butCombine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCombine.Location = new System.Drawing.Point(830, 471);
			this.butCombine.Name = "butCombine";
			this.butCombine.Size = new System.Drawing.Size(90, 26);
			this.butCombine.TabIndex = 10;
			this.butCombine.Text = "Co&mbine";
			this.toolTip1.SetToolTip(this.butCombine, "Combines multiple Employers");
			this.butCombine.Click += new System.EventHandler(this.butCombine_Click);
			// 
			// butItransUpdateCarriers
			// 
			this.butItransUpdateCarriers.Location = new System.Drawing.Point(8, 94);
			this.butItransUpdateCarriers.Name = "butItransUpdateCarriers";
			this.butItransUpdateCarriers.Size = new System.Drawing.Size(90, 26);
			this.butItransUpdateCarriers.TabIndex = 107;
			this.butItransUpdateCarriers.Text = "Update Carriers";
			this.toolTip1.SetToolTip(this.butItransUpdateCarriers, "Updates carriers using iTrans 2.0");
			this.butItransUpdateCarriers.Click += new System.EventHandler(this.butItransUpdateCarriers_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Location = new System.Drawing.Point(830, 623);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(90, 26);
			this.butCancel.TabIndex = 12;
			this.butCancel.Text = "Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(11, 29);
			this.gridMain.Name = "gridMain";
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(796, 620);
			this.gridMain.TabIndex = 13;
			this.gridMain.Title = "Carriers";
			this.gridMain.TranslationName = "TableCarriers";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// checkCDAnet
			// 
			this.checkCDAnet.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCDAnet.Location = new System.Drawing.Point(631, 6);
			this.checkCDAnet.Name = "checkCDAnet";
			this.checkCDAnet.Size = new System.Drawing.Size(96, 17);
			this.checkCDAnet.TabIndex = 99;
			this.checkCDAnet.Text = "CDAnet Only";
			this.checkCDAnet.Click += new System.EventHandler(this.checkCDAnet_Click);
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowHidden.Location = new System.Drawing.Point(490, 6);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(96, 17);
			this.checkShowHidden.TabIndex = 100;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.Click += new System.EventHandler(this.checkShowHidden_Click);
			// 
			// textCarrier
			// 
			this.textCarrier.Location = new System.Drawing.Point(118, 4);
			this.textCarrier.Name = "textCarrier";
			this.textCarrier.Size = new System.Drawing.Size(140, 20);
			this.textCarrier.TabIndex = 101;
			this.textCarrier.TextChanged += new System.EventHandler(this.textCarrier_TextChanged);
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(12, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 17);
			this.label2.TabIndex = 102;
			this.label2.Text = "Carrier";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(830, 587);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(90, 26);
			this.butOK.TabIndex = 103;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// textPhone
			// 
			this.textPhone.Location = new System.Drawing.Point(342, 4);
			this.textPhone.Name = "textPhone";
			this.textPhone.Size = new System.Drawing.Size(140, 20);
			this.textPhone.TabIndex = 104;
			this.textPhone.TextChanged += new System.EventHandler(this.textPhone_TextChanged);
			// 
			// labelPhone
			// 
			this.labelPhone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelPhone.Location = new System.Drawing.Point(264, 7);
			this.labelPhone.Name = "labelPhone";
			this.labelPhone.Size = new System.Drawing.Size(72, 17);
			this.labelPhone.TabIndex = 105;
			this.labelPhone.Text = "Phone";
			this.labelPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butRefresh
			// 
			this.butRefresh.Location = new System.Drawing.Point(732, 2);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 23);
			this.butRefresh.TabIndex = 106;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// groupItrans
			// 
			this.groupItrans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupItrans.Controls.Add(this.checkItransMissing);
			this.groupItrans.Controls.Add(this.checkItransName);
			this.groupItrans.Controls.Add(this.checkItransAddress);
			this.groupItrans.Controls.Add(this.checkITransPhone);
			this.groupItrans.Controls.Add(this.butItransUpdateCarriers);
			this.groupItrans.Location = new System.Drawing.Point(814, 162);
			this.groupItrans.Name = "groupItrans";
			this.groupItrans.Size = new System.Drawing.Size(106, 125);
			this.groupItrans.TabIndex = 109;
			this.groupItrans.TabStop = false;
			this.groupItrans.Text = "Web Import";
			this.groupItrans.Visible = false;
			// 
			// checkItransName
			// 
			this.checkItransName.Location = new System.Drawing.Point(8, 19);
			this.checkItransName.Name = "checkItransName";
			this.checkItransName.Size = new System.Drawing.Size(90, 17);
			this.checkItransName.TabIndex = 110;
			this.checkItransName.Text = "Name";
			this.checkItransName.UseVisualStyleBackColor = true;
			// 
			// checkItransAddress
			// 
			this.checkItransAddress.Location = new System.Drawing.Point(8, 37);
			this.checkItransAddress.Name = "checkItransAddress";
			this.checkItransAddress.Size = new System.Drawing.Size(90, 16);
			this.checkItransAddress.TabIndex = 109;
			this.checkItransAddress.Text = "Address";
			this.checkItransAddress.UseVisualStyleBackColor = true;
			// 
			// checkITransPhone
			// 
			this.checkITransPhone.Location = new System.Drawing.Point(8, 55);
			this.checkITransPhone.Name = "checkITransPhone";
			this.checkITransPhone.Size = new System.Drawing.Size(90, 17);
			this.checkITransPhone.TabIndex = 108;
			this.checkITransPhone.Text = "Phone";
			this.checkITransPhone.UseVisualStyleBackColor = true;
			// 
			// checkItransMissing
			// 
			this.checkItransMissing.Location = new System.Drawing.Point(8, 73);
			this.checkItransMissing.Name = "checkItransMissing";
			this.checkItransMissing.Size = new System.Drawing.Size(90, 17);
			this.checkItransMissing.TabIndex = 111;
			this.checkItransMissing.Text = "Add Missing";
			this.checkItransMissing.UseVisualStyleBackColor = true;
			// 
			// FormCarriers
			// 
			this.ClientSize = new System.Drawing.Size(927, 672);
			this.Controls.Add(this.groupItrans);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.textPhone);
			this.Controls.Add(this.labelPhone);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.textCarrier);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.checkShowHidden);
			this.Controls.Add(this.checkCDAnet);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butCombine);
			this.Controls.Add(this.butAdd);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormCarriers";
			this.ShowInTaskbar = false;
			this.Text = "Carriers";
			this.Load += new System.EventHandler(this.FormCarriers_Load);
			this.groupItrans.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormCarriers_Load(object sender, System.EventArgs e) {
			//if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
			//No.  Even Canadian users will want to see all their carriers and only use the checkbox for special situations.
			//	checkCDAnet.Checked=true;
			//}
			//else{
			//	checkCDAnet.Visible=false;
			//}
			if(IsSelectMode) {
				butCancel.Text="Cancel";
			}
			else {
				butCancel.Text="Close";
				butOK.Visible=false;
			}
			if(!Security.IsAuthorized(Permissions.CarrierCreate,true)) {
				butAdd.Enabled=false;
			}
			Clearinghouse ch=Clearinghouses.GetDefaultDental();
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && ch.TypeName==typeof(Canadian).FullName) {
				groupItrans.Visible=true;
				ItransImportFields fieldsToImport=(ItransImportFields)PrefC.GetInt(PreferenceName.ItransImportFields);
				checkITransPhone.Checked=(fieldsToImport.HasFlag(ItransImportFields.Phone));
				checkItransAddress.Checked=(fieldsToImport.HasFlag(ItransImportFields.Address));
				checkItransName.Checked=(fieldsToImport.HasFlag(ItransImportFields.Name));
				checkItransMissing.Checked=(fieldsToImport.HasFlag(ItransImportFields.AddMissing));
			}
			Carriers.RefreshCache();
			FillGrid();
		}

		private void FillGrid()
		{
			var selectedCarrierIds = gridMain.SelectedTags<Carriers.CarrierSummary>().Select(x => x.CarrierId).ToList();
			var selectedCarrierIndices = new List<int>();

			//Carriers.Refresh();

			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new GridColumn("Carrier Name", 160));
			gridMain.Columns.Add(new GridColumn("Phone", 90));
			gridMain.Columns.Add(new GridColumn("Address", 130));
			gridMain.Columns.Add(new GridColumn("City", 90));
			gridMain.Columns.Add(new GridColumn("ST", 50));
			gridMain.Columns.Add(new GridColumn("Zip", 70));
			gridMain.Columns.Add(new GridColumn("ElectID", 50));
			gridMain.Columns.Add(new GridColumn("Hidden", 50, HorizontalAlignment.Center));
			gridMain.Columns.Add(new GridColumn("Plans", 50));

			if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{
				gridMain.Columns.Add(new GridColumn("CDAnet", 50));
			}

			gridMain.Rows.Clear();

			var summaries = Carriers.GetBigList(checkCDAnet.Checked, checkShowHidden.Checked, textCarrier.Text, textPhone.Text);
			foreach (var summary in summaries)
			{
				var gridRow = new GridRow();
				gridRow.Cells.Add(summary.Name);
				gridRow.Cells.Add(summary.Phone);
				gridRow.Cells.Add(summary.AddressLine1);
				gridRow.Cells.Add(summary.City);
				gridRow.Cells.Add(summary.State);
				gridRow.Cells.Add(summary.Zip);
				gridRow.Cells.Add(summary.ElectronicId);
				gridRow.Cells.Add(summary.IsHidden ? "X" : "");
				gridRow.Cells.Add(summary.InsurancePlans.ToString());
				gridRow.Tag = summary;

				if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
				{
					gridRow.Cells.Add(summary.IsCDA ? "X" : "");
				}

				if (Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled)
				{
					gridRow.Cells[1].ForeColor = Color.Blue;
					gridRow.Cells[1].Underline = true;
				}

				gridMain.Rows.Add(gridRow);

				if (selectedCarrierIds.Contains(summary.CarrierId))
                {
					selectedCarrierIndices.Add(gridMain.Rows.Count - 1);
                }
			}

			gridMain.EndUpdate();

			foreach (var index in selectedCarrierIndices)
            {
				gridMain.SetSelected(index, true);
            }
		}

		private void textCarrier_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textPhone_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender, ODGridClickEventArgs e)
		{
			var summary = gridMain.SelectedTag<Carriers.CarrierSummary>();
			if (summary == null)
            {
				return;
            }

			var carrier = Carriers.GetCarrier(summary.CarrierId);
			if (carrier == null)
            {
				return;
            }

			if (IsSelectMode)
			{
				SelectedCarrier = carrier;

				DialogResult = DialogResult.OK;

				return;
			}

			using var formCarrierEdit = new FormCarrierEdit(carrier);
			if (formCarrierEdit.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			FillGrid();

			DataValid.SetInvalid(InvalidType.Carriers);
		}

		private void gridMain_CellClick(object sender, ODGridClickEventArgs e)
		{
			GridCell gridCellCur = gridMain.Rows[e.Row].Cells[e.Col];
			//Only grid cells with phone numbers are blue and underlined.
			if (gridCellCur.ForeColor == System.Drawing.Color.Blue && gridCellCur.Underline == true && Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled)
			{
				DentalTek.PlaceCall(gridCellCur.Text);
			}
		}

		private void checkCDAnet_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkShowHidden_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butItransUpdateCarriers_Click(object sender,EventArgs e) {
			List<string> listFields=new List<string>();
			ItransImportFields fieldsToImport=ItransImportFields.None;
			if(checkITransPhone.Checked) {
				fieldsToImport=(fieldsToImport|ItransImportFields.Phone);
				listFields.Add("Phone");
			}
			if(checkItransAddress.Checked) {
				fieldsToImport=(fieldsToImport|ItransImportFields.Address);
				listFields.Add("Address");
			}
			if(checkItransName.Checked) {
				fieldsToImport=(fieldsToImport|ItransImportFields.Name);
				listFields.Add("Name");
			}
			StringBuilder msg=new StringBuilder();
			if(listFields.Count>0) {
				msg.Insert(0,"The following carrier fields will be updated and overwritten for carriers matched by their Electronic IDs:"
				+" "+string.Join(", ",listFields)
				+"\r\n");
			}
			if(checkItransMissing.Checked) {
				fieldsToImport=(fieldsToImport|ItransImportFields.AddMissing);
				msg.AppendLine("New carriers will be added if missing from the database based on Electronic ID.");
			}
			if(msg.Length>0) {
				msg.AppendLine("Continue?");
				if(!MsgBox.Show(MsgBoxButtons.YesNo,msg.ToString())) {
					return;
				}
			}
			Preferences.Set(PreferenceName.ItransImportFields,(int)fieldsToImport);
			DataValid.SetInvalid(InvalidType.Prefs);
			string errorMsg=ItransNCpl.TryCarrierUpdate(false,fieldsToImport);
			if(!string.IsNullOrEmpty(errorMsg)) {
				MessageBox.Show(errorMsg);
				return;
			}
			MessageBox.Show("Done.");
			DataValid.SetInvalid(InvalidType.Carriers);
			FillGrid();
		}

		private void butAdd_Click(object sender, System.EventArgs e)
		{
			//The phone number will get formated while the user types inside the carrier edit window.
			//However, the user could have typed in a poorly formatted number so we will reformat the number once before load.
			string phoneFormatted = TelephoneNumbers.ReFormat(textPhone.Text);

            var carrier = new Carrier
            {
                Name = textCarrier.Text,
                Phone = phoneFormatted
            };

            if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{//Canadian. en-CA or fr-CA
				carrier.IsCDA = true;
			}


			using var formCarrierEdit = new FormCarrierEdit(carrier);
			if (formCarrierEdit.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			//Load the name and phone number of the newly added carrier to the search fields so that the new carrier shows up in the grid.
			textCarrier.Text = carrier.Name;
			textPhone.Text = carrier.Phone;

			FillGrid();

			for (int i = 0; i < table.Rows.Count; i++)
			{
				if (carrier.Id.ToString() == table.Rows[i]["CarrierNum"].ToString())
				{
					gridMain.SetSelected(i, true);
				}
			}

			DataValid.SetInvalid(InvalidType.Carriers);
		}

		private void butCombine_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsCarrierCombine)) {
				return;
			}
			if(gridMain.SelectedIndices.Length<2){
				MessageBox.Show("Please select multiple items first while holding down the control key.");
				return;
			}
			if(MessageBox.Show("Combine all these carriers into a single carrier? This will affect all patients using these carriers.  The next window will let you select which carrier to keep when combining.",""
				,MessageBoxButtons.OKCancel)!=DialogResult.OK)
			{
				return;
			}
			List<long> pickedCarrierNums=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				pickedCarrierNums.Add(PIn.Long(table.Rows[gridMain.SelectedIndices[i]]["CarrierNum"].ToString()));
			}
			FormCarrierCombine FormCB=new FormCarrierCombine();
			FormCB.CarrierNums=pickedCarrierNums;
			FormCB.ShowDialog();
			if(FormCB.DialogResult!=DialogResult.OK){
				return;
			}
			if(!VerifyCarrierCombineData(FormCB.PickedCarrierNum,pickedCarrierNums)) {
				return;
			}
			//int[] combCarrierNums=new int[tbCarriers.SelectedIndices.Length];
			//for(int i=0;i<tbCarriers.SelectedIndices.Length;i++){
			//	carrierNums[i]=Carriers.List[tbCarriers.SelectedIndices[i]].CarrierNum;
			//}
			try{
				//Prepare audit trail entry data, then combine, then make audit trail entries if successful
				List<string> carrierNames=new List<string>();
				List<List<InsurancePlan>> listInsPlans=new List<List<InsurancePlan>>();
				string carrierTo=Carriers.GetName(FormCB.PickedCarrierNum);
				for(int i=0;i<pickedCarrierNums.Count;i++) {
					carrierNames.Add(Carriers.GetName(pickedCarrierNums[i]));
					listInsPlans.Add(InsPlans.GetAllByCarrierNum(pickedCarrierNums[i]));					
				}
				Carriers.Combine(pickedCarrierNums,FormCB.PickedCarrierNum);
				//Carriers were combined successfully. Loop through all the associated insplans and make a securitylog entry that their carrier changed.
				for(int i=0;i<listInsPlans.Count;i++) {
					for(int j=0;j<listInsPlans[i].Count;j++) {
						SecurityLogs.MakeLogEntry(Permissions.InsCarrierCombine,0,"Carrier with name "+" "+carrierNames[i]+" "
							+"was combined with"+" "+carrierTo,listInsPlans[i][j].Id,listInsPlans[i][j].SecDateTEdit);
						if(carrierNames[i].Trim().ToLower()!=carrierTo.Trim().ToLower()) {
							SecurityLogs.MakeLogEntry(Permissions.InsPlanChangeCarrierName,0,"Carrier with name "+" "+carrierNames[i]+" "
								+"was merged with"+" "+carrierTo,listInsPlans[i][j].Id,listInsPlans[i][j].SecDateTEdit);
						}
					}
				}
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			DataValid.SetInvalid(InvalidType.Carriers);
			FillGrid();
		}

		private bool VerifyCarrierCombineData(long pickedCarrierNum,List<long> pickedCarrierNums) {
			List<Carrier> listCarriers = Carriers.GetCarriers(pickedCarrierNums);
			List<string> listWarnings = new List<string>();
			Carrier carCur = listCarriers.FirstOrDefault(x => x.Id==pickedCarrierNum);
			if(carCur==null) {//In case it wasn't included in the list of picked carrier nums.
				carCur=Carriers.GetCarrier(pickedCarrierNum);
			}
			if(carCur==null) {//In case it is a completely invalid carrier
				return false;//should never happen.
			}
			//==================== NAME ====================
			if(listCarriers.Any(x=>x.Name!=carCur.Name && !string.IsNullOrWhiteSpace(x.Name))) {
				listWarnings.Add("Carrier Name");
			}
			//==================== ADDRESS INFO ====================
			if(listCarriers.Any(x => x.AddressLine1!=carCur.AddressLine1 && !string.IsNullOrWhiteSpace(x.AddressLine1))
				|| listCarriers.Any(x => x.AddressLine2!=carCur.AddressLine2 && !string.IsNullOrWhiteSpace(x.AddressLine2))
				|| listCarriers.Any(x => x.City!=carCur.City && !string.IsNullOrWhiteSpace(x.City))
				|| listCarriers.Any(x => x.State!=carCur.State && !string.IsNullOrWhiteSpace(x.State))
				|| listCarriers.Any(x => x.Zip!=carCur.Zip && !string.IsNullOrWhiteSpace(x.Zip))) 
			{
				listWarnings.Add("Carrier Address");
			}
			//==================== PHONE ====================
			if(listCarriers.Any(x => x.Phone!=carCur.Phone && !string.IsNullOrWhiteSpace(x.Phone))) {
				listWarnings.Add("Carrier Phone");
			}
			//==================== ElectID ====================
			if(listCarriers.Any(x => x.ElectronicId!=carCur.ElectronicId && !string.IsNullOrWhiteSpace(x.ElectronicId))) {
				listWarnings.Add("Carrier ElectID");
			}
			//==================== TIN ====================
			if(listCarriers.Any(x => x.TIN!=carCur.TIN && !string.IsNullOrWhiteSpace(x.TIN))) {
				listWarnings.Add("Carrier TIN");
			}
			//==================== CDAnetVersion ====================
			if(listCarriers.Any(x => x.CDAnetVersion!=carCur.CDAnetVersion && !string.IsNullOrWhiteSpace(x.CDAnetVersion))) {
				listWarnings.Add("Carrier CDAnet Version");
			}
			//==================== IsCDA ====================
			if(listCarriers.Any(x=>x.IsCDA!=carCur.IsCDA)) {
				listWarnings.Add("Carrier Is CDA");
			}
			//==================== CanadianNetworkNum ====================
			if(listCarriers.Any(x => x.CanadianNetworkId!=carCur.CanadianNetworkId)) {
				listWarnings.Add("Canadian Network");
			}
			//==================== NoSendElect ====================
			if(listCarriers.Any(x => x.NoSendElect!=carCur.NoSendElect)) {
				listWarnings.Add("Send Elect");
			}
			//==================== IsHidden ====================
			if(listCarriers.Any(x => x.IsHidden!=carCur.IsHidden)) {
				listWarnings.Add("Is Hidden");
			}
			//==================== CanadianEncryptionMethod ====================
			if(listCarriers.Any(x => x.CanadianEncryptionMethod!=carCur.CanadianEncryptionMethod)) {
				listWarnings.Add("Canadian Encryption Method");
			}
			//==================== CanadianSupportedTypes ====================
			if(listCarriers.Any(x => x.CanadianSupportedTypes!=carCur.CanadianSupportedTypes)) {
				listWarnings.Add("Canadian Supported Types");
			}
			//==================== Additional fields ====================
			//If anyone asks for them, these fields can also be checked.
			// public long							SecUserNumEntry;
			// public DateTime					SecDateEntry;
			// public DateTime					SecDateTEdit;
			//====================USER PROMPT====================
			if(listWarnings.Count>0) {
				string warningMessage="WARNING!"+" "+"Mismatched data has been detected between selected carriers"+":\r\n\r\n"
					+string.Join("\r\n",listWarnings)+"\r\n\r\n"
					+"Would you like to continue combining carriers anyway?";
				if(MessageBox.Show(warningMessage,"",MessageBoxButtons.YesNo)!=DialogResult.Yes) {
					return false;
				}
			}
			return true;
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			//only visible if IsSelectMode
			if(gridMain.SelectedIndices.Length==0) {
				MessageBox.Show("Please select an item first.");
				return;
			}
			if(gridMain.SelectedIndices.Length>1) {
				MessageBox.Show("Please select only one item first.");
				return;
			}
			SelectedCarrier=Carriers.GetCarrier(PIn.Long(table.Rows[gridMain.SelectedIndices[0]]["CarrierNum"].ToString()));
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}


























