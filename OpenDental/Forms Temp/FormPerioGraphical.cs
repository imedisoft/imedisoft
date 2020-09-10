using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using SparksToothChart;
using Imedisoft.Data.Models;
using Imedisoft.Data;

namespace OpenDental {
	public partial class FormPerioGraphical:ODForm {
		///<summary>The current perio exam being loaded.</summary>
		private PerioExam _perioExamCur;
		///<summary>The current patient for the loaded perio exam</summary>
		private Patient _patCur;
		//public List<PerioMeasure> ListPerioMeasures; 
		private ToothChartRelay _toothChartRelay;

		public FormPerioGraphical(PerioExam perioExam,Patient patient) {
			_perioExamCur=perioExam;
			_patCur=patient;
			InitializeComponent();
		}

		private void FormPerioGraphic_Load(object sender,EventArgs e) {
			_toothChartRelay= new ToothChartRelay();
			_toothChartRelay.SetToothChartWrapper(toothChartWrapper);
				toothChartWrapper.DeviceFormat=new ToothChartDirectX.DirectXDeviceFormat(ComputerPrefs.LocalComputer.DirectXFormat);//Must be set before draw mode
				toothChartWrapper.DrawMode=DrawingMode.DirectX;
			
			_toothChartRelay.BeginUpdate();
			_toothChartRelay.SetToothNumberingNomenclature((ToothNumberingNomenclature)PrefC.GetInt(PreferenceName.UseInternationalToothNumbers));
			_toothChartRelay.ColorBackgroundMain=Color.White;
			_toothChartRelay.ColorText=Color.Black;
			List<Definition> listDefs=Definitions.GetDefsForCategory(DefinitionCategory.MiscColors,true);
			_toothChartRelay.SetPerioColors(
				listDefs[1].Color,//bleeding
				listDefs[2].Color,//suppuration
				PrefC.GetColor(PreferenceName.PerioColorProbing),
				PrefC.GetColor(PreferenceName.PerioColorProbingRed),
				PrefC.GetColor(PreferenceName.PerioColorGM),
				PrefC.GetColor(PreferenceName.PerioColorCAL),
				PrefC.GetColor(PreferenceName.PerioColorMGJ),
				PrefC.GetColor(PreferenceName.PerioColorFurcations),
				PrefC.GetColor(PreferenceName.PerioColorFurcationsRed),
				PrefC.GetInt(PreferenceName.PerioRedProb),
				PrefC.GetInt(PreferenceName.PerioRedFurc)
			);
			_toothChartRelay.ResetTeeth();
			try {
				_toothChartRelay.IsPerioMode=true;
			}
			catch(Exception ex) {//catch is just for old ToothChartWrapper
				MessageBox.Show(ex.Message);
				DialogResult=DialogResult.Abort;
				Close();
				return;
			}
			List<PerioMeasure> listMeas=PerioMeasures.GetAllForExam(_perioExamCur.PerioExamNum);
			#region CAL old

				//compute CAL's for each site.  If a CAL is valid, pass it in.
				PerioMeasure measureProbe;
				PerioMeasure measureGM;
				int gm;
				int pd;
				int calMB;
				int calB;
				int calDB;
				int calML;
				int calL;
				int calDL;
				for(int t=1;t<=32;t++) {
					measureProbe=null;
					measureGM=null;
					for(int i=0;i<listMeas.Count;i++) {
						if(listMeas[i].IntTooth!=t) {
							continue;
						}
						if(listMeas[i].SequenceType==PerioSequenceType.Probing) {
							measureProbe=listMeas[i];
						}
						if(listMeas[i].SequenceType==PerioSequenceType.GingMargin) {
							measureGM=listMeas[i];
						}
					}
					if(measureProbe==null||measureGM==null) {
						continue;//to the next tooth
					}
					//mb
					calMB=-1;
					gm=measureGM.MBvalue;//MBvalue must stay over 100 for hyperplasia, because that's how we're storing it in ToothChartData.ListPerioMeasure.
					if(gm>100) {//hyperplasia
						gm=100-gm;//e.g. 100-103=-3
					}
					pd=measureProbe.MBvalue;
					if(measureGM.MBvalue!=-1 && pd!=-1) {
						calMB=gm+pd;
						if(calMB<0) {
							calMB=0;//CALs can't be negative
						}
					}
					//B
					calB=-1;
					gm=measureGM.Bvalue;
					if(gm>100) {//hyperplasia
						gm=100-gm;//e.g. 100-103=-3 
					}
					pd=measureProbe.Bvalue;
					if(measureGM.Bvalue!=-1&&pd!=-1) {
						calB=gm+pd;
						if(calB<0) {
							calB=0;
						}
					}
					//DB
					calDB=-1;
					gm=measureGM.DBvalue;
					if(gm>100) {//hyperplasia
						gm=100-gm;//e.g. 100-103=-3 
					}
					pd=measureProbe.DBvalue;
					if(measureGM.DBvalue!=-1&&pd!=-1) {
						calDB=gm+pd;
						if(calDB<0) {
							calDB=0;
						}
					}
					//ML
					calML=-1;
					gm=measureGM.MLvalue;
					if(gm>100) {//hyperplasia
						gm=100-gm;//e.g. 100-103=-3 
					}
					pd=measureProbe.MLvalue;
					if(measureGM.MLvalue!=-1&&pd!=-1) {
						calML=gm+pd;
						if(calML<0) {
							calML=0;
						}
					}
					//L
					calL=-1;
					gm=measureGM.Lvalue;
					if(gm>100) {//hyperplasia
						gm=100-gm;//e.g. 100-103=-3 
					}
					pd=measureProbe.Lvalue;
					if(measureGM.Lvalue!=-1&&pd!=-1) {
						calL=gm+pd;
						if(calL<0) {
							calL=0;
						}
					}
					//DL
					calDL=-1;
					gm=measureGM.DLvalue;
					if(gm>100) {//hyperplasia
						gm=100-gm;//e.g. 100-103=-3 
					}
					pd=measureProbe.DLvalue;
					if(measureGM.DLvalue!=-1&&pd!=-1) {
						calDL=gm+pd;
						if(calDL<0) {
							calDL=0;
						}
					}
					if(calMB!=-1||calB!=-1||calDB!=-1||calML!=-1||calL!=-1||calDL!=-1){
						_toothChartRelay.AddPerioMeasure(t,PerioSequenceType.CAL,calMB,calB,calDB,calML,calL,calDL);
					}
				}
			
			#endregion CAL old
			for (int i=0;i<listMeas.Count;i++) {
				if(listMeas[i].SequenceType==PerioSequenceType.SkipTooth) {
					_toothChartRelay.SetMissing(listMeas[i].IntTooth.ToString());
				} 
				else if(listMeas[i].SequenceType==PerioSequenceType.Mobility) {
					int mob=listMeas[i].ToothValue;
					Color color=Color.Black;
					if(mob>=PrefC.GetInt(PreferenceName.PerioRedMob)) {
						color=Color.Red;
					}
					if(mob!=-1) {//-1 represents no measurement taken.
						_toothChartRelay.SetMobility(listMeas[i].IntTooth.ToString(),mob.ToString(),color);
					}
				} 
				else {
					_toothChartRelay.AddPerioMeasure(listMeas[i].IntTooth,listMeas[i].SequenceType,listMeas[i].MBvalue,listMeas[i].Bvalue,listMeas[i].DBvalue,
						listMeas[i].MLvalue,listMeas[i].Lvalue,listMeas[i].DLvalue);
				}
			}
			//if(ToothChartRelay.IsSparks3DPresent){
			List<Procedure> _listProcs=Procedures.Refresh(FormOpenDental.CurPatNum);
			for(int t=1;t<=32;t++){
				List<Procedure> listProcsForTooth=_listProcs.FindAll(x=>x.ToothNum==t.ToString() && x.ProcStatus.In(ProcStat.C,ProcStat.EC,ProcStat.EO));
				bool isImplant=false;
				bool isCrown=false;
				for(int p=0;p<listProcsForTooth.Count;p++) {
					ProcedureCode procedureCode=ProcedureCodes.GetProcCode(listProcsForTooth[p].CodeNum);
					if(procedureCode.PaintType==ToothPaintingType.Implant) {
						isImplant=true;
					}
					if(procedureCode.PaintType==ToothPaintingType.CrownDark || procedureCode.PaintType==ToothPaintingType.CrownLight) {
						isCrown=true;
					}
				}
				if(isImplant){
					_toothChartRelay.SetMissing(t.ToString());
					_toothChartRelay.SetImplant(t.ToString(),Color.Gainsboro);
					if(isCrown){
						_toothChartRelay.SetCrown(t.ToString(),Color.WhiteSmoke);
					}
				}
			}
			_toothChartRelay.EndUpdate();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			PrinterL.TryPrint(pd2_PrintPage,
				"Graphical perio chart printed",
				_patCur.PatNum,
				PrintSituation.TPPerio,
				new Margins(0,0,0,0),
				PrintoutOrigin.AtMargin
			);
		}

		private void pd2_PrintPage(object sender,PrintPageEventArgs ev) {//raised for each page to be printed.
			Graphics g=ev.Graphics;
			RenderPerioPrintout(g,_patCur,ev.MarginBounds);//,new Rectangle(0,50,ev.MarginBounds.Width,ev.MarginBounds.Height));
		}

		public void RenderPerioPrintout(Graphics g,Patient pat,Rectangle marginBounds) {
			string clinicName="";
			//This clinic name could be more accurate here in the future if we make perio exams clinic specific.
			//Perhaps if there were a perioexam.ClinicNum column.
			if(pat.ClinicNum!=0) {
				Clinic clinic=Clinics.GetById(pat.ClinicNum);
				clinicName=clinic.Description;
			} 
			else {
				clinicName=Preferences.GetString(PreferenceName.PracticeTitle);
			}
			float y=70;
			SizeF sizeFPage=new SizeF(marginBounds.Width,marginBounds.Height);
			SizeF sizeStr;
			Font font=new Font("Arial",15);
			string titleStr="Periodontal Examination";
			sizeStr=g.MeasureString(titleStr,font);
			g.DrawString(titleStr,font,Brushes.Black,new PointF(sizeFPage.Width/2f-sizeStr.Width/2f,y));
			y+=sizeStr.Height;
			//Clinic Name
			font=new Font("Arial",11);
			sizeStr=g.MeasureString(clinicName,font);
			g.DrawString(clinicName,font,Brushes.Black,new PointF(sizeFPage.Width/2f-sizeStr.Width/2f,y));
			y+=sizeStr.Height;
			//PatientName
			string patNameStr=_patCur.GetNameFLFormal();
			sizeStr=g.MeasureString(patNameStr,font);
			g.DrawString(patNameStr,font,Brushes.Black,new PointF(sizeFPage.Width/2f-sizeStr.Width/2f,y));
			y+=sizeStr.Height;
			//We put the exam date instead of the current date because the exam date isn't anywhere else on the printout.
			string examDateStr=_perioExamCur.ExamDate.ToShortDateString();//Locale specific exam date.
			sizeStr=g.MeasureString(examDateStr,font);
			g.DrawString(examDateStr,font,Brushes.Black,new PointF(sizeFPage.Width/2f-sizeStr.Width/2f,y));
			y+=sizeStr.Height;
			Bitmap bitmapTC=_toothChartRelay.GetBitmap();
			g.DrawImage(bitmapTC,sizeFPage.Width/2f-bitmapTC.Width/2f,y,bitmapTC.Width,bitmapTC.Height);
		}

		private void butSetup_Click(object sender,EventArgs e) {
			FormPerioGraphicalSetup fpgs=new FormPerioGraphicalSetup();
			if(fpgs.ShowDialog()==DialogResult.OK){
				toothChartWrapper.ColorCAL=PrefC.GetColor(PreferenceName.PerioColorCAL);
				toothChartWrapper.ColorFurcations=PrefC.GetColor(PreferenceName.PerioColorFurcations);
				toothChartWrapper.ColorFurcationsRed=PrefC.GetColor(PreferenceName.PerioColorFurcationsRed);
				toothChartWrapper.ColorGingivalMargin=PrefC.GetColor(PreferenceName.PerioColorGM);
				toothChartWrapper.ColorMGJ=PrefC.GetColor(PreferenceName.PerioColorMGJ);	
				toothChartWrapper.ColorProbing=PrefC.GetColor(PreferenceName.PerioColorProbing);
				toothChartWrapper.ColorProbingRed=PrefC.GetColor(PreferenceName.PerioColorProbingRed);
				this.toothChartWrapper.Invalidate();
			}
		}

		private void butSave_Click(object sender,EventArgs e) {
			long defNumToothCharts=Definitions.GetImageCat(ImageCategorySpecial.T);
			if(defNumToothCharts==0) {
				MessageBox.Show("In Setup, Definitions, Image Categories, a category needs to be set for graphical tooth charts.");
				return;
			}
			Bitmap bitmap=null;
			Graphics g=null;
			Document doc=new Document();
			bitmap=new Bitmap(750,1000);
			g=Graphics.FromImage(bitmap);
			g.Clear(Color.White);
			g.CompositingQuality=System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			RenderPerioPrintout(g,_patCur,new Rectangle(0,0,bitmap.Width,bitmap.Height));
			try {
				ImageStore.Import(bitmap,defNumToothCharts,ImageType.Photo,_patCur);
			}
			catch(Exception ex) {
				MessageBox.Show("Unable to save file: " + ex.Message);
				bitmap.Dispose();
				bitmap=null;
				g.Dispose();
				return;
			}
			MessageBox.Show("Saved.");
			if(g!=null) {
				g.Dispose();
				g=null;
			}
			if(bitmap!=null) {
				bitmap.Dispose();
				bitmap=null;
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private void FormPerioGraphical_FormClosing(object sender,FormClosingEventArgs e) {
			//We need to clear out the tooth graphics of the local toothchart, since they are shallow copies of the tooth chart in the Chart module.
			//Otherwise, when the form disposes, the Chart module tooth graphics would also be disposed.
			toothChartWrapper.TcData.ListToothGraphics.Clear();
		}

	}
}
