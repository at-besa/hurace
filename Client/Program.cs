﻿using System;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;

 using Hurace.Dal.Ado;
 using Hurace.Dal.Common;
 using Hurace.Dal.Domain;
 using Hurace.Dal.Interface;
 using Microsoft.Extensions.Configuration;

//using PersonAdmin.Dal.Interface;
//using PersonAdmin.Domain;

namespace Hurace.Client
{
    class DalTester
    {
        private readonly ISkierDao skierDao;

        public DalTester(ISkierDao skierDao)
        {
            this.skierDao = skierDao;
        }

        public void TestFindAll()
        {
            var persons = skierDao.FindAll();
            foreach (var p in persons)
            {
                Console.WriteLine($"{p.Id,5} | {p.FirstName,-10} | {p.LastName,-15} | {p.DateOfBirth,10:yyyy-MM-dd}");
            }
        }

        public void TestFindById()
        {
            Skier person1 = skierDao.FindById(1);
            Console.WriteLine($"FindById(1) -> {person1?.ToString() ?? "<null>"}");

            Skier person2 = skierDao.FindById(99);
            Console.WriteLine($"FindById(99) -> {person2?.ToString() ?? "<null>"}");
        }

        public void TestUpdate()
        {
            Skier skier = skierDao.FindById(1);
            Console.WriteLine($"before update: skier -> {skier?.ToString() ?? "<null>"}");
            if (skier == null) return;

            skier.DateOfBirth = DateTime.Now.AddYears(-100);
            skierDao.Update(skier);

            skier = skierDao.FindById(1);
            Console.WriteLine($"after update:  skier -> {skier?.ToString() ?? "<null>"}");
        }

        public void TestTransactions()
        {
            Skier person1 = skierDao.FindById(0);
            Skier person2 = skierDao.FindById(1);

            DateTime oldDate1 = person1.DateOfBirth;
            DateTime oldDate2 = person2.DateOfBirth;
            DateTime newDate1 = DateTime.MinValue;
            DateTime newDate2 = DateTime.MinValue;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    person1.DateOfBirth = newDate1 = oldDate1.AddDays(1);
                    person2.DateOfBirth = newDate2 = oldDate2.AddDays(1);
                    skierDao.Update(person1);
                    // throw new ArgumentException(); // comment this out to rollback transaction
                    skierDao.Update(person2);
                    scope.Complete();
                }
            }
            catch
            {
            }

            person1 = skierDao.FindById(1);
            person2 = skierDao.FindById(2);

            if (oldDate1 == person1.DateOfBirth && oldDate2 == person2.DateOfBirth)
                Console.WriteLine("Transaction was ROLLED BACK.");
            else if (newDate1 == person1.DateOfBirth && newDate2 == person2.DateOfBirth)
                Console.WriteLine("Transaction was COMMITTED.");
            else
                Console.WriteLine("No Transaction.");
        }

        #region Async
        //public async Task TestFindAllAsync()
        //{
        //    (await skierDao.FindAllAsync())
        //             .ToList()
        //             .ForEach(p => Console.WriteLine($"{p.Id,5} | {p.FirstName,-10} | {p.LastName,-15} | {p.DateOfBirth,10:yyyy-MM-dd}"));
        //}

        //public async Task TestFindByIdAsync()
        //{
        //    Skier person1 = await skierDao.FindByIdAsync(1);
        //    Console.WriteLine($"FindById(1) -> {person1?.ToString() ?? "<null>"}");

        //    Skier person2 = await skierDao.FindByIdAsync(99);
        //    Console.WriteLine($"FindById(99) -> {person2?.ToString() ?? "<null>"}");
        //}

        //public async Task TestUpdateAsync()
        //{
        //    Skier person = await skierDao.FindByIdAsync(1);
        //    Console.WriteLine($"before update: person -> {person?.ToString() ?? "<null>"}");
        //    if (person == null) return;

        //    person.DateOfBirth = DateTime.Now.AddYears(-100);
        //    await skierDao.UpdateAsync(person);

        //    person = await skierDao.FindByIdAsync(1);
        //    Console.WriteLine($"after update:  person -> {person?.ToString() ?? "<null>"}");
        //}

        //public async Task TestTransactionsAsync()
        //{
        //    Skier person1 = await skierDao.FindByIdAsync(1);
        //    Skier person2 = await skierDao.FindByIdAsync(2);

        //    DateTime oldDate1 = person1.DateOfBirth;
        //    DateTime oldDate2 = person2.DateOfBirth;
        //    DateTime newDate1 = DateTime.MinValue;
        //    DateTime newDate2 = DateTime.MinValue;

        //    try
        //    {
        //        using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //        {
        //            person1.DateOfBirth = newDate1 = oldDate1.AddDays(1);
        //            person2.DateOfBirth = newDate2 = oldDate2.AddDays(1);
        //            await skierDao.UpdateAsync(person1);
        //            // throw new ArgumentException(); // comment this out to rollback transaction
        //            await skierDao.UpdateAsync(person2);
        //            scope.Complete();
        //        }
        //    }
        //    catch
        //    {
        //    }

        //    person1 = await skierDao.FindByIdAsync(1);
        //    person2 = await skierDao.FindByIdAsync(2);

        //    if (oldDate1 == person1.DateOfBirth && oldDate2 == person2.DateOfBirth)
        //        Console.WriteLine("Transaction was ROLLED BACK.");
        //    else if (newDate1 == person1.DateOfBirth && newDate2 == person2.DateOfBirth)
        //        Console.WriteLine("Transaction was COMMITTED.");
        //    else
        //        Console.WriteLine("No Transaction.");
        //}
        #endregion
    }

    class Program
    {
        private static void PrintTitle(string text = "", int length = 60, char ch = '-')
        {
            int preLen = (length - (text.Length + 2)) / 2;
            int postLen = length - (preLen + text.Length + 2);
            Console.WriteLine($"{new String(ch, preLen)} {text} {new String(ch, postLen)}");
        }
        
        private static void Main()
        // private static async Task Main()
        {

            
            
            IConfiguration configuration = ConfigurationUtil.GetConfiguration();
            IConnectionFactory connectionFactory =
            DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");

            //var tester2 = new DalTester(new AdoSkierDao(connectionFactory));


            //PrintTitle("PersonDao.FindAll", 50);
            //tester2.TestFindAll();

            //PrintTitle("PersonDao.FindById", 50);
            //tester2.TestFindById();

            //PrintTitle("PersonDao.Update", 50);
            //tester2.TestUpdate();

            //PrintTitle("Transactions", 50);
            //tester2.TestTransactions();

            var startListInserter = new StartListInserter(connectionFactory);
            startListInserter.getRaceData();
            startListInserter.getSkierData();
            startListInserter.insertStartlist();

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

    internal class StartListInserter
    {
        private IList<Race> races;
        private IList<Skier> skiers;
        private AdoRaceDao adoRaceDao;
        private AdoSkierDao adoSkierDao;
        private AdoStartListDao adoStartListDao;
        public IList<StartList> StartListList { get; set; } = new List<StartList>();
        public StartListInserter(IConnectionFactory connectionFactory)
        {
            adoRaceDao = new AdoRaceDao(connectionFactory);
            adoSkierDao = new AdoSkierDao(connectionFactory);
            adoStartListDao = new AdoStartListDao(connectionFactory);
        }


        public void getRaceData()
        {
            races = new List<Race>(adoRaceDao.FindAll());
        }

        public void getSkierData()
        {
            skiers = new List<Skier>(adoSkierDao.FindAll());
        }

        public void insertStartlist()
        {
            int i;
            foreach (var race in races)
            {
                i = 1;
                while (i < getNumberOfStarters()+1)
                {
                    StartList startList = new StartList { Race = race };
                    startList.SkierId = getNewRandomSkierIdForRace(race.Id);
                    startList.StartPos = i;
                    StartListList.Add(startList);
                    i++;
                }
            }
            foreach (var startList in StartListList)
            {
                Console.WriteLine(startList);
                
            }
            Console.WriteLine();
            Console.WriteLine("#####################");
            Console.WriteLine();
            
            IEnumerable<IGrouping<int, StartList>> startListsGroupedByRaceId = StartListList.GroupBy(startList => startList.Race.Id);
            foreach (var item in startListsGroupedByRaceId)
            {
                Console.WriteLine(item);
            }
        }

        private int getNewRandomSkierIdForRace(int raceId)
        {
            Skier[] skierArray = adoSkierDao.FindAll().ToArray();
            List<StartList> startListsByRaceId = adoStartListDao.FindById(raceId).ToList();
            Random random = new Random();
            int index = random.Next(0, skierArray.Length);
            while (startListContainsSkierId(startListsByRaceId, skierArray[index].Id))
            {
                index = random.Next(0, skierArray.Length);
            }
            return skierArray[index].Id;
        }

        private bool startListContainsSkierId(List<StartList> startListsByRaceId, int id)
        {
            bool contains = false;
            foreach (var startList in startListsByRaceId)
            {
                if(startList.SkierId == id)
                {
                    contains = true;
                }
            }
            return contains;
        }

        private int getNumberOfStarters()
        {
            Random random = new Random();
            return random.Next(25, 36);
        }
    }
}
