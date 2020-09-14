using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data.Models
{
    /// <summary>
    ///		<para>
    ///			Makes an entry every time Imedisoft has successfully updated to a newer version.
    ///		</para>
    ///		<para>
    ///			New entries will always be for the newest version being used so that users can see
    ///			a "history" of how long they used previous versions.
    ///		</para>
    ///		<para>
    ///			This will also help EHR customers when attesting or when they get audited.
    ///		</para>
    /// </summary>
    [Table("update_histories")]
	public class UpdateHistory
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The date and time on which the update was installed.
		/// </summary>
		public DateTime InstalledOn;

		/// <summary>
		/// The version that Imedisoft was updated to.
		/// </summary>
		public string Version;

		public UpdateHistory()
		{
		}

		public UpdateHistory(string version)
		{
			Version = version;
		}
	}
}
