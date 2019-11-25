using System.Data;

namespace Hurace.Core.DAL.Common
{
    public delegate T RowMapper<T>(IDataRecord row);
}
