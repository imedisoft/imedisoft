namespace OpenDentBusiness
{
    /// <summary>
	/// Keeps track of column widths in Wiki Lists.
	/// </summary>
	public class WikiListHeaderWidth : TableBase
	{
		[CrudColumn(IsPriKey = true)]
		public long WikiListHeaderWidthNum;

		/// <summary>
		/// Name of the list that this header belongs to. 
		/// Tablename without the prefix.
		/// </summary>
		public string ListName;

		/// <summary>
		/// Name of the column that this header belongs to.
		/// </summary>
		public string ColName;

		/// <summary>
		/// Width in pixels of column.
		/// </summary>
		public int ColWidth;

		/// <summary>
		/// Newline delimited list of options for the user to select from when adding or editing a wiki list item.
		/// </summary>
		public string PickList;

		public WikiListHeaderWidth Copy()
		{
			return (WikiListHeaderWidth)MemberwiseClone();
		}
	}
}
