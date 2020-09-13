using Imedisoft.Data.Annotations;

namespace Imedisoft.Data.Models
{
    [Table("auto_code_conditions")]
	public class AutoCodeCondition 
	{
		[PrimaryKey]
		public long Id;

		[ForeignKey(typeof(AutoCodeItem), nameof(AutoCodeItem.Id))]
		public long AutoCodeItemId;

		public AutoCodeConditionType Type;
	}
}
