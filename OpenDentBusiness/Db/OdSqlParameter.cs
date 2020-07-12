using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Text;

namespace OpenDentBusiness
{
	/// <summary>
	/// Hold parameter info in a database independent manner.
	/// </summary>
	public class OdSqlParameter
	{
        private OdDbType dbType;

        /// <summary>
        /// parameterName should not include the leading character such as @ or : . 
        /// And DbHelper.ParamChar() should be used to determine the char in the query itself.
        /// </summary>
        public string ParameterName { get; set; }

        public object Value { get; set; }

        /// <summary>
        /// parameterName should not include the leading character such as @ or : . 
        /// And DbHelper.ParamChar() should be used to determine the char in the query itself.
        /// </summary>
        public OdSqlParameter(string parameterName, OdDbType dbType, object value)
		{
			ParameterName = parameterName;
			this.dbType = dbType;
			Value = value;
		}

		public MySqlDbType GetMySqlDbType()
		{
			switch (dbType)
			{
				case OdDbType.Text:
					return MySqlDbType.MediumText;

				default:
					throw new ApplicationException("Type not found");
			}
		}

		public MySqlParameter GetMySqlParameter()
		{
            return new MySqlParameter
            {
                ParameterName = DbHelper.ParamChar + ParameterName,
                Value = Value,
                MySqlDbType = GetMySqlDbType()
            };
		}
	}
}
