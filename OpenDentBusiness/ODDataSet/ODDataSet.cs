using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace OpenDentBusiness
{
    public class ODDataTableCollection : Collection<ODDataTable>
	{
		public ODDataTable this[string name]
		{
			get
			{
				foreach (ODDataTable table in this)
				{
					if (table.Name == name)
					{
						return table;
					}
				}

				ODDataTable tbl = new ODDataTable();
				tbl.Name = name;

				return tbl;
			}
		}
	}
}
