using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace OpenDentBusiness
{
    public class ODDataRow : SortedList<string, string>
	{
		public string this[int index] => Values[index];
	}
}
