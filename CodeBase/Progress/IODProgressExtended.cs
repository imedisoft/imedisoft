namespace CodeBase
{
    /// <summary>
    /// This interface is useful for use with FormProgressExtended especially when there are 
    /// certain situations where you don't want to display the FormProgressExtended.
    /// </summary>
    public interface IODProgressExtended : IODProgress
	{
		/// <summary>
		/// Gets the status of the pause button.
		/// </summary>
		bool IsPaused { get; }

		/// <summary>
		/// Gets the status of the canceled button
		/// </summary>
		bool IsCanceled { get; }

		/// <summary>
		/// Sets the pause button to enabled and changes the text to say resume.
		/// </summary>
		void AllowResume();

		/// <summary>
		/// Sets the pause and cancel buttons to invisible on the form
		/// </summary>
		void HideButtons();

		/// <summary>
		///		<para>
		///			Updates or makes a new progress bar with any of the options chosen. Tag string 
		///			determines which bar you want to update when using more than one.
		///		</para>
		///		<para>
		///			Simplified version of UpdateProgressDetailed. Options are pre-set for a block 
		///			bar, with text on top of the bar and on the right.
		///		</para>
		/// </summary>
		void UpdateProgress(string labelTop, string tagstring, string percentVal = "", int barVal = 0, int barMax = 100, bool isTopHidden = false, string labelValue = "");

		/// <summary>
		/// If paused, method will wait and return false once 'resume' is pressed. Cancel will be 
		/// handled outside of this method and will return true.
		/// </summary>
		bool IsPauseOrCancel();

		/// <summary>
		/// Changes the text of the cancel button to close.
		/// </summary>
		void OnProgressDone();
	}

	/// <summary>
	/// Null progress bar. If no progress bar exists due to lack of UI then the bar will do nothing.
	/// </summary>
	public class ODProgressExtendedNull : ODProgressDoNothing, IODProgressExtended
	{
		public bool IsCanceled => false;

		public bool IsPaused => false;

		public void HideButtons() 
		{ 
		}

		public void AllowResume() 
		{ 
		}

		public bool IsPauseOrCancel() => false;

		public void UpdateProgress(string labelTop, string tagstring, string percentVal = "", int barVal = 0, int barMax = 100, bool isTopHidden = false, string labelValue = "")
		{
		}

		public void OnProgressDone()
		{
		}
	}
}
