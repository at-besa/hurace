using System;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using System.Net;
using Hurace.Dal.Ado;
using Hurace.Dal.Common;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using Microsoft.Extensions.Configuration;
using Hurace.Dal.Importer;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Hurace.Client
{
    class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration configuration = ConfigurationUtil.GetConfiguration();
            IConnectionFactory connectionFactory =
                DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
        }
    }
}