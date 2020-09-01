using Imedisoft.Data.Annotations;
using System.Collections.Generic;

namespace Imedisoft.Data.Models
{
    /// <summary>
    /// These are the definitions for the custom appointment fields added and managed by the user.
    /// </summary>
    [Table("appt_field_defs")]
	public class AppointmentFieldDefinition
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		///		<para>
		///			The name of the field that the user will be allowed to fill in the appt edit window.
		///		</para>
		///		<para>
		///			Duplicates are prevented.
		///		</para>
		/// </summary>
		public string Name;

		/// <summary>
		/// The field type.
		/// </summary>
		public int Type;

		/// <summary>
		/// The text that contains pick list values.
		/// </summary>
		public string PickList;

		/// <summary>
		/// Returns a string representation of the appointment field definition.
		/// </summary>
		public override string ToString() => Name;
    }

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
