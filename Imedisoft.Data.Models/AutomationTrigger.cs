namespace Imedisoft.Data.Models
{
	/// <summary>
	/// Identifies a trigger for a automation action.
	/// </summary>
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
