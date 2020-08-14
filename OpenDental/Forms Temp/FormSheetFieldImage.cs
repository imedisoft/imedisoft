using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormSheetFieldImage:FormSheetFieldBase {

		public FormSheetFieldImage(SheetDef sheetDef,SheetFieldDef sheetFieldDef,bool isReadOnly):base(sheetDef,sheetFieldDef,isReadOnly) {
			InitializeComponent();
			
		}

		private void FormSheetFieldImage_Load(object sender,EventArgs e) {
			FillCombo();
			comboFieldName.Text=SheetFieldDefCur.FieldName;
			FillImage();
		}

		private void FillCombo()
		{

			comboFieldName.Items.Clear();
			string[] files = Directory.GetFiles(SheetUtil.GetImagePath());

			for (int i = 0; i < files.Length; i++)
			{
				//remove some common offending file types (non image files)
				if (files[i].EndsWith("db")
				  || files[i].EndsWith("doc")
				  || files[i].EndsWith("pdf")
				  )
				{
					continue;
				}
				comboFieldName.Items.Add(Path.GetFileName(files[i]));
			}
			//comboFieldName.Items.Add("Patient Info.gif");

		}

		private void butImport_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Multiselect = false;
			if (dlg.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			if (!File.Exists(dlg.FileName))
			{
				MessageBox.Show("File does not exist.");
				return;
			}
			if (!ImageHelper.HasImageExtension(dlg.FileName))
			{
				MessageBox.Show("Only allowed to import an image.");
				return;
			}
			string newName = dlg.FileName;

			newName = ODFileUtils.CombinePaths(SheetUtil.GetImagePath(), Path.GetFileName(dlg.FileName));
			if (File.Exists(newName))
			{
				MessageBox.Show("A file of that name already exists in SheetImages.  Please rename the file before importing.");
				return;
			}
			File.Copy(dlg.FileName, newName);

			FillCombo();
			for (int i = 0; i < comboFieldName.Items.Count; i++)
			{
				if (comboFieldName.Items[i].ToString() == Path.GetFileName(newName))
				{
					comboFieldName.SelectedIndex = i;
					comboFieldName.Text = Path.GetFileName(newName);
					FillImage();
					ShrinkToFit();
				}
			}
		}

		private void comboFieldName_TextUpdate(object sender,EventArgs e) {
			FillImage();
			ShrinkToFit();
		}

		private void comboFieldName_SelectionChangeCommitted(object sender,EventArgs e) {
			comboFieldName.Text=comboFieldName.SelectedItem.ToString();
			FillImage();
			ShrinkToFit();
		}

		private void FillImage(){
			if(comboFieldName.Text=="") {
				return;
			}
			textFullPath.Text=ODFileUtils.CombinePaths(SheetUtil.GetImagePath(),comboFieldName.Text);
			
			if(File.Exists(textFullPath.Text)){
				GC.Collect();
				try {
					pictureBox.Image=Image.FromFile(textFullPath.Text);
				}
				catch {
					pictureBox.Image=null;
					MessageBox.Show("Invalid image type.");
				}
			}
			else if(comboFieldName.Text=="Patient Info.gif") {//Interal image
				pictureBox.Image=OpenDentBusiness.Properties.Resources.Patient_Info;
				textFullPath.Text="Patient Info.gif (internal)";
			}
			else{
				pictureBox.Image=null;
			}
			if(pictureBox.Image==null) {
				textWidth2.Text="";
				textHeight2.Text="";
			}
			else {
				textWidth2.Text=pictureBox.Image.Width.ToString();
				textHeight2.Text=pictureBox.Image.Height.ToString();
			}
		}

		private void butShrink_Click(object sender,EventArgs e) {
			ShrinkToFit();
		}

		private void ShrinkToFit(){
			if(pictureBox.Image==null){
				return;
			}
			if(pictureBox.Image.Width>_sheetDefCur.Width || pictureBox.Image.Height>_sheetDefCur.Height){//image would be too big
				float imgWtoH=((float)pictureBox.Image.Width)/((float)pictureBox.Image.Height);
				float sheetWtoH=((float)_sheetDefCur.Width)/((float)_sheetDefCur.Height);
				float newRatio;
				int newW;
				int newH;
				if(imgWtoH < sheetWtoH){//image is tall and skinny
					newRatio=((float)_sheetDefCur.Height)/((float)pictureBox.Image.Height);//restrict by height
					newW=(int)(((float)pictureBox.Image.Width)*newRatio);
					newH=(int)(((float)pictureBox.Image.Height)*newRatio);
					textWidth.Text=newW.ToString();
					textHeight.Text=newH.ToString();
				}
				else{//image is short and fat
					newRatio=((float)_sheetDefCur.Width)/((float)pictureBox.Image.Width);//restrict by width
					newW=(int)(((float)pictureBox.Image.Width)*newRatio);
					newH=(int)(((float)pictureBox.Image.Height)*newRatio);
					textWidth.Text=newW.ToString();
					textHeight.Text=newH.ToString();
				}
			}
			else{
				textWidth.Text=pictureBox.Image.Width.ToString();
				textHeight.Text=pictureBox.Image.Height.ToString();
			}
		}

		private void textWidth_KeyUp(object sender,KeyEventArgs e) {
			if(!checkRatio.Checked){
				return;
			}
			if(pictureBox.Image==null){
				return;
			}
			float w;
			try{
				w=PIn.Float(textWidth.Text);
			}
			catch{
				return;
			}
			float imgWtoH=((float)pictureBox.Image.Width)/((float)pictureBox.Image.Height);
			int newH=(int)(w/imgWtoH);
			textHeight.Text=newH.ToString();
		}

		private void textHeight_KeyUp(object sender,KeyEventArgs e) {
			if(!checkRatio.Checked){
				return;
			}
			if(pictureBox.Image==null){
				return;
			}
			float h;
			try{
				h=PIn.Float(textHeight.Text);
			}
			catch{
				return;
			}
			float imgWtoH=((float)pictureBox.Image.Width)/((float)pictureBox.Image.Height);
			int newW=(int)(h*imgWtoH);
			textWidth.Text=newW.ToString();
		}

		protected override void OnDelete() {
			SheetFieldDefCur.ImageField?.Dispose();
			SheetFieldDefCur=null;
			DialogResult=DialogResult.OK;
		}

		protected override void OnOk() {
      if(!ArePosAndSizeValid()) {
				return;
      }
			if(comboFieldName.Text==""){
				MessageBox.Show("Please enter a file name first.");
				return;
			}
			if(pictureBox.Image==null) {
				if(comboFieldName.Text=="Patient Info.gif") {
					pictureBox.Image=OpenDentBusiness.Properties.Resources.Patient_Info;
				}
				else {
					GC.Collect();
					try {//catch valid files that are not valid images.
						if(!File.Exists(textFullPath.Text)) {
							MessageBox.Show("Image file does not exist.");
							return;
						}
						pictureBox.Image=Image.FromFile(textFullPath.Text);
					}
					catch {
						MessageBox.Show("Not a valid image type.");
						return;
					}
				}
			}
			SheetFieldDefCur.FieldName=comboFieldName.Text;
			SheetFieldDefCur.ImageField?.Dispose();//To prevent memory leaks
			//Make a copy of pictureBox.Image using the intended dimensions, to conserve memory.
			SheetFieldDefCur.ImageField=new Bitmap(pictureBox.Image,SheetFieldDefCur.Width,SheetFieldDefCur.Height);
			//don't save to database here.
			SheetFieldDefCur.IsNew=false;
			DialogResult=DialogResult.OK;
		}

		private void FormSheetFieldImage_FormClosing(object sender,FormClosingEventArgs e) {
			pictureBox.Image?.Dispose();
		}
	}
}