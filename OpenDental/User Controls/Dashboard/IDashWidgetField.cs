using OpenDentBusiness;

namespace OpenDental
{
    interface IDashWidgetField
	{
		/// <summary>
		/// Directly sets data required for display, rather than querying the database for this information.
		/// </summary>
		void SetData(PatientDashboardDataEventArgs eventArgs, SheetField sheetField);

		/// <summary>
		/// Refreshes all the data required for display.  Must be implemented to be able to run on a thread.
		/// </summary>
		void RefreshData(Patient patient, SheetField sheetField);

		/// <summary>
		/// Refreshes the view.  Must be implemented in a way to safely invoke back to the UI thread.
		/// </summary>
		void RefreshView();
	}
}
