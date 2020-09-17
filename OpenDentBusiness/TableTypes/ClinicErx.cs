using Imedisoft.Data.Annotations;
using System;

namespace OpenDentBusiness
{
    /// <summary>
	/// Tracks which clinics have access to eRx based on ClinicDescr.  Synchronized with HQ.
	/// </summary>
    [Table("clinic_erx")]
	public class ClinicErx : TableBase
	{
		[PrimaryKey]
		public long Id;






		///<summary>Description of a clinic from the clinic table.  Only used by OD HQ.  For customer records, use ClinicNum.</summary>
		public string ClinicDesc;

		///<summary>FK to clinic.ClinicNum.  Is the clinic that is used for accessing eRx.</summary>
		public long ClinicNum;

		///<summary>Set to true if the clinic with the given ClinicName has access to eRx.</summary>
		public ErxStatus EnabledStatus;



		///<summary>Clinic identifier used by the erx option.  Only used by OD HQ.</summary>
		public string ClinicId;

		///<summary>Unique key used by the erx option.  Only used by OD HQ.</summary>
		public string ClinicKey;


		[Ignore, Obsolete]
		public long PatNum;

		[Ignore, Obsolete]
		public string AccountId;

		[Ignore, Obsolete]
		public long RegistrationKeyNum;

		public ClinicErx Copy()
		{
			return (ClinicErx)MemberwiseClone();
		}
	}
}
