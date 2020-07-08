using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using OpenDentBusiness;
using CodeBase;
using OpenDental.Thinfinity;

namespace OpenDental{
///<summary></summary>
	public class FormDocInfo : ODForm {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.ListBox listCategory;
		private System.Windows.Forms.TextBox textDescript;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label labelFileName;
		private OpenDental.ValidDate textDate;
		private System.ComponentModel.Container components = null;//required by designer
		private System.Windows.Forms.TextBox textFileName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ListBox listType;
		private System.Windows.Forms.TextBox textSize;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textToothNumbers;
		private System.Windows.Forms.Label labelToothNums;
		private Patient _patCur;
		private Document _documentCur;
		///<summary>If the image is stored in the db, it would be a waste of time to update the image if just changing a few fields in this window, so a synch is used.</summary>
		private Document _documentOld;
		private UI.Button butOpen;
		private UI.Button butAudit;
		private Label label4;
		private TextBox textTime;
		//private string _initialCategoryName;
		private bool _isOkDisabled;
		private Label labelCrop;
		private UI.Button butCropReset;
		private Label labelCropInfo;
		private List<Def> _listImageCatDefs;

		///<summary>Deprecated. Poorly designed.  Use the other constructor. ALWAYS save docCur before loading this form.</summary>
		[Obsolete]
		public FormDocInfo(Patient patCur,Document docCur,string initialCategoryName,bool isOkDisabled=false){
			InitializeComponent();
			_patCur=patCur;
			_documentCur=docCur;
			_documentOld=_documentCur.Copy();
			_isOkDisabled=isOkDisabled;
			if(_documentCur.DocNum==0){
				List<Def> listDefNumsImageCats=Defs.GetDefsForCategory(DefCat.ImageCats,true);
				_documentCur.DocCategory=listDefNumsImageCats[0].DefNum;
				for(int i=0;i<listDefNumsImageCats.Count;i++){
					if(listDefNumsImageCats[0].ItemName==initialCategoryName){
						_documentCur.DocCategory=listDefNumsImageCats[i].DefNum;
					}
				}
			}
			Lan.F(this);
		}

		///<summary></summary>
		public FormDocInfo(Patient patCur,Document docCur,bool isOkDisabled=false){
			InitializeComponent();
			_patCur=patCur;
			_documentCur=docCur;
			_documentOld=_documentCur.Copy();
			_isOkDisabled=isOkDisabled;
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDocInfo));
			this.listCategory = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textDescript = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.labelFileName = new System.Windows.Forms.Label();
			this.textFileName = new System.Windows.Forms.TextBox();
			this.textDate = new OpenDental.ValidDate();
			this.label5 = new System.Windows.Forms.Label();
			this.listType = new System.Windows.Forms.ListBox();
			this.textSize = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textToothNumbers = new System.Windows.Forms.TextBox();
			this.labelToothNums = new System.Windows.Forms.Label();
			this.butOpen = new OpenDental.UI.Button();
			this.butAudit = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.textTime = new System.Windows.Forms.TextBox();
			this.labelCrop = new System.Windows.Forms.Label();
			this.butCropReset = new OpenDental.UI.Button();
			this.labelCropInfo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// listCategory
			// 
			this.listCategory.Location = new System.Drawing.Point(12, 30);
			this.listCategory.Name = "listCategory";
			this.listCategory.Size = new System.Drawing.Size(104, 342);
			this.listCategory.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Category";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(117, 218);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(127, 18);
			this.label2.TabIndex = 2;
			this.label2.Text = "Optional Description";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDescript
			// 
			this.textDescript.Location = new System.Drawing.Point(245, 217);
			this.textDescript.MaxLength = 255;
			this.textDescript.Multiline = true;
			this.textDescript.Name = "textDescript";
			this.textDescript.Size = new System.Drawing.Size(299, 46);
			this.textDescript.TabIndex = 2;
			// 
			// butOK
			// 
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Location = new System.Drawing.Point(664, 351);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(756, 351);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(144, 79);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 18);
			this.label3.TabIndex = 6;
			this.label3.Text = "Date";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelFileName
			// 
			this.labelFileName.Location = new System.Drawing.Point(144, 33);
			this.labelFileName.Name = "labelFileName";
			this.labelFileName.Size = new System.Drawing.Size(100, 18);
			this.labelFileName.TabIndex = 8;
			this.labelFileName.Text = "File Name";
			this.labelFileName.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textFileName
			// 
			this.textFileName.Location = new System.Drawing.Point(245, 30);
			this.textFileName.Name = "textFileName";
			this.textFileName.ReadOnly = true;
			this.textFileName.Size = new System.Drawing.Size(586, 20);
			this.textFileName.TabIndex = 9;
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(245, 76);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(104, 20);
			this.textDate.TabIndex = 1;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(144, 122);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 18);
			this.label5.TabIndex = 11;
			this.label5.Text = "Type";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listType
			// 
			this.listType.Location = new System.Drawing.Point(245, 122);
			this.listType.Name = "listType";
			this.listType.Size = new System.Drawing.Size(104, 69);
			this.listType.TabIndex = 10;
			// 
			// textSize
			// 
			this.textSize.Location = new System.Drawing.Point(245, 53);
			this.textSize.Name = "textSize";
			this.textSize.ReadOnly = true;
			this.textSize.Size = new System.Drawing.Size(134, 20);
			this.textSize.TabIndex = 13;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(144, 56);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 18);
			this.label6.TabIndex = 12;
			this.label6.Text = "File Size";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textToothNumbers
			// 
			this.textToothNumbers.Location = new System.Drawing.Point(245, 194);
			this.textToothNumbers.Name = "textToothNumbers";
			this.textToothNumbers.Size = new System.Drawing.Size(240, 20);
			this.textToothNumbers.TabIndex = 15;
			// 
			// labelToothNums
			// 
			this.labelToothNums.Location = new System.Drawing.Point(144, 196);
			this.labelToothNums.Name = "labelToothNums";
			this.labelToothNums.Size = new System.Drawing.Size(100, 18);
			this.labelToothNums.TabIndex = 14;
			this.labelToothNums.Text = "Tooth Numbers";
			this.labelToothNums.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butOpen
			// 
			this.butOpen.Location = new System.Drawing.Point(756, 55);
			this.butOpen.Name = "butOpen";
			this.butOpen.Size = new System.Drawing.Size(75, 24);
			this.butOpen.TabIndex = 16;
			this.butOpen.Text = "Open Folder";
			this.butOpen.Click += new System.EventHandler(this.butOpen_Click);
			// 
			// butAudit
			// 
			this.butAudit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAudit.Location = new System.Drawing.Point(245, 351);
			this.butAudit.Name = "butAudit";
			this.butAudit.Size = new System.Drawing.Size(92, 24);
			this.butAudit.TabIndex = 126;
			this.butAudit.Text = "Audit Trail";
			this.butAudit.Click += new System.EventHandler(this.butAudit_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(149, 102);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(95, 16);
			this.label4.TabIndex = 128;
			this.label4.Text = "Time";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textTime
			// 
			this.textTime.Location = new System.Drawing.Point(245, 99);
			this.textTime.Name = "textTime";
			this.textTime.Size = new System.Drawing.Size(104, 20);
			this.textTime.TabIndex = 129;
			// 
			// labelCrop
			// 
			this.labelCrop.Location = new System.Drawing.Point(117, 271);
			this.labelCrop.Name = "labelCrop";
			this.labelCrop.Size = new System.Drawing.Size(127, 18);
			this.labelCrop.TabIndex = 130;
			this.labelCrop.Text = "Crop, Flip, and Rotate";
			this.labelCrop.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butCropReset
			// 
			this.butCropReset.Location = new System.Drawing.Point(245, 266);
			this.butCropReset.Name = "butCropReset";
			this.butCropReset.Size = new System.Drawing.Size(64, 24);
			this.butCropReset.TabIndex = 131;
			this.butCropReset.Text = "Reset";
			this.butCropReset.Click += new System.EventHandler(this.butCropReset_Click);
			// 
			// labelCropInfo
			// 
			this.labelCropInfo.Location = new System.Drawing.Point(315, 272);
			this.labelCropInfo.Name = "labelCropInfo";
			this.labelCropInfo.Size = new System.Drawing.Size(250, 18);
			this.labelCropInfo.TabIndex = 132;
			this.labelCropInfo.Text = "(this image has no crop, flip, or rotate applied)";
			// 
			// FormDocInfo
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(852, 387);
			this.Controls.Add(this.labelCropInfo);
			this.Controls.Add(this.butCropReset);
			this.Controls.Add(this.labelCrop);
			this.Controls.Add(this.textTime);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butAudit);
			this.Controls.Add(this.butOpen);
			this.Controls.Add(this.textToothNumbers);
			this.Controls.Add(this.labelToothNums);
			this.Controls.Add(this.textSize);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.listType);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.textDescript);
			this.Controls.Add(this.textFileName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listCategory);
			this.Controls.Add(this.labelFileName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDocInfo";
			this.ShowInTaskbar = false;
			this.Text = "Item Info";
			this.Load += new System.EventHandler(this.FormDocInfo_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		///<summary></summary>
		public void FormDocInfo_Load(object sender, System.EventArgs e){
			if(_isOkDisabled) {
				butOK.Enabled=false;
			}
			listCategory.Items.Clear();
			_listImageCatDefs=Defs.GetDefsForCategory(DefCat.ImageCats,true);
			for(int i=0;i<_listImageCatDefs.Count;i++){
				string folderName=_listImageCatDefs[i].ItemName;
				listCategory.Items.Add(folderName);
				if(i==0 || _listImageCatDefs[i].DefNum==_documentCur.DocCategory){
					listCategory.SelectedIndex=i;
				}
			}
			listType.Items.Clear();
			listType.Items.AddRange(Enum.GetNames(typeof(ImageType)));
			listType.SelectedIndex=(int)_documentCur.ImgType;
			textToothNumbers.Text=Tooth.FormatRangeForDisplay(_documentCur.ToothNumbers);
			textDate.Text=_documentCur.DateCreated.ToShortDateString();
			textTime.Text=_documentCur.DateCreated.ToLongTimeString();
			textDescript.Text=_documentCur.Description;
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
				string patFolder;
				if(!TryGetPatientFolder(out patFolder)) {
					return;
				}
				textFileName.Text=ODFileUtils.CombinePaths(patFolder,_documentCur.FileName);
				if(File.Exists(textFileName.Text)) {
					FileInfo fileInfo=new FileInfo(textFileName.Text);
					textSize.Text=fileInfo.Length.ToString("n0");
				}
			}
			else if(CloudStorage.IsCloudStorage) {
				string patFolder;
				if(!TryGetPatientFolder(out patFolder)) {
					return;
				}
				textFileName.Text=ODFileUtils.CombinePaths(patFolder,_documentCur.FileName,'/');
			}
			else {
				labelFileName.Visible=false;
				textFileName.Visible=false;
				butOpen.Visible=false;
				textSize.Text=_documentCur.RawBase64.Length.ToString("n0");
			}
			textToothNumbers.Text=Tooth.FormatRangeForDisplay(_documentCur.ToothNumbers);
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				labelToothNums.Visible=false;
				textToothNumbers.Visible=false;
			}
			if(_documentCur.IsFlipped || _documentCur.DegreesRotated!=0 || _documentCur.CropW>0){
				labelCropInfo.Text="(remove the existing crop, flip, and rotations)";
			}
			else{
				butCropReset.Enabled=false;
			}
		}

		///<summary>Returns true when the ImageStore was able to find or create a patient folder for the selected patient.  Sets patFolder to the corresponding folder name.
		///Otherwise, displays an error message to the user (with additional details regarding what went wrong) and returns false.
		///Optionally set isFormClosedOnError false if the DialogResult should not be set to Abort and the current window closed.</summary>
		private bool TryGetPatientFolder(out string patFolder,bool isFormClosedOnError=true) {
			patFolder="";
			try {
				patFolder=ImageStore.GetPatientFolder(_patCur,ImageStore.GetPreferredAtoZpath());
			}
			catch(Exception ex) {
				FriendlyException.Show(ex.Message,ex);
				if(isFormClosedOnError) {
					this.DialogResult=DialogResult.Abort;
					this.Close();
				}
				return false;
			}
			return true;
		}

		private void butOpen_Click(object sender,EventArgs e) {
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
				System.Diagnostics.Process.Start("Explorer",Path.GetDirectoryName(textFileName.Text));
			}
			else if(CloudStorage.IsCloudStorage) {//First download, then open
				FormProgress FormP=new FormProgress();
				FormP.DisplayText="Downloading...";
				FormP.NumberFormat="F";
				FormP.NumberMultiplication=1;
				FormP.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
				FormP.TickMS=1000;
				string patFolder;
				if(!TryGetPatientFolder(out patFolder,false)) {
					return;
				}
				OpenDentalCloud.Core.TaskStateDownload state=CloudStorage.DownloadAsync(patFolder
					,_documentCur.FileName
					,new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
				FormP.ShowDialog();
				if(FormP.DialogResult==DialogResult.Cancel) {
					state.DoCancel=true;
					return;
				}
				//Create temp file here or create the file with the actual name?  Changes made when opening the file won't be saved, so I think temp file is best.
				string tempFile=PrefC.GetRandomTempFile(Path.GetExtension(_documentCur.FileName));
				File.WriteAllBytes(tempFile,state.FileContent);
				if(ODBuild.IsWeb()) {
					ThinfinityUtils.HandleFile(tempFile);
				}
				else {
					System.Diagnostics.Process.Start(tempFile);
				}
			}
		}

		private void butCropReset_Click(object sender, EventArgs e){
			_documentCur.CropX=0;
			_documentCur.CropY=0;
			_documentCur.CropW=0;
			_documentCur.CropH=0;
			_documentCur.DegreesRotated=0;
			_documentCur.IsFlipped=false;
			labelCropInfo.Text="(this image has no crop, flip, or rotate applied)";
			butCropReset.Enabled=false;
		}

		private void butAudit_Click(object sender,EventArgs e) {
			List<Permissions> listPermissions=new List<Permissions>();
			listPermissions.Add(Permissions.ImageEdit);
			listPermissions.Add(Permissions.ImageDelete);
			FormAuditOneType formA=new FormAuditOneType(0,listPermissions,Lan.g(this,"Audit Trail for Document"),_documentCur.DocNum);
			formA.ShowDialog();
		}

		private void butOK_Click(object sender, System.EventArgs e){
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			if(textDate.Text=="") {
				MsgBox.Show(this,"Please enter a date.");
				return;
			}
			if(textTime.Text=="") {
				MsgBox.Show(this,"Please enter a time.");
				return;
			}
			DateTime time;
			if(!DateTime.TryParse(textTime.Text,out time)) {
				MsgBox.Show(this,"Please enter a valid time.");
				return;
			}
			//We had a security bug where users could change the date to a more recent date, and then subsequently delete.
			//The code below is for that specific scenario.
			DateTime dateTimeEntered=PIn.DateT(textDate.Text+" "+textTime.Text);
			if(dateTimeEntered>_documentCur.DateCreated) {
				//user is trying to change the date to some date after the previously linked date
				//is the new doc date allowed?
				if(!Security.IsAuthorized(Permissions.ImageDelete,_documentCur.DateCreated,true)) {
					//suppress the default security message above (it's too confusing for this case) and generate our own here
					MessageBox.Show(this,Lan.g(this,"Not allowed to future date this image from")+": "
						+"\r\n"+_documentCur.DateCreated.ToString()+" to "+dateTimeEntered.ToString()
						+"\r\n\r\n"+Lan.g(this,"A user with the SecurityAdmin permission must grant you access for")
						+":\r\n"+GroupPermissions.GetDesc(Permissions.ImageDelete));
					return;
				}
			}
			try{
				_documentCur.ToothNumbers=Tooth.FormatRangeForDb(textToothNumbers.Text);
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			_documentCur.DocCategory=_listImageCatDefs[listCategory.SelectedIndex].DefNum;
			_documentCur.ImgType=(ImageType)listType.SelectedIndex;
			_documentCur.Description=textDescript.Text;			
			_documentCur.DateCreated=dateTimeEntered;	
			try{//incomplete
				_documentCur.ToothNumbers=Tooth.FormatRangeForDb(textToothNumbers.Text);
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			//DocCur.Note=textNote.Text;
      //Docs.Cur.LastAltered=DateTime.Today;
			//if(IsNew){
			//	DocCur.Insert(PatCur);
			//}
			//else{
			if(Documents.Update(_documentCur,_documentOld)) {
				ImageStore.LogDocument(Lan.g(this,"Document Edited")+": ",Permissions.ImageEdit,_documentCur,_documentOld.DateTStamp);
			}
			//}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		
	}
}