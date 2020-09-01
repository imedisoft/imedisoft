using Imedisoft.Data.Annotations;
using System;
using System.Collections;

namespace OpenDentBusiness
{
    /// <summary>
    /// These are the definitions for the custom patient fields added and managed by the user.
    /// </summary>
    [Table("pat_field_defs")]
	public class PatFieldDef : TableBase
	{
		[PrimaryKey]
		public long Id;

		/// <summary>
		/// The name of the field that the user will be allowed to fill in the patient info window.
		/// </summary>
		public string FieldName;

		public PatFieldType FieldType;

		/// <summary>
		/// The text that contains pick list values.
		/// </summary>
		public string PickList;

		public int ItemOrder;

		public bool IsHidden;

		public PatFieldDef Copy() => (PatFieldDef)MemberwiseClone();
	}

	public enum PatFieldType
	{
		Text,

		PickList,

		/// <summary>
		/// Stored in db as entered, already localized.
		/// For example, it could be 2/04/11, 2/4/11, 2/4/2011, or any other variant. 
		/// This makes it harder to create queries that filter by date, but easier to display dates as part of results.
		/// </summary>
		Date,

		/// <summary>
		/// If checked, value stored as "1".  If unchecked, row deleted.
		/// </summary>
		Checkbox,

		/// <summary>
		/// This seems to have been added without implementing. 
		/// Not sure what will happen if someone tries to use it.
		/// </summary>
		Currency,
	}
}
