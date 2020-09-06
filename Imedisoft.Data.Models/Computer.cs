using Imedisoft.Data.Annotations;
using System;

namespace Imedisoft.Data
{
    [Table("computers")]
	public class Computer
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The name of the computer.
		/// </summary>
		public string MachineName;

		/// <summary>
		/// Allows us to tell which computers are running. All workstations record a heartbeat here
		/// at an interval of 3 minutes. So if the heartbeat is fairly fresh, then that's an 
		/// accurate indicator of whether Open Dental is running on that computer.
		/// </summary>
		public DateTime LastHeartbeat;

		/// <summary>
		/// Returns a string representation of the computer.
		/// </summary>
		public override string ToString() => MachineName;
	}
}
