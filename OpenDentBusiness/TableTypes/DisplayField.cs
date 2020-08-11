using Imedisoft.Data.Annotations;
using System;
using System.Collections;
using System.ComponentModel;

namespace OpenDentBusiness
{
    /// <summary>
    /// Allows customization of which fields display in various lists and grids.
    /// For now, the only grid is ProgressNotes. 
    /// Will also eventually let users set column widths and translate titles.
    /// For now, the selections are the same for all computers.
    /// </summary>
    [Table("display_fields")]
	public class DisplayField : TableBase
	{
		[PrimaryKey, Column("DisplayFieldNum")]
		public long Id;

		/// <summary>
		/// This is the internal name that OD uses to identify the field within this category.
		/// This will be the default description if the user doesn't specify an alternate.
		/// Ortho Chart display fields use this column to indicate if this field should be used for signatures.
		/// </summary>
		public string InternalName;

		/// <summary>
		/// Order to display in the grid or list. Every entry must have a unique itemorder.
		/// </summary>
		[Column("ItemOrder")]
		public int Order;

		/// <summary>
		/// Optional alternate description to display for field. 
		/// Can be in another language. 
		/// For the ortho category, this is the 'key', since InternalName is blank.
		/// </summary>
		public string Description;

		/// <summary>
		/// For grid columns, this lets user override the column width. 
		/// Especially useful for foreign languages.
		/// </summary>
		public int ColumnWidth;

		/// <summary>
		/// Enum:DisplayFieldCategory If category is 0, then this is attached to a ChartView.
		/// </summary>
		public DisplayFieldCategory Category;

		/// <summary>FK to chartview.ChartViewNum. 0 if attached to a category.</summary>
		public long ChartViewNum;

		/// <summary>
		/// Newline delimited string which contains the selectable options in combo box dropdowns.
		/// Specifically for the Ortho chart.
		/// </summary>
		public string PickList;

		/// <summary>
		/// Because ortho chart display fields utilize the InternalName field for signatures, this field is here to override description.
		/// Some users want to use different fields but use the same description for mulitple tabs.
		/// E.g. The display field of WeightWeekly shows as "Weight" and in another tab the field for WeightMonthly can also show as "Weight".
		/// </summary>
		public string DescriptionOverride;

		public DisplayField()
		{
		}

		public DisplayField(string internalName, int columnWidth, DisplayFieldCategory category)
		{
			InternalName = internalName;
			ColumnWidth = columnWidth;
			Description = "";
			Category = category;
		}

		public DisplayField Copy()
		{
			return (DisplayField)MemberwiseClone();
		}

		/// <summary>
		/// Returns a string representation of the display field.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
			=> string.IsNullOrEmpty(Description) ? InternalName : Description;
	}

	public enum DisplayFieldCategory
	{
		/// <summary>
		/// This indicates progress notes.
		/// </summary>
		None,

		[Description("Patient Select")]
		PatientSelect,

		[Description("Patient Information")]
		PatientInformation,

		[Description("Account Module")]
		AccountModule,

		[Description("Recall List")]
		RecallList,

		[Description("Chart Patient Information")]
		ChartPatientInformation,

		[Description("Procedure Group Note")]
		ProcedureGroupNote,

		[Description("Treatment Plan Module")]
		TreatmentPlanModule,

		[Description("Ortho Chart")]
		OrthoChart,

		[Description("Appointment Bubble")]
		AppointmentBubble,

		[Description("Account Patient Information")]
		AccountPatientInformation,

		[Description("Statement Main Grid")]
		StatementMainGrid,

		[Description("Family Recall Grid")]
		FamilyRecallGrid,

		[Description("Appointment Edit")]
		AppointmentEdit,

		[Description("Planned Appointment Edit")]
		PlannedAppointmentEdit,

		[Description("Outstanding Ins Report")]
		OutstandingInsReport,

		[Description("Patient Search")]
		CEMTSearchPatients,

		[Description("A/R Manager Sent Grid")]
		ArManagerSentGrid,

		[Description("A/R Manager Unsent Grid")]
		ArManagerUnsentGrid,

		[Description("A/R Manager Excluded Grid")]
		ArManagerExcludedGrid,
	}
}
