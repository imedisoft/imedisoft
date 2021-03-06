using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Crud table attributes cannot be used by inherited classes because some properties would not
    /// work if they were inherited. Simply add the desired attributes to the "inheriting" class 
    /// which will effectively override the attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class CrudTableAttribute : Attribute
	{
		public CrudTableAttribute()
		{
			TableName = "";
			IsDeleteForbidden = false;
			IsSynchable = false;
			AuditPerms = CrudAuditPerm.None;
			IsSecurityStamped = false;
			HasBatchWriteMethods = false;
			IsTableHist = false;
		}

        /// <summary>
		/// If tablename is different than the lowercase class name.
		/// </summary>
        public string TableName { get; set; }

        /// <summary>
		/// Set to true for tables where rows are not deleted.
		/// </summary>
        public bool IsDeleteForbidden { get; set; }

        public bool IsSynchable { get; set; }

        /// <summary>
		/// Enum containing all permissions used as an FKey entry for the Securitylog table.
		/// The Crud generator uses these to add an additional function call to Delete(), 
		/// and a new function ClearFkey() to ensure that securitylog FKeys are not orphaned.
		/// </summary>
        public CrudAuditPerm AuditPerms { get; set; }

        /// <summary>
		/// If IsSecurityStamped is true, the table must include the field SecUserNumEntry.
		/// 
		///		<para>
		///			If IsSynchable and IsSecurityStamped are BOTH true, the Crud generator will 
		///			create a Sync function that takes userNum and sets the SecUserNumEntry field 
		///			before inserting. Security.CurUser isn't accessible from the Crud due to 
		///			remoting role, must be passed in.
		///		</para>
        ///		<para>IsSecurityStamped is ignored if IsSynchable is false.</para>
		///	</summary>
        public bool IsSecurityStamped { get; set; }

        public bool HasBatchWriteMethods { get; set; }

		public bool IsTableHist { get; set; }

		public bool IsLargeTable { get; set; }

		/// <summary>
		/// True if the CrudGenerator should produce a RowToObj method for use as an alternative to
		/// the TableToList(GetTable(command)) pattern.
		/// 
		/// The TableToList(GetTable) pattern causes two copies of the data to be held in memory, 
		/// one copy as a DataTable and one as the list of objects.
		/// 
		/// Tables with UsesDataReader=true will have the option of calling 
		/// Db.GetList(command,RowToObj) that uses a DataReader and converts DataRows 
		/// (IDataRecords) into objects one at a time to reduce the memory required to get a list 
		/// of objects.
		/// </summary>
		public bool UsesDataReader { get; set; } = false;

		/// <summary>
		/// Returns a bitwise enum that represents all permissions used by the security log FKey column for the class passed in.
		/// </summary>
		public static CrudAuditPerm GetCrudAuditPermForClass(Type typeClass)
		{
			object[] attributes = typeClass.GetCustomAttributes(typeof(CrudTableAttribute), true);
			if (attributes.Length == 0)
			{
				return CrudAuditPerm.None;
			}

			for (int i = 0; i < attributes.Length; i++)
			{
				if (attributes[i].GetType() != typeof(CrudTableAttribute))
				{
					continue;
				}
				if (((CrudTableAttribute)attributes[i]).AuditPerms != CrudAuditPerm.None)
				{
					return ((CrudTableAttribute)attributes[i]).AuditPerms;
				}
			}

			return CrudAuditPerm.None;
		}
	}

	///<summary>Hard coded list of all permission names that are used for securitylog.FKey.  Uses 2^n values for use in bitwise operations.
	///This enum can only hold 31 permissions (and none) before we will need to create a new one.  Instead of creating a new enum, we could instead
	///create a new table to hold a composite key between the permission type the table name and foreign key.</summary>
	[Flags]
	public enum CrudAuditPerm
	{
		///<summary>Perm#:0 - Value:0</summary>
		None = 0,
		///<summary>Perm#:1 - Value:1</summary>
		AppointmentCompleteEdit = 1,
		///<summary>Perm#:2 - Value:2</summary>
		AppointmentCreate = 2,
		///<summary>Perm#:3 - Value:4</summary>
		AppointmentEdit = 4,
		///<summary>Perm#:4 - Value:8</summary>
		AppointmentMove = 8,
		///<summary>Perm#:5 - Value:16</summary>
		ClaimHistoryEdit = 16,
		///<summary>Perm#:6 - Value:32</summary>
		ImageDelete = 32,
		///<summary>Perm#:7 - Value:64</summary>
		ImageEdit = 64,
		///<summary>Perm#:8 - Value:128</summary>
		InsPlanChangeCarrierName = 128,
		///<summary>Perm#:9 - Value:256</summary>
		RxCreate = 256,
		///<summary>Perm#:10 - Value:512</summary>
		RxEdit = 512,
		///<summary>Perm#:11 - Value:1024</summary>
		TaskNoteEdit = 1024,
		///<summary>Perm#:12 - Value:2048</summary>
		PatientPortal = 2048,
		///<summary>Perm#:13 - Value:4096</summary>
		ProcFeeEdit = 4096,
		///<summary>Perm#:14 - Value:8192</summary>
		LogFeeEdit = 8192,
		///<summary>Perm#15 - Value:16384</summary>
		LogSubscriberEdit = 16384
	}
}
