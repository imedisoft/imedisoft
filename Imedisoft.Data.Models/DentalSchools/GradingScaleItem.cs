using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    /// <summary>
    /// The specific grades allowed on a scale.
    /// </summary>
    [Table("grading_scale_items")]
	public class GradingScaleItem
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(GradingScale), nameof(GradingScale.Id))]
		public long GradingScaleId;

		/// <summary>
		/// For example A, B, C, D, F. Optional. If not specified, it shows the number.
		/// </summary>
		public string Text;

		/// <summary>
		/// For example A=4, A-=3.8, pass=1, etc.  Required.  Enforced to be equal to or less than GradingScale.MaxPointsPoss.
		/// </summary>
		public float Value;

		/// <summary>
		/// Optional additional info about what this particular grade means. Just used as guidance 
		/// and does not get copied to the individual student record.
		/// </summary>
		public string Description;
	}
}
