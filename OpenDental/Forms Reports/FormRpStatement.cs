using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using System.Linq;
using CodeBase;
using OpenDentBusiness.IO;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental
{
	///<summary></summary>
	public class FormRpStatement : ODForm
	{
		private System.Windows.Forms.PrintPreviewControl printPreviewControl2;
		private OpenDental.UI.Button butPrint;
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Panel panelZoom;
		private OpenDental.UI.Button butFwd;
		private OpenDental.UI.Button butBack;
		private System.Windows.Forms.Label labelTotPages;
		private System.ComponentModel.Container components = null;
		private int totalPages;
		///<summary>Holds the data for one statement.</summary>
		//private DataSet dataSett;
		private Statement Stmt;

		//private ImageStoreBase imageStore;

		///<summary></summary>
		public FormRpStatement()
		{
			InitializeComponent();
		}

		///<summary></summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpStatement));
			this.printPreviewControl2 = new System.Windows.Forms.PrintPreviewControl();
			this.butPrint = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.panelZoom = new System.Windows.Forms.Panel();
			this.butFwd = new OpenDental.UI.Button();
			this.butBack = new OpenDental.UI.Button();
			this.labelTotPages = new System.Windows.Forms.Label();
			this.panelZoom.SuspendLayout();
			this.SuspendLayout();
			// 
			// printPreviewControl2
			// 
			this.printPreviewControl2.AutoZoom = false;
			this.printPreviewControl2.Location = new System.Drawing.Point(-116, -7);
			this.printPreviewControl2.Name = "printPreviewControl2";
			this.printPreviewControl2.Size = new System.Drawing.Size(842, 538);
			this.printPreviewControl2.TabIndex = 6;
			this.printPreviewControl2.Zoom = 1D;
			// 
			// butPrint
			// 
			this.butPrint.Image = global::Imedisoft.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(340, 5);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(85, 27);
			this.butPrint.TabIndex = 8;
			this.butPrint.Text = "          Print";
			this.butPrint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Visible = false;
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butClose
			// 
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(434, 5);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 27);
			this.butClose.TabIndex = 7;
			this.butClose.Text = "Close";
			// 
			// panelZoom
			// 
			this.panelZoom.Controls.Add(this.butFwd);
			this.panelZoom.Controls.Add(this.butBack);
			this.panelZoom.Controls.Add(this.labelTotPages);
			this.panelZoom.Controls.Add(this.butClose);
			this.panelZoom.Controls.Add(this.butPrint);
			this.panelZoom.Location = new System.Drawing.Point(35, 419);
			this.panelZoom.Name = "panelZoom";
			this.panelZoom.Size = new System.Drawing.Size(524, 37);
			this.panelZoom.TabIndex = 12;
			// 
			// butFwd
			// 
			this.butFwd.Image = global::Imedisoft.Properties.Resources.Right;
			this.butFwd.Location = new System.Drawing.Point(195, 7);
			this.butFwd.Name = "butFwd";
			this.butFwd.Size = new System.Drawing.Size(18, 22);
			this.butFwd.TabIndex = 13;
			this.butFwd.Click += new System.EventHandler(this.butFwd_Click);
			// 
			// butBack
			// 
			this.butBack.Image = global::Imedisoft.Properties.Resources.Left;
			this.butBack.Location = new System.Drawing.Point(123, 7);
			this.butBack.Name = "butBack";
			this.butBack.Size = new System.Drawing.Size(18, 22);
			this.butBack.TabIndex = 12;
			this.butBack.Click += new System.EventHandler(this.butBack_Click);
			// 
			// labelTotPages
			// 
			this.labelTotPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTotPages.Location = new System.Drawing.Point(143, 9);
			this.labelTotPages.Name = "labelTotPages";
			this.labelTotPages.Size = new System.Drawing.Size(47, 18);
			this.labelTotPages.TabIndex = 11;
			this.labelTotPages.Text = "1 / 2";
			this.labelTotPages.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// FormRpStatement
			// 
			this.ClientSize = new System.Drawing.Size(787, 610);
			this.Controls.Add(this.panelZoom);
			this.Controls.Add(this.printPreviewControl2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormRpStatement";
			this.Text = "Statement Preview";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.FormRpStatement_Load);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.FormRpStatement_Layout);
			this.panelZoom.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRpStatement_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
		{
			printPreviewControl2.Location = new Point(0, 0);
			printPreviewControl2.Height = ClientSize.Height - 39;
			printPreviewControl2.Width = ClientSize.Width;
			panelZoom.Location = new Point(ClientSize.Width - 620, ClientSize.Height - 38);
		}

		private void FormRpStatement_Load(object sender, System.EventArgs e)
		{
			//this only happens during debugging
			labelTotPages.Text = "1 / " + totalPages.ToString();
			printPreviewControl2.Zoom = ((double)printPreviewControl2.ClientSize.Height
				/ ODprintout.DefaultPaperSize.Height);
		}

		///<summary>Creates a new pdf, attaches it to a new doc, and attaches that to the statement.  If it cannot create a pdf, for example if no AtoZ 
		///folders, then it will simply result in a docnum of zero, so no attached doc. Only used for batch statment printing. Returns the path of the
		///temp file where the pdf is saved.Temp file should be deleted manually.  Will return an empty string when unable to create the file.</summary>
		public static string CreateStatementPdfSheets(Statement stmt, Patient pat, Family fam, DataSet dataSet, bool isSilent = false)
		{
			Statement StmtCur = stmt;
			SheetDef sheetDef = SheetUtil.GetStatementSheetDef();
			Sheet sheet = SheetUtil.CreateSheet(sheetDef, StmtCur.PatNum, StmtCur.HidePayment);
			sheet.Parameters.Add(new SheetParameter(true, "Statement") { ParamValue = StmtCur });
			SheetFiller.FillFields(sheet, dataSet, StmtCur, pat: pat, fam: fam);
			SheetUtil.CalculateHeights(sheet, dataSet, StmtCur, pat: pat, patGuar: fam.Guarantor);
			string tempPath = Storage.CombinePaths(Storage.GetTempPath(), StmtCur.PatNum.ToString() + ".pdf");
			SheetPrinting.CreatePdf(sheet, tempPath, StmtCur, dataSet, null, pat: pat, patGuar: fam.Guarantor);
			List<Definition> listImageCatDefs = Definitions.GetDefsForCategory(DefinitionCategory.ImageCats, true);
			long category = 0;
			for (int i = 0; i < listImageCatDefs.Count; i++)
			{
				if (Regex.IsMatch(listImageCatDefs[i].Value, @"S"))
				{
					category = listImageCatDefs[i].Id;
					break;
				}
			}
			if (category == 0)
			{
				category = listImageCatDefs[0].Id;//put it in the first category.
			}
			//create doc--------------------------------------------------------------------------------------
			OpenDentBusiness.Document docc = null;
			try
			{
				docc = ImageStore.Import(tempPath, category, pat);
			}
			catch
			{
				if (!isSilent)
				{
					MsgBox.Show("Error saving document.");
				}
				return "";
			}
			docc.ImgType = ImageType.Document;
			if (StmtCur.IsInvoice)
			{
				docc.Description = "Invoice";
			}
			else
			{
				if (StmtCur.IsReceipt == true)
				{
					docc.Description = "Receipt";
				}
				else
				{
					docc.Description = "Statement";
				}
			}
			docc.AddedOnDate = StmtCur.DateSent;
			StmtCur.DocNum = docc.Id;//this signals the calling class that the pdf was created successfully.
			Statements.AttachDoc(StmtCur.StatementNum, docc);
			return tempPath;
		}

		/////<summary>Prints one statement to a specified printer which is passed in as a PrintDocument field.  Used when printer selection happens before a batch</summary>
		//public void PrintStatement(Statement stmt,PrintDocument pd,DataSet dataSet,Family fam,Patient pat) {
		//	PrintStatement(stmt,false,pd,dataSet,fam,pat);
		//}

		///<summary>Prints one statement.  Does not generate pdf or print from existing pdf.</summary>
		public void PrintStatement(Statement stmt, bool previewOnly, DataSet dataSet, Family fam, Patient pat)
		{
			Stmt = stmt;
			//TODO: Implement ODprintout pattern - MigraDoc
			Margins margins = new Margins(40, 40, 40, 60);
			if (CultureInfo.CurrentCulture.Name.EndsWith("CH"))
			{//CH is for switzerland. eg de-CH
			 //leave a big margin on the bottom for the routing slip
				margins = new Margins(40, 40, 40, 440);//4.4" from bottom
			}
			PrinterL.CreateODprintout(
				auditDescription: "Statement from" + " " + stmt.DateTStamp.ToShortDateString() + " " + "printed",
				printSit: PrintSituation.Statement,
				auditPatNum: pat.PatNum,
				margins: margins
			);
			if (ODprintout.CurPrintout.SettingsErrorCode != PrintoutErrorCode.Success)
			{
				Cursor = Cursors.WaitCursor;
				return;
			}
			if (!previewOnly)
			{
				Cursor = Cursors.Default;
				if (!PrinterL.TrySetPrinter(ODprintout.CurPrintout))
				{
					Cursor = Cursors.WaitCursor;
					return;
				}
			}
			MigraDoc.DocumentObjectModel.Document doc = CreateDocument(ODprintout.CurPrintout.PrintDoc, fam, pat, dataSet);
			MigraDoc.Rendering.Printing.MigraDocPrintDocument printdoc = new MigraDoc.Rendering.Printing.MigraDocPrintDocument();
			MigraDoc.Rendering.DocumentRenderer renderer = new MigraDoc.Rendering.DocumentRenderer(doc);
			renderer.PrepareDocument();
			totalPages = renderer.FormattedDocument.PageCount;
			labelTotPages.Text = "1 / " + totalPages.ToString();
			printdoc.Renderer = renderer;
			printdoc.PrinterSettings = ODprintout.CurPrintout.PrintDoc.PrinterSettings;
			if (previewOnly)
			{
				printPreviewControl2.Document = printdoc;
			}
			else
			{
				try
				{
					printdoc.Print();
				}
				catch
				{
					MessageBox.Show("Printer not available");
				}
			}
		}

		///<summary>Supply pd so that we know the paper size and margins.</summary>
		private MigraDoc.DocumentObjectModel.Document CreateDocument(PrintDocument pd, Family fam, Patient pat, DataSet dataSet)
		{
			MigraDoc.DocumentObjectModel.Document doc = new MigraDoc.DocumentObjectModel.Document();
			if (Plugins.HookMethod(this, "FormRpStatement.CreateDocument", doc, pd, fam, pat, dataSet, Stmt))
			{
				return doc;
			}
			doc.DefaultPageSetup.PageWidth = Unit.FromInch((double)pd.DefaultPageSettings.PaperSize.Width / 100);
			doc.DefaultPageSetup.PageHeight = Unit.FromInch((double)pd.DefaultPageSettings.PaperSize.Height / 100);
			doc.DefaultPageSetup.TopMargin = Unit.FromInch((double)pd.DefaultPageSettings.Margins.Top / 100);
			doc.DefaultPageSetup.LeftMargin = Unit.FromInch((double)pd.DefaultPageSettings.Margins.Left / 100);
			doc.DefaultPageSetup.RightMargin = Unit.FromInch((double)pd.DefaultPageSettings.Margins.Right / 100);
			doc.DefaultPageSetup.BottomMargin = Unit.FromInch((double)pd.DefaultPageSettings.Margins.Bottom / 100);
			MigraDoc.DocumentObjectModel.Section section = doc.AddSection();//so that Swiss will have different footer for each patient.
			string text;
			MigraDoc.DocumentObjectModel.Font font;
			//GetPatGuar(PatNums[famIndex][0]);
			//Family fam=Patients.GetFamily(Stmt.PatNum);
			Patient PatGuar = fam.ListPats[0];//.Clone();
											  //Patient pat=fam.GetPatient(Stmt.PatNum);
			DataTable tableMisc = dataSet.Tables["misc"];
			double patInsEstLimited = 0;
			double statementTotal = 0;
			//LimitedStatements have Total and InsEst for only those transactions selected for the statement
			if (Stmt.StatementType == StmtType.LimitedStatement)
			{
				patInsEstLimited = PIn.Double(tableMisc.Rows.OfType<DataRow>()
					.Where(x => x["descript"].ToString() == "patInsEst")
					.Select(x => x["value"].ToString()).FirstOrDefault());//safe, if string is blank or null PIn.Double will return 0
				statementTotal = dataSet.Tables.OfType<DataTable>().Where(x => x.TableName.StartsWith("account"))
					.SelectMany(x => x.Rows.OfType<DataRow>())
					.Where(x => x["AdjNum"].ToString() != "0"//adjustments, may be charges or credits
						|| x["ProcNum"].ToString() != "0"//procs, will be charges with credits==0
						|| x["PayNum"].ToString() != "0"//patient payments, will be credits with charges==0
						|| x["ClaimPaymentNum"].ToString() != "0").ToList()//claimproc payments+writeoffs, will be credits with charges==0
					.Sum(x => PIn.Double(x["chargesDouble"].ToString()) - PIn.Double(x["creditsDouble"].ToString()));//add charges-credits
			}
			//HEADING-----------------------------------------------------------------------------------------------------------
			#region Heading
			Paragraph par = section.AddParagraph();
			ParagraphFormat parformat = new ParagraphFormat();
			parformat.Alignment = ParagraphAlignment.Center;
			par.Format = parformat;
			font = MigraDocHelper.CreateFont(14, true);
			if (Stmt.IsInvoice)
			{
				if (CultureInfo.CurrentCulture.Name == "en-NZ" || CultureInfo.CurrentCulture.Name == "en-AU")
				{//New Zealand and Australia
					text = "TAX INVOICE";
				}
				else
				{
					text = "INVOICE";
					text += " #" + Stmt.StatementNum.ToString();//Some larger customers of OD need this to show in order to properly pay.
				}
			}
			else if (Stmt.IsReceipt)
			{
				text = "RECEIPT";
				if (CultureInfo.CurrentCulture.Name.EndsWith("SG"))
				{//SG=Singapore
					text += " #" + Stmt.StatementNum.ToString();
				}
			}
			else
			{
				text = "STATEMENT";
				if (Stmt.StatementType == StmtType.LimitedStatement)
				{
					text += " (" + "Limited" + ")";
				}
			}
			par.AddFormattedText(text, font);
			text = DateTime.Today.ToShortDateString();
			font = MigraDocHelper.CreateFont(10);
			par.AddLineBreak();
			par.AddFormattedText(text, font);
			text = "Account Number" + " ";
			if (Preferences.GetBool(PreferenceName.StatementAccountsUseChartNumber))
			{
				text += PatGuar.ChartNumber;
			}
			else
			{
				text += PatGuar.PatNum;
			}
			par.AddLineBreak();
			par.AddFormattedText(text, font);
			TextFrame frame;
			#endregion Heading
			//"COPY" for foreign countries' TAX INVOICES------------------------------------------------------------------------
			#region Tax Invoice Copy
			if (Stmt.IsInvoiceCopy && CultureInfo.CurrentCulture.Name != "en-US")
			{//We don't show this for US.
				font = MigraDocHelper.CreateFont(28, true, System.Drawing.Color.Red);
				frame = section.AddTextFrame();
				frame.RelativeVertical = RelativeVertical.Page;
				frame.RelativeHorizontal = RelativeHorizontal.Page;
				frame.MarginLeft = Unit.Zero;
				frame.MarginTop = Unit.Zero;
				frame.Top = TopPosition.Parse("0.35 in");
				frame.Left = LeftPosition.Parse("5.4 in");
				frame.Width = Unit.FromInch(3);
				par = frame.AddParagraph();
				par.Format.Font = font;
				par.AddText("COPY");
			}
			#endregion Tax Invoice Copy
			//Practice Address--------------------------------------------------------------------------------------------------
			#region Practice Address
			if (Preferences.GetBool(PreferenceName.StatementShowReturnAddress))
			{
				font = MigraDocHelper.CreateFont(10);
				frame = section.AddTextFrame();
				frame.RelativeVertical = RelativeVertical.Page;
				frame.RelativeHorizontal = RelativeHorizontal.Page;
				frame.MarginLeft = Unit.Zero;
				frame.MarginTop = Unit.Zero;
				frame.Top = TopPosition.Parse("0.5 in");
				frame.Left = LeftPosition.Parse("0.3 in");
				frame.Width = Unit.FromInch(3);
				if (PrefC.HasClinicsEnabled && Clinics.Count(true) > 0 //if using clinics
						&& Clinics.GetById(PatGuar.ClinicNum) != null)//and this patient assigned to a clinic
				{
					Clinic clinic = Clinics.GetById(PatGuar.ClinicNum);
					par = frame.AddParagraph();
					par.Format.Font = font;
					par.AddText(clinic.Description);
					par.AddLineBreak();
					if (CultureInfo.CurrentCulture.Name == "en-AU")
					{//Australia
						Provider defaultProv = Providers.GetById(Preferences.GetLong(PreferenceName.PracticeDefaultProv));
						par.AddText("ABN: " + defaultProv.NationalProviderID);
						par.AddLineBreak();
					}
					if (CultureInfo.CurrentCulture.Name == "en-NZ")
					{//New Zealand
						Provider defaultProv = Providers.GetById(Preferences.GetLong(PreferenceName.PracticeDefaultProv));
						par.AddText("GST: " + defaultProv.SSN);
						par.AddLineBreak();
					}
					par.AddText(clinic.AddressLine1);
					par.AddLineBreak();
					if (clinic.AddressLine2 != "")
					{
						par.AddText(clinic.AddressLine2);
						par.AddLineBreak();
					}
					if (CultureInfo.CurrentCulture.Name.EndsWith("CH"))
					{//CH is for switzerland. eg de-CH
						par.AddText(clinic.Zip + " " + clinic.City);
					}
					else if (CultureInfo.CurrentCulture.Name.EndsWith("SG"))
					{//SG=Singapore
						par.AddText(clinic.City + " " + clinic.Zip);
					}
					else
					{
						par.AddText(clinic.City + ", " + clinic.State + " " + clinic.Zip);
					}
					par.AddLineBreak();
					text = clinic.Phone;
					if (text.Length == 10)
					{
						text = "(" + text.Substring(0, 3) + ")" + text.Substring(3, 3) + "-" + text.Substring(6);
					}
					par.AddText(text);
					par.AddLineBreak();
				}
				else
				{
					par = frame.AddParagraph();
					par.Format.Font = font;
					par.AddText(Preferences.GetString(PreferenceName.PracticeTitle));
					par.AddLineBreak();
					if (CultureInfo.CurrentCulture.Name == "en-AU")
					{//Australia
						Provider defaultProv = Providers.GetById(Preferences.GetLong(PreferenceName.PracticeDefaultProv));
						par.AddText("ABN: " + defaultProv.NationalProviderID);
						par.AddLineBreak();
					}
					if (CultureInfo.CurrentCulture.Name == "en-NZ")
					{//New Zealand
						Provider defaultProv = Providers.GetById(Preferences.GetLong(PreferenceName.PracticeDefaultProv));
						par.AddText("GST: " + defaultProv.SSN);
						par.AddLineBreak();
					}
					par.AddText(Preferences.GetString(PreferenceName.PracticeAddress));
					par.AddLineBreak();
					if (Preferences.GetString(PreferenceName.PracticeAddress2) != "")
					{
						par.AddText(Preferences.GetString(PreferenceName.PracticeAddress2));
						par.AddLineBreak();
					}
					if (CultureInfo.CurrentCulture.Name.EndsWith("CH"))
					{//CH is for switzerland. eg de-CH
						par.AddText(Preferences.GetString(PreferenceName.PracticeZip) + " " + Preferences.GetString(PreferenceName.PracticeCity));
					}
					else if (CultureInfo.CurrentCulture.Name.EndsWith("SG"))
					{//SG=Singapore
						par.AddText(Preferences.GetString(PreferenceName.PracticeCity) + " " + Preferences.GetString(PreferenceName.PracticeZip));
					}
					else
					{
						par.AddText(Preferences.GetString(PreferenceName.PracticeCity) + ", " + Preferences.GetString(PreferenceName.PracticeST) + " " + Preferences.GetString(PreferenceName.PracticeZip));
					}
					par.AddLineBreak();
					text = Preferences.GetString(PreferenceName.PracticePhone);
					if (text.Length == 10)
					{
						text = "(" + text.Substring(0, 3) + ")" + text.Substring(3, 3) + "-" + text.Substring(6);
					}
					par.AddText(text);
					par.AddLineBreak();
				}
			}
			#endregion
			//AMOUNT ENCLOSED---------------------------------------------------------------------------------------------------
			#region Amount Enclosed
			Table table;
			Column col;
			Row row;
			Cell cell;
			frame = MigraDocHelper.CreateContainer(section, 450, 110, 330, 29);
			if (!Stmt.HidePayment)
			{
				table = MigraDocHelper.DrawTable(frame, 0, 0, 29);
				col = table.AddColumn(Unit.FromInch(1.1));
				col = table.AddColumn(Unit.FromInch(1.1));
				col = table.AddColumn(Unit.FromInch(1.1));
				row = table.AddRow();
				row.Format.Alignment = ParagraphAlignment.Center;
				row.Borders.Color = Colors.Black;
				row.Shading.Color = Colors.LightGray;
				row.TopPadding = Unit.FromInch(0);
				row.BottomPadding = Unit.FromInch(0);
				font = MigraDocHelper.CreateFont(8, true);
				cell = row.Cells[0];
				par = cell.AddParagraph();
				par.AddFormattedText("Amount Due", font);
				cell = row.Cells[1];
				par = cell.AddParagraph();
				par.AddFormattedText("Date Due", font);
				cell = row.Cells[2];
				par = cell.AddParagraph();
				par.AddFormattedText("Amount Enclosed", font);
				row = table.AddRow();
				row.Format.Alignment = ParagraphAlignment.Center;
				row.Borders.Left.Color = Colors.Gray;
				row.Borders.Bottom.Color = Colors.Gray;
				row.Borders.Right.Color = Colors.Gray;
				font = MigraDocHelper.CreateFont(9);
				if (Stmt.StatementType == StmtType.LimitedStatement)
				{
					//statementTotal and patInsEstLimited calculated above and used here and in the Floating Balance region
					if (Preferences.GetBool(PreferenceName.BalancesDontSubtractIns))
					{
						row.Cells[0].AddParagraph().AddFormattedText(statementTotal.ToString("F"), font);
					}
					else
					{//this is typical
						row.Cells[0].AddParagraph().AddFormattedText((statementTotal - patInsEstLimited).ToString("F"), font);
					}
				}
				else
				{
					double balTotal = PatGuar.BalTotal;
					if (!Preferences.GetBool(PreferenceName.BalancesDontSubtractIns))
					{//this is typical
						balTotal -= PatGuar.InsEst;
					}
					for (int m = 0; m < tableMisc.Rows.Count; m++)
					{
						//only add the payplandue value for version 1. (version 2+ already account for it when calculating aging)
						if (tableMisc.Rows[m]["descript"].ToString() == "payPlanDue" && PrefC.GetInt(PreferenceName.PayPlansVersion) == 1)
						{
							balTotal += PIn.Double(tableMisc.Rows[m]["value"].ToString());
							//payPlanDue;//PatGuar.PayPlanDue;
						}
					}
					InstallmentPlan installPlan;
					if (Stmt.ListInstallmentPlans == null)
					{
						installPlan = InstallmentPlans.GetOneForFam(PatGuar.PatNum);
					}
					else
					{
						installPlan = Stmt.ListInstallmentPlans.FirstOrDefault();
					}
					if (installPlan != null)
					{
						//show lesser of normal total balance or the monthly payment amount.
						if (installPlan.MonthlyPayment < balTotal)
						{
							text = installPlan.MonthlyPayment.ToString("F");
						}
						else
						{
							text = balTotal.ToString("F");
						}
					}
					else
					{//no installmentplan
						text = balTotal.ToString("F");
					}
					cell = row.Cells[0];
					par = cell.AddParagraph();
					par.AddFormattedText(text, font);
				}
				if (Preferences.GetLong(PreferenceName.StatementsCalcDueDate) == -1)
				{
					text = "Upon Receipt";
				}
				else
				{
					text = DateTime.Today.AddDays(Preferences.GetLong(PreferenceName.StatementsCalcDueDate)).ToShortDateString();
				}
				cell = row.Cells[1];
				par = cell.AddParagraph();
				par.AddFormattedText(text, font);
			}
			#endregion
			//Credit Card Info--------------------------------------------------------------------------------------------------
			#region Credit Card Info
			if (!Stmt.HidePayment)
			{
				float yPos = 60;
				font = MigraDocHelper.CreateFont(7, true);
				text = "CREDIT CARD TYPE";
				MigraDocHelper.DrawString(frame, text, font, 0, yPos);
				float rowHeight = 26;
				System.Drawing.Font wfont = new System.Drawing.Font("Arial", 7, FontStyle.Bold);
				Graphics g = this.CreateGraphics();//just to measure strings
				MigraDocHelper.DrawLine(frame, System.Drawing.Color.Black, g.MeasureString(text, wfont).Width,
					yPos + wfont.GetHeight(g), 326, yPos + wfont.GetHeight(g));
				yPos += rowHeight;
				text = "#";
				MigraDocHelper.DrawString(frame, text, font, 0, yPos);
				MigraDocHelper.DrawLine(frame, System.Drawing.Color.Black, g.MeasureString(text, wfont).Width,
					yPos + wfont.GetHeight(g), 326, yPos + wfont.GetHeight(g));
				yPos += rowHeight;
				text = "3 DIGIT CSV";
				MigraDocHelper.DrawString(frame, text, font, 0, yPos);
				MigraDocHelper.DrawLine(frame, System.Drawing.Color.Black, g.MeasureString(text, wfont).Width,
					yPos + wfont.GetHeight(g), 326, yPos + wfont.GetHeight(g));
				yPos += rowHeight;
				text = "EXPIRES";
				MigraDocHelper.DrawString(frame, text, font, 0, yPos);
				MigraDocHelper.DrawLine(frame, System.Drawing.Color.Black, g.MeasureString(text, wfont).Width,
					yPos + wfont.GetHeight(g), 326, yPos + wfont.GetHeight(g));
				yPos += rowHeight;
				text = "AMOUNT APPROVED";
				MigraDocHelper.DrawString(frame, text, font, 0, yPos);
				MigraDocHelper.DrawLine(frame, System.Drawing.Color.Black, g.MeasureString(text, wfont).Width,
					yPos + wfont.GetHeight(g), 326, yPos + wfont.GetHeight(g));
				yPos += rowHeight;
				text = "NAME";
				MigraDocHelper.DrawString(frame, text, font, 0, yPos);
				MigraDocHelper.DrawLine(frame, System.Drawing.Color.Black, g.MeasureString(text, wfont).Width,
					yPos + wfont.GetHeight(g), 326, yPos + wfont.GetHeight(g));
				yPos += rowHeight;
				text = "SIGNATURE";
				MigraDocHelper.DrawString(frame, text, font, 0, yPos);
				MigraDocHelper.DrawLine(frame, System.Drawing.Color.Black, g.MeasureString(text, wfont).Width,
					yPos + wfont.GetHeight(g), 326, yPos + wfont.GetHeight(g));
				yPos -= rowHeight;
				text = "(As it appears on card)";
				wfont = new System.Drawing.Font("Arial", 5);
				font = MigraDocHelper.CreateFont(5);
				MigraDocHelper.DrawString(frame, text, font, 625 - g.MeasureString(text, wfont).Width / 2 + 5, yPos + 13);
			}
			#endregion
			//Patient's Billing Address-----------------------------------------------------------------------------------------
			#region Patient Billing Address
			font = MigraDocHelper.CreateFont(11);
			frame = MigraDocHelper.CreateContainer(section, 62.5f + 12.5f, 225 + 1, 300, 200);
			par = frame.AddParagraph();
			par.Format.Font = font;
			if (Stmt.SinglePatient)
			{
				par.AddText(fam.GetNameInFamFLnoPref(Stmt.PatNum));
			}
			else
			{
				par.AddText(PatGuar.GetNameFLFormal());
			}
			par.AddLineBreak();
			par.AddText(PatGuar.Address);
			par.AddLineBreak();
			if (PatGuar.Address2 != "")
			{
				par.AddText(PatGuar.Address2);
				par.AddLineBreak();
			}
			if (CultureInfo.CurrentCulture.Name.EndsWith("CH"))
			{//CH is for switzerland. eg de-CH
				par.AddText((PatGuar.Zip + " " + PatGuar.City).Trim());
			}
			else if (CultureInfo.CurrentCulture.Name.EndsWith("SG"))
			{//SG=Singapore
				par.AddText((PatGuar.City + " " + PatGuar.Zip).Trim());
			}
			else
			{
				par.AddText(((PatGuar.City + ", " + PatGuar.State).Trim(new[] { ',', ' ' }) + " " + PatGuar.Zip).Trim());
			}
			if (!string.IsNullOrWhiteSpace(PatGuar.Country))
			{
				if (CultureInfo.CurrentCulture.Name.EndsWith("CH") || CultureInfo.CurrentCulture.Name.EndsWith("SG"))
				{//if Singapore or Switzerland
					if (!string.IsNullOrWhiteSpace(PatGuar.City + PatGuar.Zip))
					{//and either city or zip are not blank, add line break
						par.AddLineBreak();
					}
				}
				else
				{//all other cultures
					if (!string.IsNullOrWhiteSpace(PatGuar.City + PatGuar.State + PatGuar.Zip))
					{//any field, city, state or zip contain data, add line break
						par.AddLineBreak();
					}
				}
				par.AddText(PatGuar.Country);
			}
			#endregion
			//perforated line---------------------------------------------------------------------------------------------------
			#region Perforated line
			//yPos=350;//3.62 inches from top, 1/3 page down
			frame = MigraDocHelper.CreateContainer(section, 0, 350, 850, 30);
			if (!Stmt.HidePayment)
			{
				MigraDocHelper.DrawLine(frame, System.Drawing.Color.LightGray, 0, 0, 850, 0);
				text = "PLEASE DETACH AND RETURN THE UPPER PORTION WITH YOUR PAYMENT";
				font = MigraDocHelper.CreateFont(6, true, System.Drawing.Color.Gray);
				par = frame.AddParagraph();
				par.Format.Alignment = ParagraphAlignment.Center;
				par.Format.Font = font;
				par.AddText(text);
			}
			#endregion
			//Australian Provider Legend
			#region Australian Provider Legend
			int legendOffset = 0;
			if (CultureInfo.CurrentCulture.Name == "en-AU")
			{//English (Australia)
				Providers.RefreshCache();
				List<Provider> listProviders = Providers.GetAll(true);
				legendOffset = 25 + 15 * (1 + listProviders.Count);
				MigraDocHelper.InsertSpacer(section, legendOffset);
				frame = MigraDocHelper.CreateContainer(section, 45, 390, 250, legendOffset);
				par = frame.AddParagraph();
				par.Format.Font = MigraDocHelper.CreateFont(8, true);
				par.AddLineBreak();
				par.AddText("PROVIDERS:");
				par = frame.AddParagraph();
				par.Format.Font = MigraDocHelper.CreateFont(8, false);
				for (int i = 0; i < listProviders.Count; i++)
				{//All non-hidden providers are added to the legend.
					Provider prov = listProviders[i];
					string suffix = "";
					if (prov.Suffix.Trim() != "")
					{
						suffix = ", " + prov.Suffix.Trim();
					}
					par.AddText(prov.Abbr + " - " + prov.FirstName + " " + prov.LastName + suffix + " - " + prov.MedicaidID);
					par.AddLineBreak();
				}
				par.AddLineBreak();
			}
			#endregion
			//Aging-------------------------------------------------------------------------------------------------------------
			#region Aging
			MigraDocHelper.InsertSpacer(section, 275);
			frame = MigraDocHelper.CreateContainer(section, 55, 390 + legendOffset, 250, 29);
			if (!Stmt.HidePayment && Stmt.StatementType != StmtType.LimitedStatement)
			{
				table = MigraDocHelper.DrawTable(frame, 0, 0, 29);
				col = table.AddColumn(Unit.FromInch(1.1));
				col = table.AddColumn(Unit.FromInch(1.1));
				col = table.AddColumn(Unit.FromInch(1.1));
				col = table.AddColumn(Unit.FromInch(1.1));
				row = table.AddRow();
				row.Format.Alignment = ParagraphAlignment.Center;
				row.Borders.Color = Colors.Black;
				row.Shading.Color = Colors.LightGray;
				row.TopPadding = Unit.FromInch(0);
				row.BottomPadding = Unit.FromInch(0);
				font = MigraDocHelper.CreateFont(8, true);
				cell = row.Cells[0];
				par = cell.AddParagraph();
				par.AddFormattedText("0-30", font);
				cell = row.Cells[1];
				par = cell.AddParagraph();
				par.AddFormattedText("31-60", font);
				cell = row.Cells[2];
				par = cell.AddParagraph();
				par.AddFormattedText("61-90", font);
				cell = row.Cells[3];
				par = cell.AddParagraph();
				par.AddFormattedText("over 90", font);
				row = table.AddRow();
				row.Format.Alignment = ParagraphAlignment.Center;
				row.Borders.Left.Color = Colors.Gray;
				row.Borders.Bottom.Color = Colors.Gray;
				row.Borders.Right.Color = Colors.Gray;
				font = MigraDocHelper.CreateFont(9);
				text = PatGuar.Bal_0_30.ToString("F");
				cell = row.Cells[0];
				par = cell.AddParagraph();
				par.AddFormattedText(text, font);
				text = PatGuar.Bal_31_60.ToString("F");
				cell = row.Cells[1];
				par = cell.AddParagraph();
				par.AddFormattedText(text, font);
				text = PatGuar.Bal_61_90.ToString("F");
				cell = row.Cells[2];
				par = cell.AddParagraph();
				par.AddFormattedText(text, font);
				text = PatGuar.BalOver90.ToString("F");
				cell = row.Cells[3];
				par = cell.AddParagraph();
				par.AddFormattedText(text, font);
			}
			/*
			ODGridColumn gcol;
			ODGridRow grow;
			if(!Stmt.HidePayment) {
				ODGrid gridAging=new ODGrid();
				gridAging.TranslationName="";
				this.Controls.Add(gridAging);
				gridAging.BeginUpdate();
				gridAging.Columns.Clear();
				gcol=new ODGridColumn("0-30",70,HorizontalAlignment.Center);
				gridAging.Columns.Add(gcol);
				gcol=new ODGridColumn("31-60",70,HorizontalAlignment.Center);
				gridAging.Columns.Add(gcol);
				gcol=new ODGridColumn("61-90",70,HorizontalAlignment.Center);
				gridAging.Columns.Add(gcol);
				gcol=new ODGridColumn("over 90",70,HorizontalAlignment.Center);
				gridAging.Columns.Add(gcol);
				if(Prefs.GetBool(PrefName.BalancesDontSubtractIns")) {//less common
					gcol=new ODGridColumn("Balance",70,HorizontalAlignment.Center);
					gridAging.Columns.Add(gcol);
					gcol=new ODGridColumn("InsPending",70,HorizontalAlignment.Center);
					gridAging.Columns.Add(gcol);
					gcol=new ODGridColumn("AfterIns",70,HorizontalAlignment.Center);
					gridAging.Columns.Add(gcol);
				}
				else{//more common
					gcol=new ODGridColumn("Total",70,HorizontalAlignment.Center);
					gridAging.Columns.Add(gcol);
					gcol=new ODGridColumn("- InsEst",70,HorizontalAlignment.Center);
					gridAging.Columns.Add(gcol);
					gcol=new ODGridColumn("= Balance",70,HorizontalAlignment.Center);
					gridAging.Columns.Add(gcol);
				}
				gridAging.Rows.Clear();
				//Annual max--------------------------
				grow=new ODGridRow();
				grow.Cells.Add(PatGuar.Bal_0_30.ToString("F"));
				grow.Cells.Add(PatGuar.Bal_31_60.ToString("F"));
				grow.Cells.Add(PatGuar.Bal_61_90.ToString("F"));
				grow.Cells.Add(PatGuar.BalOver90.ToString("F"));
				grow.Cells.Add(PatGuar.BalTotal.ToString("F"));
				grow.Cells.Add(PatGuar.InsEst.ToString("F"));
				grow.Cells.Add((PatGuar.BalTotal-PatGuar.InsEst).ToString("F"));
				gridAging.Rows.Add(grow);
				gridAging.EndUpdate();
				MigraDocHelper.DrawGrid(section,gridAging);
				gridAging.Dispose();
			*/
			#endregion
			//Floating Balance, Ins info----------------------------------------------------------------------------------------
			#region FloatingBalance
			frame = MigraDocHelper.CreateContainer(section, 460, 380 + legendOffset, 250, 200);
			//table=MigraDocHelper.DrawTable(frame,0,0,90);
			par = frame.AddParagraph();
			parformat = new ParagraphFormat();
			parformat.Alignment = ParagraphAlignment.Right;
			par.Format = parformat;
			font = MigraDocHelper.CreateFont(10, false);
			MigraDoc.DocumentObjectModel.Font fontBold = MigraDocHelper.CreateFont(10, true);
			if (Stmt.IsInvoice)
			{
				text = "Procedures:";
				par.AddFormattedText(text, font);
				par.AddLineBreak();
				text = "Adjustments:";
				par.AddFormattedText(text, font);
				par.AddLineBreak();
				if (PrefC.GetInt(PreferenceName.PayPlansVersion) == (int)PayPlanVersions.AgeCreditsAndDebits)
				{
					text = "Pay Plan Charges:";
					par.AddFormattedText(text, font);
					par.AddLineBreak();
				}
				text = "Total:";
				par.AddFormattedText(text, font);
				par.AddLineBreak();
			}
			else if (Preferences.GetBool(PreferenceName.BalancesDontSubtractIns))
			{
				text = "Balance:";
				par.AddFormattedText(text, fontBold);
				//par.AddLineBreak();
				//text = "Ins Pending:";
				//par.AddFormattedText(text, font);
				//par.AddLineBreak();
				//text = "After Ins:";
				//par.AddFormattedText(text, font);
				//par.AddLineBreak();
			}
			else
			{//this is more common
				if (Preferences.GetBool(PreferenceName.FuchsOptionsOn))
				{
					text = "Balance:";
					par.AddFormattedText(text, font);
					par.AddLineBreak();
					text = "-Ins Estimate:";
					par.AddFormattedText(text, font);
					par.AddLineBreak();
					text = "=Owed Now:";
					par.AddFormattedText(text, fontBold);
					par.AddLineBreak();
				}
				else
				{
					text = "Total:";
					par.AddFormattedText(text, font);
					par.AddLineBreak();
					text = "-Ins Estimate:";
					par.AddFormattedText(text, font);
					par.AddLineBreak();
					text = "=Balance:";
					par.AddFormattedText(text, fontBold);
					par.AddLineBreak();
				}
			}
			frame = MigraDocHelper.CreateContainer(section, 730, 380 + legendOffset, 100, 200);
			//table=MigraDocHelper.DrawTable(frame,0,0,90);
			par = frame.AddParagraph();
			parformat = new ParagraphFormat();
			parformat.Alignment = ParagraphAlignment.Left;
			par.Format = parformat;
			font = MigraDocHelper.CreateFont(10, false);
			//numbers:
			if (Stmt.IsInvoice)
			{
				double adjAmt = 0;
				double procAmt = 0;
				double payplanAmt = 0;
				DataTable tableAcct;
				string tableName;
				for (int i = 0; i < dataSet.Tables.Count; i++)
				{
					tableAcct = dataSet.Tables[i];
					tableName = tableAcct.TableName;
					if (!tableName.StartsWith("account"))
					{
						continue;
					}
					for (int p = 0; p < tableAcct.Rows.Count; p++)
					{
						if (tableAcct.Rows[p]["AdjNum"].ToString() != "0")
						{
							adjAmt -= PIn.Double(tableAcct.Rows[p]["creditsDouble"].ToString());
							adjAmt += PIn.Double(tableAcct.Rows[p]["chargesDouble"].ToString());
						}
						else if (tableAcct.Rows[p]["PayPlanChargeNum"].ToString() != "0")
						{
							payplanAmt += PIn.Double(tableAcct.Rows[p]["chargesDouble"].ToString());
						}
						else
						{//must be a procedure
							procAmt += PIn.Double(tableAcct.Rows[p]["chargesDouble"].ToString());
						}
					}
				}
				text = procAmt.ToString("c");
				par.AddFormattedText(text, font);
				par.AddLineBreak();
				text = adjAmt.ToString("c");
				par.AddFormattedText(text, font);
				par.AddLineBreak();
				if (PrefC.GetInt(PreferenceName.PayPlansVersion) == (int)PayPlanVersions.AgeCreditsAndDebits)
				{
					text = payplanAmt.ToString("c");
					par.AddFormattedText(text, font);
					par.AddLineBreak();
					text = (procAmt + adjAmt + payplanAmt).ToString("c");
				}
				else
				{
					text = (procAmt + adjAmt).ToString("c");
				}
				par.AddFormattedText(text, fontBold);
			}
			else if (Stmt.StatementType == StmtType.LimitedStatement)
			{
				//statementTotal and patInsEstLimited calculated above and used here and in the Amount Enclosed region
				if (Preferences.GetBool(PreferenceName.BalancesDontSubtractIns))
				{
					par.AddFormattedText(statementTotal.ToString("c"), font);
				}
				else
				{//this is typical
					par.AddFormattedText(statementTotal.ToString("c"), font).AddLineBreak();
					par.AddFormattedText(patInsEstLimited.ToString("c"), font).AddLineBreak();
					par.AddFormattedText((statementTotal - patInsEstLimited).ToString("c"), fontBold);
				}
			}
			else if (Preferences.GetBool(PreferenceName.BalancesDontSubtractIns))
			{
				if (Stmt.SinglePatient)
				{
					//Show the current patient's balance without subtracting insurance estimates.
					text = pat.EstBalance.ToString("c");
					par.AddFormattedText(text, font);
				}
				else
				{
					//Show the current family's balance without subtracting insurance estimates.
					text = PatGuar.BalTotal.ToString("c");
					par.AddFormattedText(text, fontBold);
				}
			}
			else
			{//more common
				if (Stmt.SinglePatient)
				{
					double patInsEst = 0;
					for (int m = 0; m < tableMisc.Rows.Count; m++)
					{
						if (tableMisc.Rows[m]["descript"].ToString() == "patInsEst")
						{
							patInsEst = PIn.Double(tableMisc.Rows[m]["value"].ToString());
						}
					}
					double patBal = pat.EstBalance - patInsEst;
					text = pat.EstBalance.ToString("c");
					par.AddFormattedText(text, font);
					par.AddLineBreak();
					text = patInsEst.ToString("c");
					par.AddFormattedText(text, font);
					par.AddLineBreak();
					text = patBal.ToString("c");
					par.AddFormattedText(text, fontBold);
				}
				else
				{
					text = PatGuar.BalTotal.ToString("c");
					par.AddFormattedText(text, font);
					par.AddLineBreak();
					text = PatGuar.InsEst.ToString("c");
					par.AddFormattedText(text, font);
					par.AddLineBreak();
					text = (PatGuar.BalTotal - PatGuar.InsEst).ToString("c");
					par.AddFormattedText(text, fontBold);
					par.AddLineBreak();
				}
			}
			MigraDocHelper.InsertSpacer(section, 80);
			#endregion FloatingBalance
			//Bold note---------------------------------------------------------------------------------------------------------
			#region Bold note
			if (Stmt.NoteBold != "")
			{
				MigraDocHelper.InsertSpacer(section, 7);
				font = MigraDocHelper.CreateFont(10, true, System.Drawing.Color.DarkRed);
				par = section.AddParagraph();
				par.Format.Font = font;
				par.AddText(Stmt.NoteBold);
				MigraDocHelper.InsertSpacer(section, 8);
			}
			#endregion Bold note
			//Payment plan grid definition--------------------------------------------------------------------------------------
			#region PayPlan grid definition
			GridColumn gcol;
			GridRow grow;
			ODGrid gridPP = new ODGrid();
			gridPP.TranslationName = "";
			this.Controls.Add(gridPP);
			gridPP.BeginUpdate();
			gridPP.Columns.Clear();
			gcol = new GridColumn("Date", 73);
			gridPP.Columns.Add(gcol);
			gcol = new GridColumn("Description", 270);
			gridPP.Columns.Add(gcol);
			gcol = new GridColumn("Charges", 60, HorizontalAlignment.Right);
			gridPP.Columns.Add(gcol);
			gcol = new GridColumn("Credits", 60, HorizontalAlignment.Right);
			gridPP.Columns.Add(gcol);
			gcol = new GridColumn("Balance", 60, HorizontalAlignment.Right);
			gridPP.Columns.Add(gcol);
			gridPP.Width = gridPP.WidthAllColumns + 20;
			gridPP.EndUpdate();
			#endregion PayPlan grid definition
			//Payment plan grid.  There will be only one, if any----------------------------------------------------------------
			#region PayPlan grid
			//We currently show payment plan breakdowns on all statements, receipts, and invoices.
			DataTable tablePP = dataSet.Tables["payplan"];
			GridCell gcell;
			if (tablePP.Rows.Count > 0)
			{
				//MigraDocHelper.InsertSpacer(section,5);
				par = section.AddParagraph();
				par.Format.Font = MigraDocHelper.CreateFont(10, true);
				par.Format.Alignment = ParagraphAlignment.Center;
				//par.Format.SpaceBefore=Unit.FromInch(.05);
				//par.Format.SpaceAfter=Unit.FromInch(.05);
				par.AddText("Payment Plans");
				MigraDocHelper.InsertSpacer(section, 2);
				gridPP.BeginUpdate();
				gridPP.Rows.Clear();
				for (int p = 0; p < tablePP.Rows.Count; p++)
				{
					grow = new GridRow();
					grow.Cells.Add(tablePP.Rows[p]["date"].ToString());
					grow.Cells.Add(tablePP.Rows[p]["description"].ToString());
					grow.Cells.Add(tablePP.Rows[p]["charges"].ToString());
					grow.Cells.Add(tablePP.Rows[p]["credits"].ToString());
					gcell = new GridCell(tablePP.Rows[p]["balance"].ToString());
					if (p == tablePP.Rows.Count - 1)
					{
						gcell.Bold = true;
					}
					else if (tablePP.Rows[p + 1]["balance"].ToString() == "")
					{//if next row balance is blank.
						gcell.Bold = true;
					}
					grow.Cells.Add(gcell);
					gridPP.Rows.Add(grow);
				}
				gridPP.EndUpdate();
				MigraDocHelper.DrawGrid(section, gridPP);
				MigraDocHelper.InsertSpacer(section, 2);
				par = section.AddParagraph();
				par.Format.Font = MigraDocHelper.CreateFont(10, true);
				par.Format.Alignment = ParagraphAlignment.Right;
				par.Format.RightIndent = Unit.FromInch(0.25);
				double payPlanDue = 0;
				for (int m = 0; m < tableMisc.Rows.Count; m++)
				{
					if (tableMisc.Rows[m]["descript"].ToString() == "payPlanDue")
					{
						payPlanDue = PIn.Double(tableMisc.Rows[m]["value"].ToString());
					}
				}
				par.AddText("Payment Plan Amount Due: " + payPlanDue.ToString("c"));//PatGuar.PayPlanDue.ToString("c"));
				MigraDocHelper.InsertSpacer(section, 10);
			}
			#endregion PayPlan grid
			//Body Table definition---------------------------------------------------------------------------------------------
			#region Body Table definition
			ODGrid gridPat = new ODGrid();
			gridPat.TranslationName = "";
			this.Controls.Add(gridPat);
			gridPat.BeginUpdate();
			gridPat.Columns.Clear();
			gcol = new GridColumn("Date", 73);
			gridPat.Columns.Add(gcol);
			gcol = new GridColumn("Patient", 100);
			gridPat.Columns.Add(gcol);
			//prov
			if (Clinics.IsMedicalClinic(Clinics.ClinicId))
			{
				gcol = new GridColumn("Code", 87);
				gridPat.Columns.Add(gcol);
			}
			else
			{
				gcol = new GridColumn("Code", 45);
				gridPat.Columns.Add(gcol);
				gcol = new GridColumn("Tooth", 42);
				gridPat.Columns.Add(gcol);
			}
			gcol = new GridColumn("Description", 270);
			gridPat.Columns.Add(gcol);
			gcol = new GridColumn("Charges", 60, HorizontalAlignment.Right);
			gridPat.Columns.Add(gcol);
			gcol = new GridColumn("Credits", 60, HorizontalAlignment.Right);
			gridPat.Columns.Add(gcol);
			if (Stmt.IsInvoice)
			{
				gcol = new GridColumn("Total", 60, HorizontalAlignment.Right);
				gridPat.Columns.Add(gcol);
			}
			else
			{
				gcol = new GridColumn("Balance", 60, HorizontalAlignment.Right);
				gridPat.Columns.Add(gcol);
			}
			gridPat.Width = gridPat.WidthAllColumns + 20;
			gridPat.EndUpdate();
			#endregion Body Table definition
			//Loop through each table.  Could be one intermingled, or one for each patient--------------------------------------
			#region Main Grid(s)
			DataTable tableAccount;
			string tablename;
			long patnum;
			for (int i = 0; i < dataSet.Tables.Count; i++)
			{
				tableAccount = dataSet.Tables[i];
				tablename = tableAccount.TableName;
				if (!tablename.StartsWith("account"))
				{
					continue;
				}
				par = section.AddParagraph();
				par.Format.Font = MigraDocHelper.CreateFont(10, true);
				par.Format.SpaceBefore = Unit.FromInch(.05);
				par.Format.SpaceAfter = Unit.FromInch(.05);
				patnum = 0;
				if (tablename != "account")
				{//account123 etc.
					patnum = PIn.Long(tablename.Substring(7));
				}
				if (patnum != 0)
				{
					par.AddText(fam.GetNameInFamFLnoPref(patnum));
				}
				//if(FamilyStatementDataList[famIndex].PatAboutList[i].ApptDescript!=""){
				//	par=section.AddParagraph();
				//	par.Format.Font=MigraDocHelper.CreateFont(9);//same as body font
				//	par.AddText(FamilyStatementDataList[famIndex].PatAboutList[i].ApptDescript);
				//}
				gridPat.BeginUpdate();
				gridPat.Rows.Clear();
				//lineData=FamilyStatementDataList[famIndex].PatDataList[i].PatData;
				foreach (DataRow rowCur in tableAccount.Rows)
				{
					if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
					{//Canadian. en-CA or fr-CA
						if (Stmt.IsReceipt)
						{
							if (rowCur["StatementNum"].ToString() != "0")
							{//Hide statement rows for Canadian receipts.
								continue;
							}
							if (rowCur["ClaimNum"].ToString() != "0")
							{//Hide claim rows and claim payment rows for Canadian receipts.
								continue;
							}
						}
					}
					if (CultureInfo.CurrentCulture.Name == "en-US")
					{
						if (Stmt.IsReceipt)
						{
							if (rowCur["PayNum"].ToString() == "0")
							{//Hide everything except patient payments
								continue;
							}
						}
						//js Some additional features would be nice for receipts, such as hiding the bal column, the aging, and the amount due sections.
					}
					grow = new GridRow();
					grow.Cells.Add(rowCur["date"].ToString());
					grow.Cells.Add(rowCur["patient"].ToString());
					if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
					{//Canadian. en-CA or fr-CA
						if (Stmt.IsReceipt)
						{
							grow.Cells.Add("");//Code: blank in Canada normally because this information is used on taxes and is considered a security concern.
							grow.Cells.Add("");//Tooth: blank in Canada normally because this information is used on taxes and is considered a security concern.
						}
						else
						{
							grow.Cells.Add(rowCur["ProcCode"].ToString());
							if (!Clinics.IsMedicalClinic(Clinics.ClinicId))
							{
								grow.Cells.Add(rowCur["tth"].ToString());
							}
						}
					}
					else
					{
						grow.Cells.Add(rowCur["ProcCode"].ToString());
						if (!Clinics.IsMedicalClinic(Clinics.ClinicId))
						{
							grow.Cells.Add(rowCur["tth"].ToString());
						}
					}
					if (CultureInfo.CurrentCulture.Name == "en-AU")
					{//English (Australia)
						if (rowCur["prov"].ToString().Trim() != "")
						{
							grow.Cells.Add(rowCur["prov"].ToString() + " - " + rowCur["description"].ToString());
						}
						else
						{//No provider on this account row item, so don't put the extra leading characters.
							grow.Cells.Add(rowCur["description"].ToString());
						}
					}
					else if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
					{//Canadian. en-CA or fr-CA
						if (Stmt.IsReceipt)
						{
							if (PIn.Long(rowCur["ProcNum"].ToString()) == 0)
							{
								grow.Cells.Add(rowCur["description"].ToString());
							}
							else
							{//Only clear description for procedures.
								grow.Cells.Add("");//Description: blank in Canada normally because this information is used on taxes and is considered a security concern.
							}
						}
						else
						{
							grow.Cells.Add(rowCur["description"].ToString());
						}
					}
					else
					{//Assume English (United States)
						grow.Cells.Add(rowCur["description"].ToString());
					}
					grow.Cells.Add(rowCur["charges"].ToString());
					grow.Cells.Add(rowCur["credits"].ToString());
					grow.Cells.Add(rowCur["balance"].ToString());
					gridPat.Rows.Add(grow);
				}
				gridPat.EndUpdate();
				MigraDocHelper.DrawGrid(section, gridPat);
				//Total
				frame = MigraDocHelper.CreateContainer(section);
				font = MigraDocHelper.CreateFont(9, true);
				float totalPos = ((float)(doc.DefaultPageSetup.PageWidth.Inch//-doc.DefaultPageSetup.LeftMargin.Inch
																			 //-doc.DefaultPageSetup.RightMargin.Inch)
					) * 100f) / 2f + (float)gridPat.WidthAllColumns / 2f + 7;
				RectangleF rectF = new RectangleF(0, 0, totalPos, 16);
				if (patnum != 0)
				{
					MigraDocHelper.DrawString(frame, " ",
						//I decided this was unnecessary:
						//dataSet.Tables["patient"].Rows[fam.GetIndex(patnum)]["balance"].ToString(),
						font, rectF, ParagraphAlignment.Right);
					//MigraDocHelper.DrawString(frame,FamilyStatementDataList[famIndex].PatAboutList[i].Balance.ToString("F"),font,rectF,
					//	ParagraphAlignment.Right);
				}
			}
			gridPat.Dispose();
			#endregion
			//Future appointments-----------------------------------------------------------------------------------------------
			#region Future appointments
			if (!Stmt.IsReceipt && !Stmt.IsInvoice)
			{
				font = MigraDocHelper.CreateFont(9);
				DataTable tableAppt = dataSet.Tables["appts"];
				if (tableAppt.Rows.Count > 0)
				{
					par = section.AddParagraph();
					par.Format.Font = font;
					par.AddText("Scheduled Appointments:");
				}
				for (int i = 0; i < tableAppt.Rows.Count; i++)
				{
					par.AddLineBreak();
					par.AddText(tableAppt.Rows[i]["descript"].ToString());
				}
				if (tableAppt.Rows.Count > 0)
				{
					MigraDocHelper.InsertSpacer(section, 10);
				}
			}
			#endregion Future appointments
			//Region specific static notes--------------------------------------------------------------------------------------
			#region Region specific static notes i.e. "KEEP THIS RECEIPT FOR INCOME TAX PURPOSES" for Canada
			if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{//Canadian. en-CA or fr-CA
				if (Stmt.IsReceipt)
				{
					font = MigraDocHelper.CreateFont(9);
					par = section.AddParagraph();
					par.Format.Font = font;
					par.AddText("KEEP THIS RECEIPT FOR INCOME TAX PURPOSES");
					MigraDocHelper.InsertSpacer(section, 10);
				}
			}
			#endregion
			//Note--------------------------------------------------------------------------------------------------------------
			#region Note and Note BOLD
			font = MigraDocHelper.CreateFont(9);
			par = section.AddParagraph();
			par.Format.Font = font;
			par.AddText(Stmt.Note);
			//bold note
			if (Stmt.NoteBold != "")
			{
				MigraDocHelper.InsertSpacer(section, 10);
				font = MigraDocHelper.CreateFont(10, true, System.Drawing.Color.DarkRed);
				par = section.AddParagraph();
				par.Format.Font = font;
				par.AddText(Stmt.NoteBold);
			}
			#endregion
			//Swiss Banking-----------------------------------------------------------------------------------------------------
			#region SwissBanking
			if (CultureInfo.CurrentCulture.Name.EndsWith("CH"))
			{//CH is for switzerland. eg de-CH
			 //&& pagesPrinted==0)//only on the first page
			 //{
			 //float yred=744;//768;//660 for testing
			 //Red line (just temp)
			 //g.DrawLine(Pens.Red,0,yred,826,yred);
				MigraDoc.DocumentObjectModel.Font swfont = MigraDocHelper.CreateFont(10);
				//new Font(FontFamily.GenericSansSerif,10);
				//Bank Address---------------------------------------------------------
				HeaderFooter footer = section.Footers.Primary;
				footer.Format.Borders.Color = Colors.Black;
				//footer.AddParagraph(Prefs.GetString(PrefName.BankAddress"));
				frame = footer.AddTextFrame();
				frame.RelativeVertical = RelativeVertical.Line;
				frame.RelativeHorizontal = RelativeHorizontal.Page;
				frame.MarginLeft = Unit.Zero;
				frame.MarginTop = Unit.Zero;
				frame.Top = TopPosition.Parse("0 in");
				frame.Left = LeftPosition.Parse("0 in");
				frame.Width = Unit.FromInch(8.3);
				frame.Height = 300;
				//RectangleF=new RectangleF(0,0,
				MigraDocHelper.DrawString(frame, Preferences.GetString(PreferenceName.BankAddress), swfont, 30, 30);
				MigraDocHelper.DrawString(frame, Preferences.GetString(PreferenceName.BankAddress), swfont, 246, 30);
				//Office Name and Address----------------------------------------------
				text = Preferences.GetString(PreferenceName.PracticeTitle) + "\r\n"
					+ Preferences.GetString(PreferenceName.PracticeAddress) + "\r\n";
				if (Preferences.GetString(PreferenceName.PracticeAddress2) != "")
				{
					text += Preferences.GetString(PreferenceName.PracticeAddress2) + "\r\n";
				}
				text += Preferences.GetString(PreferenceName.PracticeZip) + " " + Preferences.GetString(PreferenceName.PracticeCity);
				MigraDocHelper.DrawString(frame, text, swfont, 30, 89);
				MigraDocHelper.DrawString(frame, text, swfont, 246, 89);
				//Bank account number--------------------------------------------------
				string origBankNum = Preferences.GetString(PreferenceName.PracticeBankNumber);//must be exactly 9 digits. 2+6+1.
																				  //the 6 digit portion might have 2 leading 0's which would not go into the dashed bank num.
				string dashedBankNum = "?";
				//examples: 01-200027-2
				//          01-4587-1  (from 010045871)
				if (origBankNum.Length == 9)
				{
					dashedBankNum = origBankNum.Substring(0, 2) + "-"
						+ origBankNum.Substring(2, 6).TrimStart(new char[] { '0' }) + "-"
						+ origBankNum.Substring(8, 1);
				}
				swfont = MigraDocHelper.CreateFont(9, true);
				//new Font(FontFamily.GenericSansSerif,9,FontStyle.Bold);
				MigraDocHelper.DrawString(frame, dashedBankNum, swfont, 95, 169);
				MigraDocHelper.DrawString(frame, dashedBankNum, swfont, 340, 169);
				//Amount------------------------------------------------------------
				double amountdue = PatGuar.BalTotal - PatGuar.InsEst;
				text = amountdue.ToString("F2");
				text = text.Substring(0, text.Length - 3);
				swfont = MigraDocHelper.CreateFont(10);
				MigraDocHelper.DrawString(frame, text, swfont, new RectangleF(50, 205, 100, 25), ParagraphAlignment.Right);
				MigraDocHelper.DrawString(frame, text, swfont, new RectangleF(290, 205, 100, 25), ParagraphAlignment.Right);
				text = amountdue.ToString("F2");//eg 92.00
				text = text.Substring(text.Length - 2, 2);//eg 00
				MigraDocHelper.DrawString(frame, text, swfont, 185, 205);
				MigraDocHelper.DrawString(frame, text, swfont, 425, 205);
				//Patient Address-----------------------------------------------------
				string patAddress = PatGuar.FName + " " + PatGuar.LName + "\r\n"
					+ PatGuar.Address + "\r\n";
				if (PatGuar.Address2 != "")
				{
					patAddress += PatGuar.Address2 + "\r\n";
				}
				patAddress += PatGuar.Zip + " " + PatGuar.City;
				patAddress += ((PatGuar.Country == "") ? "" : "\r\n" + PatGuar.Country);
				MigraDocHelper.DrawString(frame, text, swfont, 495, 218);//middle left
				MigraDocHelper.DrawString(frame, text, swfont, 30, 263);//Lower left
																		//Compute Reference#------------------------------------------------------
																		//Reference# has exactly 27 digits
																		//First 6 numbers are what we are calling the BankRouting number.
																		//Next 20 numbers represent the invoice #.
																		//27th number is the checksum
				string referenceNum = Preferences.GetString(PreferenceName.BankRouting);//6 digits
				if (referenceNum.Length != 6)
				{
					referenceNum = "000000";
				}
				referenceNum += PatGuar.PatNum.ToString().PadLeft(12, '0')
					//"000000000000"//12 0's
					+ DateTime.Today.ToString("yyyyMMdd");//+8=20
														  //for testing:
														  //referenceNum+="09090271100000067534";
														  //"00000000000000037112";
				referenceNum += Modulo10(referenceNum).ToString();
				//at this point, the referenceNum will always be exactly 27 digits long.
				string spacedRefNum = referenceNum.Substring(0, 2) + " " + referenceNum.Substring(2, 5) + " " + referenceNum.Substring(7, 5)
					+ " " + referenceNum.Substring(12, 5) + " " + referenceNum.Substring(17, 5) + " " + referenceNum.Substring(22, 5);
				//text=spacedRefNum.Substring(0,15)+"\r\n"+spacedRefNum.Substring(16)+"\r\n";
				//reference# at lower left above address.  Small
				swfont = MigraDocHelper.CreateFont(7);
				MigraDocHelper.DrawString(frame, spacedRefNum, swfont, 30, 243);
				//Reference# at upper right---------------------------------------------------------------
				swfont = MigraDocHelper.CreateFont(10);
				MigraDocHelper.DrawString(frame, spacedRefNum, swfont, 490, 140);
				//Big long number at the lower right--------------------------------------------------
				/*The very long number on the bottom has this format:
				>13 numbers > 27 numbers + 9 numbers >
				>Example: 0100000254306>904483000000000000000371126+ 010045871>
				>
				>The first group of 13 numbers would begin with either 01 or only have 
				>042 without any other following numbers.  01 would be used if there is 
				>a specific amount, and 042 would be used if there is not a specific 
				>amount billed. So in the example, the billed amount is 254.30.  It has 
				>01 followed by leading zeros to fill in the balance of the digits 
				>required.  The last digit is a checksum done by the program.  If the 
				>amount would be 1,254.30 then the example should read 0100001254306.
				>
				>There is a > separator, then the reference number made up previously.
				>
				>Then a + separator, followed by the bank account number.  Previously, 
				>the number printed without the zeros, but in this case it has the zeros 
				>and not the dashes.*/
				swfont = new MigraDoc.DocumentObjectModel.Font("OCR-B 10 BT", 12);
				text = "01" + amountdue.ToString("F2").Replace(".", "").PadLeft(10, '0');
				text += Modulo10(text).ToString() + ">"
					+ referenceNum + "+ " + origBankNum + ">";
				MigraDocHelper.DrawString(frame, text, swfont, 255, 345);
			}
			#endregion SwissBanking
			return doc;
		}

		///<summary>data may only contain numbers between 0 und 9</summary>
		private int Modulo10(string strNumber)
		{
			//try{
			int[] intTable = { 0, 9, 4, 6, 8, 2, 7, 1, 3, 5 };
			int intTransfer = 0;
			for (int intIndex = 0; intIndex < strNumber.Length; intIndex++)
			{
				if (!Char.IsDigit(strNumber[intIndex]))
				{
					continue;
				}
				int digit = Convert.ToInt32(strNumber.Substring(intIndex, 1));
				int modulus = (intTransfer + digit) % 10;
				intTransfer = intTable[modulus];
			}
			return (10 - intTransfer) % 10;
			//}
			//catch{
			//	return 0;
			//}
		}

		private void butBack_Click(object sender, System.EventArgs e)
		{
			if (printPreviewControl2.StartPage == 0)
				return;
			printPreviewControl2.StartPage--;
			labelTotPages.Text = (printPreviewControl2.StartPage + 1).ToString()
				+ " / " + totalPages.ToString();
		}

		private void butFwd_Click(object sender, System.EventArgs e)
		{
			if (printPreviewControl2.StartPage == totalPages - 1) return;
			printPreviewControl2.StartPage++;
			labelTotPages.Text = (printPreviewControl2.StartPage + 1).ToString()
				+ " / " + totalPages.ToString();
		}

		private void butPrint_Click(object sender, System.EventArgs e)
		{
			//just for debugging
			/*PrintReport(false);
			DialogResult=DialogResult.Cancel;*/
		}
	}
}
