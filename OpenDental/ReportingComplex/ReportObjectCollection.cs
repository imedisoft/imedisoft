using System.Collections;

namespace OpenDental.ReportingComplex
{
    /// <summary>
    /// Contains the ReportObject objects for every report object in the report.
    /// </summary>
    public class ReportObjectCollection : CollectionBase
	{
		/// <summary>
		/// Returns the ReportObject with the given index.
		/// </summary>
		public ReportObject this[int index]
		{
			get
			{
				return (ReportObject)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		/// <summary>
		/// Returns the ReportObject with the given name.
		/// </summary>
		public ReportObject this[string name]
		{
			get
			{
				foreach (ReportObject reportObject in List)
				{
					if (reportObject.Name == name)
					{
						return reportObject;
					}
				}

				return null;
			}
		}

		public int Add(ReportObject value)
		{
			return List.Add(value);
		}

		public int IndexOf(ReportObject value)
		{
			return List.IndexOf(value);
		}

		public void Insert(int index, ReportObject value)
		{
			List.Insert(index, value);
		}
	}
}
