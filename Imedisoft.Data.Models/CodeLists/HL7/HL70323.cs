using System.Collections.Generic;

namespace Imedisoft.Data.Models.CodeLists.HL7
{
    /// <summary>
    ///		<para>
    ///			Action Code (IIS)
    ///		</para>
    ///		<para>
    ///			Corresponds to HL7 table <b>0323</b>.
    ///		</para>
    /// </summary>
    public static class HL70323
	{
		public const char Add = 'A';
		public const char Delete = 'D';
		public const char Update = 'U';

        public static IEnumerable<DataItem<char>> GetDataItems()
        {
            yield return new DataItem<char>(Add, "Add");
            yield return new DataItem<char>(Delete, "Delete");
            yield return new DataItem<char>(Update, "Update");
        }
	}
}
