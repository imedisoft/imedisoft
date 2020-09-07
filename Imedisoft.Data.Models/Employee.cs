using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
    [Table("employees")]
	public class Employee
	{
		[PrimaryKey]
		public long Id;

		public string LastName;
		public string FirstName;
		public string Initials;

		/// <summary>
		/// This is just text used to quickly display the clockstatus. e.g. Working, Break, Lunch, Home, etc...
		/// </summary>
		public string ClockStatus;

		/// <summary>
		/// The phone extension for the employee. e.g. 101, 102, etc...
		/// </summary>
		public int PhoneExt;

		/// <summary>
		/// Used to store the payroll identification number used to generate payroll reports. ADP uses six digit number between 000051 and 999999.
		/// </summary>
		public string PayrollId;

		public string WirelessPhone;

		public string EmailWork;
		public string EmailPersonal;

		public bool IsFurloughed;
		public bool IsWorkingHome;
		public bool IsHidden;

		public Employee Copy()
		{
			return (Employee)MemberwiseClone();
		}
	}
}
