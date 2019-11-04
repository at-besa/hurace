using Hurace.Dal.Ado;
using Hurace.Dal.Common;
using Hurace.Dal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hurace.Dal.Importer
{
    class StartListsImporter : IImporter
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
            if(!adoStartListDao.FindAll().Any())
            {
                GenerateStartLists();
            
                foreach (var startList in StartLists)
                {
                    adoStartListDao.Insert(startList);
//                Console.WriteLine($"Inserting worked: {adoStartListDao.Insert(startList)} for: {startList}");
                }
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
                    startList.SkierId = GetNewRandomSkierIdForRace(race);
                    startList.StartPos = i;
                    StartLists.Add(startList);
                    i++;
                }
            }
        }

        private int GetNewRandomSkierIdForRace(Race race)
        {
            Skier[] skierArray = adoSkierDao.FindAll().ToArray();
            Random random = new Random();
            int index = random.Next(0, skierArray.Length);
            while (!SkierAllowedForRace(race, skierArray[index]))
            {
                index = random.Next(0, skierArray.Length);
            }
            return skierArray[index].Id;
        }

        private bool SkierAllowedForRace(Race race, Skier skier)
        {
            bool allowed = true;
            foreach (var startList in StartLists)
            {
                if ((startList.SkierId == skier.Id && 
                    startList.Race.Id == race.Id) ||
                    skier.Sex != race.Sex)
                {
                    allowed = false;
                }
            }
            return allowed;
        }

        private int GetNumberOfStarters()
        {
            Random random = new Random();
            return random.Next(25, 36);
        }
    }
}
