using Imedisoft.Data.Annotations;
using OpenDentBusiness;
using System;

namespace Imedisoft.Data.Models
{
    [Table("security_logs")]
	public class SecurityLog
	{
		[PrimaryKey]
		public long Id;

		public Permissions Type;

		//[ForeignKey(typeof(Userod), nameof(Userod.Id))]
		public long UserId;

		/// <summary>
		/// The date and time of the log entry.
		/// </summary>
		[Column(AutoGenerated = true)]
		public DateTime LogDate;

		/// <summary>
		/// The log message.
		/// </summary>
		public string LogMessage;

		/// <summary>
		/// The source component that created the log entry.
		/// </summary>
		public SecurityLogSource LogSource;

		//[ForeignKey(typeof(Patient), nameof(Patient.PatNum))]
		public long? PatientId;

		public string MachineName;

		///<summary>A foreign key to a table associated with the PermType.  0 indicates not in use.  
		///This is typically used for objects that have specific audit trails so that users can see all audit entries related to a particular object.  
		///Every permission using FKey should be included and implmented in the CrudAuditPerms enum so that securitylog FKeys are note orphaned.
		///Additonaly, the tabletype will to have the [CrudTable(CrudAuditPerms=CrudAuditPerm._____] added with the new CrudAuditPerm you created.
		///For the patient portal, it is used to indicate logs created on behalf of other patients.  
		///It's uses include:  AptNum with PermType AppointmentCreate, AppointmentEdit, or AppointmentMove tracks all appointment logs for a particular 
		///appointment.
		///CodeNum with PermType ProcFeeEdit currently only tracks fee changes.  
		///PatNum with PermType PatientPortal represents an entry that a patient made on behalf of another patient.
		///	The PatNum column will represent the patient who is taking the action.  
		///PlanNum with PermType InsPlanChangeCarrierName tracks carrier name changes.</summary>
		public long? ObjectId;

		/// <summary>
		/// Used to store the previous DateTStamp or SecDateTEdit of the object FKey refers to.
		/// </summary>
		public DateTime? ObjectDate;

		[Ignore]
		public string PatientName;

		[Ignore]
		public string Hash;
	}
}