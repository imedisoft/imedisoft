using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("automations")]
	public class Automation
	{
		[PrimaryKey]
		public long Id;

		public string Description;

		/// <summary>
		/// The action that triggers this automation.
		/// </summary>
		public AutomationTrigger Trigger;

		/// <summary>
		/// The action to take when the action is triggered.
		/// </summary>
		public AutomationAction Action;

		/// <summary>
		///		<para>
		///			A comma-delimited list of procedure codes that will trigger the action.
		///		</para>
		///		<para>
		///			Applies when <see cref="Trigger"/> is set to <see cref="AutomationTrigger.CompleteProcedure"/>.
		///		</para>
		/// </summary>
		public string ProcedureCodes;

		/// <summary>
		///		<para>
		///			The ID of the sheet to print when the action is triggered. Must be a custom 
		///			sheet.
		///		</para>
		///		<para>
		///			Also, note that this organization does not allow passing parameters to the 
		///			sheet such as which procedures were completed, or which appointment was broken.
		///		</para>
		///		<para>
		///			Applies when <see cref="Action"/> is set to one of the following:
		///			<list type="bullet">
		///				<item><see cref="AutomationAction.PrintPatientLetter"/></item>
		///				<item><see cref="AutomationAction.PrintReferralLetter"/></item>
		///				<item><see cref="AutomationAction.PrintRxInstruction"/></item>
		///				<item><see cref="AutomationAction.ShowExamSheet"/></item>
		///				<item><see cref="AutomationAction.ShowConsentForm"/></item>
		///			</list>
		///		</para>
		/// </summary>
		// [ForeignKey(typeof(SheetDef), nameof(SheetDef.SheetDefNum))]
		public long? SheetDefinitionId;

		/// <summary>
		///		<para>
		///			The ID of the commlog type for the commlog entry created by the action.
		///		</para>
		///		<para>
		///			Applies when <see cref="Action"/> is set to <see cref="AutomationAction.CreateCommlog"/>.
		///		</para>
		/// </summary>
		[ForeignKey(typeof(Definition), nameof(Definition.Id))]
		public long? CommType;

		/// <summary>
		/// If a commlog action, then this is the text that goes in the commlog.
		/// If this is a ShowStatementNoteBold action, then this is the NoteBold. Might later be expanded to work with email or to use variables.
		/// </summary>
		public string MessageContent;

		/// <summary>
		///		<para>
		///			Applies when <see cref="Action"/> is set to <see cref="AutomationAction.SetApptType"/>.
		///		</para>
		/// </summary>
		// [ForeignKey(typeof(AppointmentType), nameof(AppointmentType.Id))]
		public long? AppointmentTypeId;

		public PatientStatus PatientStatus;
	}
}
