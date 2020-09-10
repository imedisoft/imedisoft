using Imedisoft.Data.Cache;
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Imedisoft.Data
{
    public partial class Definitions
	{
		[CacheGroup(nameof(InvalidType.Defs))]
		private class DefinitionCache : ListCache<Definition>
		{
			protected override IEnumerable<Definition> Load()
				=> SelectMany("SELECT * FROM `definitions` ORDER BY `category`, `sort_order`");
		}

		private static readonly DefinitionCache cache = new DefinitionCache();

		public static Dictionary<string, List<Definition>> GetDictionaryNoCache(bool includeHidden = false)
		{
			var results = new Dictionary<string, List<Definition>>();
			var definitions = SelectMany("SELECT * FROM `definitions` ORDER BY `category`, `sort_order`").ToList();

			foreach (var definitionCategory in DefinitionCategory.All)
            {
				results[definitionCategory] = GetForCategory(definitionCategory, includeHidden, definitions).ToList();
			}

			return results;
		}

		private static IEnumerable<Definition> GetForCategory(string definitionCategory, bool includeHidden, List<Definition> definitions)
		{
			foreach (var definition in definitions)
			{
				if (definition.Category != definitionCategory)
				{
					continue;
				}

				if (definition.IsHidden && !includeHidden)
				{
					continue;
				}

				yield return definition;
			}
		}

        /// <summary>
        /// Gets a list of defs from the list of defnums and passed-in cat.
        /// </summary>
        public static List<Definition> GetDefs(string definitionCategory, List<long> definitionIds) 
			=> GetDefsForCategory(definitionCategory).FindAll(x => definitionIds.Contains(x.Id));

		/// <summary>
		/// Get one def from Long. 
		/// Returns null if not found. 
		/// Only used for very limited situations.
		/// Other Get functions tend to be much more useful since they don't return null.
		/// There is also BIG potential for silent bugs if you use this.ItemOrder instead of GetOrder().
		/// </summary>
		public static Definition GetDef(string definitionCategory, long definitionId, List<Definition> definitions = null)
		{
			definitions ??= GetDefsForCategory(definitionCategory);

			return definitions.FirstOrDefault(x => x.Id == definitionId);
		}

		/// <summary>
		/// Returns the Def with the exact itemName passed in. 
		/// Returns null if not found.
		/// If itemName is blank, then it returns the first def in the category.
		/// </summary>
		public static Definition GetDefByExactName(string definitionCategory, string itemName) 
			=> GetDef(definitionCategory, GetByExactName(definitionCategory, itemName));

		/// <summary>
		/// Returns 0 if it can't find the named def. 
		/// If the name is blank, then it returns the first def in the category.
		/// </summary>
		public static long GetByExactName(string definitionCategory, string itemName)
		{
			var definitions = GetDefsForCategory(definitionCategory);
			if (string.IsNullOrEmpty(itemName) && definitions.Count > 0)
			{
				return definitions[0].Id; // return the first one in the list
			}

            var definition = definitions.FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.InvariantCultureIgnoreCase));
			if (definition == null)
			{
				return 0;
			}

			return definition.Id;
		}

		/// <summary>
		/// Returns the named def. If it can't find the name, then it returns the first def in the category.
		/// </summary>
		public static long GetByExactNameNeverZero(string definitionCategory, string itemName)
		{
			var definitions = GetDefsForCategory(definitionCategory);

			// We have been getting bug submissions from customers where listDefs will be null (e.g. DefinitionCategory.ProviderSpecialties cat itemName "General")
			// Therefore, we should check for null or and entirely empty category first before looking for a match.
			if (definitions == null || definitions.Count == 0)
			{
				// There are no defs for the category passed in, create one because this method should never return zero.
				var definition = new Definition
				{
					Category = definitionCategory,
					SortOrder = 0,
					Name = itemName
				};

				Insert(definition);
				RefreshCache();

				return definition.Id;
			}
			else
			{
				// From this point on, we know our list of definitions contains at least one def.
				var definition = definitions.FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.InvariantCultureIgnoreCase));
				if (definition != null)
				{
					return definition.Id;
				}

				// Couldn't find a match so return the first definition from our list as a last resort.
				return definitions[0].Id;
			}
		}

		/// <summary>
		/// Returns defs from the AdjTypes that contain '+' in the ItemValue column.
		/// </summary>
		public static List<Definition> GetPositiveAdjTypes() 
			=> GetDefsForCategory(DefinitionCategory.AdjTypes, true).FindAll(x => x.Value == "+");

		/// <summary>
		/// Returns defs from the AdjTypes that contain '-' in the ItemValue column.
		/// </summary>
		public static List<Definition> GetNegativeAdjTypes() 
			=> GetDefsForCategory(DefinitionCategory.AdjTypes, true).FindAll(x => x.Value == "-");

		/// <summary>
		/// Returns defs from the AdjTypes that contain 'dp' in the ItemValue column.
		/// </summary>
		public static List<Definition> GetDiscountPlanAdjTypes() 
			=> GetDefsForCategory(DefinitionCategory.AdjTypes, true).FindAll(x => x.Value == "dp");

		/// <summary>
		/// Returns a DefNum for the special image category specified. Returns 0 if no match found.
		/// </summary>
		public static long GetImageCat(ImageCategorySpecial imageCategory)
		{
            var definition = GetDefsForCategory(DefinitionCategory.ImageCats, true).FirstOrDefault(x => x.Value.Contains(imageCategory.ToString()));

			return definition?.Id ?? 0;
		}

		/// <summary>
		/// Gets the order of the def within Short or -1 if not found.
		/// </summary>
		public static int GetOrder(string definitionCategory, long definitionId) 
			=> GetDefsForCategory(definitionCategory, true).FindIndex(x => x.Id == definitionId);

		public static string GetValue(string definitionCategory, long definitionId)
		{
            var definition = GetDefsForCategory(definitionCategory).LastOrDefault(x => x.Id == definitionId);

			return definition?.Value ?? "";
		}

		/// <summary>
		/// Returns Color.White if no match found. 
		/// Pass in a list of defs to save from making deep copies of the cache if you are going to call this method repeatedly.
		/// </summary>
		public static Color GetColor(string definitionCategory, long definitionId, List<Definition> definitions = null)
		{
			definitions ??= GetDefsForCategory(definitionCategory);

            var definition = definitions.LastOrDefault(x => x.Id == definitionId);

			return definition?.Color ?? Color.White;
		}

		public static bool GetHidden(string definitionCategory, long definitionId) 
			=> GetDef(definitionCategory, definitionId)?.IsHidden ?? false;

		/// <summary>
		/// Pass in a list of all defs to save from making deep copies of the cache if you are going to call this method repeatedly.
		/// </summary>
		public static string GetName(string definitionCategory, long definitionId, List<Definition> definitions = null)
		{
			if (definitionId == 0)
			{
				return "";
			}

            var definition = GetDef(definitionCategory, definitionId, definitions);

			return definition?.Name ?? "";
		}

		/// <summary>
		/// Gets the name of the def without requiring a category. 
		/// If it's a hidden def, it tacks (hidden) onto the end. 
		/// Use for single defs, not in a loop situation.
		/// </summary>
		public static string GetNameWithHidden(long definitionId)
		{
			if (definitionId == 0)
			{
				return "";
			}

			foreach (string definitionCategory in DefinitionCategory.All)
			{
                Definition definition = GetDef(definitionCategory, definitionId);
				if (definition == null)
				{
					continue;
				}

				if (definition.IsHidden)
				{
					return definition.Name + " (hidden)";
				}

				return definition.Name;
			}

			return "";
		}

		public static List<Definition> GetDefsNoCache(string definitionCategory) 
			=> SelectMany("SELECT * FROM `definitions` WHERE `category` = @category",
				new MySqlParameter("category", definitionCategory)).ToList();

		/// <summary>
		/// Returns definitions that are associated to the DefinitionCategory. fKey, and defLinkType passed in.
		/// </summary>
		public static List<Definition> GetDefsByDefLinkFKey(string definitionCategory, long fKey, DefLinkType defLinkType)
		{
			var definitionLinks = DefLinks.GetDefLinksByType(defLinkType).FindAll(x => x.FKey == fKey);

			return GetDefs(definitionCategory, definitionLinks.Select(x => x.DefinitionId).Distinct().ToList());
		}

		/// <summary>
		/// Throws an exception if there are no definitions in the category provided.
		/// This is to preserve old behavior.
		/// </summary>
		public static Definition GetFirstForCategory(string definitionCategory, bool isShort = false)
		{
			return GetDefsForCategory(definitionCategory, isShort).First();
		}

		public static long Insert(Definition definition) 
			=> ExecuteInsert(definition);
		
		public static void Update(Definition definition) 
			=> ExecuteUpdate(definition);

		public static void Save(Definition definition)
        {
			if (definition.Id == 0) Insert(definition);
            else
            {
				Update(definition);
            }
        }

		/// <summary>
		/// Hides the specified <paramref name="definition"/> definition.
		/// </summary>
		/// <param name="definition">The definition to hide.</param>
		public static void Hide(Definition definition)
		{
			if (false == definition.IsHidden)
			{
				definition.IsHidden = true;

				Update(definition);
			}
		}

		///<summary>CAUTION.  This does not perform all validations.  Throws exceptions.</summary>
		public static void Delete(Definition definition)
		{
			string command;

			List<string> countCommands = new List<string>();
			switch (definition.Category)
			{
				case DefinitionCategory.ClaimCustomTracking:
					countCommands.Add("SELECT COUNT(*) FROM securitylog WHERE DefNum=" + definition.Id);
					countCommands.Add("SELECT COUNT(*) FROM claim WHERE CustomTracking=" + definition.Id);
					break;

				case DefinitionCategory.ClaimErrorCode:
					countCommands.Add("SELECT COUNT(*) FROM claimtracking WHERE TrackingErrorDefNum=" + definition.Id);
					break;

				case DefinitionCategory.InsurancePaymentType:
					countCommands.Add("SELECT COUNT(*) FROM claimpayment WHERE PayType=" +definition.Id);
					break;

				case DefinitionCategory.SupplyCats:
					countCommands.Add("SELECT COUNT(*) FROM supply WHERE Category=" + definition.Id);
					break;

				case DefinitionCategory.AccountQuickCharge:
					break;//Users can delete AcctProcQuickCharge entries.  Nothing has an FKey to a AcctProcQuickCharge Def so no need to check anything.
				
				case DefinitionCategory.AutoNoteCats:
					AutoNotes.RemoveFromCategory(definition.Id);//set any autonotes assinged to this category to 0 (unassigned), user already warned about this
					countCommands.Add("SELECT COUNT(*) FROM autonote WHERE Category=" + POut.Long(definition.Id));//just in case update failed or concurrency issue
					break;

				case DefinitionCategory.WebSchedNewPatApptTypes:
					//Do not let the user delete the last WebSchedNewPatApptTypes definition.  Must be at least one.
					command = "SELECT COUNT(*) FROM definition WHERE Category='" + DefinitionCategory.WebSchedNewPatApptTypes + "'";
					if (PIn.Int(Database.ExecuteString(command), false) <= 1)
					{
						throw new ApplicationException("NOT Allowed to delete the last def of this type.");
					}
					break;

				default:
					throw new ApplicationException("NOT Allowed to delete this type of def.");
			}

			for (int i = 0; i < countCommands.Count; i++)
			{
				if (Database.ExecuteLong(countCommands[i]) != 0)
				{
					throw new ApplicationException("Def is in use.  Not allowed to delete.");
				}
			}

			ExecuteDelete(definition);

			// Fix the sort orders of the definitions following this one...
			Database.ExecuteNonQuery(
				"UPDATE `definitions` SET `sort_order` = `sort_order` - 1 WHERE `category` = @category AND `sort_order` > " + definition.SortOrder,
					new MySqlParameter("category", definition.Category));
		}

		/// <summary>
		/// Returns true if the passed-in def is deprecated. 
		/// This method must be updated whenever another def is deprecated.
		/// </summary>
		public static bool IsDefDeprecated(Definition definition)
		{
			if (definition.Category == DefinitionCategory.AccountColors && definition.Name == "Received Pre-Auth")
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns true if the category needs at least one of its definitions to be unhidden.
		/// The list of categories in the if statement of this method should only include those that can be hidden.
		/// </summary>
		public static bool NeedOneUnhidden(string category)
		{
			if (//Definitions of categories that are commented out can be hidden and we want to allow users to hide ALL of them if they so desire
				category == DefinitionCategory.AdjTypes
				|| category == DefinitionCategory.ApptConfirmed
				|| category == DefinitionCategory.ApptProcsQuickAdd
				|| category == DefinitionCategory.BillingTypes
				|| category == DefinitionCategory.BlockoutTypes
				|| category == DefinitionCategory.ClaimPaymentGroups
				|| category == DefinitionCategory.ClaimPaymentTracking
				|| category == DefinitionCategory.CommLogTypes
				|| category == DefinitionCategory.ContactCategories
				|| category == DefinitionCategory.Diagnosis
				|| category == DefinitionCategory.ImageCats
				|| category == DefinitionCategory.InsuranceFilingCodeGroup
				|| category == DefinitionCategory.LetterMergeCats
				|| category == DefinitionCategory.PaymentTypes
				|| category == DefinitionCategory.PaySplitUnearnedType
				|| category == DefinitionCategory.ProcButtonCats
				|| category == DefinitionCategory.ProcCodeCats
				|| category == DefinitionCategory.Prognosis
				|| category == DefinitionCategory.ProviderSpecialties
				|| category == DefinitionCategory.RecallUnschedStatus
				|| category == DefinitionCategory.TaskPriorities
				|| category == DefinitionCategory.TxPriorities)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns true if there are any entries in definition that do not have a Category named "General".  
		/// Returning false means the user has ProcButtonCategory customizations.
		/// </summary>
		public static bool HasCustomCategories()
		{
			var definitions = GetDefsForCategory(DefinitionCategory.ProcButtonCats);

			foreach (var definition in definitions)
			{
				if (!definition.Name.Equals("General", StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Determines whether the specified <paramref name="definition"/> is in use by 
		/// any of the given preferences.
		/// </summary>
		/// <param name="definition">The definition.</param>
		/// <param name="preferenceNames">The names of the preferences to check.</param>
		/// <returns>True if the definition is in use; otherwise, false.</returns>
		private static bool IsInUseByPreference(Definition definition, params string[] preferenceNames)
        {
			if (definition == null || definition.Id == 0) return false;

			foreach (var preferenceName in preferenceNames)
            {
				if (Prefs.GetLong(preferenceName) == definition.Id)
                {
					return true;
                }
            }

			return false;
        }

		/// <summary>
		/// Returns true if this definition is in use within the program. Consider enhancing this method if you add a definition category.
		/// Does not check patient billing type or provider specialty since those are handled in their S-class.
		/// </summary>
		public static bool IsDefinitionInUse(Definition definition)
		{
			var countCommands = new List<string>();

			switch (definition.Category)
			{
				case DefinitionCategory.AdjTypes:
					if (IsInUseByPreference(definition,
						PrefName.BrokenAppointmentAdjustmentType,
						PrefName.TreatPlanDiscountAdjustmentType,
						PrefName.BillingChargeAdjustmentType,
						PrefName.FinanceChargeAdjustmentType,
						PrefName.SalesTaxAdjustmentType))
					{
						return true;
					}

					countCommands.Add("SELECT COUNT(*) FROM adjustment WHERE AdjType=" + definition.Id);
					break;

				case DefinitionCategory.ApptConfirmed:
					if (IsInUseByPreference(definition,
						PrefName.AppointmentTimeArrivedTrigger,
						PrefName.AppointmentTimeSeatedTrigger,
						PrefName.AppointmentTimeDismissedTrigger,
						PrefName.WebSchedNewPatConfirmStatus,
						PrefName.WebSchedRecallConfirmStatus,
						PrefName.ApptEConfirmStatusSent,
						PrefName.ApptEConfirmStatusAccepted,
						PrefName.ApptEConfirmStatusDeclined,
						PrefName.ApptEConfirmStatusSendFailed))
					{
						return true;
					}

					countCommands.Add("SELECT COUNT(*) FROM appointment WHERE Confirmed=" + POut.Long(definition.Id));
					break;

				case DefinitionCategory.AutoNoteCats:
					countCommands.Add("SELECT COUNT(*) FROM autonote WHERE Category=" + POut.Long(definition.Id));
					break;

				case DefinitionCategory.BillingTypes:
					if (IsInUseByPreference(definition, PrefName.PracticeDefaultBillType))
                    {
						return true;
                    }
					break;

				case DefinitionCategory.ContactCategories:
					countCommands.Add("SELECT COUNT(*) FROM contact WHERE Category=" + POut.Long(definition.Id));
					break;

				case DefinitionCategory.Diagnosis:
					countCommands.Add("SELECT COUNT(*) FROM procedurelog WHERE Dx=" + POut.Long(definition.Id));
					break;

				case DefinitionCategory.ImageCats:
					countCommands.Add("SELECT COUNT(*) FROM document WHERE DocCategory=" + POut.Long(definition.Id));
					countCommands.Add("SELECT COUNT(*) FROM sheetfielddef WHERE FieldType=" + POut.Int((int)SheetFieldType.PatImage) + " AND FieldName=" + POut.Long(definition.Id));
					break;

				case DefinitionCategory.PaymentTypes:
					if (IsInUseByPreference(definition, 
						PrefName.RecurringChargesPayTypeCC, 
						PrefName.AccountingCashPaymentType))
					{
						return true;
					}
					countCommands.Add("SELECT COUNT(*) FROM payment WHERE PayType=" + POut.Long(definition.Id));
					break;

				case DefinitionCategory.PaySplitUnearnedType:
					if (IsInUseByPreference(definition, PrefName.TpUnearnedType))
					{
						return true;
					}
					countCommands.Add("SELECT COUNT(*) FROM paysplit WHERE UnearnedType=" + POut.Long(definition.Id));
					break;

				case DefinitionCategory.Prognosis:
					countCommands.Add("SELECT COUNT(*) FROM procedurelog WHERE Prognosis=" + POut.Long(definition.Id));
					break;

				case DefinitionCategory.RecallUnschedStatus:
					if (IsInUseByPreference(definition,
						PrefName.RecallStatusMailed,
						PrefName.RecallStatusTexted,
						PrefName.RecallStatusEmailed,
						PrefName.RecallStatusEmailedTexted))
					{
						return true;
					}

					countCommands.Add("SELECT COUNT(*) FROM appointment WHERE UnschedStatus=" + POut.Long(definition.Id));
					countCommands.Add("SELECT COUNT(*) FROM recall WHERE RecallStatus=" + POut.Long(definition.Id));
					break;

				case DefinitionCategory.TaskPriorities:
					countCommands.Add("SELECT COUNT(*) FROM task WHERE PriorityDefNum=" + POut.Long(definition.Id));
					break;

				case DefinitionCategory.TxPriorities:
					countCommands.Add("SELECT COUNT(*) FROM procedurelog WHERE Priority=" + POut.Long(definition.Id));
					break;

				case DefinitionCategory.CommLogTypes:
					countCommands.Add("SELECT COUNT(*) FROM commlog WHERE CommType=" + POut.Long(definition.Id));
					break;

				default:
					break;
			}

			return countCommands.Any(x => Database.ExecuteLong(x) != 0);
		}

		///<summary>Merges old document DocCategory(FK DefNum) into the new DocCategory(FK DefNum).</summary>
		public static void MergeImageCatDefNums(long defNumFrom, long defNumTo)
		{
			string command = "UPDATE document"
			+ " SET DocCategory=" + POut.Long(defNumTo)
			+ " WHERE DocCategory=" + POut.Long(defNumFrom);
			Database.ExecuteNonQuery(command);
			command = "UPDATE mount"
			+ " SET DocCategory=" + POut.Long(defNumTo)
			+ " WHERE DocCategory=" + POut.Long(defNumFrom);
			Database.ExecuteNonQuery(command);
			command = "UPDATE lettermerge"
			+ " SET Category=" + POut.Long(defNumTo)
			+ " WHERE Category=" + POut.Long(defNumFrom);
			Database.ExecuteNonQuery(command);
			command = "UPDATE sheetfielddef"
			+ " SET FieldName='" + POut.Long(defNumTo) + "'"
			+ " WHERE FieldType=" + POut.Int((int)SheetFieldType.PatImage) + " AND FieldName='" + POut.Long(defNumFrom) + "'";
			Database.ExecuteNonQuery(command);
			command = "UPDATE sheetfield"
			+ " SET FieldName='" + POut.Long(defNumTo) + "'"
			+ " WHERE FieldType=" + POut.Int((int)SheetFieldType.PatImage) + " AND FieldName='" + POut.Long(defNumFrom) + "'";
			Database.ExecuteNonQuery(command);
			long progNum = Programs.GetProgramNum(ProgramName.XVWeb);
			if (progNum != 0)
			{
				command = "UPDATE programproperty"
				+ " SET PropertyValue='" + POut.Long(defNumTo) + "'"
				+ " WHERE ProgramNum=" + POut.Long(progNum) + " AND PropertyDesc='ImageCategory' AND PropertyValue='" + POut.Long(defNumFrom) + "'";
				Database.ExecuteNonQuery(command);
			}
		}



		public static List<Definition> GetDefsForCategory(string definitionCategory, bool isShort = false)
			=> cache.Find(definition => definition.Category == definitionCategory);



		/// <summary>
		/// Gets all definitions.
		/// </summary>
		/// <returns></returns>
		public static List<Definition> GetAll()
			=> cache.GetAll();

		/// <summary>
		/// Gets all visible definitions in the specified category.
		/// </summary>
		/// <param name="definitionCategory">The category.</param>
		/// <returns>All definitions in the specified category.</returns>
		public static List<Definition> GetByCategory(string definitionCategory)
			=> GetByCategory(definitionCategory, false);

		/// <summary>
		/// Gets all definitions in the specified category.
		/// </summary>
		/// <param name="definitionCategory">The category.</param>
		/// <param name="includeHidden">Value indicating whether to include hidden definitions.</param>
		/// <returns>All definitions in the specified category.</returns>
		public static List<Definition> GetByCategory(string definitionCategory, bool includeHidden)
			=> includeHidden ? 
				cache.Find(definition => definition.Category == definitionCategory) :
				cache.Find(definition => definition.Category == definitionCategory && definition.IsHidden == false);




		public static List<Definition> GetCatList(string definitionCategory)
			=> SelectMany("SELECT * FROM `definitions` WHERE `category` = @category ORDER BY `sort_order`",
				new MySqlParameter("category", definitionCategory)).ToList();


		/// <summary>
		/// Refreshes the local definitions cache.
		/// </summary>
		public static void RefreshCache() 
			=> cache.Refresh();
	}

	public enum ImageCategorySpecial
	{
		///<summary>Show in Chart module.</summary>
		X,

		///<summary>Show in patient forms.</summary>
		F,

		/// <summary>Show in patient portal.</summary>
		L,

		///<summary>Patient picture (only one)</summary>
		P,

		///<summary>Statements (only one)</summary>
		S,

		///<summary>Graphical tooth charts and perio charts (only one)</summary>
		T,

		/// <summary>Treatment plan (only one)</summary>
		R,

		/// <summary>Expanded by default.</summary>
		E
	}
}
