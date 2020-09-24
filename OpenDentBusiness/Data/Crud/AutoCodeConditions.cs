//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: v4.0.30319
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Imedisoft.Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Imedisoft.Data
{
	public partial class AutoCodeConditions
	{
		public static AutoCodeCondition FromReader(MySqlDataReader dataReader)
		{
			return new AutoCodeCondition
			{
				AutoCodeItemId = (long)dataReader["auto_code_item_id"],
				Type = (AutoCodeConditionType)Convert.ToInt32(dataReader["type"])
			};
		}

		/// <summary>
		/// Selects a single AutoCodeCondition object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static AutoCodeCondition SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects multiple <see cref="AutoCodeCondition"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<AutoCodeCondition> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);
	}
}