using CodeBase;
using Imedisoft.Data;
using Imedisoft.Data.Cache;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OpenDentBusiness
{
    public class Carriers
	{
		[CacheGroup(nameof(InvalidType.Carriers))]
        private class CarrierCache : ListCache<Carrier>
        {
            protected override IEnumerable<Carrier> Load() 
				=> Crud.CarrierCrud.SelectMany("SELECT * FROM `carriers` ORDER BY `name`");
        }

        private static readonly CarrierCache cache = new CarrierCache();

		public static bool GetContainsKey(long carrierId) 
			=> cache.Any(carrier => carrier.Id == carrierId);

		public static Carrier GetOne(long carrierId) 
			=> cache.FirstOrDefault(carrier => carrier.Id == carrierId);

		public static Carrier GetFirstOrDefault(Predicate<Carrier> predicate) 
			=> cache.FirstOrDefault(predicate);

		public static List<Carrier> GetWhere(Predicate<Carrier> predicate) 
			=> cache.Find(predicate);

		public static void RefreshCache() 
			=> cache.Refresh();



		public class CarrierSummary
        {
			public long CarrierId;
			public string AddressLine1;
			public string AddressLine2;
			public string Name;
			public string City;
			public string State;
			public string Zip;
			public string ElectronicId;
			public string Phone;
			public bool IsCDA;
			public bool IsHidden;
			public long InsurancePlans;
		}

		private static CarrierSummary CarrierSummaryFromReader(MySqlDataReader dataReader)
        {
			return new CarrierSummary
			{
				CarrierId = (long)dataReader["id"],
				AddressLine1 = (string)dataReader["address_line1"],
				AddressLine2 = (string)dataReader["address_line2"],
				Name = (string)dataReader["name"],
				City = (string)dataReader["city"],
				State = (string)dataReader["state"],
				Zip = (string)dataReader["zip"],
				ElectronicId = (string)dataReader["electronic_id"],
				Phone = (string)dataReader["phone"],
				IsCDA = Convert.ToInt32(dataReader["is_cda"]) == 1,
				IsHidden = Convert.ToInt32(dataReader["is_hidden"]) == 1,
				InsurancePlans = (long)dataReader["ins_plans"]
			};
        }

		public static IEnumerable<CarrierSummary> GetBigList(bool isCanadian, bool showHidden, string carrierName, string carrierPhone)
		{
			if (!string.IsNullOrEmpty(carrierName))
			{
				carrierName = '%' + carrierName + '%';
			}
			else
			{
				carrierName = "%";
			}

			var digits = "";
			foreach (var c in carrierPhone)
            {
				if (c >= '0' && c <= '9')
                {
					digits += c;
                }
            }

			string regexp = "";
			for (int i = 0; i < digits.Length; i++)
			{
				if (i != 0)
				{
					regexp += "[^0-9]*"; // zero or more intervening digits that are not numbers
				}

				regexp += digits[i];
			}

            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("name", carrierName)
            };

            var command =
				"SELECT " +
					"c.`id`, `address_line1`, `address_line2`, c.`name`, " +
					"`city`, `state`, `zip`, `electronic_id`, `phone`, " +
					"`is_cda`, `is_hidden`, " +
					"COUNT(ip.`PlanNum`) AS `ins_plans` " +
				"FROM `carriers` c " +
				"LEFT JOIN `insplan` ip ON ip.`CarrierNum` = c.id " +
				"WHERE c.`name` LIKE @name";

			if (regexp != "")
			{
				command += " AND `phone` REGEXP @phone";

				parameters.Add(new MySqlParameter("phone", regexp));
			}

			if (isCanadian)
			{
				command += " AND `is_cda` = 1";
			}

			if (!showHidden)
			{
				command += " AND c.`is_hidden` = 0";
			}

			command += " GROUP BY c.`id` ORDER BY c.`name`";

			return Database.SelectMany(command, CarrierSummaryFromReader, parameters.ToArray());
		}


		/// <summary>
		/// Validates the specified <paramref name="carrier"/>.
		/// </summary>
		/// <param name="carrier">The carrier to validate.</param>
		/// <exception cref="Exception">If the carrier is not valid.</exception>
		private static void Validate(Carrier carrier)
        {
			if (carrier.IsCDA)
			{
				if (string.IsNullOrEmpty(carrier.ElectronicId))
				{
					throw new Exception("Carrier Identification Number required.");
				}

				if (!Regex.IsMatch(carrier.ElectronicId, "^[0-9]{6}$"))
				{
					throw new Exception("Carrier Identification Number must be exactly 6 numbers.");
				}
			}

			if (carrier.Id == 0) return;

			(string electId, bool isCda) = Database.SelectOne(
				"SELECT `elect_id`, `is_cda` FROM `carriers` WHERE `id` = " + carrier.Id,
					dataReader => ((string)dataReader["elect_id"], Convert.ToInt32(dataReader["is_cda"]) > 0));

			if (isCda && !string.IsNullOrEmpty(electId) && !electId.Equals(carrier.ElectronicId))
			{
				var count = Database.ExecuteLong("SELECT COUNT(*) FROM etrans WHERE CarrierNum= " + carrier.Id + " OR CarrierNum2=" + POut.Long(carrier.Id));
				if (count > 0)
				{
					throw new Exception("Not allowed to change Carrier Identification Number because it's in use in the claim history.");
				}
			}
		}

		public static void Update(Carrier carrier)
		{
			var oldCarrier = Crud.CarrierCrud.SelectOne(carrier.Id);

			Update(carrier, oldCarrier, Security.CurrentUser.Id);
		}

		public static void Update(Carrier carrier, Carrier oldCarrier, long userId)
		{
			Validate(carrier);

			Crud.CarrierCrud.Update(carrier, oldCarrier);

			InsEditLogs.MakeLogEntry(carrier, oldCarrier, InsEditLogType.Carrier, userId);

			if (oldCarrier.Name != carrier.Name)
			{
				SecurityLogs.Write(Permissions.InsPlanChangeCarrierName, 
					"Carrier name changed in Edit Carrier window from " + oldCarrier.Name + " to " + carrier.Name);
			}
		}

		public static long Insert(Carrier carrier, Carrier carrierOld = null, bool useExistingPriKey = false)
		{
			carrier.AddedByUserId = Security.CurrentUser.Id;

			Validate(carrier);

			if (carrierOld == null)
			{
				carrierOld = carrier.Copy();
			}

			Crud.CarrierCrud.Insert(carrier, useExistingPriKey);
			if (carrierOld.Id != 0)
			{
				InsEditLogs.MakeLogEntry(carrier, carrierOld, InsEditLogType.Carrier, carrier.AddedByUserId);
			}
			else
			{
				InsEditLogs.MakeLogEntry(carrier, null, InsEditLogType.Carrier, carrier.AddedByUserId);
			}

			return carrier.Id;
		}

		public static void Delete(Carrier Cur)
		{

			//look for dependencies in insplan table.
			string command = "SELECT insplan.PlanNum,CONCAT(CONCAT(LName,', '),FName) FROM insplan "
				+ "LEFT JOIN inssub ON insplan.PlanNum=inssub.PlanNum "
				+ "LEFT JOIN patient ON inssub.Subscriber=patient.PatNum "
				+ "WHERE insplan.CarrierNum = " + POut.Long(Cur.Id) + " "
				+ "ORDER BY LName,FName";
			DataTable table = Database.ExecuteDataTable(command);
			string strInUse;
			if (table.Rows.Count > 0)
			{
				strInUse = "";//new string[table.Rows.Count];
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if (i > 0)
					{
						strInUse += "; ";
					}
					strInUse += PIn.String(table.Rows[i][1].ToString());
				}
				throw new ApplicationException("Not allowed to delete carrier because it is in use.  Subscribers using this carrier include " + strInUse);
			}
			//look for dependencies in etrans table.
			command = "SELECT DateTimeTrans FROM etrans WHERE CarrierNum=" + POut.Long(Cur.Id)
				+ " OR CarrierNum2=" + POut.Long(Cur.Id);
			table = Database.ExecuteDataTable(command);
			if (table.Rows.Count > 0)
			{
				strInUse = "";
				for (int i = 0; i < table.Rows.Count; i++)
				{
					if (i > 0)
					{
						strInUse += ", ";
					}
					strInUse += PIn.Date(table.Rows[i][0].ToString()).ToShortDateString();
				}
				throw new ApplicationException("Not allowed to delete carrier because it is in use in the etrans table.  Dates of claim sent history include " + strInUse);
			}
			command = "DELETE from carrier WHERE CarrierNum = " + POut.Long(Cur.Id);
			Database.ExecuteNonQuery(command);
			//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
			InsEditLogs.MakeLogEntry(null, Cur, InsEditLogType.Carrier, Security.CurrentUser.Id);
		}

		///<summary>Returns a list of insplans that are dependent on the Cur carrier. Used to display in carrier edit.</summary>
		public static List<string> DependentPlans(Carrier Cur)
		{
			string command = "SELECT CONCAT(CONCAT(LName,', '),FName) FROM patient,insplan,inssub"
				+ " WHERE patient.PatNum=inssub.Subscriber"
				+ " AND insplan.PlanNum=inssub.PlanNum"
				+ " AND insplan.CarrierNum = '" + POut.Long(Cur.Id) + "'"
				+ " ORDER BY LName,FName";
			DataTable table = Database.ExecuteDataTable(command);
			List<string> retStr = new List<string>();
			for (int i = 0; i < table.Rows.Count; i++)
			{
				retStr.Add(PIn.String(table.Rows[i][0].ToString()));
			}
			return retStr;
		}

		///<summary>Gets the name of a carrier based on the carrierNum.  
		///This also refreshes the list if necessary, so it will work even if the list has not been refreshed recently.</summary>
		public static string GetName(long carrierNum)
		{
			string carrierName = "";

			//This is an uncommon pre-check because places throughout the program explicitly did not correctly send out a cache refresh signal.
			if (!GetContainsKey(carrierNum))
			{
				RefreshCache();
			}

			ODException.SwallowAnyException(() =>
			{
				carrierName = GetOne(carrierNum).Name;
			});

			return carrierName;
		}

		public static Carrier GetCarrierDB(long carrierNum)
		{
			return Crud.CarrierCrud.SelectOne("SELECT * FROM carrier WHERE CarrierNum=" + POut.Long(carrierNum));
		}

		///<summary>Gets the specified carrier from Cache. 
		///This also refreshes the list if necessary, so it will work even if the list has not been refreshed recently.</summary>
		public static Carrier GetCarrier(long carrierNum)
		{
			//No need to check RemotingRole; no call to db.
			Carrier retVal = new Carrier() { Name = "" };
			//This is an uncommon pre-check because places throughout the program explicitly did not correctly send out a cache refresh signal.
			if (!GetContainsKey(carrierNum))
			{
				RefreshCache();
			}
			ODException.SwallowAnyException(() =>
			{
				retVal = GetOne(carrierNum);
			});
			//New and empty carrier can only happen if corrupted.
			return retVal;
		}

		///<summary>Primarily used when user clicks OK from the InsPlan window.  Gets a carrierNum from the database based on the other supplied carrier
		///data.  Sets the CarrierNum accordingly. If there is no matching carrier, then a new carrier is created.  The end result is a valid carrierNum
		///to use.</summary>
		public static Carrier GetIdentical(Carrier carrier, Carrier carrierOld = null)
		{
			if (carrier.Name == "")
			{
				return new Carrier();//should probably be null instead
			}

			Carrier retVal = carrier.Copy();
			string command = "SELECT CarrierNum,Phone FROM carrier WHERE "
				+ "CarrierName = '" + POut.String(carrier.Name) + "' "
				+ "AND Address = '" + POut.String(carrier.AddressLine1) + "' "
				+ "AND Address2 = '" + POut.String(carrier.AddressLine2) + "' "
				+ "AND City = '" + POut.String(carrier.City) + "' "
				+ "AND State LIKE '" + POut.String(carrier.State) + "' "//This allows user to remove trailing spaces from the FormInsPlan interface.
				+ "AND Zip = '" + POut.String(carrier.Zip) + "' "
				+ "AND ElectID = '" + POut.String(carrier.ElectronicId) + "' "
				+ "AND NoSendElect = " + POut.Int((int)carrier.NoSendElect);
			DataTable table = Database.ExecuteDataTable(command);
			//Previously carrier.Phone has been given to us after being formatted by ValidPhone in the UI (FormInsPlan).
			//Strip all formatting from the given phone number and the DB phone numbers to compare.
			//The phone in the database could be in a different format if it was imported in an old version.
			string carrierPhoneStripped = carrier.Phone.StripNonDigits();
			for (int i = 0; i < table.Rows.Count; i++)
			{
				string phone = PIn.String(table.Rows[i]["Phone"].ToString());
				if (phone.StripNonDigits() == carrierPhoneStripped)
				{
					//A matching carrier was found in the database, so we will use it.
					retVal.Id = PIn.Long(table.Rows[i][0].ToString());
					return retVal;
				}
			}
			//No match found.  Decide what to do.  Usually add carrier.--------------------------------------------------------------
			//Canada:
			if (CultureInfo.CurrentCulture.Name.EndsWith("CA"))
			{//Canadian. en-CA or fr-CA
				throw new ApplicationException("Carrier not found.");//gives user a chance to add manually.
			}
			//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
			carrier.AddedByUserId = Security.CurrentUser.Id;
			Insert(carrier, carrierOld); //insert function takes care of logging.
			retVal.Id = carrier.Id;
			return retVal;
		}

		///<summary>Returns true if all fields for one carrier match all fields for another carrier.  
		///Returns false if one of the carriers is null or any of the fields don't match.</summary>
		public static bool Compare(Carrier carrierOne, Carrier carrierTwo)
		{
			if (carrierOne == null || carrierTwo == null)
			{
				return false;
			}
			if (carrierOne.AddressLine1 != carrierTwo.AddressLine1
				|| carrierOne.AddressLine2 != carrierTwo.AddressLine2
				|| carrierOne.Name != carrierTwo.Name
				|| carrierOne.City != carrierTwo.City
				|| carrierOne.ElectronicId != carrierTwo.ElectronicId
				|| carrierOne.NoSendElect != carrierTwo.NoSendElect
				|| carrierOne.Phone != carrierTwo.Phone
				|| carrierOne.State != carrierTwo.State
				|| carrierOne.Zip != carrierTwo.Zip)
			{
				return false;
			}
			return true;
		}

		///<summary>Returns an arraylist of Carriers with names similar to the supplied string.  Used in dropdown list from carrier field for faster entry.  There is a small chance that the list will not be completely refreshed when this is run, but it won't really matter if one carrier doesn't show in dropdown.</summary>
		public static List<Carrier> GetSimilarNames(string carrierName)
		{
			carrierName = carrierName.ToUpper();

			return cache.Find(carrier => !carrier.IsHidden && carrier.Name.ToUpper().Contains(carrierName));
		}

		///<summary>Surround with try/catch Combines all the given carriers into one. 
		///The carrier that will be used as the basis of the combination is specified in the pickedCarrier argument. 
		///Updates insplan and etrans, then deletes all the other carriers.</summary>
		public static void Combine(List<long> carrierNums, long pickedCarrierNum)
		{

			if (carrierNums.Count == 1)
			{
				return;//nothing to do
			}
			//remove pickedCarrierNum from the carrierNums list to make the queries easier to construct.
			List<long> carrierNumList = new List<long>();
			for (int i = 0; i < carrierNums.Count; i++)
			{
				if (carrierNums[i] == pickedCarrierNum)
					continue;
				carrierNumList.Add(carrierNums[i]);
			}
			//Now, do the actual combining----------------------------------------------------------------------------------
			for (int i = 0; i < carrierNums.Count; i++)
			{
				if (carrierNums[i] == pickedCarrierNum)
					continue;
				string command = "SELECT * FROM insplan WHERE CarrierNum = " + POut.Long(carrierNums[i]);
				List<InsurancePlan> listInsPlan = Crud.InsPlanCrud.SelectMany(command);
				command = "UPDATE insplan SET CarrierNum = '" + POut.Long(pickedCarrierNum) + "' "
					+ "WHERE CarrierNum = " + POut.Long(carrierNums[i]);
				Database.ExecuteNonQuery(command);
				command = "UPDATE etrans SET CarrierNum='" + POut.Long(pickedCarrierNum) + "' "
					+ "WHERE CarrierNum=" + POut.Long(carrierNums[i]);
				Database.ExecuteNonQuery(command);
				command = "UPDATE etrans SET CarrierNum2='" + POut.Long(pickedCarrierNum) + "' "
					+ "WHERE CarrierNum2=" + POut.Long(carrierNums[i]);
				Database.ExecuteNonQuery(command);
				//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
				listInsPlan.ForEach(x =>
				{ //Log InsPlan CarrierNum change.
					InsEditLogs.MakeLogEntry("CarrierNum", Security.CurrentUser.Id, POut.Long(carrierNums[i]), POut.Long(pickedCarrierNum),
						InsEditLogType.InsPlan, x.Id, 0, x.GroupNumber + " - " + x.GroupName);
				});
				Carrier carrierCur = GetCarrier(carrierNums[i]); //gets from cache
				command = "DELETE FROM carrier"
					+ " WHERE CarrierNum = '" + carrierNums[i].ToString() + "'";
				Database.ExecuteNonQuery(command);
				//Security.CurUser.UserNum gets set on MT by the DtoProcessor so it matches the user from the client WS.
				InsEditLogs.MakeLogEntry(null, carrierCur, InsEditLogType.Carrier, Security.CurrentUser.Id);
			}
		}

		///<summary>Used in the FormCarrierCombine window.</summary>
		public static List<Carrier> GetCarriers(List<long> carrierNums)
		{
			//No need to check RemotingRole; no call to db.
			return GetWhere(x => x.Id.In(carrierNums));
		}

		///<summary>Used in FormInsPlan to check whether another carrier is already using this id.
		///That way, it won't tell the user that this might be an invalid id.</summary>
		public static bool ElectIdInUse(string electID)
		{
			//No need to check RemotingRole; no call to db.
			if (string.IsNullOrEmpty(electID))
			{
				return true;
			}
			return (cache.FirstOrDefault(x => x.ElectronicId == electID) != null);
		}

		///<summary>Used from insplan window when requesting benefits.  Gets carrier based on electID.  Returns empty list if no match found.</summary>
		public static List<Carrier> GetAllByElectId(string electID)
		{
			//No need to check RemotingRole; no call to db.
			return GetWhere(x => x.ElectronicId == electID);
		}

		///<summary>If carrierName is blank (empty string) this will throw an ApplicationException.  If a carrier is not found with the exact name,
		///including capitalization, a new carrier is created, inserted in the database, and returned.</summary>
		public static Carrier GetByNameAndPhone(string carrierName, string phone, bool updateCacheIfNew = false)
		{
			//No need to check RemotingRole; no call to db.
			if (string.IsNullOrEmpty(carrierName))
			{
				throw new ApplicationException("Carrier cannot be blank");
			}
			Carrier carrier = GetFirstOrDefault(x => x.Name == carrierName && x.Phone == phone);
			if (carrier == null)
			{
				carrier = new Carrier();
				carrier.Name = carrierName;
				carrier.Phone = phone;
				Insert(carrier); //Insert function takes care of logging.
				if (updateCacheIfNew)
				{
					Signalods.SetInvalid(InvalidType.Carriers);
					RefreshCache();
				}
			}
			return carrier;
		}

		///<summary>Returns null if no match is found. You are allowed to pass in nulls.</summary>
		public static Carrier GetByNameAndPhoneNoInsert(string carrierName, string phone)
		{
			if (string.IsNullOrEmpty(carrierName) || string.IsNullOrEmpty(phone))
			{
				return null;
			}
			return GetFirstOrDefault(x => x.Name == carrierName && x.Phone == phone);
		}

		/*
		///<summary>Gets a dictionary of carrier names for the supplied patient list.</summary>
		public static Dictionary<long,string> GetCarrierNames(List<Patient> patients){
			
			if(patients.Count==0){
				return new Dictionary<long,string>();
			}
			string command="SELECT patient.PatNum,carrier.CarrierName "
				+"FROM patient "
				+"LEFT JOIN patplan ON patient.PatNum=patplan.PatNum "
				+"LEFT JOIN insplan ON patplan.PlanNum=insplan.PlanNum "
				+"LEFT JOIN carrier ON carrier.CarrierNum=insplan.CarrierNum "
				+"WHERE";
			for(int i=0;i<patients.Count;i++){
				if(i>0){
					command+=" OR";
				}
				command+=" patient.PatNum="+POut.Long(patients[i].PatNum);
			}
			command+=" GROUP BY patient.PatNum,carrier.CarrierName";
			DataTable table=Db.GetTable(command);
			Dictionary<long,string> retVal=new Dictionary<long,string>();
			for(int i=0;i<table.Rows.Count;i++){
				retVal.Add(PIn.Long(table.Rows[i]["PatNum"].ToString()),table.Rows[i]["CarrierName"].ToString());
			}
			return retVal;
		}*/

		public static List<Carrier> GetByNameAndTin(string carrierName, string tin)
		{
			return GetWhere(x => x.Name.Trim().ToLower() == carrierName.Trim().ToLower() && x.TIN == tin);
		}

		public static Carrier GetCarrierByName(string carrierName)
		{
			return GetFirstOrDefault(x => x.Name == carrierName);
		}

		public static Carrier GetCarrierByNameNoCache(string carrierName)
		{
			return Crud.CarrierCrud.SelectOne("SELECT * FROM `carriers` WHERE `name` = '" + POut.String(carrierName) + "'");
		}

		public static bool IsMedicaid(Carrier carrier)
		{
			ElectID electId = ElectIDs.GetID(carrier.ElectronicId);

			if (electId != null && electId.IsMedicaid)
			{
				// Emdeon Medical requires loop 2420E when the claim is sent to DMERC (Medicaid) carriers.
				return true;
			}

			return false;
		}
	}
}
