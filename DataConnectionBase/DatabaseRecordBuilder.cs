using MySql.Data.MySqlClient;

namespace DataConnectionBase
{
    public delegate T DatabaseRecordBuilder<T>(MySqlDataReader dataReader);
}
