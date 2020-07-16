using MySql.Data.MySqlClient;

namespace Imedisoft.Data
{
    public delegate T DataRecordBuilder<T>(MySqlDataReader dataReader);
}
