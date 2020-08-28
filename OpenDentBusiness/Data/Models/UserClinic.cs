using Imedisoft.Data.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness
{
    /// <summary>
    /// Represents a link between a user and a clinic.
    /// </summary>
    [Table("user_clinics")]
	[CrudTable(IsSynchable = true)]
	public class UserClinic : TableBase
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(Userod), nameof(Userod.Id))]
		public long UserId;

		[ForeignKey(typeof(Clinic), nameof(Clinic.Id))]
		public long ClinicId;

		public UserClinic()
		{
		}

		public UserClinic(long clinicId, long userId)
		{
			UserId = userId;
			ClinicId = clinicId;
		}

		public UserClinic Copy() => (UserClinic)MemberwiseClone();
	}
}
