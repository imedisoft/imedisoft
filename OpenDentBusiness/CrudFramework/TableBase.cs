using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DataConnectionBase;

namespace OpenDentBusiness
{
	/// <summary>
	/// The base class for classes that correspond to a table in the database.
	/// Make sure to mark each derived class [Serializable].
	/// </summary>
	abstract public class TableBase
	{
		private static int _maxAllowedPacket = 0;

		/// <summary>
		/// Not a db column.
		/// Always false by default.
		/// Will only be true if explicitly set to true by programmer.
		/// When CRUD grabs a table from db, it is naturally set to False.
		/// Once set, this value is not used by the CRUD in any manner. 
		/// Just used by the programmer for making decisions about whether to Insert or Update.
		/// </summary>
		public bool IsNew { get; set; }

        /// <summary>
		/// Not a db column.
		/// Tag can be useful when you need to associate this object to another object and do not have a control Tag.
		/// </summary>
        [XmlIgnore]
        public object TagOD { get; set; }

		/// <summary>
		/// We cannot make the returned value too large, because we want to allow the server to process 
		/// information from the previous packet while downloading the next packet in parallel.
		/// </summary>
		public static int MaxAllowedPacketCount
		{
			get
			{
				if (_maxAllowedPacket > 0)
				{
					// This value is not expected to change often. 
					// Return cached value to prevent running a query below for MySQL (most of our customers).
					return _maxAllowedPacket;
				}

				int kilobyte = 1024;//1KB
				int megabyte = kilobyte * kilobyte;//1MB

                int retVal = MiscData.GetMaxAllowedPacket() - 8 * kilobyte;

				// Minimum of 8K (for network packet headers), maximum of 1MB for parallel.
				_maxAllowedPacket = Math.Min(Math.Max(retVal, 8 * kilobyte), megabyte); 

				return _maxAllowedPacket;
			}
		}
	}
}
