using Hurace.Dal.Ado;
using Hurace.Dal.Common;
using Hurace.Dal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hurace.Dal.Importer
{
    class StartListsImporter
    {

        private AdoRaceDao adoRaceDao;
        private AdoSkierDao adoSkierDao;
        private AdoStartListDao adoStartListDao;
        public IList<StartList> StartLists { get; set; } = new List<StartList>();
        public StartListsImporter(IConnectionFactory connectionFactory)
        {
            adoRaceDao = new AdoRaceDao(connectionFactory);
            adoSkierDao = new AdoSkierDao(connectionFactory);
            adoStartListDao = new AdoStartListDao(connectionFactory);
        }

        public void Import()
        {
            if(adoStartListDao.FindAll().Count() != 0)
            {
                throw new Exception("Already data in StartList");
            }
            GenerateStartLists();
            foreach (var startList in StartLists)
            {
                Console.WriteLine($"Inserting worked: {adoStartListDao.Insert(startList)} for: {startList}");
            }
        }

        private void GenerateStartLists()
        {
            int i;
            IList<Race> races = new List<Race>(adoRaceDao.FindAll());
            foreach (var race in races)
            {
                i = 1;
                while (i < GetNumberOfStarters() + 1)
                {
                    StartList startList = new StartList { Race = race };
                    startList.SkierId = GetNewRandomSkierIdForRace(race.Id);
                    startList.StartPos = i;
                    StartLists.Add(startList);
                    i++;
                }
            }

            //foreach (var startList in StartLists)
            //{
            //    Console.WriteLine(startList);

            //}
            //Console.WriteLine();
            //Console.WriteLine("#####################");
            //Console.WriteLine();

            //IEnumerable<IGrouping<int, StartList>> startListsGroupedByRaceId = StartLists.GroupBy(startList => startList.Race.Id);
            //foreach (var item in startListsGroupedByRaceId)
            //{
            //    Console.WriteLine(item);
            //}
        }

        private int GetNewRandomSkierIdForRace(int raceId)
        {
            Skier[] skierArray = adoSkierDao.FindAll().ToArray();
            Random random = new Random();
            int index = random.Next(0, skierArray.Length);
            while (StartListContainsSkierId(raceId, skierArray[index].Id))
            {
                index = random.Next(0, skierArray.Length);
            }
            return skierArray[index].Id;
        }

        private bool StartListContainsSkierId(int raceId, int skierId)
        {
            bool contains = false;
            foreach (var startList in StartLists)
            {
                if (startList.SkierId == skierId && startList.Race.Id == raceId)
                {
                    contains = true;
                }
            }
            return contains;
        }

        private int GetNumberOfStarters()
        {
            Random random = new Random();
            return random.Next(25, 36);
        }
    }
}
