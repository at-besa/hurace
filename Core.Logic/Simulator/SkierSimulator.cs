using System;

namespace Hurace.Core.Logic.Simulator
{
    public class SkierSimulator
    {
        RaceControlLogic raceControlLogic = RaceControlLogic.Instance;
        public DateTime[] BaseTimeForRaceType { get; set; }
        
        public SkierSimulator()
        {
            BaseTimeForRaceType = new DateTime[]
            {
                new DateTime().AddSeconds(30).AddMilliseconds(300),
                new DateTime().AddSeconds(28).AddMilliseconds(100),
                new DateTime().AddSeconds(32).AddMilliseconds(800),
                new DateTime().AddSeconds(35).AddMilliseconds(800),
                new DateTime().AddSeconds(29).AddMilliseconds(500),
                new DateTime().AddSeconds(25).AddMilliseconds(500)
            };
            
        }

        public void Start(bool start, int raceid)
        {


//            raceControlLogic.InsertNewSplittime();
        }
        
        private DateTime GetCorrectSplittime(int raceTypeId, int runNo, int splittimeNo)
        {
            var random = new Random();
            DateTime splittime = GetBaseSplittimeForRaceType(raceTypeId).AddSeconds(runNo);
            for (int i = 1; i < splittimeNo; i++)
            {
                splittime = splittime.Add(new TimeSpan(0,0,0, 
                    GetBaseSplittimeForRaceType(raceTypeId).AddSeconds(runNo).Second, 
                    GetBaseSplittimeForRaceType(raceTypeId).AddSeconds(runNo).Millisecond + random.Next(0, 1501)));
            }
            return splittime;
        }
        
        private DateTime GetBaseSplittimeForRaceType(int raceTypeId)
        {
            return BaseTimeForRaceType[raceTypeId];
        }
    }
}