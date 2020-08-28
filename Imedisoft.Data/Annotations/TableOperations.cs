using System;

namespace Imedisoft.Data.Annotations
{
    /// <summary>
    /// Identifies the operations that are allowed on a table.
    /// </summary>
    [Flags]
    public enum TableOperations
    {
        /// <summary>
        /// Records may be inserted into the table.
        /// </summary>
        Create = 1,

        /// <summary>
        /// Existing records may be updated.
        /// </summary>
        Update = 4,

        /// <summary>
        /// Records may be deleted from the table.
        /// </summary>
        Delete = 8,

        /// <summary>
        /// All operations are allowed.
        /// </summary>
        All = Create | Update | Delete
    }
}
