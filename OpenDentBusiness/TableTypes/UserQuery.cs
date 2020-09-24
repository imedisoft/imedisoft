using Imedisoft.Data.Annotations;

namespace OpenDentBusiness
{
    /// <summary>
    /// A list of query favorites that users can run.
    /// </summary>
    [Table]
	public class UserQuery : TableBase
	{
		[PrimaryKey]
		public long QueryNum;

		public string Description;

		/// <summary>
		/// The name of the file to export to.
		/// </summary>
		public string FileName;

		/// <summary>
		/// The text of the query.
		/// </summary>
		public string QueryText;

		/// <summary>
		/// Determines whether the query is safe for users with lower permissions.  
		/// Also causes this user query to show as a sub menu to the User Query menu item within the Reports main menu.
		/// </summary>
		public bool IsReleased;

		/// <summary>
		/// Determines whether the Query Favorites window should prompt for query values via FormQueryParser/'SET Fields' popup when running query.
		/// </summary>
		public bool IsPromptSetup;

		public UserQuery Copy() => (UserQuery)MemberwiseClone();
	}
}
