using CodeBase;
using Imedisoft.Data;
using OpenDental.ReportingComplex;
using OpenDentBusiness;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OpenDental
{
    public partial class FormRpDiscountPlan : ODForm
	{

		public FormRpDiscountPlan()
		{
			InitializeComponent();

		}

		private void butOK_Click(object sender, EventArgs e)
		{
			ReportComplex report = new ReportComplex(true, false);
			DataTable table = RpDiscountPlan.GetTable(textDescription.Text);
			Font fontMain = new Font("Tahoma", 8);
			Font fontTitle = new Font("Tahoma", 15, FontStyle.Bold);
			Font fontSubTitle = new Font("Tahoma", 10, FontStyle.Bold);
			report.ReportName = "Discount Plan List";
			report.AddTitle("Title", "Discount Plan List", fontTitle);
			report.AddSubTitle("Practice Title", Preferences.GetString(PreferenceName.PracticeTitle), fontSubTitle);
			QueryObject query = report.AddQuery(table, "Date" + ": " + DateTimeOD.Today.ToString("d"));
			query.AddColumn("Description", 230, font: fontMain);
			query.AddColumn("FeeSched", 175, font: fontMain);
			query.AddColumn("AdjType", 175, font: fontMain);
			query.AddColumn("Patient", 165, font: fontMain);
			report.AddPageNum(fontMain);
			if (!report.SubmitQueries())
			{
				return;
			}
			report.AddFooterText("Total", "Total: " + report.TotalRows.ToString(), fontMain, 10, ContentAlignment.MiddleRight);
			FormReportComplex FormR = new FormReportComplex(report);
			FormR.ShowDialog();
			DialogResult = DialogResult.OK;
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
