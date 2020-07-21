using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// A generic comparison that puts primary teeth after perm teeth.
    /// </summary>
    class ToothComparer : IComparer<string>
	{
		/// <summary>
		/// A generic comparison that puts primary teeth after perm teeth.
		/// </summary>
		public int Compare (string toothA,string toothB) 
			=> Tooth.ToOrdinal(toothA).CompareTo(Tooth.ToOrdinal(toothB));
	}
}
