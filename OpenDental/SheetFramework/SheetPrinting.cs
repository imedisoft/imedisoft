using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace OpenDental
{
    public class SheetPrinting
	{
		///<summary>Print margin of the default printer. only used in page break calulations, and only top and bottom are used.</summary>
		private static Margins _printMargin = new Margins(0, 0, 40, 60);//jordan static only because it's an unchanging val.

		#region Properties
		///<summary>The treatment finder needs this so that it can use the same Margins in its page calculations.</summary>
		public static Margins PrintMargin
		{
			get
			{
				return _printMargin;
			}
		}
		#endregion Properties

		#region Methods - Public Printing
		///<summary>If printing a statement, use the polymorphism that takes a DataSet otherwise this method will make another call to the db.</summary>
		public static void Print(Sheet sheet, int copies = 1, bool isRxControlled = false, Statement stmt = null, MedLab medLab = null, bool isPrintDocument = true, bool isPreviewMode = false)
		{
			SheetPrintingJob sheetPrintingJob = new SheetPrintingJob();
			sheetPrintingJob.Print(sheet, copies, isRxControlled, stmt, medLab, isPrintDocument, isPreviewMode);
		}

		///<Summary>DataSet should be prefilled with AccountModules.GetAccount() before calling this method if printing a statement.  Returns null if document failed to print.</Summary>
		public static void Print(Sheet sheet, DataSet dataSet, int copies = 1, bool isRxControlled = false, Statement stmt = null, MedLab medLab = null,
			bool isPrintDocument = true, bool isPreviewMode = false)
		{
			SheetPrintingJob sheetPrintingJob = new SheetPrintingJob();
			sheetPrintingJob.Print(sheet, dataSet, copies, isRxControlled, stmt, medLab);
		}

		public static void PrintBatch(List<Sheet> sheetBatch)
		{
			SheetPrintingJob sheetPrintingJob = new SheetPrintingJob();
			sheetPrintingJob.PrintBatch(sheetBatch);
		}

		///<summary>Performs validation on each Rx before printing.  If one of the given Rx does not pass validation, then the entire batch is aborted, because it is easy for user to fix errors.</summary>
		public static void PrintMultiRx(List<RxPat> listRxs)
		{
			SheetPrintingJob sheetPrintingJob = new SheetPrintingJob();
			sheetPrintingJob.PrintMultiRx(listRxs);
		}

		///<summary>Performs validation on the given Rx before printing.  Returns false if validation failed.</summary>
		public static bool PrintRx(Sheet sheet, RxPat rx)
		{
			SheetPrintingJob sheetPrintingJob = new SheetPrintingJob();
			return sheetPrintingJob.PrintRx(sheet, rx);
		}

		///<summary>Validates one Rx.  Returns a string of error messages.  Blank string indicates no errors.
		///Some Rx require certain data to be present when printing.</summary>
		public static string ValidateRxForSheet(RxPat rx)
		{
			if (!Prefs.GetBool(PrefName.RxHasProc))
			{
				return "";//The global preference allows the user to completely disable Rx ProcCode validation, even if some Rx are flagged as required.
			}
			if (Clinics.ClinicId != 0)
			{//Not HQ
				Clinic clinic = Clinics.GetById(Clinics.ClinicId);
				if (!clinic.HasProcedureOnRx)
				{
					//The clinic option allows the user to completely disable Rx ProcCode validation for one clinic.
					//A customer who needs this feature has some clinics in states which require ProCode and some clinics in states which do not.
					return "";
				}
			}
			if (!rx.IsProcRequired)
			{
				return "";//No ProCode validation required for this specific Rx.
			}
			StringBuilder sb = new StringBuilder();
			if (rx.ProcNum == 0)
			{
				if (sb.Length > 0)
				{
					sb.Append(", ");
				}
				sb.Append("Procedure");
			}
			if (rx.DaysOfSupply <= 0)
			{
				if (sb.Length > 0)
				{
					sb.Append(", ");
				}
				sb.Append("Days of Supply");
			}
			return sb.ToString();
		}
		#endregion Methods - Public Printing

		#region Methods - Drawing
		///<Summary>DataSet should be prefilled with AccountModules.GetAccount() before calling this method if printing a statement.</Summary>
		public static void DrawFieldGrid(SheetField field, Sheet sheet, Graphics g, XGraphics gx, DataSet dataSet, Statement stmt, MedLab medLab,
			bool isPrinting = false, Patient pat = null, Patient patGuar = null)
		{
			SheetDrawingJob sheetDrawingJob = new SheetDrawingJob();
			sheetDrawingJob.DrawFieldGrid(field, sheet, g, gx, dataSet, stmt, medLab, isPrinting, pat, patGuar);
		}

		public static void DrawProcsGraphics(List<Procedure> procList, ToothChartRelay toothChartRelay, List<ToothInitial> toothInitialList)
		{
			//this method must also stay in OD.exe
			Procedure proc;
			string[] teeth;
			System.Drawing.Color cLight = System.Drawing.Color.White;
			System.Drawing.Color cDark = System.Drawing.Color.White;
			List<Definition> listDefs = Definitions.GetDefsForCategory(DefinitionCategory.ChartGraphicColors, true);
			for (int i = 0; i < procList.Count; i++)
			{
				proc = procList[i];
				//if(proc.ProcStatus!=procStat) {
				//  continue;
				//}
				if (proc.HideGraphics)
				{
					continue;
				}
				if (ProcedureCodes.GetProcCode(proc.CodeNum).PaintType == ToothPaintingType.Extraction && (
					proc.ProcStatus == ProcStat.C
					|| proc.ProcStatus == ProcStat.EC
					|| proc.ProcStatus == ProcStat.EO
					))
				{
					continue;//prevents the red X. Missing teeth already handled.
				}
				if (ProcedureCodes.GetProcCode(proc.CodeNum).GraphicColor == System.Drawing.Color.FromArgb(0))
				{
					switch (proc.ProcStatus)
					{
						case ProcStat.C:
							cDark = listDefs[1].Color;
							cLight = listDefs[6].Color;
							break;
						case ProcStat.TP:
							cDark = listDefs[0].Color;
							cLight = listDefs[5].Color;
							break;
						case ProcStat.EC:
							cDark = listDefs[2].Color;
							cLight = listDefs[7].Color;
							break;
						case ProcStat.EO:
							cDark = listDefs[3].Color;
							cLight = listDefs[8].Color;
							break;
						case ProcStat.R:
							cDark = listDefs[4].Color;
							cLight = listDefs[9].Color;
							break;
						case ProcStat.Cn:
							cDark = listDefs[16].Color;
							cLight = listDefs[17].Color;
							break;
						case ProcStat.D://Can happen with invalidated locked procs.
						default:
							continue;//Don't draw.
					}
				}
				else
				{
					cDark = ProcedureCodes.GetProcCode(proc.CodeNum).GraphicColor;
					cLight = ProcedureCodes.GetProcCode(proc.CodeNum).GraphicColor;
				}
				switch (ProcedureCodes.GetProcCode(proc.CodeNum).PaintType)
				{
					case ToothPaintingType.BridgeDark:
						if (ToothInitials.ToothIsMissingOrHidden(toothInitialList, proc.ToothNum))
						{
							toothChartRelay.SetPontic(proc.ToothNum, cDark);
						}
						else
						{
							toothChartRelay.SetCrown(proc.ToothNum, cDark);
						}
						break;
					case ToothPaintingType.BridgeLight:
						if (ToothInitials.ToothIsMissingOrHidden(toothInitialList, proc.ToothNum))
						{
							toothChartRelay.SetPontic(proc.ToothNum, cLight);
						}
						else
						{
							toothChartRelay.SetCrown(proc.ToothNum, cLight);
						}
						break;
					case ToothPaintingType.CrownDark:
						toothChartRelay.SetCrown(proc.ToothNum, cDark);
						break;
					case ToothPaintingType.CrownLight:
						toothChartRelay.SetCrown(proc.ToothNum, cLight);
						break;
					case ToothPaintingType.DentureDark:
						if (proc.Surf == "U")
						{
							teeth = new string[14];
							for (int t = 0; t < 14; t++)
							{
								teeth[t] = (t + 2).ToString();
							}
						}
						else if (proc.Surf == "L")
						{
							teeth = new string[14];
							for (int t = 0; t < 14; t++)
							{
								teeth[t] = (t + 18).ToString();
							}
						}
						else
						{
							teeth = proc.ToothRange.Split(new char[] { ',' });
						}
						for (int t = 0; t < teeth.Length; t++)
						{
							if (ToothInitials.ToothIsMissingOrHidden(toothInitialList, teeth[t]))
							{
								toothChartRelay.SetPontic(teeth[t], cDark);
							}
							else
							{
								toothChartRelay.SetCrown(teeth[t], cDark);
							}
						}
						break;
					case ToothPaintingType.DentureLight:
						if (proc.Surf == "U")
						{
							teeth = new string[14];
							for (int t = 0; t < 14; t++)
							{
								teeth[t] = (t + 2).ToString();
							}
						}
						else if (proc.Surf == "L")
						{
							teeth = new string[14];
							for (int t = 0; t < 14; t++)
							{
								teeth[t] = (t + 18).ToString();
							}
						}
						else
						{
							teeth = proc.ToothRange.Split(new char[] { ',' });
						}
						for (int t = 0; t < teeth.Length; t++)
						{
							if (ToothInitials.ToothIsMissingOrHidden(toothInitialList, teeth[t]))
							{
								toothChartRelay.SetPontic(teeth[t], cLight);
							}
							else
							{
								toothChartRelay.SetCrown(teeth[t], cLight);
							}
						}
						break;
					case ToothPaintingType.Extraction:
						toothChartRelay.SetBigX(proc.ToothNum, cDark);
						break;
					case ToothPaintingType.FillingDark:
						toothChartRelay.SetSurfaceColors(proc.ToothNum, proc.Surf, cDark);
						break;
					case ToothPaintingType.FillingLight:
						toothChartRelay.SetSurfaceColors(proc.ToothNum, proc.Surf, cLight);
						break;
					case ToothPaintingType.Implant:
						toothChartRelay.SetImplant(proc.ToothNum, cDark);
						break;
					case ToothPaintingType.PostBU:
						toothChartRelay.SetBU(proc.ToothNum, cDark);
						break;
					case ToothPaintingType.RCT:
						toothChartRelay.SetRCT(proc.ToothNum, cDark);
						break;
					case ToothPaintingType.Sealant:
						toothChartRelay.SetSealant(proc.ToothNum, cDark);
						break;
					case ToothPaintingType.Veneer:
						toothChartRelay.SetVeneer(proc.ToothNum, cLight);
						break;
					case ToothPaintingType.Watch:
						toothChartRelay.SetWatch(proc.ToothNum, cDark);
						break;
				}
			}
		}

		///<summary>Generates an Image of the ToothChart.  Set isForSheet=true for dimensions and colors appropriate for Sheets, false for dimensions and colors appropriate for display in Patient Dashboard (matches dimensions for .</summary>
		public static Image GetToothChartHelper(long patNum, bool showCompleted, TreatPlan treatPlan = null, List<Procedure> listProceduresFilteredOverride = null
			, bool isInPatientDashboard = false)
		{
			//This method is designed to stay in the OD.exe.
			int colorBackgroundIndex = 14;
			int colorTextIndex = 15;
			int width = 500;
			int height = 370;
			if (isInPatientDashboard)
			{
				colorBackgroundIndex = 10;
				colorTextIndex = 11;
				width = 410;
				height = 307;
			}
			Form formOldBitmap = null;
			SparksToothChart.ToothChartWrapper toothChartWrapper = new SparksToothChart.ToothChartWrapper();
			ToothChartRelay toothChartRelay = new ToothChartRelay(false);
			toothChartRelay.SetToothChartWrapper(toothChartWrapper);
			Control toothChart = null;//the Sparks3D tooth chart

				toothChartWrapper.Size = new Size(width, height);
				toothChartWrapper.UseHardware = ComputerPrefs.LocalComputer.GraphicsUseHardware;
				toothChartWrapper.PreferredPixelFormatNumber = ComputerPrefs.LocalComputer.PreferredPixelFormatNum;
				toothChartWrapper.DeviceFormat = new SparksToothChart.ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);
				toothChartWrapper.DrawMode = ComputerPrefs.LocalComputer.GraphicsSimple;
				ComputerPrefs.LocalComputer.PreferredPixelFormatNum = toothChartWrapper.PreferredPixelFormatNumber;
				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
				formOldBitmap = new Form();
				formOldBitmap.FormBorderStyle = FormBorderStyle.None;
				formOldBitmap.Size = new Size(width, height);
				formOldBitmap.Controls.Add(toothChartWrapper);
				//formOldBitmap.Show();
			
			toothChartRelay.SetToothNumberingNomenclature((ToothNumberingNomenclature)PrefC.GetInt(PrefName.UseInternationalToothNumbers));
			List<Definition> listDefs = Definitions.GetDefsForCategory(DefinitionCategory.ChartGraphicColors);
			toothChartRelay.ColorBackgroundMain = listDefs[colorBackgroundIndex].Color;
			toothChartRelay.ColorText = listDefs[colorTextIndex].Color;
			toothChartRelay.ResetTeeth();
			List<ToothInitial> toothInitialList = patNum == 0 ? new List<ToothInitial>() : ToothInitials.Refresh(patNum);
			//first, primary.  That way, you can still set a primary tooth missing afterwards.
			for (int i = 0; i < toothInitialList.Count; i++)
			{
				if (toothInitialList[i].InitialType == ToothInitialType.Primary)
				{
					toothChartRelay.SetPrimary(toothInitialList[i].ToothNum);
				}
			}
			for (int i = 0; i < toothInitialList.Count; i++)
			{
				switch (toothInitialList[i].InitialType)
				{
					case ToothInitialType.Missing:
						toothChartRelay.SetMissing(toothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Hidden:
						toothChartRelay.SetHidden(toothInitialList[i].ToothNum);
						break;
					case ToothInitialType.Rotate:
						toothChartRelay.MoveTooth(toothInitialList[i].ToothNum, toothInitialList[i].Movement, 0, 0, 0, 0, 0);
						break;
					case ToothInitialType.TipM:
						toothChartRelay.MoveTooth(toothInitialList[i].ToothNum, 0, toothInitialList[i].Movement, 0, 0, 0, 0);
						break;
					case ToothInitialType.TipB:
						toothChartRelay.MoveTooth(toothInitialList[i].ToothNum, 0, 0, toothInitialList[i].Movement, 0, 0, 0);
						break;
					case ToothInitialType.ShiftM:
						toothChartRelay.MoveTooth(toothInitialList[i].ToothNum, 0, 0, 0, toothInitialList[i].Movement, 0, 0);
						break;
					case ToothInitialType.ShiftO:
						toothChartRelay.MoveTooth(toothInitialList[i].ToothNum, 0, 0, 0, 0, toothInitialList[i].Movement, 0);
						break;
					case ToothInitialType.ShiftB:
						toothChartRelay.MoveTooth(toothInitialList[i].ToothNum, 0, 0, 0, 0, 0, toothInitialList[i].Movement);
						break;
					case ToothInitialType.Drawing:
						toothChartRelay.AddDrawingSegment(toothInitialList[i].Copy());
						break;
				}
			}
			//We passed in a list of procs we want to see on the toothchart.  Use that.
			List<Procedure> listProceduresFiltered = new List<Procedure>();
			if (listProceduresFilteredOverride != null)
			{
				listProceduresFiltered = listProceduresFilteredOverride;
			}
			else if (patNum > 0)
			{//Custom list of procedures was not passed in, go get all for the patient like we always have.
				List<Procedure> listProceduresAll = Procedures.Refresh(patNum);
				listProceduresFiltered = OpenDentBusiness.SheetPrinting.FilterProceduresForToothChart(listProceduresAll, treatPlan, showCompleted);
			}
			listProceduresFiltered.Sort(OpenDentBusiness.SheetPrinting.CompareProcListFiltered);
			//Draw tooth chart
			DrawProcsGraphics(listProceduresFiltered, toothChartRelay, toothInitialList);

				toothChartWrapper.AutoFinish = true;
			
			toothChartRelay.EndUpdate();

				Image retVal = toothChartRelay.GetBitmap();
				formOldBitmap.Close();//automatically disposes, too
				return retVal;
			
		}

		/*
		///<summary>If drawing grids for a statement, use the overload that takes a DataSet otherwise this method will make another call to the db.</summary>
		public static void DrawFieldGrid(SheetField field,Sheet sheet,Graphics g,XGraphics gx,Statement stmt=null,MedLab medLab=null) {
			DataSet dataSet=null;
			if(sheet.SheetType==SheetTypeEnum.Statement && stmt!=null) {
				//This should never get hit.  This line of code is here just in case I forgot to update a random spot in our code.
				//Worst case scenario we will end up calling the database a few extra times for the same data set.
				//It use to call this method many, many times so anything is an improvement at this point.
				dataSet=AccountModules.GetAccount(stmt.PatNum,stmt,doShowHiddenPaySplits:stmt.IsReceipt);
			}
		}*/
		#endregion Methods - Drawing

		#region Methods - CreatePdf
		///<summary>Creates a PDF in memory and returns it, does not save to disk.</summary>
		public static PdfDocument CreatePdf(Sheet sheet)
		{
			SheetDrawingJob sheetDrawingJob = new SheetDrawingJob();
			return sheetDrawingJob.CreatePdf(sheet);
		}

		///<summary>If making a statement, use the overload that takes a DataSet otherwise this method will make another call to the db.  If fullFileName is in the A to Z folder, pass in fullFileName as a temp file and then upload that file to the cloud.</summary>
		public static void CreatePdf(Sheet sheet, string fullFileName, Statement stmt, MedLab medLab = null)
		{
			SheetDrawingJob sheetDrawingJob = new SheetDrawingJob();
			sheetDrawingJob.CreatePdf(sheet, fullFileName, stmt, medLab);
		}

		///<summary>Creates a file where fullFileName is a local path. If fullFileName is in the A to Z folder, pass in fullFileName as a temp file and then upload that file to the cloud.</summary>
		public static PdfDocument CreatePdf(Sheet sheet, string fullFileName, Statement stmt, DataSet dataSet, MedLab medLab, Patient pat = null, Patient patGuar = null, bool doSave = true)
		{
			SheetDrawingJob sheetDrawingJob = new SheetDrawingJob();
			return sheetDrawingJob.CreatePdf(sheet, fullFileName, stmt, dataSet, medLab, pat, patGuar, doSave);
		}

		///<summary>Called for every page that is generated for a PDF document. Pages and yPos must be tracked outside of this function. See also pd_PrintPage.  DataSet should be prefilled with AccountModules.GetAccount() before calling this method if making a statement.</Summary>
		public static int CreatePdfPage(Sheet sheet, PdfPage page, DataSet dataSet, Patient pat, Patient patGuar, int yPos)
		{
			SheetDrawingJob sheetDrawingJob = new SheetDrawingJob();
			return sheetDrawingJob.CreatePdfPage(sheet, page, dataSet, pat, patGuar, yPos);
		}
		#endregion Methods - CreatePdf
	}
}
