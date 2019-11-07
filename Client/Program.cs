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
            // private static async Task Main()
        {
            IConfiguration configuration = ConfigurationUtil.GetConfiguration();
            IConnectionFactory connectionFactory =
                DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");

            //            DalTester dalTester = new DalTester(new AdoSkierDao(connectionFactory));
            //            dalTester.TestFindById();
            //            dalTester.TestUpdate();

            //            IRaceDataDao raceDataDao = new AdoRaceDataDao(connectionFactory);
            ////            Console.WriteLine(String.Join("\n", raceDataDao.FindAll()));

            //            AdoSkierDao skierDao = new AdoSkierDao(connectionFactory);
            //            foreach (var skier in skierDao.FindAll())
            //            {
            //                skierDao.Update(skier);
            //                Console.WriteLine($"udpated {skier}");
            //            }

            //            AdoRaceDao raceDao = new AdoRaceDao(connectionFactory);
            //            foreach (var race in raceDao.FindAll())
            //            {
            //                raceDao.Update(race);
            //                Console.WriteLine($"udpated {race}");
            //            }

            //new StartListsImporter(connectionFactory).Import();
            //new RaceDataImporter(connectionFactory).Import();

            new SplittimesImporter(connectionFactory).Import();

            #region Async

            //PrintTitle("PersonDao.FindAllAsync", 50);
            //await tester2.TestFindAllAsync();

            //PrintTitle("PersonDao.FindByIdAsync", 50);
            //await tester2.TestFindByIdAsync();

            //PrintTitle("PersonDao.UpdateAsync", 50);
            //await tester2.TestUpdateAsync();

            //PrintTitle("TransactionsAsync", 50);
            //await tester2.TestTransactionsAsync();

            #endregion
        }
    }
}