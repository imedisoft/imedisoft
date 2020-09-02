using Imedisoft.Data.Annotations;
using Imedisoft.Data.Models;
using System;

namespace OpenDentBusiness
{
	/// <summary>
	/// Links active and inactive treatment plans to procedurelog rows.  
	/// These rows will be deleted as their corresponding procedures get set complete.
	/// </summary>
	[Table]
	public class TreatPlanAttach : TableBase
	{
		[PrimaryKey]
		public long TreatPlanAttachNum;

		[ForeignKey(typeof(TreatPlan), nameof(TreatPlan.TreatPlanNum))]
		public long TreatPlanNum;

		[ForeignKey(typeof(Procedure), nameof(Procedure.ProcNum))]
		public long ProcNum;

		/// <summary>
		/// FK to definition.DefNum, which contains the text of the priority. 
		/// Identical to Procedure.Priority but used to allow different priorities
		/// for the same procedure depending on which TP it is a part of.
		/// </summary>
		[ForeignKey(typeof(Definition), nameof(Definition.Id))]
		public long Priority;
	}
}
