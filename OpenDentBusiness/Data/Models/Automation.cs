using Imedisoft.Data.Annotations;
using System;
using System.Collections;
using System.ComponentModel;

namespace OpenDentBusiness
{
    /// <summary>
    /// A trigger event causes one or more actions.
    /// </summary>
    [Table("automations")]
	public class Automation : TableBase
	{
		[PrimaryKey]
		public long Id;

		public string Description;

		/// <summary>
		/// The action that triggers this automation.
		/// </summary>
		public AutomationTrigger Trigger;

		/// <summary>
		/// If this has a CompleteProcedure trigger, this is a comma-delimited list of codes that will trigger the action.
		/// </summary>
		public string ProcedureCodes;

		/// <summary>
		/// Enum:AutomationAction The action taken as a result of the trigger.
		/// To get more than one action, create multiple automation entries.
		/// </summary>
		public AutomationAction AutoAction;

		/// <summary>
		/// FK to sheetdef.SheetDefNum.
		/// If the action is to print a sheet, then this tells which sheet to print.
		/// So it must be a custom sheet.
		/// Also, not that this organization does not allow passing parameters to the sheet such as which procedures were completed, or which appt was broken.
		/// </summary>
		[ForeignKey(typeof(SheetDef), nameof(SheetDef.SheetDefNum))]
		public long SheetDefinitionId;

		/// <summary>
		/// Only used if action is CreateCommlog.
		/// </summary>
		[ForeignKey(typeof(Def), nameof(Def.DefNum))]
		public long CommType;

		/// <summary>
		/// If a commlog action, then this is the text that goes in the commlog.
		/// If this is a ShowStatementNoteBold action, then this is the NoteBold. Might later be expanded to work with email or to use variables.
		/// </summary>
		public string MessageContent;

		/// <summary>
		/// Enum:ApptStatus . This column is not used anymore.
		/// </summary>
		public ApptStatus AptStatus;

		[ForeignKey(typeof(AppointmentType), nameof(AppointmentType.Id))]
		public long AppointmentTypeId;

		/// <summary>
		/// Enum:PatientStatus - used to determine which status to change to for ChangePatientStatus automation actions.
		/// Should never be 'Deleted'
		/// </summary>
		public PatientStatus PatStatus;

		public Automation Copy() => (Automation)MemberwiseClone();
	}
}
