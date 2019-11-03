using System;
using System.Transactions;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Client
{
    public class DalTester
    {
            private readonly ISkierDao skierDao;

        public DalTester(ISkierDao skierDao)
        {
            this.skierDao = skierDao;
        }

        public void TestFindAll()
        {
            var skiers = skierDao.FindAll();
            foreach (var p in skiers)
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
}