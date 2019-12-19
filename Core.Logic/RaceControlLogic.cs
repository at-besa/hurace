using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic
{
    public class RaceControlLogic : IRaceControlLogic
    {
        public static RaceControlLogic Instance { get; } = new RaceControlLogic();
        private readonly StartListLogic startListLogic = StartListLogic.Instance;
        private readonly RaceManagementManagementLogic raceManagementManagementLogic = RaceManagementManagementLogic.Instance;
        public RaceControlModel RaceControlModel { get; private set; }

        private ICollection<SplittimeModel> WinnerSplitimes { get; set; }
        private ICollection<SplittimeModel> ActualSplitimes { get; set; } 
        private ICollection<SplittimeModel> LastSplitimes { get; set; } 
        
        
        private IConnectionFactory connectionFactory;
        
        
        private RaceControlLogic()
        {
            var configuration = ConfigurationUtil.GetConfiguration();
            connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
            
        }
        
        public async Task<RaceControlModel> GetRaceControlForRaceId(int raceId)
        {
            return await Task.Run(async () =>
            {
                var race = new AdoRaceDao(connectionFactory).FindById(raceId);
                var startlistmodel = await startListLogic.GetStartListForRaceId(raceId);
                
                RaceControlModel = new RaceControlModel
                {
                    RaceModel = new RaceModel(race),
                    StartListModel = startlistmodel
                };

                return RaceControlModel;
            });
        }

        public async Task<bool> StartRun(StartListMemberModel slm, int raceId)
        {
            return await Task.Run(() =>
            {
                //if (RaceControlModel.StartListModel.StartListMembers.FirstOrDefault(model => !model.Blocked) == null)
                //{
                    slm.Blocked = false;
                    return Update(slm, raceId);
                //}
                return false;
            });
        }

        private bool Update(StartListMemberModel slm, int raceId)
        {
            var raceData = new RaceData
            {
                RaceId = raceId,
                SkierId = slm.Skier.Id,
                Disqualified = slm.Disqualified,
                Running = slm.Running,
                Blocked = slm.Blocked,
                Finished = slm.Finished
            };
                    
            return new AdoRaceDataDao(connectionFactory).Update(raceData);
        }

        private void EvaluateWinnerSplittimes()
        {
            if (ActualSplitimes != null)
            {
	            if (LastSplitimes == null)
		            WinnerSplitimes = ActualSplitimes;
	            else
	            {
		            if (ActualSplitimes.Last().Time < WinnerSplitimes.Last().Time && ActualSplitimes.Count == RaceControlModel.RaceModel.Splittimes)
		            {
			            WinnerSplitimes = ActualSplitimes;
		            }
                }
		        LastSplitimes = ActualSplitimes;
	        }
        }

        public async Task<ICollection<SplittimeModel>> GetSplittimesForSkier(int skierId, int runNo)
        {
            return await Task.Run(() =>
            {
                var raceDatas = new AdoRaceDataDao(connectionFactory).FindAllBySkierId(skierId);
                var raceDataForThisRaceRun =
                    raceDatas.FirstOrDefault(data => data.RaceId == RaceControlModel.StartListModel.raceId);


                if (raceDataForThisRaceRun != null)
                {
                    var splittimes = new AdoSplittimeDao(connectionFactory).FindByRaceDataId(raceDataForThisRaceRun.Id).Where(splittime => splittime.RunNo == runNo);

                    EvaluateWinnerSplittimes();

                    ActualSplitimes = new ObservableCollection<SplittimeModel>();
                    foreach (var splittime in splittimes)
                    {
                        ActualSplitimes.Add(new SplittimeModel
                        {
                            RaceDataId = splittime.RaceDataId,
                            RunNo = splittime.RunNo,
                            SplittimeNo = splittime.SplittimeNo,
                            Time = splittime.Time,
                            TimeOffsetToWinner = WinnerSplitimes != null ? WinnerSplitimes.FirstOrDefault(model => model.SplittimeNo == splittime.SplittimeNo).Time - splittime.Time : splittime.Time- splittime.Time
                        });
                    }
                }

                return ActualSplitimes;
            });
        }

    }
}