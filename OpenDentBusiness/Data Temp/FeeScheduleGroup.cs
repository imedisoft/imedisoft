using Imedisoft.Data.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenDentBusiness
{
    [Table("fee_schedule_groups")]
	public class FeeScheduleGroup : TableBase
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(FeeSchedule), nameof(FeeSchedule.Id))]
		public long FeeScheduleId;

		public string Description;

		public string ClinicIds;


		/// <summary>
		/// The list of Clinic.ClinicNums filled from the ClinicNums comma delimited string. 
		/// Does not filter out restricted clinics.
		/// </summary>
		[XmlIgnore, JsonIgnore]
		public List<long> ListClinicNumsAll
		{
			get
			{
				if (ClinicIds == "")
				{
					return new List<long>();
				}
				return new List<long>(ClinicIds.Split(',').Select(long.Parse).Distinct().ToList());
			}
			set
			{
				ClinicIds = string.Join(",", value);
			}
		}
	}
}
