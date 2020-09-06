using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("icd10")]
	public class Icd10
	{
		/// <summary>
		/// ICD-10-CM or ICD-10-PCS code. Dots are included. Not allowed to edit this column once saved in the database.
		/// </summary>
		[PrimaryKey]
		public string Code;

		/// <summary>
		/// Short Description provided by ICD10 documentation.
		/// </summary>
		public string Description;

		/// <summary>
		///		<para>
		///			False if the code is a "header" – not valid for submission on a UB04.
		///		</para>
		///		<para>
		///			True if the code is valid for submission on a UB04.
		///		</para>
		/// </summary>
		public bool IsCode;
	}
}
