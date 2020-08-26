namespace OpenDental.UI
{
	/// <summary>
	/// Identifies the selection behaviour of a <see cref="ODGrid"/> instance.
	/// </summary>
	public enum GridSelectionMode
	{
		/// <summary>
		/// Nothing can be selected.
		/// </summary>
		None,

		/// <summary>
		/// Only one row can be selected.
		/// </summary>
		One,

		/// <summary>
		/// Only one cell can be selected.
		/// </summary>
		OneCell,

		/// <summary>
		/// Multiple rows can be selected, and the user can use the SHIFT, CTRL, and arrow keys to make selections
		/// </summary>
		MultiExtended,
	}
}
