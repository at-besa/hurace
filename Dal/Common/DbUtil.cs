using System;
using System.Data.Common;

namespace Hurace.Dal.Common
{
  public static class DbUtil
  {
#if !NET_STANDARD_2_1
        //needed before 2_1 standard, 
    public static DbProviderFactory GetDbProviderFactory(string providerName) {
      switch (providerName)
      {
        case "Microsoft.Data.SqlClient": return Microsoft.Data.SqlClient.SqlClientFactory.Instance;
        // "System.Data.SqlClient" => System.Data.SqlClient.SqlClientFactory.Instance,
        case "MySql.Data.MySqlClient":  return MySql.Data.MySqlClient.MySqlClientFactory.Instance;
        default: throw new ArgumentException("Invalid provider name \"{providerName}\"");
      }
    }
#endif

#if NET_STANDARD_2_1
    public static void RegisterAdoProviders()
    {
      // Use new Implementation of MS SQL Provider
      DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", Microsoft.Data.SqlClient.SqlClientFactory.Instance);
      // DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
      DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySql.Data.MySqlClient.MySqlClientFactory.Instance);
    }
#endif
  }
}
