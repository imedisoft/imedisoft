using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class AutoCodeItems
	{
		[CacheGroup(nameof(InvalidType.AutoCodes))]
		private class AutoCodeItemCache : DictionaryCache<long, AutoCodeItem>
        {
            protected override IEnumerable<AutoCodeItem> Load() 
				=> SelectMany("SELECT * FROM `auto_code_items`");

			protected override long GetKey(AutoCodeItem item) 
				=> item.Id;
        }

		private static readonly AutoCodeItemCache cache = new AutoCodeItemCache();

		public static bool GetContainsKey(long autoCodeItemId) 
			=> cache.Contains(autoCodeItemId);

		public static void RefreshCache() 
			=> cache.Refresh();

		public static AutoCodeItem GetById(long autoCodeItemId)
			=> cache.Find(autoCodeItemId);

		public static List<AutoCodeItem> GetByAutoCode(long autoCodeId) 
			=> cache.Find(x => x.AutoCodeId == autoCodeId).ToList();

		public static void Save(AutoCodeItem autoCodeItem)
        {
			if (autoCodeItem.Id == 0) ExecuteInsert(autoCodeItem);
            else
            {
				ExecuteUpdate(autoCodeItem);
            }
        }

		public static void Delete(AutoCodeItem autoCodeItem) 
			=> ExecuteDelete(autoCodeItem);
		
		public static long GetProcedureCode(long autoCodeId, string tooth, string surfaces, bool isAdditional, int age, bool willBeMissing)
		{
			var autoCodeItems = GetByAutoCode(autoCodeId);
			if (autoCodeItems.Count == 0)
			{
				return 0;
			}

			foreach (var autoCodeItem in autoCodeItems)
			{
				var conditions = AutoCodeConditions.GetByAutoCodeItem(autoCodeItem.Id);
				var conditionsMet = true;

				foreach (var condition in conditions)
				{
					if (!AutoCodeConditions.ConditionIsMet(condition.Type, tooth, surfaces, isAdditional, willBeMissing, age))
					{
						conditionsMet = false;

						break;
					}
				}

				if (conditionsMet)
				{
					return autoCodeItem.ProcedureCodeId;
				}
			}

			return autoCodeItems[0].ProcedureCodeId;
		}

		private static long VerifyProcedureCode(long procedureCodeId, string tooth, string surfaces, bool isAdditional, long patientId, int age)
		{
			var currentAutoCodeItem = cache.FirstOrDefault(autoCodeItem => autoCodeItem.ProcedureCodeId == procedureCodeId);
			if (currentAutoCodeItem == null)
            {
				return procedureCodeId;
			}

			var autoCode = AutoCodes.GetById(currentAutoCodeItem.AutoCodeId);
			if (autoCode == null)
            {
				return procedureCodeId;
			}

			var autoCodeItems = GetByAutoCode(autoCode.Id);
			if (autoCodeItems.Count == 0)
			{
				return procedureCodeId;
			}

			var willBeMissing = Procedures.WillBeMissing(tooth, patientId);

			foreach (var autoCodeItem in autoCodeItems)
			{
				var conditions = AutoCodeConditions.GetByAutoCodeItem(autoCodeItem.Id);
				var conditionsMet = true;

				foreach (var condition in conditions)
				{
					if (!AutoCodeConditions.ConditionIsMet(condition.Type, tooth, surfaces, isAdditional, willBeMissing, age))
					{
						conditionsMet = false;
					}
				}

				if (conditionsMet)
				{
					return autoCodeItem.ProcedureCodeId;
				}
			}

			return procedureCodeId;
		}

		public static bool ShouldPromptForCodeChange(Procedure procedure, ProcedureCode procedureCode, Patient patient, bool isMandibular, List<ClaimProc> claimProcedures, out long procedureCodeId)
		{
			procedureCodeId = procedure.CodeNum;

			if (procedureCode.TreatArea == TreatmentArea.Mouth || procedureCode.TreatArea == TreatmentArea.Quad || procedureCode.TreatArea == TreatmentArea.Sextant || Procedures.IsAttachedToClaim(procedure, claimProcedures))
			{
				return false;
			}

			if (procedureCode.TreatArea == TreatmentArea.Arch && string.IsNullOrEmpty(procedure.Surf))
            {
				return false;
            }

			procedureCodeId = procedureCode.TreatArea switch
			{
				TreatmentArea.Arch => VerifyProcedureCode(
					procedureCode.CodeNum, procedure.Surf == "U" ? "1" : "32", "", procedure.IsAdditional, patient.PatNum, patient.Age),

				TreatmentArea.ToothRange => VerifyProcedureCode(
					procedureCode.CodeNum, isMandibular ? "32" : "1", "", procedure.IsAdditional, patient.PatNum, patient.Age),

				_ => VerifyProcedureCode(
					procedureCode.CodeNum, procedure.ToothNum, Tooth.SurfTidyForClaims(procedure.Surf, procedure.ToothNum), procedure.IsAdditional, patient.PatNum, patient.Age)
			};

			return procedureCode.CodeNum != procedureCodeId;
		}
	}
}
