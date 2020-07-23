using DataConnectionBase;
using Imedisoft.Data;
using Imedisoft.Data.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace OpenDentBusiness
{
	public partial class AutoCodes
	{
		[CacheGroup(nameof(InvalidType.AutoCodes))]
		private class AutoCodeCache : DictionaryCache<long, AutoCode>
		{
			protected override IEnumerable<AutoCode> Load()
				=> SelectMany("SELECT * from autocode");

			protected override long GetKey(AutoCode item)
				=> item.Id;
		}

		private static readonly AutoCodeCache cache = new AutoCodeCache();

		public static List<AutoCode> GetListDeep(bool isShort = false)
		{
			if (isShort)
            {
				return cache.Find(x => !x.IsHidden).ToList();
            }

			return cache.GetAll();
		}

		public static AutoCode GetOne(long codeNum)
		{
			return cache.Find(codeNum);
		}

		public static bool GetContainsKey(long codeNum)
		{
			return cache.Contains(codeNum);
		}

		public static int GetCount()
		{
			return cache.Count();
		}

		public static void RefreshCache()
		{
			cache.Refresh();
		}

		///<summary>Surround with try/catch.  Currently only called from FormAutoCode and FormAutoCodeEdit.</summary>
		public static void Delete2(AutoCode autoCodeCur)
		{
			string strInUse = "";
			List<ProcButton> listProcButtons = ProcButtons.GetDeepCopy();
			List<ProcButtonItem> listProcButtonItems = ProcButtonItems.GetDeepCopy();
			for (int i = 0; i < listProcButtons.Count; i++)
			{
				for (int j = 0; j < listProcButtonItems.Count; j++)
				{
					if (listProcButtonItems[j].ProcButtonNum == listProcButtons[i].ProcButtonNum
						&& listProcButtonItems[j].AutoCodeNum == autoCodeCur.Id)
					{
						if (strInUse != "")
						{
							strInUse += "; ";
						}
						//Add the procedure button description to the list for display.
						strInUse += listProcButtons[i].Description;
						break;//Button already added to the description, check the other buttons in the list.
					}
				}
			}

			if (strInUse != "")
			{
				throw new ApplicationException(
					"Not allowed to delete autocode because it is in use.  Procedure buttons using this autocode include " + strInUse);
			}

			Delete(autoCodeCur.Id);
		}

		///<summary>Used in ProcButtons.SetToDefault.  Returns 0 if the given autocode does not exist.</summary>
		public static long GetNumFromDescript(string descript)
		{
			AutoCode autoCode = cache.FirstOrDefault(x => !x.IsHidden && x.Description == descript);
			return (autoCode == null) ? 0 : autoCode.Id;
		}

		//------

		///<summary>Deletes all current autocodes and then adds the default autocodes.  Procedure codes must have already been entered or they cannot be added as an autocode.</summary>
		public static void SetToDefault()
		{

			string command = "DELETE FROM autocode";
			Database.ExecuteNonQuery(command);
			command = "DELETE FROM autocodecond";
			Database.ExecuteNonQuery(command);
			command = "DELETE FROM autocodeitem";
			Database.ExecuteNonQuery(command);
			if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{//Canadian. en-CA or fr-CA
				SetToDefaultCanada();
				return;
			}
			SetToDefaultMySQL();
		}

		private static void SetToDefaultMySQL()
		{
			//No need to check RemotingRole; Private method.
			long autoCodeNum;
			long autoCodeItemNum;
			//Amalgam-------------------------------------------------------------------------------------------------------
			string command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Amalgam',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//1Surf
			if (ProcedureCodes.IsValidCode("D2140"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2140") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
				+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2Surf
			if (ProcedureCodes.IsValidCode("D2150"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2150") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3Surf
			if (ProcedureCodes.IsValidCode("D2160"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2160") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4Surf
			if (ProcedureCodes.IsValidCode("D2161"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2161") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5Surf
			if (ProcedureCodes.IsValidCode("D2161"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2161") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Composite-------------------------------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Composite',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//1SurfAnt
			if (ProcedureCodes.IsValidCode("D2330"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2330") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfAnt
			if (ProcedureCodes.IsValidCode("D2331"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2331") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfAnt
			if (ProcedureCodes.IsValidCode("D2332"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2332") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfAnt
			if (ProcedureCodes.IsValidCode("D2335"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2335") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfAnt
			if (ProcedureCodes.IsValidCode("D2335"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2335") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Posterior Composite----------------------------------------------------------------------------------------------
			//1SurfPost
			if (ProcedureCodes.IsValidCode("D2391"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2391") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Posterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPost
			if (ProcedureCodes.IsValidCode("D2392"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2392") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Posterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPost
			if (ProcedureCodes.IsValidCode("D2393"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2393") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Posterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPost
			if (ProcedureCodes.IsValidCode("D2394"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2394") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Posterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPost
			if (ProcedureCodes.IsValidCode("D2394"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2394") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Posterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Root Canal-------------------------------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Root Canal',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//Ant
			if (ProcedureCodes.IsValidCode("D3310"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D3310") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Premolar
			if (ProcedureCodes.IsValidCode("D3320"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D3320") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Molar
			if (ProcedureCodes.IsValidCode("D3330"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D3330") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
			}
			//PFM Bridge-------------------------------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('PFM Bridge',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//Pontic
			if (ProcedureCodes.IsValidCode("D6242"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D6242") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Pontic) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Retainer
			if (ProcedureCodes.IsValidCode("D6752"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D6752") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Retainer) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Ceramic Bridge-------------------------------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Ceramic Bridge',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//Pontic
			if (ProcedureCodes.IsValidCode("D6245"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D6245") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Pontic) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Retainer
			if (ProcedureCodes.IsValidCode("D6740"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D6740") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Retainer) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Denture-------------------------------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Denture',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//Max
			if (ProcedureCodes.IsValidCode("D5110"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D5110") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Maxillary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Mand
			if (ProcedureCodes.IsValidCode("D5120"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D5120") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Mandibular) + ")";
				Database.ExecuteNonQuery(command);
			}
			//BU/P&C-------------------------------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('BU/P&C',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//BU
			if (ProcedureCodes.IsValidCode("D2950"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2950") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Posterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//P&C
			if (ProcedureCodes.IsValidCode("D2954"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D2954") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Root Canal Retreat--------------------------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('RCT Retreat',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//Ant
			if (ProcedureCodes.IsValidCode("D3346"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D3346") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Premolar
			if (ProcedureCodes.IsValidCode("D3347"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D3347") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Molar
			if (ProcedureCodes.IsValidCode("D3348"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("D3348") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
			}
		}

		///<summary>Deletes all current autocodes and then adds the default autocodes.  Procedure codes must have already been entered or they cannot be added as an autocode.</summary>
		public static void SetToDefaultCanada()
		{

			string command;
			long autoCodeNum;
			long autoCodeItemNum;
			//Amalgam-Bonded--------------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Amalgam-Bonded',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//1SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("21121"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21121") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//1SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("21121"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21121") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("21122"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21122") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("21122"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21122") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("21123"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21123") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("21123"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21123") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("21124"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21124") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("21124"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21124") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("21125"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21125") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("21125"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21125") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//1SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("21231"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21231") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//1SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("21231"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21231") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("21232"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21232") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("21232"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21232") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("21233"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21233") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("21233"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21233") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("21234"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21234") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("21234"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21234") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("21235"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21235") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("21235"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21235") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//1SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("21241"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21241") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("21242"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21242") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("21243"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21243") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("21244"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21244") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("21245"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21245") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Amalgam Non-Bonded----------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Amalgam Non-Bonded',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//1SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("21111"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21111") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//1SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("21111"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21111") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("21112"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21112") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("21112"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21112") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("21113"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21113") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("21113"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21113") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("21114"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21114") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("21114"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21114") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("21115"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21115") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("21115"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21115") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//1SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("21211"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21211") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//1SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("21211"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21211") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("21212"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21212") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("21212"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21212") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("21213"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21213") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("21213"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21213") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("21214"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21214") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("21214"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21214") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("21215"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21215") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("21215"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21215") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//1SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("21221"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21221") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("21222"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21222") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("21223"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21223") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("21224"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21224") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("21225"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("21225") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Composite-------------------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Composite',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//1SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("23111"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23111") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("23112"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23112") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("23113"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23113") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("23114"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23114") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPermanentAnterior
			if (ProcedureCodes.IsValidCode("23115"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23115") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//1SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("23311"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23311") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("23312"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23312") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("23313"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23313") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("23314"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23314") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPermanentPremolar
			if (ProcedureCodes.IsValidCode("23315"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23315") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//1SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("23321"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23321") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("23322"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23322") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("23323"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23323") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("23324"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23324") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPermanentMolar
			if (ProcedureCodes.IsValidCode("23325"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23325") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Permanent) + ")";
				Database.ExecuteNonQuery(command);
			}
			//1SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("23411"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23411") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("23412"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23412") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("23413"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23413") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("23414"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23414") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPrimaryAnterior
			if (ProcedureCodes.IsValidCode("23415"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23415") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//1SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("23511"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23511") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.One_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//2SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("23512"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23512") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Two_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//3SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("23513"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23513") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Three_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//4SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("23514"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23514") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Four_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//5SurfPrimaryMolar
			if (ProcedureCodes.IsValidCode("23515"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("23515") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Five_Surf) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Primary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Root Canal------------------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Root Canal',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//Anterior
			if (ProcedureCodes.IsValidCode("33111"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("33111") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Anterior) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Premolar
			if (ProcedureCodes.IsValidCode("33121"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("33121") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Premolar) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Molar
			if (ProcedureCodes.IsValidCode("33131"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("33131") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Molar) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Denture---------------------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Denture',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//Maxillary
			if (ProcedureCodes.IsValidCode("51101"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("51101") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Maxillary) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Mandibular
			if (ProcedureCodes.IsValidCode("51302"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("51302") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Mandibular) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Bridge----------------------------------------------------------------------------------------------
			command = "INSERT INTO autocode (Description,IsHidden,LessIntrusive) VALUES ('Bridge',0,0)";
			autoCodeNum = Database.ExecuteInsert(command);
			//Pontic
			if (ProcedureCodes.IsValidCode("62501"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("62501") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Pontic) + ")";
				Database.ExecuteNonQuery(command);
			}
			//Retainer
			if (ProcedureCodes.IsValidCode("67211"))
			{
				command = "INSERT INTO autocodeitem (AutoCodeNum,CodeNum) VALUES (" + POut.Long(autoCodeNum) + ","
					+ ProcedureCodes.GetCodeNum("67211") + ")";
				autoCodeItemNum = Database.ExecuteInsert(command);
				command = "INSERT INTO autocodecond (AutoCodeItemNum,Cond) VALUES (" + POut.Long(autoCodeItemNum) + ","
					+ POut.Long((int)AutoCondition.Retainer) + ")";
				Database.ExecuteNonQuery(command);
			}
		}


	}
}
