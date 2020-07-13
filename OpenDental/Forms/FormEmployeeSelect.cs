using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Collections.Generic;

namespace OpenDental{
///<summary></summary>
	public class FormEmployeeSelect : ODForm {
		private OpenDental.UI.Button butClose;
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butAdd;
		//private ArrayList ALemployees;
		private OpenDental.UI.ODGrid gridMain;
		private OpenDental.UI.Button butDelete;
		private Label label1;
		private bool isChanged;
		private CheckBox checkFurloughed;
		private CheckBox checkNonFurloughed;
		private CheckBox checkWorkingOffice;
		private CheckBox checkWorkingHome;
		private UI.Button butExport;
		private CheckBox checkHidden;
		private TextBox textSearch;
		///<summary>Unfiltered.</summary>
		private List<Employee> _listEmployeesFull;
		private Label label2;

		///<summary></summary>
		private List<Employee> _listEmployeesShowing;

		///<summary></summary>
		public FormEmployeeSelect(){
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmployeeSelect));
			this.butClose = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.checkFurloughed = new System.Windows.Forms.CheckBox();
			this.checkNonFurloughed = new System.Windows.Forms.CheckBox();
			this.checkWorkingOffice = new System.Windows.Forms.CheckBox();
			this.checkWorkingHome = new System.Windows.Forms.CheckBox();
			this.butExport = new OpenDental.UI.Button();
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.textSearch = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(793, 589);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 16;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butAdd
			// 
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Image = global::Imedisoft.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(12, 589);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(78, 26);
			this.butAdd.TabIndex = 21;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.Location = new System.Drawing.Point(12, 51);
			this.gridMain.Name = "gridMain";
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(856, 523);
			this.gridMain.TabIndex = 22;
			this.gridMain.TranslationName = "FormEmployees";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(261, 589);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(108, 29);
			this.label1.TabIndex = 24;
			this.label1.Text = "Delete all unused employees";
			// 
			// butDelete
			// 
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Image = global::Imedisoft.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(158, 589);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(97, 26);
			this.butDelete.TabIndex = 17;
			this.butDelete.Text = "Delete All";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// checkFurloughed
			// 
			this.checkFurloughed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFurloughed.Checked = true;
			this.checkFurloughed.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkFurloughed.Location = new System.Drawing.Point(622, 7);
			this.checkFurloughed.Name = "checkFurloughed";
			this.checkFurloughed.Size = new System.Drawing.Size(104, 18);
			this.checkFurloughed.TabIndex = 25;
			this.checkFurloughed.Text = "Furloughed";
			this.checkFurloughed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFurloughed.UseVisualStyleBackColor = true;
			this.checkFurloughed.CheckedChanged += new System.EventHandler(this.checkFurloughed_CheckedChanged);
			// 
			// checkNonFurloughed
			// 
			this.checkNonFurloughed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkNonFurloughed.Checked = true;
			this.checkNonFurloughed.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkNonFurloughed.Location = new System.Drawing.Point(622, 26);
			this.checkNonFurloughed.Name = "checkNonFurloughed";
			this.checkNonFurloughed.Size = new System.Drawing.Size(104, 18);
			this.checkNonFurloughed.TabIndex = 26;
			this.checkNonFurloughed.Text = "Non-Furloughed";
			this.checkNonFurloughed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkNonFurloughed.UseVisualStyleBackColor = true;
			this.checkNonFurloughed.CheckedChanged += new System.EventHandler(this.checkNonFurloughed_CheckedChanged);
			// 
			// checkWorkingOffice
			// 
			this.checkWorkingOffice.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWorkingOffice.Checked = true;
			this.checkWorkingOffice.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkWorkingOffice.Location = new System.Drawing.Point(740, 26);
			this.checkWorkingOffice.Name = "checkWorkingOffice";
			this.checkWorkingOffice.Size = new System.Drawing.Size(128, 18);
			this.checkWorkingOffice.TabIndex = 28;
			this.checkWorkingOffice.Text = "Working Office";
			this.checkWorkingOffice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWorkingOffice.UseVisualStyleBackColor = true;
			this.checkWorkingOffice.CheckedChanged += new System.EventHandler(this.checkWorkingOffice_CheckedChanged);
			// 
			// checkWorkingHome
			// 
			this.checkWorkingHome.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWorkingHome.Checked = true;
			this.checkWorkingHome.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkWorkingHome.Location = new System.Drawing.Point(740, 7);
			this.checkWorkingHome.Name = "checkWorkingHome";
			this.checkWorkingHome.Size = new System.Drawing.Size(128, 18);
			this.checkWorkingHome.TabIndex = 27;
			this.checkWorkingHome.Text = "Working From Home";
			this.checkWorkingHome.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWorkingHome.UseVisualStyleBackColor = true;
			this.checkWorkingHome.CheckedChanged += new System.EventHandler(this.checkWorkingHome_CheckedChanged);
			// 
			// butExport
			// 
			this.butExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butExport.Location = new System.Drawing.Point(553, 589);
			this.butExport.Name = "butExport";
			this.butExport.Size = new System.Drawing.Size(83, 26);
			this.butExport.TabIndex = 29;
			this.butExport.Text = "Export List";
			this.butExport.Click += new System.EventHandler(this.butExport_Click);
			// 
			// checkHidden
			// 
			this.checkHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidden.Location = new System.Drawing.Point(482, 7);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.Size = new System.Drawing.Size(104, 18);
			this.checkHidden.TabIndex = 30;
			this.checkHidden.Text = "Hidden";
			this.checkHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidden.UseVisualStyleBackColor = true;
			this.checkHidden.CheckedChanged += new System.EventHandler(this.checkHidden_CheckedChanged);
			// 
			// textSearch
			// 
			this.textSearch.Location = new System.Drawing.Point(69, 15);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(179, 20);
			this.textSearch.TabIndex = 0;
			this.textSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textSearch_KeyUp);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(9, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 18);
			this.label2.TabIndex = 32;
			this.label2.Text = "Search";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormEmployeeSelect
			// 
			this.AcceptButton = this.butClose;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(880, 627);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textSearch);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.butExport);
			this.Controls.Add(this.checkWorkingOffice);
			this.Controls.Add(this.checkWorkingHome);
			this.Controls.Add(this.checkNonFurloughed);
			this.Controls.Add(this.checkFurloughed);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormEmployeeSelect";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Employees";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormEmployee_Closing);
			this.Load += new System.EventHandler(this.FormEmployeeSelect_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormEmployeeSelect_Load(object sender, System.EventArgs e) {
			RefreshList();
			FillGrid();
			Height=System.Windows.Forms.Screen.FromControl(this).WorkingArea.Height-2;
			Top=2;
		}

		private void RefreshList(){
			Employees.RefreshCache();
			_listEmployeesFull=Employees.GetDeepCopy();
		}

		private void FillGrid(){
			_listEmployeesShowing=new List<Employee>();
			for(int i=0;i<_listEmployeesFull.Count;i++){
				if(textSearch.Text!=""){
					if(!_listEmployeesFull[i].LName.ToLower().Contains(textSearch.Text.ToLower())
						&& !_listEmployeesFull[i].FName.ToLower().Contains(textSearch.Text.ToLower())
						//&&!_listEmployeesFull[i].EmployeeNum.ToString().Contains(textSearch.Text)
						&&!_listEmployeesFull[i].WirelessPhone.Replace("-","").Replace("(","").Replace(")","").Contains(textSearch.Text))
					{
						continue;
					}
				}
				if(!checkHidden.Checked && _listEmployeesFull[i].IsHidden){
					continue;
				}
				if(!checkFurloughed.Checked && _listEmployeesFull[i].IsFurloughed){
					continue;
				}
				if(!checkNonFurloughed.Checked && !_listEmployeesFull[i].IsFurloughed){
					continue;
				}
				if(!checkWorkingHome.Checked && _listEmployeesFull[i].IsWorkingHome){
					continue;
				}
				if(!checkWorkingOffice.Checked && !_listEmployeesFull[i].IsWorkingHome){
					continue;
				}
				_listEmployeesShowing.Add(_listEmployeesFull[i]);
			}
			gridMain.BeginUpdate();
			gridMain.ListGridColumns.Clear();
			GridColumn col=new GridColumn(Lan.G("FormEmployeeSelect","FName"),70);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("FormEmployeeSelect","LName"),70);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("FormEmployeeSelect","MI"),30);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("FormEmployeeSelect","Hid"),30,HorizontalAlignment.Center);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("FormEmployeeSelect","Wireless"),120);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("FormEmployeeSelect","Email Work"),220);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("FormEmployeeSelect","Email Personal"),220);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("FormEmployeeSelect","Furlo"),35,HorizontalAlignment.Center);
			gridMain.ListGridColumns.Add(col);
			col=new GridColumn(Lan.G("FormEmployeeSelect","Home"),30,HorizontalAlignment.Center);
			gridMain.ListGridColumns.Add(col);
			gridMain.ListGridRows.Clear();
			GridRow row;
			for(int i=0;i<_listEmployeesShowing.Count;i++){
				row=new GridRow();
				row.Cells.Add(_listEmployeesShowing[i].FName);
				row.Cells.Add(_listEmployeesShowing[i].LName);
				row.Cells.Add(_listEmployeesShowing[i].MiddleI);
				if(_listEmployeesShowing[i].IsHidden){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				gridMain.ListGridRows.Add(row);
				row.Cells.Add(_listEmployeesShowing[i].WirelessPhone);
				row.Cells.Add(_listEmployeesShowing[i].EmailWork);
				row.Cells.Add(_listEmployeesShowing[i].EmailPersonal);
				row.Cells.Add(_listEmployeesShowing[i].IsFurloughed?"X":"");
				row.Cells.Add(_listEmployeesShowing[i].IsWorkingHome?"X":"");
			}
			gridMain.EndUpdate();
		}

		private void textSearch_KeyUp(object sender, KeyEventArgs e){
			FillGrid();
		}

		private void checkHidden_CheckedChanged(object sender, EventArgs e){
			FillGrid();
		}

		private void checkFurloughed_CheckedChanged(object sender, EventArgs e){
			FillGrid();
		}

		private void checkNonFurloughed_CheckedChanged(object sender, EventArgs e){
			FillGrid();
		}

		private void checkWorkingHome_CheckedChanged(object sender, EventArgs e){
			FillGrid();
		}

		private void checkWorkingOffice_CheckedChanged(object sender, EventArgs e){
			FillGrid();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormEmployeeEdit formEmployeeEdit=new FormEmployeeEdit();
			formEmployeeEdit.EmployeeCur=new Employee();
			formEmployeeEdit.IsNew=true;
			formEmployeeEdit.ShowDialog();
			if(formEmployeeEdit.DialogResult!=DialogResult.OK){
				return;
			}
			RefreshList();
			FillGrid();
			isChanged=true;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			long empNum=_listEmployeesShowing[e.Row].EmployeeNum;
			FormEmployeeEdit formEmployeeEdit=new FormEmployeeEdit();
			formEmployeeEdit.EmployeeCur=_listEmployeesShowing[e.Row];
			formEmployeeEdit.ShowDialog();
			if(formEmployeeEdit.DialogResult!=DialogResult.OK){
				return;
			}
			RefreshList();
			FillGrid();
			isChanged=true;
			for(int i=0;i<_listEmployeesShowing.Count;i++){
				if(_listEmployeesShowing[i].EmployeeNum==empNum){
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Schedules may be lost.  Continue?")){
				return;
			}
			for(int i=0;i<_listEmployeesShowing.Count;i++){
				try{
					Employees.Delete(_listEmployeesShowing[i].EmployeeNum);
				}
				catch{}
			}
			RefreshList();
			FillGrid();
		}

		private void butExport_Click(object sender, EventArgs e){
			gridMain.Export("Employees");
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			this.Close();
		}

		private void FormEmployee_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(isChanged){
				DataValid.SetInvalid(InvalidType.Employees);
			}
		}

	
	}
}
