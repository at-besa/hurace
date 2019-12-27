using System;
using System.Collections.Generic;
using System.Linq;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Importer
{
    class RaceDataImporter : IImporter
    {
        private const int DISQUALIFIED_PERCENTAGE = 80;
        private AdoRaceDataDao AdoRaceDataDao { get; set; }
        private AdoStartListDao AdoStartListDao { get; set; }
        private IEnumerable<RaceData> RaceDatas { get; set; }
        public RaceDataImporter(IConnectionFactory connectionFactory)
        {
            AdoRaceDataDao = new AdoRaceDataDao(connectionFactory);
            AdoStartListDao = new AdoStartListDao(connectionFactory);
        }

        public void Import()
        {
            if(!AdoRaceDataDao.FindAll().Any())
            {
                RaceDatas = GenerateRaceDatas();
                foreach (var raceData in RaceDatas)
                {
                    AdoRaceDataDao.Insert(raceData); 
                }
            }
        }

        private IEnumerable<RaceData> GenerateRaceDatas()
        {
            var startLists = AdoStartListDao.FindAll();
            var raceDatas = new List<RaceData>();

            if (startLists.Count() == 0)
            {
                throw new Exception("No StartListMember data in database to generate RaceData");
            }

            foreach (var startList in startLists)
            {
                raceDatas.Add(new RaceData { 
                    RaceId = startList.Race.Id,
                    SkierId = startList.SkierId,
                    Disqualified = decideIfDisqualifiedByPercentage(DISQUALIFIED_PERCENTAGE)
                });
            }
            return raceDatas;
        }

        private bool decideIfDisqualifiedByPercentage(int disqualifiedPercentage)
        {
            var random = new Random();
            var disqualified = false;
            if(random.Next(0, 101) > disqualifiedPercentage)
            {
                disqualified = true;
            }
            return disqualified;
        }
    }
}
