using System.Data.Common;
using System.Threading.Tasks;

namespace Hurace.Core.DAL.Common
{
    public interface IConnectionFactory
    {
        string ConnectionString { get; }
        string ProviderName { get; }
        DbConnection CreateConnection();

        Task<DbConnection> CreateConnectionAsync();
    }
}
