using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.IO;
using PdfSharp.Pdf;

namespace OpenDental {
	public partial class FormRpServiceDateView:ODForm {
		#region Public Variables
		///<summary>This will be the PatNum or the Guarantor's PatNum.</summary>
		public readonly long PatNum;
		///<summary>Whether or not the window is displaying results for the entire family.</summary>
		public readonly bool IsFamily;
		#endregion
		#region Private Variables
		private bool _headingPrinted;
		private int _pagesPrinted;
		private int _headingPrintH;
		private Family _fam;
		#endregion
		
		public FormRpServiceDateView(long patNum,bool isFamily) {
			InitializeComponent();
			
			PatNum=patNum;
			IsFamily=isFamily;
			_fam=Patients.GetFamily(patNum);
		}

		private void FormRpServiceDate_Load(object sender,EventArgs e) {
			FillGrid();
			Text="Service Date View -"+" "+_fam.GetPatient(PatNum).GetNameFL()+(IsFamily ? " "+"(Family)" : "");
		}

		private void FillGrid() {
			DataTable table=RpServiceDateView.GetData(PatNum,IsFamily,checkDetailedView.Checked);
			gridMain.BeginUpdate();
			//Columns
			gridMain.ListGridColumns.Clear();
			gridMain.ListGridColumns.Add(new GridColumn("Service Date",90));
			gridMain.ListGridColumns.Add(new GridColumn("Trans Date",80));
			gridMain.ListGridColumns.Add(new GridColumn("Patient",150));
			gridMain.ListGridColumns.Add(new GridColumn("Reference",220));
			gridMain.ListGridColumns.Add(new GridColumn("Charge",80,HorizontalAlignment.Right));
			gridMain.ListGridColumns.Add(new GridColumn("Credit",80,HorizontalAlignment.Right));
			gridMain.ListGridColumns.Add(new GridColumn("Prov",80));
			gridMain.ListGridColumns.Add(new GridColumn("InsBal",80,HorizontalAlignment.Right));
			gridMain.ListGridColumns.Add(new GridColumn("AcctBal",80,HorizontalAlignment.Right));
			//Rows
			gridMain.ListGridRows.Clear();
			DataRow lastRow=table.Select().LastOrDefault();
			foreach(DataRow row in table.Rows) {
				GridRow newRow=new GridRow();
				DateTime serviceDate=PIn.Date(row["Date"].ToString());
				DateTime transDate=PIn.Date(row["Trans Date"].ToString());
				newRow.Cells.Add((serviceDate.Year<1880) ? "" : serviceDate.ToShortDateString());
				newRow.Cells.Add((transDate.Year<1880) ? "" : transDate.ToShortDateString());
				newRow.Cells.Add(row["Patient"].ToString());
				string strReference=row["Reference"].ToString();
				newRow.Cells.Add(strReference);
				bool isUnallocated=strReference.ToLower().Contains("unallocated");
				newRow.Cells.Add(isUnallocated ? "" : PIn.Decimal(row["Charge"].ToString()).ToString("f"));
				newRow.Cells.Add(isUnallocated ? "" : PIn.Decimal(row["Credit"].ToString()).ToString("f"));
				newRow.Cells.Add(row["Pvdr"].ToString());
				decimal insBal=PIn.Decimal(row["InsBal"].ToString());
				decimal acctBal=PIn.Decimal(row["AcctBal"].ToString());
				bool isTotalsRow=row==lastRow || strReference.ToLower().Contains("Total for Date".ToLower());
				bool isProc=row["Type"].ToString().ToLower()=="proc" && checkDetailedView.Checked;
				//Show insBal and acctBal when not on totals row and detailed is checked and either of the amounts are not zero.
				bool showDetailedRow=isTotalsRow || isProc
					|| (checkDetailedView.Checked && (Math.Abs(insBal).IsGreaterThanZero() || Math.Abs(acctBal).IsGreaterThanZero()));
				newRow.Cells.Add(showDetailedRow ? insBal.ToString("f") : "");
				newRow.Cells.Add(showDetailedRow ? acctBal.ToString("f") : "");
				newRow.Tag=row;
				if(isTotalsRow) {
					newRow.Bold=true;
				}
				gridMain.ListGridRows.Add(newRow);
			}
			gridMain.EndUpdate();
		}


		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butSavePDFToImages_Click(object sender,EventArgs e) {
			if(gridMain.ListGridRows.Count==0) {
				MessageBox.Show("Grid is empty.");
				return;
			}
			//Get image category to save to. First image "Statement(S)" category.
			List<Definition> listImageCatDefs=Definitions.GetDefsForCategory(DefinitionCategory.ImageCats,true).Where(x => x.Value.Contains("S")).ToList();
			if(listImageCatDefs.IsNullOrEmpty()) {
				MessageBox.Show("No image category set for Statements.");
				return;
			}
			string tempFile= Storage.GetTempFileName(".pdf");
			CreatePDF(tempFile);
			Patient patCur=_fam.GetPatient(PatNum);

			Document docSave=new Document();
			docSave.DocNum=Documents.Insert(docSave);
			docSave.ImgType=ImageType.Document;
			docSave.DateCreated=DateTime.Now;
			docSave.PatNum=PatNum;
			docSave.DocCategory=listImageCatDefs.FirstOrDefault().Id;
			docSave.Description=$"ServiceDateView"+docSave.DocNum+$"{docSave.DateCreated.Year}_{docSave.DateCreated.Month}_{docSave.DateCreated.Day}";

			string fileName=ODFileUtils.CleanFileName(docSave.Description);
			string filePath=ImageStore.GetPatientFolder(patCur, OpenDentBusiness.FileIO.FileAtoZ.GetPreferredAtoZpath());
			while(Storage.FileExists(Storage.CombinePaths(filePath,fileName+".pdf"))) {
				fileName+="x";
			}
			Storage.Copy(tempFile,Storage.CombinePaths(filePath,fileName+".pdf"));
			docSave.FileName=fileName+".pdf";//file extension used for both DB images and AtoZ images
			Documents.Update(docSave);
			try {
				File.Delete(tempFile); //cleanup the temp file.
			}
			catch {
			}
			MessageBox.Show("PDF saved successfully.");
		}

		private void CreatePDF(string tempFile) {
			MigraDoc.Rendering.PdfDocumentRenderer pdfRenderer=new MigraDoc.Rendering.PdfDocumentRenderer(true,PdfFontEmbedding.Always);
			pdfRenderer.Document=CreateDocument();
			pdfRenderer.RenderDocument();
			pdfRenderer.PdfDocument.Save(tempFile);
		}

		private MigraDoc.DocumentObjectModel.Document CreateDocument() {
			MigraDoc.DocumentObjectModel.Document doc= new MigraDoc.DocumentObjectModel.Document();
			doc.DefaultPageSetup.PageWidth=MigraDoc.DocumentObjectModel.Unit.FromInch(8.5);
			doc.DefaultPageSetup.PageHeight=MigraDoc.DocumentObjectModel.Unit.FromInch(11);
			doc.DefaultPageSetup.TopMargin=MigraDoc.DocumentObjectModel.Unit.FromInch(.5);
			doc.DefaultPageSetup.LeftMargin=MigraDoc.DocumentObjectModel.Unit.FromInch(.5);
			doc.DefaultPageSetup.RightMargin=MigraDoc.DocumentObjectModel.Unit.FromInch(.5);
			MigraDoc.DocumentObjectModel.Section section=doc.AddSection();
			MigraDoc.DocumentObjectModel.Font headingFont=MigraDocHelper.CreateFont(13,true);
			MigraDoc.DocumentObjectModel.Font subHeadingFont=MigraDocHelper.CreateFont(10,true);
			#region printHeading
			//Heading---------------------------------------------------------------------------------------------------------------
			MigraDoc.DocumentObjectModel.Paragraph par=section.AddParagraph();
			MigraDoc.DocumentObjectModel.ParagraphFormat parformat=new MigraDoc.DocumentObjectModel.ParagraphFormat();
			parformat.Alignment=MigraDoc.DocumentObjectModel.ParagraphAlignment.Center;
			par.Format=parformat;
			string text="Service Date View";
			par.AddFormattedText(text,headingFont);
			par.AddLineBreak();
			//SubHeading---------------------------------------------------------------------------------------------------------------
			text=(IsFamily ? "Entire Family:"+" " : "")+$"{_fam.GetNameInFamFL(PatNum)}";
			par.AddFormattedText(text,subHeadingFont);
			par.AddLineBreak();
			text="Date"+" "+DateTime.Now.ToShortDateString();
			par.AddFormattedText(text,subHeadingFont);
			#endregion
			MigraDocHelper.InsertSpacer(section,10);
			section.PageSetup.Orientation=MigraDoc.DocumentObjectModel.Orientation.Landscape;
			MigraDocHelper.DrawGrid(section,gridMain);
			return doc;
		}

		private void butPrint_Click(object sender,EventArgs e) {
			if(gridMain.ListGridRows.Count==0) {
				MessageBox.Show("Grid is empty.");
				return;
			}
			_pagesPrinted=0;
			_headingPrinted=false;
			PrinterL.TryPrintOrDebugRpPreview(pd_PrintPage,"Service date view printed",PrintoutOrientation.Landscape);
		}

		private void pd_PrintPage(object sender,PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			Graphics g=e.Graphics;
			string text;
			System.Drawing.Font headingFont=new System.Drawing.Font("Arial",13,FontStyle.Bold);
			System.Drawing.Font subHeadingFont=new System.Drawing.Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!_headingPrinted) {
				text="Service Date View";
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=(IsFamily ? "Entire Family:"+" " : "")+$"{_fam.GetNameInFamFL(PatNum)}";
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				text=DateTime.Now.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				_headingPrinted=true;
				_headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,_pagesPrinted,bounds,_headingPrintH);
			_pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}
	}
}