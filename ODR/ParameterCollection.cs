using System;
using System.Collections;

namespace ODR
{
	public class ParameterCollection : CollectionBase
	{
		public Parameter this[int index]
		{
			get
			{
				return ((Parameter)List[index]);
			}
			set
			{
				List[index] = value;
			}
		}

		public Parameter this[string name]
		{
			get
			{
				foreach (Parameter p in List)
				{
					if (p.Name == name)
						return p;
				}
				return null;
			}
		}

		public int Add(Parameter value)
		{
			return (List.Add(value));
		}

		public int IndexOf(Parameter value)
		{
			return (List.IndexOf(value));
		}

		public void Insert(int index, Parameter value)
		{
			List.Insert(index, value);
		}
	}
}
