﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Imedisoft.Data;
using Imedisoft.Data.Models;
using OpenDentBusiness;

namespace OpenDental {
	///<summary>Only used for editing smoking documentation.</summary>
	public partial class FormEhrMeasureEventEdit:ODForm {
		private EhrMeasureEvent _measureEventCur;
		public string MeasureDescript;

		public FormEhrMeasureEventEdit(EhrMeasureEvent measureEventCur) {
			InitializeComponent();
			_measureEventCur=measureEventCur;
		}

		private void FormEhrMeasureEventEdit_Load(object sender,EventArgs e) {
			textDateTime.Text=_measureEventCur.Date.ToString();
			Patient patCur=Patients.GetPat(_measureEventCur.PatientId??0);
			if(patCur!=null) {
				textPatient.Text=patCur.GetNameFL();
			}
			if(!String.IsNullOrWhiteSpace(MeasureDescript)) {
				labelMoreInfo.Text=MeasureDescript;
			}
			if(_measureEventCur.Type==EhrMeasureEventType.TobaccoUseAssessed) {
				Loinc lCur=Loincs.GetByCode(_measureEventCur.EventCode);//TobaccoUseAssessed events can be one of three types, all LOINC codes
				if(lCur!=null) {
					textType.Text=lCur.LongCommonName;//Example: History of tobacco use Narrative
				}
				Snomed sCur=Snomeds.GetByCode(_measureEventCur.ResultCode);//TobaccoUseAssessed results can be any SNOMEDCT code, we recommend one of 8 codes, but the CQM measure allows 54 codes and we let the user select any SNOMEDCT they want
				if(sCur!=null) {
					textResult.Text=sCur.Description;//Examples: Non-smoker (finding) or Smoker (finding)
				}
				//only visible if event is a tobacco use assessment
				textTobaccoDesireToQuit.Visible=true;
				textTobaccoDuration.Visible=true;
				textTobaccoStartDate.Visible=true;
				labelTobaccoDesireToQuit.Visible=true;
				labelTobaccoDesireScale.Visible=true;
				labelTobaccoStartDate.Visible=true;
				textTobaccoDesireToQuit.Text=_measureEventCur.TobaccoCessationDesire.ToString();
				if(_measureEventCur.TobaccoStartDate.HasValue) {
					textTobaccoStartDate.Text=_measureEventCur.TobaccoStartDate.Value.ToShortDateString();
				}
				CalcTobaccoDuration();
			}
			else {
				//Currently, the TobaccoUseAssessed events are the only ones that can be deleted.
				butDelete.Enabled=false;
			}
			if(textType.Text==""){//if not set by LOINC name above, then either not a TobaccoUseAssessed event or the code was not in the LOINC table, fill with EventType
				textType.Text=_measureEventCur.Type.ToString();
			}
			textMoreInfo.Text=_measureEventCur.MoreInfo;
		}

		public void CalcTobaccoDuration() {
			textTobaccoDuration.Text="";
			if(textTobaccoStartDate.errorProvider1.GetError(textTobaccoStartDate)!="") {
				return;
			}
			DateTime startDate=PIn.Date(textTobaccoStartDate.Text);
			if(startDate>DateTime.Today || startDate.Year<1880) {
				return;
			}
			int years=DateTime.Now.Year-startDate.Year;
			int months=0;
			if(startDate.Month<DateTime.Now.Month) {//startdate anniversary was in a previous
				months=DateTime.Now.Month-startDate.Month;
				if(DateTime.Now.Day>startDate.Day) {//start month is before current month, and start day is before current day, don't count current month
					months--;
				}
			}
			else if(startDate.Month==DateTime.Now.Month) {//startdate anniversary this month
				if(startDate.Day>DateTime.Now.Day) {//start date anniversary hasn't happened this month, subtract a year and months=11
					years--;
					months=11;
				}
			}
			else {//startdate anniversary later in the year
				years--;
				months=12-(startDate.Month-DateTime.Now.Month);
				if(startDate.Day>DateTime.Now.Day) {
					months--;//haven't reached the day of the month of the startdate, don't count the current month yet
				}
			}
			textTobaccoDuration.Text=years.ToString()+"y "+months.ToString()+"m";
		}

		private void textTobaccoStartDate_Validated(object sender,EventArgs e) {
			CalcTobaccoDuration();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(MsgBoxButtons.OKCancel,"Delete?")) {
				return;
			}
			string logEntry="Ehr Measure Event was deleted."+"  "
				+"Date"+": "+PIn.Date(textDateTime.Text)+"  "
				+"Type"+": "+_measureEventCur.Type.ToString()+"  "
				+"Patient"+": "+textPatient.Text;
			SecurityLogs.MakeLogEntry(Permissions.EhrMeasureEventEdit,_measureEventCur.PatientId,logEntry);
			EhrMeasureEvents.Delete(_measureEventCur.Id);
			DialogResult=DialogResult.Cancel;
		}

		private void butOK_Click(object sender,EventArgs e) {
			//inserts never happen here.  Only updates.
			DateTime dateTEvent=PIn.Date(textDateTime.Text);
			if(dateTEvent==DateTime.MinValue) {
				MessageBox.Show("Please enter a valid date time.");//because this must always be valid
				return;
			}
			if(textTobaccoStartDate.Visible //only visible for tobacco use assessments
				&& textTobaccoStartDate.errorProvider1.GetError(textTobaccoStartDate)!="")
			{
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
			if(textTobaccoDesireToQuit.Visible //only visible for tobacco use assessments
				&& textTobaccoDesireToQuit.errorProvider1.GetError(textTobaccoDesireToQuit)!="")
			{
				MessageBox.Show("Please fix data entry errors first.");
				return;
			}
			List<string> listLogEdits=new List<string>();
			if(_measureEventCur.MoreInfo!=textMoreInfo.Text) {
				listLogEdits.Add("More Info was changed.");
				_measureEventCur.MoreInfo=textMoreInfo.Text;
			}
			if(_measureEventCur.Date!=dateTEvent) {
				listLogEdits.Add("Date was changed from"+": "+_measureEventCur.Date.ToString()+" "+"to"+": "+dateTEvent.ToString()+".");
				_measureEventCur.Date=dateTEvent;
			}
			if(textTobaccoStartDate.Visible && textTobaccoDesireToQuit.Visible) {
				_measureEventCur.TobaccoStartDate=PIn.Date(textTobaccoStartDate.Text);
				_measureEventCur.TobaccoCessationDesire=PIn.Byte(textTobaccoDesireToQuit.Text);
			}
			if(listLogEdits.Count>0) {
				listLogEdits.Insert(0,"EHR Measure Event was edited.");
				SecurityLogs.MakeLogEntry(Permissions.EhrMeasureEventEdit,_measureEventCur.PatientId,string.Join("  ",listLogEdits));
			}
			EhrMeasureEvents.Save(_measureEventCur);
			
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
