using Hurace.Dal.Ado;
using Hurace.Dal.Common;
using Hurace.Dal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hurace.Dal.Importer
{
    class SplittimesImporter : IImporter
    {
        private AdoRaceDataDao adoRaceDataDao;
        private AdoRaceDao adoRaceDao;
        private AdoRaceTypeDao adoRaceTypeDao;
        public SplittimesImporter(IConnectionFactory connectionfactroy)
        {
            adoRaceDataDao = new AdoRaceDataDao(connectionfactroy);
            adoRaceDao = new AdoRaceDao(connectionfactroy);
            adoRaceTypeDao = new AdoRaceTypeDao(connectionfactroy);
        }
        public void Import()
        {
            //get all racedata
            //join with race on raceId for typeId and splittimes 
            //join with racetype on typeId for numberOf runs

                //insert into splittime foreach racedata
                    //check if skier is disqualified for this race
                        // if so calculate random splittime of random run when he is disqualified
                        //insert numberOf runs times
                            //insert number of splittimes
                                //get splittime with random variance
            
            JoinRaceDataAndRace();
        }

        private void JoinRaceDataAndRace()
        {
            var raceDataList = new List<RaceData>(adoRaceDataDao.FindAll());
            var raceList = new List<Race>(adoRaceDao.FindAll());

            var raceDataJoinRace = from raceData in raceDataList
                                   join race in raceList
                                   on raceData.Race equals race
                                   select new
                                   {
                                       Id = raceData.Id,
                                       RaceId = race.Id,
                                       SkierId = raceData.SkierId,
                                       Disqualified = raceData.Disqualified
                                   };
            foreach (var item in raceDataJoinRace)
            {
                Console.WriteLine($"{item.Id},\t{item.RaceId},\t{item.SkierId},\t{item.Disqualified}");
            }
        }
    }
}
