using System.Data;

namespace Hurace.Dal.Common
{
    public delegate T RowMapper<T>(IDataRecord row);
}
