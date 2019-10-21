﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
 using Hurace.Dal.Ado;
 using Hurace.Dal.Common;
 using Hurace.Dal.Interface;
 using Hurace.Domain;
 using Microsoft.Extensions.Configuration;

//using PersonAdmin.Dal.Interface;
//using PersonAdmin.Domain;

namespace Hurace.Client
{
    class DalTester
    {
        private readonly IPersonDao personDao;

        public DalTester(IPersonDao personDao)
        {
            this.personDao = personDao;
        }


        public void TestFindAll()
        {
            var persons = personDao.FindAll();
            foreach (var p in persons)
            {
                Console.WriteLine($"{p.Id,5} | {p.FirstName,-10} | {p.LastName,-15} | {p.DateOfBirth,10:yyyy-MM-dd}");
            }
        }

        public void TestFindById()
        {
            Person person1 = personDao.FindById(1);
            Console.WriteLine($"FindById(1) -> {person1?.ToString() ?? "<null>"}");

            Person person2 = personDao.FindById(99);
            Console.WriteLine($"FindById(99) -> {person2?.ToString() ?? "<null>"}");
        }

        public void TestUpdate()
        {
            Person person = personDao.FindById(1);
            Console.WriteLine($"before update: person -> {person?.ToString() ?? "<null>"}");
            if (person == null) return;

            person.DateOfBirth = DateTime.Now.AddYears(-100);
            personDao.Update(person);

            person = personDao.FindById(1);
            Console.WriteLine($"after update:  person -> {person?.ToString() ?? "<null>"}");
        }

        public void TestTransactions()
        {
            Person person1 = personDao.FindById(1);
            Person person2 = personDao.FindById(2);

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
                    personDao.Update(person1);
                    // throw new ArgumentException(); // comment this out to rollback transaction
                    personDao.Update(person2);
                    scope.Complete();
                }
            }
            catch
            {
            }

            person1 = personDao.FindById(1);
            person2 = personDao.FindById(2);

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
        //    (await personDao.FindAllAsync())
        //             .ToList()
        //             .ForEach(p => Console.WriteLine($"{p.Id,5} | {p.FirstName,-10} | {p.LastName,-15} | {p.DateOfBirth,10:yyyy-MM-dd}"));
        //}

        //public async Task TestFindByIdAsync()
        //{
        //    Person person1 = await personDao.FindByIdAsync(1);
        //    Console.WriteLine($"FindById(1) -> {person1?.ToString() ?? "<null>"}");

        //    Person person2 = await personDao.FindByIdAsync(99);
        //    Console.WriteLine($"FindById(99) -> {person2?.ToString() ?? "<null>"}");
        //}

        //public async Task TestUpdateAsync()
        //{
        //    Person person = await personDao.FindByIdAsync(1);
        //    Console.WriteLine($"before update: person -> {person?.ToString() ?? "<null>"}");
        //    if (person == null) return;

        //    person.DateOfBirth = DateTime.Now.AddYears(-100);
        //    await personDao.UpdateAsync(person);

        //    person = await personDao.FindByIdAsync(1);
        //    Console.WriteLine($"after update:  person -> {person?.ToString() ?? "<null>"}");
        //}

        //public async Task TestTransactionsAsync()
        //{
        //    Person person1 = await personDao.FindByIdAsync(1);
        //    Person person2 = await personDao.FindByIdAsync(2);

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
        //            await personDao.UpdateAsync(person1);
        //            // throw new ArgumentException(); // comment this out to rollback transaction
        //            await personDao.UpdateAsync(person2);
        //            scope.Complete();
        //        }
        //    }
        //    catch
        //    {
        //    }

        //    person1 = await personDao.FindByIdAsync(1);
        //    person2 = await personDao.FindByIdAsync(2);

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
            DefaultConnectionFactory.FromConfiguration(configuration, "PersonDbConnection");

            var tester2 = new DalTester(new AdoPersonDao(connectionFactory));

            PrintTitle("PersonDao.FindAll", 50);
            tester2.TestFindAll();

            PrintTitle("PersonDao.FindById", 50);
            tester2.TestFindById();

            PrintTitle("PersonDao.Update", 50);
            tester2.TestUpdate();

            PrintTitle("Transactions", 50);
            tester2.TestTransactions();

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
