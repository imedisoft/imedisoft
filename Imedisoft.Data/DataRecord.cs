namespace Imedisoft.Data
{
    /// <summary>
    /// The base class of all database record entities.
    /// </summary>
    public abstract class DataRecord : DataRecordBase
    {
        /// <summary>
        /// Gets or sets the ID of the database record.
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the database record is a new record.
        /// </summary>
        public bool IsNew => Id == 0;
    }
}
