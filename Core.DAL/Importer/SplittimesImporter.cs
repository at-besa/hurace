//using System;
//using System.Collections.Generic;
//using Hurace.Core.DAL.Ado;
//using Hurace.Core.DAL.Common;
//using Hurace.Core.DAL.Domain;
//
//namespace Hurace.Core.DAL.Importer
//{
//    class SplittimesImporter : IImporter
//    {
//        private AdoRaceDataDao adoRaceDataDao;
//        private AdoSplitTimeDao adoSplittimeDao;
//        private IList<SplitTime> Splittimes { get; set; } = new List<SplitTime>();
//
//        public DateTime[] BaseTimeForRaceType { get; set; }
//        public SplittimesImporter(IConnectionFactory connectionfactroy)
//        {
//            adoRaceDataDao = new AdoRaceDataDao(connectionfactroy);
//            adoSplittimeDao = new AdoSplitTimeDao(connectionfactroy);
//            BaseTimeForRaceType = new DateTime[]{
//                    new DateTime().AddSeconds(30).AddMilliseconds(300),
//                    new DateTime().AddSeconds(28).AddMilliseconds(100),
//                    new DateTime().AddSeconds(32).AddMilliseconds(800),
//                    new DateTime().AddSeconds(35).AddMilliseconds(800),
//                    new DateTime().AddSeconds(29).AddMilliseconds(500),
//                    new DateTime().AddSeconds(25).AddMilliseconds(500)
//            };
//        }
//        public void Import()
//        {
//            GenerateSplittimes();
//            foreach (var splittime in Splittimes)
//            {
//                //Console.WriteLine(splittime);
//                Console.WriteLine($"Inserting worked: {adoSplittimeDao.Insert(splittime)}"); 
//            }
//        }
//
//        private void GenerateSplittimes()
//        {
//            var raceDatas = new List<RaceData>(adoRaceDataDao.FindAll());
//            foreach (var raceData in raceDatas)
//            {
//                if (raceData.Disqualified)
//                {
//                    var random = new Random();
//                    var runs = random.Next(1, raceData.RaceId.Type.NumberOfRuns + 1);
//                    if (runs == 1)
//                    {
//                        var splittimes = random.Next(1, raceData.RaceId.Splittimes + 1);
//
//                        for (int splittimeNo = 1; splittimeNo <= splittimes; splittimeNo++)
//                        {
//                            var splittime = new SplitTime();
//                            splittime.RunNo = 1;
//                            splittime.RaceDataId = raceData.Id;
//                            splittime.SplittimeNo = splittimeNo;
//                            splittime.Time = GetCorrectSplittime(raceData.RaceId.Type.Id, 1, splittimeNo);
//                            Splittimes.Add(splittime);
//                        }
//                    }
//                    else
//                    {
//                        for (int runNo = 1; runNo <= raceData.RaceId.Type.NumberOfRuns; runNo++)
//                        {
//                            int splittimes;
//                            if(runNo == 1)
//                            {
//                                splittimes = raceData.RaceId.Splittimes;
//                            }
//                            else
//                            {
//                                splittimes = random.Next(1, raceData.RaceId.Splittimes + 1);
//                            }
//                            for (int splittimeNo = 1; splittimeNo <= splittimes; splittimeNo++)
//                            {
//                                var splittime = new SplitTime();
//                                splittime.RunNo = runNo;
//                                splittime.RaceDataId = raceData.Id;
//                                splittime.SplittimeNo = splittimeNo;
//                                splittime.Time = GetCorrectSplittime(raceData.RaceId.Type.Id, runNo, splittimeNo);
//                                Splittimes.Add(splittime);
//                            }
//
//                        }
//                    }
//
//                }
//                else
//                {
//                    for (int runNo = 1; runNo <= raceData.RaceId.Type.NumberOfRuns; runNo++)
//                    {
//                        for (int splittimeNo = 1; splittimeNo <= raceData.RaceId.Splittimes; splittimeNo++)
//                        {
//                            var splittime = new SplitTime();
//                            splittime.RunNo = runNo;
//                            splittime.RaceDataId = raceData.Id;
//                            splittime.SplittimeNo = splittimeNo;
//                            splittime.Time = GetCorrectSplittime(raceData.RaceId.Type.Id, runNo, splittimeNo);
//                            Splittimes.Add(splittime);
//                        }
//
//                    }
//
//                }
//
//
//            }
//
//        }
//
//        private DateTime GetCorrectSplittime(int raceTypeId, int runNo, int splittimeNo)
//        {
//            var random = new Random();
//            DateTime splittime = GetBaseSplittimeForRaceType(raceTypeId).AddSeconds(runNo);
//            for (int i = 1; i < splittimeNo; i++)
//            {
//                splittime = splittime.Add(new TimeSpan(0,0,0, 
//                    GetBaseSplittimeForRaceType(raceTypeId).AddSeconds(runNo).Second, 
//                    GetBaseSplittimeForRaceType(raceTypeId).AddSeconds(runNo).Millisecond + random.Next(0, 1501)));
//            }
//            return splittime;
//        }
//
//        private DateTime GetBaseSplittimeForRaceType(int raceTypeId)
//        {
//            return BaseTimeForRaceType[raceTypeId];
//        }
//    }
//}
