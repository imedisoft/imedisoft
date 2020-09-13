using System.Collections.Generic;

namespace Imedisoft.Data.Models
{
    public static class AppointmentFieldType
	{
		public const int Text = 0;
		public const int PickList = 1;

		public static IEnumerable<DataItem<int>> Values
		{
			get
			{
				yield return new DataItem<int>(Text, Translation.Common.Text);
				yield return new DataItem<int>(PickList, Translation.Common.PickList);
			}
		}
	}
}
