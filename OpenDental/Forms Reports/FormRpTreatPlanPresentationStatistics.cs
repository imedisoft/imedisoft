using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDentBusiness;
using System.Linq;
using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Models;

namespace OpenDental
{
	public partial class FormRpTreatPlanPresentationStatistics : ODForm
	{
		private List<User> _listUsers;
		private List<Clinic> _listClinics;
		public FormRpTreatPlanPresentationStatistics()
		{
			InitializeComponent();

		}

		private void FormRpTreatPlanPresenter_Load(object sender, EventArgs e)
		{
			date1.SelectionStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
			date2.SelectionStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
			_listUsers = Users.GetAll(true);
			listUser.Items.AddRange(_listUsers.Select(x => x.UserName).ToArray());
			checkAllUsers.Checked = true;
			if (PrefC.HasClinicsEnabled)
			{
				if (!Security.CurrentUser.ClinicIsRestricted)
				{
					listClin.Items.Add("Unassigned");
				}
				_listClinics = Clinics.GetByUser(Security.CurrentUser);
				listClin.Items.AddRange(_listClinics.Select(x => x.Abbr).ToArray());
				checkAllClinics.Checked = true;
			}
			else
			{
				listClin.Visible = false;
				checkAllClinics.Visible = false;
				labelClin.Visible = false;
				groupGrossNet.Location = new Point(185, 225);
				groupOrder.Location = new Point(185, 295);
				groupUser.Location = new Point(185, 365);
				listUser.Width += 30;
			}
		}

		private void RunReport(List<long> listUserNums, List<long> listClinicsNums)
		{
			ReportComplex report = new ReportComplex(true, false);
			report.AddTitle("Title", "Presented Procedure Totals");
			report.AddSubTitle("PracTitle", Preferences.GetString(PreferenceName.PracticeTitle));
			report.AddSubTitle("Date", date1.SelectionStart.ToShortDateString() + " - " + date2.SelectionStart.ToShortDateString());
			List<User> listSelectedUsers = new List<User>();
			if (checkAllUsers.Checked)
			{
				report.AddSubTitle("Users", "All Users");
				listSelectedUsers.AddRange(_listUsers); //add all users
			}
			else
			{
				for (int i = 0; i < listUser.SelectedIndices.Count; i++)
				{
					listSelectedUsers.Add(_listUsers[listUser.SelectedIndices[i]]); //add selected users
				}
				report.AddSubTitle("Users", string.Join(",", listSelectedUsers.Select(x => x.UserName)));
			}
			List<Clinic> listSelectedClinics = new List<Clinic>();
			if (PrefC.HasClinicsEnabled)
			{
				if (checkAllClinics.Checked)
				{
					report.AddSubTitle("Clinics", "All Clinics");
					listSelectedClinics.Add(new Clinic()
					{
						Id = 0,
						Description = "Unassigned"
					});
					listSelectedClinics.AddRange(_listClinics); //add all clinics and the unassigned clinic.
				}
				else
				{
					for (int i = 0; i < listClin.SelectedIndices.Count; i++)
					{
						if (Security.CurrentUser.ClinicIsRestricted)
						{
							listSelectedClinics.Add(_listClinics[listClin.SelectedIndices[i]]);
						}
						else
						{
							if (listClin.SelectedIndices[i] == 0)
							{
								listSelectedClinics.Add(new Clinic()
								{
									Id = 0,
									Description = "Unassigned"
								});
							}
							else
							{
								listSelectedClinics.Add(_listClinics[listClin.SelectedIndices[i] - 1]);//Minus 1 from the selected index
							}
						}
					}
					report.AddSubTitle("Clinics", string.Join(",", listSelectedClinics.Select(x => x.Description)));
				}
			}
			List<long> clinicNums = listSelectedClinics.Select(y => y.Id).ToList();
			List<long> userNums = listSelectedUsers.Select(y => y.Id).ToList();
			DataTable table = RpTreatPlanPresentationStatistics.GetTreatPlanPresentationStatistics(date1.SelectionStart, date2.SelectionStart, radioFirstPresented.Checked
				, checkAllClinics.Checked, PrefC.HasClinicsEnabled, radioPresenter.Checked, radioGross.Checked, checkAllUsers.Checked, userNums, clinicNums);
			QueryObject query = report.AddQuery(table, "", "", SplitByKind.None, 1, true);
			query.AddColumn("Presenter", 100, FieldValueType.String);
			query.AddColumn("# of Plans", 85, FieldValueType.Integer);
			query.AddColumn("# of Procs", 85, FieldValueType.Integer);
			query.AddColumn("# of ProcsSched", 100, FieldValueType.Integer);
			query.AddColumn("# of ProcsComp", 100, FieldValueType.Integer);
			if (radioGross.Checked)
			{
				query.AddColumn("GrossTPAmt", 95, FieldValueType.Number);
				query.AddColumn("GrossSchedAmt", 95, FieldValueType.Number);
				query.AddColumn("GrossCompAmt", 95, FieldValueType.Number);
			}
			else
			{
				query.AddColumn("NetTPAmt", 95, FieldValueType.Number);
				query.AddColumn("NetSchedAmt", 95, FieldValueType.Number);
				query.AddColumn("NetCompAmt", 95, FieldValueType.Number);
			}
			if (!report.SubmitQueries())
			{
				DialogResult = DialogResult.Cancel;
				return;
			}
			FormReportComplex FormR = new FormReportComplex(report);
			FormR.ShowDialog();
			//DialogResult=DialogResult.OK;
		}

		private void checkAllUsers_Click(object sender, EventArgs e)
		{
			if (checkAllUsers.Checked)
			{
				listUser.SelectedIndices.Clear();
			}
		}

		private void listUser_Click(object sender, EventArgs e)
		{
			if (listUser.SelectedIndices.Count > 0)
			{
				checkAllUsers.Checked = false;
			}
		}

		private void checkAllClinics_Click(object sender, EventArgs e)
		{
			if (checkAllClinics.Checked)
			{
				listClin.SelectedIndices.Clear();
			}
		}

		private void listClin_Click(object sender, EventArgs e)
		{
			if (listClin.SelectedIndices.Count > 0)
			{
				checkAllClinics.Checked = false;
			}
		}

		private void butOK_Click(object sender, EventArgs e)
		{
			if (date2.SelectionStart < date1.SelectionStart)
			{
				MessageBox.Show("End date cannot be before start date.");
				return;
			}
			if (!checkAllUsers.Checked && listUser.SelectedIndices.Count == 0)
			{
				MessageBox.Show("Please select at least one user.");
				return;
			}
			if (PrefC.HasClinicsEnabled && !checkAllClinics.Checked && listClin.SelectedIndices.Count == 0)
			{
				MessageBox.Show("Please select at least one clinic.");
				return;
			}
			List<long> listUserNums = new List<long>();
			List<long> listClinicNums = new List<long>();
			if (checkAllUsers.Checked)
			{
				listUserNums = _listUsers.Select(x => x.Id).ToList();
			}
			else
			{
				listUserNums = listUser.SelectedIndices.OfType<int>().ToList().Select(x => _listUsers[x].Id).ToList();
			}
			if (PrefC.HasClinicsEnabled)
			{
				if (checkAllClinics.Checked)
				{
					listClinicNums = _listClinics.Select(x => x.Id).ToList();
				}
				else
				{
					for (int i = 0; i < listClin.SelectedIndices.Count; i++)
					{
						if (Security.CurrentUser.ClinicIsRestricted)
						{
							listClinicNums.Add(_listClinics[listClin.SelectedIndices[i]].Id);
						}
						else if (listClin.SelectedIndices[i] != 0)
						{
							listClinicNums.Add(_listClinics[listClin.SelectedIndices[i] - 1].Id);
						}
					}
				}
				if (!Security.CurrentUser.ClinicIsRestricted && (listClin.GetSelected(0) || checkAllClinics.Checked))
				{
					listClinicNums.Add(0);
				}
			}
			RunReport(listUserNums, listClinicNums);
		}

		private void butCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
