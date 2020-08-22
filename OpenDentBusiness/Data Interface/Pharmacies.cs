﻿using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness
{
    public class Pharmacies
	{
		#region CachePattern
		private class PharmacyCache : CacheListAbs<Pharmacy>
		{
			protected override List<Pharmacy> GetCacheFromDb()
			{
				string command = "SELECT * FROM pharmacy ORDER BY StoreName";
				return Crud.PharmacyCrud.SelectMany(command);
			}
			protected override List<Pharmacy> TableToList(DataTable table)
			{
				return Crud.PharmacyCrud.TableToList(table);
			}
			protected override Pharmacy Copy(Pharmacy pharmacy)
			{
				return pharmacy.Copy();
			}
			protected override DataTable ListToTable(List<Pharmacy> listPharmacies)
			{
				return Crud.PharmacyCrud.ListToTable(listPharmacies, "Pharmacy");
			}
			protected override void FillCacheIfNeeded()
			{
				Pharmacies.GetTableFromCache(false);
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static PharmacyCache _pharmacyCache = new PharmacyCache();

		public static List<Pharmacy> GetDeepCopy(bool isShort = false)
		{
			return _pharmacyCache.GetDeepCopy(isShort);
		}

		public static Pharmacy GetFirstOrDefault(Func<Pharmacy, bool> match, bool isShort = false)
		{
			return _pharmacyCache.GetFirstOrDefault(match, isShort);
		}

		public static List<Pharmacy> GetWhere(Predicate<Pharmacy> match, bool isShort = false)
		{
			return _pharmacyCache.GetWhere(match, isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache()
		{
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table)
		{
			//No need to check RemotingRole; no call to db.
			_pharmacyCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache)
		{

			return _pharmacyCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		public static Pharmacy GetOne(long pharmacyNum)
		{
			return Crud.PharmacyCrud.SelectOne(pharmacyNum);
		}

		public static List<Pharmacy> GetAllNoCache()
		{
			return Crud.PharmacyCrud.SelectMany("SELECT * FROM pharmacy ORDER BY StoreName");
		}

		public static long Insert(Pharmacy pharmacy)
		{
			return Crud.PharmacyCrud.Insert(pharmacy);
		}

		public static void Update(Pharmacy pharmacy)
		{
			Crud.PharmacyCrud.Update(pharmacy);
		}

		public static void DeleteObject(long pharmacyNum)
		{
			Crud.PharmacyCrud.Delete(pharmacyNum);
		}

		public static string GetDescription(long PharmacyNum)
		{
			Pharmacy pharmacy = GetFirstOrDefault(x => x.PharmacyNum == PharmacyNum);
			return pharmacy == null ? "" : pharmacy.StoreName;
		}

		public static List<long> GetChangedSincePharmacyNums(DateTime changedSince)
		{
			DataTable dt = Database.ExecuteDataTable("SELECT PharmacyNum FROM pharmacy WHERE DateTStamp > " + POut.DateT(changedSince));
			List<long> provnums = new List<long>(dt.Rows.Count);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				provnums.Add(PIn.Long(dt.Rows[i]["PharmacyNum"].ToString()));
			}
			return provnums;
		}

		/// <summary>Used along with GetChangedSincePharmacyNums</summary>
		public static List<Pharmacy> GetMultPharmacies(List<long> pharmacyNums)
		{
			string strPharmacyNums = "";
			DataTable table;

			if (pharmacyNums.Count > 0)
			{
				for (int i = 0; i < pharmacyNums.Count; i++)
				{
					if (i > 0)
					{
						strPharmacyNums += "OR ";
					}
					strPharmacyNums += "PharmacyNum='" + pharmacyNums[i].ToString() + "' ";
				}

				table = Database.ExecuteDataTable("SELECT * FROM pharmacy WHERE " + strPharmacyNums);
			}
			else
			{
				table = new DataTable();
			}

			Pharmacy[] multPharmacys = Crud.PharmacyCrud.TableToList(table).ToArray();
			List<Pharmacy> pharmacyList = new List<Pharmacy>(multPharmacys);
			return pharmacyList;
		}

		/// <summary>Gets a list of Pharmacies for a given clinic based on PharmClinic links.</summary>
		/// <param name="clinicNum">
		/// The primary key of the clinic.
		/// </param>
		public static List<Pharmacy> GetPharmaciesForClinic(long clinicNum)
		{

			string command = "SELECT * "
				+ "FROM pharmacy "
				+ "WHERE PharmacyNum IN ("
					+ "SELECT PharmacyNum "
					+ "FROM pharmclinic "
					+ "WHERE clinicNum = " + POut.Long(clinicNum)
				+ ") ORDER BY StoreName";
			return Crud.PharmacyCrud.SelectMany(command);
		}
	}
}
