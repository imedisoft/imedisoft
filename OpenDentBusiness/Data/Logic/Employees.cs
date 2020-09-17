using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class Employees
	{
		///<summary>Updates the employee's ClockStatus if necessary based on their clock events. This method handles future clock events as having
		///already occurred. Ex: If I clock out for home at 6:00 but edit my time card to say 7:00, at 6:30 my status will say Home.</summary>
		public static void UpdateClockStatus(long employeeNum)
		{

			//Get the last clockevent for the employee. Will include clockevent with "in" before NOW, and "out" anytime before 23:59:59 of TODAY.
			string command = @"SELECT * FROM clockevent 
				WHERE TimeDisplayed2<=" + DbHelper.DateAddSecond(DbHelper.DateAddDay(DbHelper.Curdate(), "1"), "-1") + " AND TimeDisplayed1<=" + DbHelper.Now() + @"
				AND EmployeeNum=" + POut.Long(employeeNum) + @"
				ORDER BY IF(YEAR(TimeDisplayed2) < 1880,TimeDisplayed1,TimeDisplayed2) DESC";
			command = DbHelper.LimitOrderBy(command, 1);
			ClockEvent clockEvent = OpenDentBusiness.Crud.ClockEventCrud.SelectOne(command);
			Employee employee = GetEmp(employeeNum);

			if (clockEvent != null && clockEvent.TimeDisplayed2 > DateTime.Now)
			{//Future time manual clock out.
				employee.ClockStatus = "Manual Entry";
			}
			else if (clockEvent == null //Employee has never clocked in
				|| (clockEvent.TimeDisplayed2.Year > 1880 && clockEvent.ClockStatus == TimeClockStatus.Home))//Clocked out for home
			{
				employee.ClockStatus = TimeClockStatus.Home.ToString();
			}
			else if (clockEvent.TimeDisplayed2.Year > 1880 && clockEvent.ClockStatus == TimeClockStatus.Lunch)
			{//Clocked out for lunch
				employee.ClockStatus = TimeClockStatus.Lunch.ToString();
			}
			else if (clockEvent.TimeDisplayed1.Year > 1880 && clockEvent.TimeDisplayed2.Year < 1880 && clockEvent.ClockStatus == TimeClockStatus.Break)
			{
				employee.ClockStatus = TimeClockStatus.Break.ToString();
			}
			else if (clockEvent.TimeDisplayed2.Year > 1880 && clockEvent.ClockStatus == TimeClockStatus.Break)
			{//Clocked back in from break
				employee.ClockStatus = "Working";
			}
			else
			{//The employee has not clocked out yet.
				employee.ClockStatus = "Working";
			}

			ExecuteUpdate(employee);
		}

		private class EmployeeCache : ListCache<Employee>
		{
			protected override IEnumerable<Employee> Load()
				=> SelectMany("SELECT * FROM `employees` ORDER BY `is_hidden`, `first_name`, `last_name`");
		}

		private static readonly EmployeeCache cache = new EmployeeCache();

		public static Employee GetFirstOrDefault(Predicate<Employee> predicate, bool isShort = false) 
			=> cache.FirstOrDefault(predicate);

		public static List<Employee> GetAll(bool isShort = false) 
			=> cache.GetAll();

		public static List<Employee> GetWhere(Predicate<Employee> predicate) 
			=> cache.Find(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static IEnumerable<Employee> GetForTimeCard() 
			=> SelectMany("SELECT * FROM `employees` WHERE `is_hidden` = 0 ORDER BY `last_name`, `first_name`");

		private static void ValidateEmployee(Employee employee)
		{
			if (string.IsNullOrEmpty(employee.LastName) &&  string.IsNullOrEmpty(employee.FirstName))
			{
				throw new ApplicationException("Must include either first name or last name");
			}
		}

		public static long Insert(Employee employee)
		{
			ValidateEmployee(employee);

			return ExecuteInsert(employee);
		}

		public static void Update(Employee employee)
		{
			ValidateEmployee(employee);

			ExecuteUpdate(employee);
		}

		public static void Delete(long employeeId)
		{
			string command = "SELECT COUNT(*) FROM clockevent WHERE EmployeeNum=" + POut.Long(employeeId);
			if (Database.ExecuteString(command) != "0")
			{
				throw new ApplicationException("Not allowed to delete employee because of attached clock events.");
			}

			command = "SELECT COUNT(*) FROM timeadjust WHERE EmployeeNum=" + POut.Long(employeeId);
			if (Database.ExecuteString(command) != "0")
			{
				throw new ApplicationException("Not allowed to delete employee because of attached time adjustments.");
			}

			command = "SELECT COUNT(*) FROM userod WHERE EmployeeNum=" + POut.Long(employeeId);
			if (Database.ExecuteString(command) != "0")
			{
				throw new ApplicationException("Not allowed to delete employee because of attached user.");
			}

			command = "UPDATE appointment SET Assistant=0 WHERE Assistant=" + POut.Long(employeeId);
			Database.ExecuteNonQuery(command);

			command = "SELECT ScheduleNum FROM schedule WHERE EmployeeNum=" + POut.Long(employeeId);
			DataTable table = Database.ExecuteDataTable(command);
			List<string> listScheduleNums = new List<string>();//Used for deleting scheduleops below
			for (int i = 0; i < table.Rows.Count; i++)
			{
				//Add entry to deletedobjects table if it is a provider schedule type
				DeletedObjects.SetDeleted(DeletedObjectType.ScheduleProv, PIn.Long(table.Rows[i]["ScheduleNum"].ToString()));
				listScheduleNums.Add(table.Rows[i]["ScheduleNum"].ToString());
			}

			if (listScheduleNums.Count > 0)
			{
				command = "DELETE FROM scheduleop WHERE ScheduleNum IN(" + POut.String(String.Join(",", listScheduleNums)) + ")";
				Database.ExecuteNonQuery(command);
			}

			command = "DELETE FROM schedule WHERE EmployeeNum=" + POut.Long(employeeId);
			Database.ExecuteNonQuery(command);

			command = "DELETE FROM employee WHERE EmployeeNum =" + POut.Long(employeeId);
			Database.ExecuteNonQuery(command);

			command = "DELETE FROM timecardrule WHERE EmployeeNum=" + POut.Long(employeeId);
			Database.ExecuteNonQuery(command);
		}

		public static string GetNameFL(Employee emp)
		{
			return emp.FirstName + " " + emp.Initials + " " + emp.LastName;
		}

		public static string GetNameFL(long employeeNum)
		{
			Employee employee = GetFirstOrDefault(x => x.Id == employeeNum);
			return employee == null ? "" : GetNameFL(employee);
		}

		///<summary>Loops through List to find matching employee, and returns first 2 letters of first name.  Will later be improved with abbr field.</summary>
		public static string GetAbbr(long employeeNum)
		{
			string retVal = "";

			Employee employee = GetFirstOrDefault(x => x.Id == employeeNum);
			if (employee != null)
			{
				retVal = employee.FirstName;
				if (retVal.Length > 2)
				{
					retVal = retVal.Substring(0, 2);
				}
			}
			return retVal;
		}

		public static Employee GetEmp(long employeeId) 
			=> GetFirstOrDefault(x => x.Id == employeeId);

		/// <summary>
		/// Find formatted name in list. 
		/// Takes in a name that was previously formatted by Employees.GetNameFL and finds a match in the list. 
		/// If no match is found then returns null.
		/// </summary>
		public static Employee GetEmp(string name, List<Employee> employees) 
			=> employees.FirstOrDefault(employee => GetNameFL(employee) == name);

		public static List<Employee> GetEmps(List<long> employeeIds)
		{
			return GetWhere(x => employeeIds.Contains(x.Id));
		}

		/// <summary>Gets all employees associated to users that have a clinic set to the clinic passed in.  Passing in 0 will get a list of employees not assigned to any clinic.  Gets employees from the cache which is sorted by FName, LastName.</summary>
		public static List<Employee> GetEmpsForClinic(long clinicNum) 
			=> GetEmpsForClinic(clinicNum, false);
		

		/// <summary>Gets all the employees for a specific clinicNum, according to their associated user.  Pass in a clinicNum of 0 to get the list of unassigned or "all" employees (depending on isAll flag).  In addition to setting clinicNum to 0, set isAll true to get a list of all employees or false to get a list of employees that are not associated to any clinics.  Always gets the list of employees from the cache which is sorted by FName, LastName.</summary>
		public static List<Employee> GetEmpsForClinic(long clinicNum, bool isAll, bool getUnassigned = false)
		{
			List<Employee> listEmpsShort = GetAll(true);


			List<Employee> listEmpsWithClinic = new List<Employee>();
			List<Employee> listEmpsUnassigned = new List<Employee>();
			Dictionary<long, List<UserClinic>> dictUserClinics = new Dictionary<long, List<UserClinic>>();
			foreach (Employee empCur in listEmpsShort)
			{
				List<User> listUsers = Users.GetByEmployee(empCur.Id);
				if (listUsers.Count == 0)
				{
					listEmpsUnassigned.Add(empCur);
					continue;
				}
				foreach (User userCur in listUsers)
				{//At this point we know there is at least one Userod associated to this employee.
					if (userCur.ClinicId == 0)
					{//User's default clinic is HQ
						listEmpsUnassigned.Add(empCur);
						continue;
					}
					if (!dictUserClinics.ContainsKey(userCur.Id))
					{//User is restricted to a clinic(s).  Compare to clinicNum
						dictUserClinics[userCur.Id] = UserClinics.GetForUser(userCur.Id);//run only once per user
					}
					if (dictUserClinics[userCur.Id].Count == 0)
					{//unrestricted user, employee should show in all lists
						listEmpsUnassigned.Add(empCur);
						listEmpsWithClinic.Add(empCur);
					}
					else if (dictUserClinics[userCur.Id].Any(x => x.ClinicId == clinicNum))
					{//user restricted to this clinic
						listEmpsWithClinic.Add(empCur);
					}
				}
			}

			if (getUnassigned)
			{
				return listEmpsUnassigned.Union(listEmpsWithClinic).OrderBy(x => Employees.GetNameFL(x)).ToList();
			}

			//Returning the isAll employee list was handled above (all non-hidden emps, ListShort).
			if (clinicNum == 0)
			{//Return list of unassigned employees.  This is used for the 'Headquarters' clinic filter.
				return listEmpsUnassigned.GroupBy(x => x.Id).Select(x => x.First()).ToList();//select distinct emps
			}

			//Return list of employees restricted to the specified clinic.
			return listEmpsWithClinic.GroupBy(x => x.Id).Select(x => x.First()).ToList();//select distinct emps
		}

		/// <summary>
		/// Returns -1 if employeeNum is not found.  0 if not hidden and 1 if hidden.
		/// </summary>		
		public static int IsHidden(long employeeNum)
		{
			Employee employee = GetFirstOrDefault(x => x.Id == employeeNum);

			return employee == null ? -1 : (employee.IsHidden ? 1 : 0);
		}

		public static long GetEmpNumAtExtension(int phoneExt)
		{
			Employee employee = GetFirstOrDefault(x => x.PhoneExt == phoneExt);

			return employee == null ? -1 : employee.Id;
		}

		public static int SortByLastName(Employee x, Employee y)
		{
			return x.LastName.CompareTo(y.LastName);
		}

		public static int SortByFirstName(Employee x, Employee y)
		{
			return x.FirstName.CompareTo(y.FirstName);
		}

		public class EmployeeComparer : IComparer<Employee>
		{

			private SortBy SortOn = SortBy.lastName;

			public EmployeeComparer(SortBy sortBy)
			{
				SortOn = sortBy;
			}

			public int Compare(Employee x, Employee y)
			{
                int ret;
                switch (SortOn)
				{
					case SortBy.empNum:
						ret = x.Id.CompareTo(y.Id);
						break;

					case SortBy.ext:
						ret = x.PhoneExt.CompareTo(y.PhoneExt);
						break;

					case SortBy.firstName:
						ret = x.FirstName.CompareTo(y.FirstName);
						break;

					case SortBy.LFName:
						ret = x.LastName.CompareTo(y.LastName);
						if (ret == 0)
						{
							ret = x.FirstName.CompareTo(y.FirstName);
						}
						break;

					case SortBy.lastName:
					default:
						ret = x.LastName.CompareTo(y.LastName);
						break;
				}

				if (ret == 0) return x.LastName.CompareTo(y.LastName);
				
				return ret;
			}

			public enum SortBy
			{
				///<summary>0 - By Extension.</summary>
				ext,

				///<summary>1 - By EmployeeNum.</summary>
				empNum,

				///<summary>2 - By FName.</summary>
				firstName,

				///<summary>3 - By LName.</summary>
				lastName,

				///<summary>4 - By LName, then FName.</summary>
				LFName
			};
		}
	}
}
