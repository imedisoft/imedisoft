using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness
{
	public enum AutomationTrigger
	{
		CompleteProcedure,
		BreakAppointment,
		CreateApptNewPat,

		/// <summary>
		/// Regardless of module.  Usually only used with conditions.
		/// </summary>
		OpenPatient,

		CreateAppt,

		/// <summary>
		/// Attaching a procedure to a scheduled appointment.
		/// </summary>
		ScheduleProcedure,

		SetBillingType,

		/// <summary>
		/// Creating a new Rx
		/// </summary>
		RxCreate,
	}
}
