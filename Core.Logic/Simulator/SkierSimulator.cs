using System;
using System.Threading;
using System.Threading.Tasks;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic.Simulator
{
    public class SkierSimulator
    {
	    private RaceControlLogic raceControlLogic;
        public int[] BaseTimeForRaceType { get; set; }
        private int raceId;
        private bool start;

        public SkierSimulator()
        {
            BaseTimeForRaceType = new []
            {
                1,
                1,
                1,
                1,
                1,
                1
            };

            //BaseTimeForRaceType = new[]
            //{
	           // 10,
	           // 8,
	           // 12,
	           // 11,
	           // 9,
	           // 7
            //};
        }

        public void Start(bool start, int raceid)
        {
	        raceControlLogic = RaceControlLogic.Instance;
            this.raceId = raceid;
	        this.start = start;
        }
        
        public async void GenerateSplittimesForSkierRun(int raceTypeId,int raceDataId, int runNo, int splittimeNo)
        {
	        await Task.Run(() =>
	        {
		        var random = new Random();
		        DateTime splittime = new DateTime();
		        for (int i = 1; i <= splittimeNo; i++)
		        {
			        var val = GetBaseSplittimeForRaceType(i, raceTypeId);
			        splittime = splittime.Add(new TimeSpan(0, 0, 0, val, i == 1 ? 0 : random.Next(0, 999)));
                    Thread.Sleep(splittime.Millisecond + (val*100));
                    if (!raceControlLogic.InsertNewSplittime(new SplitTimeModel()
                    {
	                    Time = splittime,
	                    RunNo = runNo,
	                    RaceDataId = raceDataId,
	                    SplitTimeNo = i
                    }))
                    {
                        break;
                    }
		        }

            });

        }
        
        private int GetBaseSplittimeForRaceType(int splittimenumber, int racetype)
        {
	        return splittimenumber == 1 ? 0 : BaseTimeForRaceType[racetype] * splittimenumber-1;
        }
    }
}