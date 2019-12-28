using Hurace.Core.DAL.Common;

namespace Api.Util
{
    public class ConnectionFactoryHolder
    {
        private static ConnectionFactoryHolder _instance;
        private static IConnectionFactory _connectionFactory;
        private ConnectionFactoryHolder()
        {
            var configuration = ConfigurationUtil.GetConfiguration();
            _connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");            
        }

        public static ConnectionFactoryHolder getInstace()
        {
            return _instance ??= new ConnectionFactoryHolder();
        }

        public IConnectionFactory getConnectionFactory()
        {
            return _connectionFactory;
        }
    }
}