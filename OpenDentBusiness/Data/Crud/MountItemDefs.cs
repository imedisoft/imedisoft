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
using System.Collections.Generic;

namespace Imedisoft.Data
{
    public partial class MountItemDefs
	{
		public static MountItemDef FromReader(MySqlDataReader dataReader)
		{
			return new MountItemDef
			{
				Id = (long)dataReader["id"],
				MountDefId = (long)dataReader["mount_def_id"],
				X = (int)dataReader["x"],
				Y = (int)dataReader["y"],
				Width = (int)dataReader["width"],
				Height = (int)dataReader["height"],
				SortOrder = (int)dataReader["sort_order"]
			};
		}

		/// <summary>
		/// Selects a single MountItemDef object from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static MountItemDef SelectOne(string command, params MySqlParameter[] parameters)
			=> Database.SelectOne(command, FromReader, parameters);

		/// <summary>
		/// Selects the <see cref="MountItemDef"/> object with the specified key from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="MountItemDef"/> to select.</param>
		public static MountItemDef SelectOne(long id)
			=> SelectOne("SELECT * FROM `mount_item_defs` WHERE `id` = " + id);

		/// <summary>
		/// Selects multiple <see cref="MountItemDef"/> objects from the database using the specified SQL command.
		/// </summary>
		/// <param name="command">The SELECT command to execute.</param>
		/// <param name="parameters">The (optional) command parameters.</param>
		public static IEnumerable<MountItemDef> SelectMany(string command, params MySqlParameter[] parameters)
			=> Database.SelectMany(command, FromReader, parameters);

		/// <summary>
		/// Inserts the specified <see cref="MountItemDef"/> into the database.
		/// </summary>
		/// <param name="mountItemDef">The <see cref="MountItemDef"/> to insert into the database.</param>
		private static long ExecuteInsert(MountItemDef mountItemDef)
			=> mountItemDef.Id = Database.ExecuteInsert(
				"INSERT INTO `mount_item_defs` " +
				"(`mount_def_id`, `x`, `y`, `width`, `height`, `sort_order`) " +
				"VALUES (" +
					"@mount_def_id, @x, @y, @width, @height, @sort_order" +
				")",
					new MySqlParameter("mount_def_id", mountItemDef.MountDefId),
					new MySqlParameter("x", mountItemDef.X),
					new MySqlParameter("y", mountItemDef.Y),
					new MySqlParameter("width", mountItemDef.Width),
					new MySqlParameter("height", mountItemDef.Height),
					new MySqlParameter("sort_order", mountItemDef.SortOrder));

		/// <summary>
		/// Updates the specified <see cref="MountItemDef"/> in the database.
		/// </summary>
		/// <param name="mountItemDef">The <see cref="MountItemDef"/> to update.</param>
		private static void ExecuteUpdate(MountItemDef mountItemDef)
			=> Database.ExecuteNonQuery(
				"UPDATE `mount_item_defs` SET " +
					"`mount_def_id` = @mount_def_id, " +
					"`x` = @x, " +
					"`y` = @y, " +
					"`width` = @width, " +
					"`height` = @height, " +
					"`sort_order` = @sort_order " +
				"WHERE `id` = @id",
					new MySqlParameter("id", mountItemDef.Id),
					new MySqlParameter("mount_def_id", mountItemDef.MountDefId),
					new MySqlParameter("x", mountItemDef.X),
					new MySqlParameter("y", mountItemDef.Y),
					new MySqlParameter("width", mountItemDef.Width),
					new MySqlParameter("height", mountItemDef.Height),
					new MySqlParameter("sort_order", mountItemDef.SortOrder));

		/// <summary>
		/// Deletes a single <see cref="MountItemDef"/> object from the database.
		/// </summary>
		/// <param name="id">The primary key of the <see cref="MountItemDef"/> to delete.</param>
		private static void ExecuteDelete(long id)
			 => Database.ExecuteNonQuery("DELETE FROM `mount_item_defs` WHERE `id` = " + id);

		/// <summary>
		/// Deletes the specified <see cref="MountItemDef"/> object from the database.
		/// </summary>
		/// <param name="mountItemDef">The <see cref="MountItemDef"/> to delete.</param>
		private static void ExecuteDelete(MountItemDef mountItemDef)
			=> ExecuteDelete(mountItemDef.Id);
	}
}
