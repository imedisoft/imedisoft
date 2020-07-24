using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenDental.UI
{
    public class ListGridColumns : List<GridColumn>
	{
		/// <summary>
		/// Gets the index of the column with the specified heading.
		/// </summary>
		public int GetIndex(string heading)
		{
			for (int i = 0; i < Count; i++)
			{
				if (this[i].Heading == heading)
				{
					return i;
				}
			}

			return -1;
		}
	}
}
