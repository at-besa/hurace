﻿using Hurace.Dal.Ado;
using Hurace.Dal.Common;
using Hurace.Dal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hurace.Dal.Importer
{
    class RaceDataImporter
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
            if(AdoRaceDataDao.FindAll().Count() != 0)
            {
                throw new Exception("Already data in RaceData");
            }
            RaceDatas = GenerateRaceDatas();
            foreach (var raceData in RaceDatas)
            {
                Console.WriteLine($"Inserting worked: {AdoRaceDataDao.Insert(raceData)} for: {raceData}"); 
            }
        }

        private IEnumerable<RaceData> GenerateRaceDatas()
        {
            var startLists = AdoStartListDao.FindAll();
            var raceDatas = new List<RaceData>();

            if (startLists.Count() == 0)
            {
                throw new Exception("No StartList data in database to generate RaceData");
            }

            foreach (var startList in startLists)
            {
                raceDatas.Add(new RaceData { 
                    Race = startList.Race,
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
