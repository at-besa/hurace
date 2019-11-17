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

//using PersonAdmin.Dal.Interface;
//using PersonAdmin.Domain;

namespace Hurace.Client
{
    class Program
    {
        private static void PrintTitle(string text = "", int length = 60, char ch = '-')
        {
            int preLen = (length - (text.Length + 2)) / 2;
            int postLen = length - (preLen + text.Length + 2);
            Console.WriteLine($"{new String(ch, preLen)} {text} {new String(ch, postLen)}");
        }

        public static void Main(string[] args)
        {
            IConfiguration configuration = ConfigurationUtil.GetConfiguration();
            IConnectionFactory connectionFactory =
                DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
        }
    }
}