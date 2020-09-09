using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    /// <summary>
    /// Describes a scale to be used in grading. 
    /// </summary>
    [Table("grading_scales")]
	public class GradingScale
	{
		[PrimaryKey]
		public long Id;

		public GradingScaleType Type;

		public string Description;
	}

	/// <summary>
	/// Identifies the type of a grading scale. Used to determine how grades are assigned.
	/// </summary>
	public enum GradingScaleType
	{
		/// <summary>
		/// User-Defined list of possible grades. Grade is calculated as an average.
		/// </summary>
		PickList,

		/// <summary>
		/// Percentage Scale 0-100. Grade is calculated as an average.
		/// </summary>
		Percentage,

		/// <summary>
		/// Allows point values for grades. Grade is calculated as a sum of all points out of points possible.
		/// </summary>
		Weighted
	}
}
