namespace OpenDental.ReportingComplex
{
    /// <summary>
    /// Every ReportObject in an ODReport must be attached to a Section.
    /// </summary>
    public class Section
	{
        public Section(AreaSectionType type, int height)
		{
			SectionType = type;
			Height = height;
		}

        public int Height { get; set; }

        public int Width { get; set; }

        public AreaSectionType SectionType { get; set; }
    }

	/// <summary>
	/// The type of section is used in the Section class.
	/// Only ONE of each type is allowed except for the GroupHeader and GroupFooter which are optional and can have one pair for each group.
	/// The order of the sections is locked and user cannot change.
	/// </summary>
	public enum AreaSectionType
	{
		None,
		
		/// <summary>
		/// Printed at the top of the report.
		/// </summary>
		ReportHeader,
		
		/// <summary>
		/// Printed at the top of each page.
		/// </summary>
		PageHeader,
		
		/// <summary>
		/// Title of a specific group
		/// </summary>
		GroupTitle,
		
		/// <summary>
		/// Will print at the top of a specific group.
		/// </summary>
		GroupHeader,
		
		/// <summary>
		/// This is the data of the report and represents one row of data.
		/// This section gets printed once for each record in the datatable.
		/// </summary>
		Detail,
		
		/// <summary>
		/// Contains a buffer and/or a total of a column
		/// </summary>
		GroupFooter,
		
		/// <summary>
		/// Prints at the bottom of each page, including after the reportFooter
		/// </summary>
		PageFooter,
		
		/// <summary>
		/// Prints at the bottom of the report, but before the page footer for the last page.
		/// </summary>
		ReportFooter,
		
		/// <summary>
		/// Query Section, contains groups of queries.
		/// </summary>
		Query
	}
}
