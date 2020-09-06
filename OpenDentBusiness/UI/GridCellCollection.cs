using System.Collections.Generic;

namespace OpenDental.UI
{
	public class GridCellCollection : List<GridCell>
	{
		public void Add(string value)
		{
			Add(new GridCell(value ?? ""));
		}
	}
}
