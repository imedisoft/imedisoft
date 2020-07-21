using Imedisoft.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace OpenDentBusiness
{
    public class Equipments
	{
		public static List<Equipment> GetList(DateTime fromDate, DateTime toDate, EnumEquipmentDisplayMode display, string snDesc)
		{
			string command = "";
			if (display == EnumEquipmentDisplayMode.Purchased)
			{
				command = "SELECT * FROM equipment "
					+ "WHERE DatePurchased >= " + POut.Date(fromDate)
					+ " AND DatePurchased <= " + POut.Date(toDate)
					+ " AND (SerialNumber LIKE '%" + POut.String(snDesc) + "%' OR Description LIKE '%" + POut.String(snDesc) + "%' OR Location LIKE '%" + POut.String(snDesc) + "%')"
					+ " ORDER BY DatePurchased";
			}

			if (display == EnumEquipmentDisplayMode.Sold)
			{
				command = "SELECT * FROM equipment "
					+ "WHERE DateSold >= " + POut.Date(fromDate)
					+ " AND DateSold <= " + POut.Date(toDate)
					+ " AND (SerialNumber LIKE '%" + POut.String(snDesc) + "%' OR Description LIKE '%" + POut.String(snDesc) + "%' OR Location LIKE '%" + POut.String(snDesc) + "%')"
					+ " ORDER BY DatePurchased";
			}

			if (display == EnumEquipmentDisplayMode.All)
			{
				command = "SELECT * FROM equipment "
					+ "WHERE ((DatePurchased >= " + POut.Date(fromDate) + " AND DatePurchased <= " + POut.Date(toDate) + ")"
						+ " OR (DateSold >= " + POut.Date(fromDate) + " AND DateSold <= " + POut.Date(toDate) + "))"
					+ " AND (SerialNumber LIKE '%" + POut.String(snDesc) + "%' OR Description LIKE '%" + POut.String(snDesc) + "%' OR Location LIKE '%" + POut.String(snDesc) + "%')"
					+ " ORDER BY DatePurchased";
			}

			return Crud.EquipmentCrud.SelectMany(command);
		}

		public static long Insert(Equipment equip)
		{
			return Crud.EquipmentCrud.Insert(equip);
		}

		public static void Update(Equipment equip)
		{
			Crud.EquipmentCrud.Update(equip);
		}

		public static void Delete(Equipment equip)
		{
			string command = "DELETE FROM equipment WHERE EquipmentNum = " + POut.Long(equip.EquipmentNum);
			Database.ExecuteNonQuery(command);
		}

		/// <summary>
		/// Generates a unique 3 char alphanumeric serialnumber. Checks to make sure it's not already in use.
		/// </summary>
		public static string GenerateSerialNum()
		{
			string retVal = "";
			bool isDuplicate = true;
			Random rand = new Random();
			while (isDuplicate)
			{
				retVal = "";
				for (int i = 0; i < 4; i++)
				{
					int r = rand.Next(0, 34);
					if (r < 9)
					{
						retVal += (char)('1' + r);//1-9, no zero
					}
					else
					{
						retVal += (char)('A' + r - 9);
					}
				}

				string command = "SELECT COUNT(*) FROM equipment WHERE SerialNumber = '" + POut.String(retVal) + "'";
				if (Database.ExecuteLong(command) == 0)
				{
					isDuplicate = false;
				}
			}
			return retVal;
		}

		/// <summary>
		/// Checks the database for equipment that has the supplied serial number.
		/// </summary>
		public static bool HasExisting(Equipment equip)
		{
			string command = "SELECT COUNT(*) FROM equipment WHERE SerialNumber = '" + POut.String(equip.SerialNumber) + "' AND EquipmentNum != " + POut.Long(equip.EquipmentNum);
			if (Database.ExecuteLong(command) == 0)
			{
				return false;
			}
			return true;
		}
	}

	public enum EnumEquipmentDisplayMode
	{
		Purchased,
		Sold,
		All
	}
}
