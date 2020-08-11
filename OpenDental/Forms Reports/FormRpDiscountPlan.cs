using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.ReportingComplex;
using System.Data;
using CodeBase;

namespace OpenDental {
	public partial class FormRpDiscountPlan:ODForm {

		public FormRpDiscountPlan() {
			InitializeComponent();
			Lan.F(this);
		}

		private void butOK_Click(object sender,EventArgs e) {
			ReportComplex report=new ReportComplex(true,false);
			DataTable table=RpDiscountPlan.GetTable(textDescription.Text);
			Font fontMain=new Font("Tahoma",8);
			Font fontTitle=new Font("Tahoma",15,FontStyle.Bold);
			Font fontSubTitle=new Font("Tahoma",10,FontStyle.Bold);
			report.ReportName=Lan.G(this,"Discount Plan List");
			report.AddTitle("Title",Lan.G(this,"Discount Plan List"),fontTitle);
			report.AddSubTitle("Practice Title",Prefs.GetString(PrefName.PracticeTitle),fontSubTitle);
			QueryObject query=report.AddQuery(table,Lan.G(this,"Date")+": "+DateTimeOD.Today.ToString("d"));
			query.AddColumn("Description",230,font:fontMain);
			query.AddColumn("FeeSched",175,font:fontMain);
			query.AddColumn("AdjType",175,font:fontMain);
			query.AddColumn("Patient",165,font:fontMain);
			report.AddPageNum(fontMain);
			if(!report.SubmitQueries()) {
				return;
			}
			report.AddFooterText("Total","Total: "+report.TotalRows.ToString(),fontMain,10,ContentAlignment.MiddleRight);
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}