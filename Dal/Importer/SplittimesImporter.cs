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
        private AdoSplittimeDao adoSplittimeDao;
        private IList<Splittime> Splittimes { get; set; } = new List<Splittime>();

        public DateTime[] BaseTimeForRaceType { get; set; }
        public SplittimesImporter(IConnectionFactory connectionfactroy)
        {
            adoRaceDataDao = new AdoRaceDataDao(connectionfactroy);
            adoSplittimeDao = new AdoSplittimeDao(connectionfactroy);
            BaseTimeForRaceType = new DateTime[]{
                    new DateTime().AddSeconds(30).AddMilliseconds(300),
                    new DateTime().AddSeconds(28).AddMilliseconds(100),
                    new DateTime().AddSeconds(32).AddMilliseconds(800),
                    new DateTime().AddSeconds(35).AddMilliseconds(800),
                    new DateTime().AddSeconds(29).AddMilliseconds(500),
                    new DateTime().AddSeconds(25).AddMilliseconds(500)
            };
        }
        public void Import()
        {
            GenerateSplittimes();
            foreach (var splittime in Splittimes)
            {
                Console.WriteLine(splittime);
                //adoSplittimeDao.Insert(splittime);
            }
        }

        private void GenerateSplittimes()
        {
            var raceDatas = new List<RaceData>(adoRaceDataDao.FindAll());
            foreach (var raceData in raceDatas)
            {
                if (raceData.Disqualified)
                {
                    // do not add all splitttimes
                    Console.WriteLine($"skier {raceData.SkierId} is disqualified in race: {raceData.Race.Id} ");
                    var random = new Random();
                    var runs = random.Next(1, raceData.Race.Type.NumberOfRuns + 1);
                    if (runs == 1)
                    {
                        var splittimes = random.Next(1, raceData.Race.Splittimes + 1);

                        for (int splittimeNo = 1; splittimeNo <= splittimes; splittimeNo++)
                        {
                            var splittime = new Splittime();
                            splittime.RunNo = 1;
                            splittime.RaceDataId = raceData.Id;
                            splittime.SplittimeNo = splittimeNo;
                            splittime.Time = GetCorrectSplittime(raceData.Race.Type.Id, 1, splittimeNo);
                            Splittimes.Add(splittime);
                        }
                    }
                    else
                    {
                        for (int runNo = 1; runNo <= raceData.Race.Type.NumberOfRuns; runNo++)
                        {
                            int splittimes;
                            if(runNo == 1)
                            {
                                splittimes = raceData.Race.Splittimes;
                            }
                            else
                            {
                                splittimes = random.Next(1, raceData.Race.Splittimes + 1);
                            }
                            for (int splittimeNo = 1; splittimeNo <= splittimes; splittimeNo++)
                            {
                                var splittime = new Splittime();
                                splittime.RunNo = runNo;
                                splittime.RaceDataId = raceData.Id;
                                splittime.SplittimeNo = splittimeNo;
                                splittime.Time = GetCorrectSplittime(raceData.Race.Type.Id, runNo, splittimeNo);
                                Splittimes.Add(splittime);
                            }

                        }
                    }

                }
                else
                {
                    for (int runNo = 1; runNo <= raceData.Race.Type.NumberOfRuns; runNo++)
                    {
                        for (int splittimeNo = 1; splittimeNo <= raceData.Race.Splittimes; splittimeNo++)
                        {
                            var splittime = new Splittime();
                            splittime.RunNo = runNo;
                            splittime.RaceDataId = raceData.Id;
                            splittime.SplittimeNo = splittimeNo;
                            splittime.Time = GetCorrectSplittime(raceData.Race.Type.Id, runNo, splittimeNo);
                            Splittimes.Add(splittime);
                        }

                    }

                }


            }

        }

        private DateTime GetCorrectSplittime(int raceTypeId, int runNo, int splittimeNo)
        {
            var random = new Random();
            DateTime splittime = GetBaseSplittimeForRaceType(raceTypeId).AddSeconds(runNo + 1);
            for (int i = 0; i < splittimeNo + 1; i++)
            {
                splittime.AddSeconds(splittime.Second);
                splittime.AddMilliseconds(splittime.Millisecond);
            }
            splittime.AddMilliseconds(random.Next(0, 1501));

            return splittime;
        }

        private DateTime GetBaseSplittimeForRaceType(int raceTypeId)
        {
            return BaseTimeForRaceType[raceTypeId];
        }
    }
}
