using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenDentBusiness
{
    /// <summary>
    /// Employers are refreshed as needed. 
    /// A full refresh is frequently triggered if an employerNum cannot be found in the HList. 
    /// Important retrieval is done directly from the db.
    /// </summary>
    public class Employers
	{
        private class EmployerCache : DictionaryCache<long, Employer>
        {
            protected override long GetKey(Employer item)
            {
				return item.Id;
            }

            protected override IEnumerable<Employer> Load()
            {
                return Crud.EmployerCrud.SelectMany("SELECT * FROM employer");
			}
        }

		private readonly static EmployerCache cache = new EmployerCache();

		public static Employer GetOne(long employerId) 
			=> cache.Find(employerId);

		public static List<Employer> GetAll() 
			=> cache.GetAll();

		public static void RefreshCache() 
			=> cache.Refresh();

		public static void Update(Employer empCur, Employer empOld)
		{
			Crud.EmployerCrud.Update(empCur, empOld);

			InsEditLogs.MakeLogEntry(empCur, empOld, InsEditLogType.Employer, Security.CurrentUser.Id);
		}

		public static long Insert(Employer Cur)
		{
			InsEditLogs.MakeLogEntry(Cur, null, InsEditLogType.Employer, Security.CurrentUser.Id);

			return Crud.EmployerCrud.Insert(Cur);
		}

		public static void Save(Employer employer)
        {
			if (employer.Id == 0)
            {
				InsEditLogs.MakeLogEntry(employer, null, InsEditLogType.Employer, Security.CurrentUser.Id);

				employer.Id = Crud.EmployerCrud.Insert(employer);
			}
            else
            {
				var employerOld = Crud.EmployerCrud.SelectOne(employer.Id);

				Crud.EmployerCrud.Update(employer, employerOld);

				InsEditLogs.MakeLogEntry(employer, employerOld, InsEditLogType.Employer, Security.CurrentUser.Id);
			}
        }

		/// <summary>
		/// There MUST not be any dependencies before calling this or there will be invalid foreign keys.  
		/// This is only called from FormEmployers after proper validation.
		/// </summary>
		public static void Delete(Employer employer)
		{
			Database.ExecuteNonQuery("DELETE from employer WHERE EmployerNum = " + employer.Id);

			InsEditLogs.MakeLogEntry(null, employer, InsEditLogType.Employer, Security.CurrentUser.Id);
		}

		/// <summary>
		/// Returns a list of patients that are dependent on the Cur employer.
		/// The list includes carriage returns for easy display. 
		/// Used before deleting an employer to make sure employer is not in use.
		/// </summary>
		public static string DependentPatients(Employer employer)
		{
			DataTable table = Database.ExecuteDataTable("SELECT CONCAT(CONCAT(LName,', '),FName) FROM patient WHERE EmployerNum = " + employer.Id);

			string retStr = "";
			for (int i = 0; i < table.Rows.Count; i++)
			{
				if (i > 0)
				{
					retStr += "\r\n";
				}

				retStr += PIn.String(table.Rows[i][0].ToString());
			}

			return retStr;
		}

		/// <summary>
		/// Returns a list of insplans that are dependent on the Cur employer. 
		/// The list includes carriage returns for easy display. 
		/// Used before deleting an employer to make sure employer is not in use.
		/// </summary>
		public static string DependentInsPlans(Employer Cur)
		{
			string command = "SELECT carrier.CarrierName,CONCAT(CONCAT(patient.LName,', '),patient.FName) "
				+ "FROM insplan "
				+ "LEFT JOIN inssub ON insplan.PlanNum=inssub.PlanNum "
				+ "LEFT JOIN patient ON inssub.Subscriber=patient.PatNum "
				+ "LEFT JOIN carrier ON insplan.CarrierNum=carrier.CarrierNum "
				+ "WHERE insplan.EmployerNum = " + Cur.Id;

			DataTable table = Database.ExecuteDataTable(command);
			string retStr = "";
			for (int i = 0; i < table.Rows.Count; i++)
			{
				if (i > 0)
				{
					retStr += "\r\n";//return, newline for multiple names.
				}
				retStr += PIn.String(table.Rows[i][1].ToString()) + ": " + PIn.String(table.Rows[i][0].ToString());
			}
			return retStr;
		}

		/// <summary>
		/// Gets the name of an employer based on the employerNum.
		/// This also refreshes the list if necessary, so it will work even if the list has not been refreshed recently.
		/// </summary>
		public static string GetName(long employerId) => GetEmployer(employerId).Name ?? "";

		/// <summary>
		/// Gets an employer based on the employerNum. 
		/// This will work even if the list has not been refreshed recently, but if you are going to need a lot of names all at once, then it is faster to refresh first.
		/// </summary>
		public static Employer GetEmployer(long employerId)
		{
			if (employerId == 0)
			{
				return new Employer();
			}

			Employer emp = null;

			try
			{
				emp = GetOne(employerId);

				if (emp == null)
				{
					RefreshCache();

					emp = GetOne(employerId);
				}
			}
            catch { }

			if (emp == null)
			{
				return new Employer(); // Could only happen if corrupted or we're looking up an employer that no longer exists.
			}

			return emp;
		}

		public static Employer GetEmployerNoCache(long employerId)
		{
			if (employerId == 0)
			{
				return null;
			}

			return Crud.EmployerCrud.SelectOne(employerId);
		}

		/// <summary>
		/// Gets an employerNum from the database based on the supplied name.
		/// If that empName does not exist, then a new employer is created, and the employerNum for the new employer is returned.
		/// </summary>
		public static long GetEmployerNum(string employerName)
		{
			if (string.IsNullOrEmpty(employerName))
			{
				return 0;
			}

			var employerId = Database.ExecuteLong(
				"SELECT EmployerNum FROM employer WHERE EmpName = @name",
					new MySqlParameter("name", employerName));

			if (employerId > 0) return employerId;

            var employer = new Employer
            {
                Name = employerName
            };

            Insert(employer);

			return employer.Id;
		}

		/// <summary>
		/// Returns an employer if an exact match is found for the text supplied in the database.
		/// Returns null if nothing found.
		/// </summary>
		public static Employer GetByName(string employerName) 
			=> Crud.EmployerCrud.SelectOne(
				"SELECT * FROM employer WHERE EmpName = '" + POut.String(employerName) + "'");

		/// <summary>
		/// Returns an arraylist of Employers with names similar to the supplied string.
		/// Used in dropdown list from employer field for faster entry.
		/// There is a small chance that the list will not be completely refreshed when this is run, but it won't really matter if one employer doesn't show in dropdown.
		/// </summary>
		public static List<Employer> GetSimilarNames(string employerName) 
			=> cache.Find(employer 
				=> employer.Name.StartsWith(employerName, StringComparison.CurrentCultureIgnoreCase));

		/// <summary>
		/// Combines all the given employers into one. 
		/// Updates patient and insplan. 
		/// Then deletes all the others.
		/// </summary>
		public static void Combine(List<long> employerIds)
		{
			if (employerIds.Count < 2) return;

			long newEmployerId = employerIds[0];

			for (int i = 1; i < employerIds.Count; i++)
			{
				Database.ExecuteNonQuery(
					"UPDATE patient SET EmployerNum = " + newEmployerId + " WHERE EmployerNum = " + employerIds[i]);

				Database.ExecuteNonQuery(
					"UPDATE insplan SET EmployerNum = " + newEmployerId + " WHERE EmployerNum = " + employerIds[i]);

				var insurancePlans = Crud.InsPlanCrud.SelectMany("SELECT * FROM insplan WHERE EmployerNum = " + employerIds[i]);
				foreach (var insurancePlan in insurancePlans)
                {
					InsEditLogs.MakeLogEntry(
						"EmployerNum", 
						Security.CurrentUser.Id, 
						employerIds[i].ToString(), 
						newEmployerId.ToString(),
						InsEditLogType.InsPlan, insurancePlan.Id, 0, insurancePlan.GroupNumber + " - " + insurancePlan.GroupName);
				}

				Delete(GetEmployer(employerIds[i]));
			}
		}
	}
}
