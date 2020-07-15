using MySql.Data.MySqlClient;

namespace DataConnectionBase
{
    public delegate T DataRecordBuilder<T>(MySqlDataReader dataReader);
}
