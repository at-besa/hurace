using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Schema;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;
using Hurace.Core.Logic.Simulator;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System.Windows.Threading;

namespace Hurace.Core.Logic
{
    public class RaceControlLogic : IRaceControlLogic
    {
        public static RaceControlLogic Instance { get; } = new RaceControlLogic();
        private readonly StartListLogic startListLogic = StartListLogic.Instance;
        private readonly RaceManagementLogic raceManagementLogic = RaceManagementLogic.Instance;
        public RaceControlModel RaceControlModel { get; private set; }
        private StartListMemberModel actualStartListMember { get; set; }

        private ICollection<SplitTimeModel> WinnerSplitimes { get; set; }
        private ICollection<SplitTimeModel> ActualSplitimes { get; set; } 
        private ICollection<SplitTimeModel> LastSplitimes { get; set; } 
        private bool SimulatorActivated { get; set; }
        
        private SkierSimulator simulator = new SkierSimulator();
        
        private IConnectionFactory connectionFactory;

        RaceData raceDataForThisRaceRun;

        private RaceControlLogic()
        {
            var configuration = ConfigurationUtil.GetConfiguration();
            connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
            
        }
        
        public async Task<bool> SimulatorOnOff(bool onOff, int raceId)
        {
            return await Task.Run(() =>
            {
                SimulatorActivated = onOff;
                simulator.Start(SimulatorActivated, raceId);

                return SimulatorActivated;
            });
        }


        public async Task<RaceControlModel> GetRaceControlForRaceId(int raceId, int runNo)
        {
            return await Task.Run(async () =>
            {
                var race = new AdoRaceDao(connectionFactory).FindById(raceId);
                var startlistmodel = await startListLogic.GetStartListForRaceId(raceId, 1);
                if(runNo == 0)
		            runNo = await EvaluateRunNo(startlistmodel);


                RaceControlModel = new RaceControlModel
                {
                    RaceModel = new RaceModel(race){ActualRun = runNo},
                    StartListModel = startlistmodel
                };


                return RaceControlModel;
            });
        }

        public async Task<StartListModel> SortStartListforSecondRun()
        {
	        return await Task.Run(() =>
	        {

		        foreach (var startListMemberModel in RaceControlModel.StartListModel.StartListMembers)
		        {
			        startListMemberModel.Running = false;
			        startListMemberModel.Blocked = true;
			        startListMemberModel.Finished = false;
			        startListMemberModel.RunNo = 2;
			        Update(startListMemberModel, RaceControlModel.RaceModel.Id);
		        }

		        var newStartList = new ObservableCollection<StartListMemberModel>(RaceControlModel.StartListModel.StartListMembers.OrderBy(model => new AdoSplitTimeDao(connectionFactory).FindByIds(model.RaceDataId, 1, RaceControlModel.RaceModel.Splittimes - 1)?.Time ?? DateTime.Now));
		        for (var i = 0; i < newStartList.Count; i++)
		        {
			        var startListMemberModel = newStartList[i];
			        startListMemberModel.Startposition = i+1;
			        if (startListMemberModel.Disqualified)
			        {
				        newStartList.Move(i, newStartList.Count-1);
				        startListMemberModel.Startposition = 0;
			        }

		        }

		        RaceControlModel.StartListModel.StartListMembers = newStartList;
		        

		        return RaceControlModel.StartListModel;
	        });
        }


        public async Task SetRaceFinished()
        {
	        await Task.Run(() =>
	        {
                RaceControlModel.RaceModel.Status = new Status()
                {
                    Id = 4,
                    Name = "finished"
                };
		        var race = RaceControlModel.RaceModel.ToRace();

                var fdbbck = new AdoRaceDao(connectionFactory).Update(race);

            });
        }



        private async Task<int> EvaluateRunNo(StartListModel slm)
        {
	        return await Task.Run(() =>
	        {
		        bool run1StillRunning = false;
		        foreach (var member in slm.StartListMembers)
		        {
			        if (!(member.Finished || member.Disqualified))
			        {
                        run1StillRunning = true;
				        break;
                    }
		        }
		        return run1StillRunning ? 1 : 2;
	        });
        }

        public async Task<bool> StartRun(StartListMemberModel slm, int raceId)
        {
            return await Task.Run(() =>
            {
	            if (!slm.Finished && !slm.Disqualified && !slm.Running)
	            {
		            slm.Blocked = false;
		            actualStartListMember = slm;
		            if (SimulatorActivated)
		            {
			            if (raceDataForThisRaceRun != null)
				            simulator.GenerateSplittimesForSkierRun(RaceControlModel.RaceModel.Type.Id, raceDataForThisRaceRun.Id, slm.RunNo, RaceControlModel.RaceModel.Splittimes);
		            }
		            return Update(slm, raceId);

                }
	            return false;
            });
        }

        public async Task<bool> Clearance(StartListMemberModel slm, int raceId)
        {
	        return await Task.Run(() =>
	        {
		        var member = RaceControlModel.StartListModel.StartListMembers.FirstOrDefault(model => model.Startposition == slm.Startposition+1);
		        if (member != null)
		        {
                    member.Blocked = false;
			        return Update(member, raceId);
                }

		        return false;
	        });
        }
        public async Task<bool> Disqualify(StartListMemberModel slm, int raceId)
        {
	        return await Task.Run(() =>
	        {
		        if (slm != null)
		        {
			        slm.Disqualified = true;
			        slm.Running = false;
			        return Update(slm, raceId);
		        }

		        return false;
	        });
        }
        private bool Update(StartListMemberModel slm, int raceId)
        {
            var raceData = new RaceData
            {
                Id = slm.RaceDataId,
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
		            if (ActualSplitimes.Count > 0 && ActualSplitimes.Last().Time < WinnerSplitimes.Last().Time && ActualSplitimes.Count == RaceControlModel.RaceModel.Splittimes)
		            {
			            WinnerSplitimes = ActualSplitimes;
		            }
                }
		        LastSplitimes = ActualSplitimes;
	        }
        }

        public bool InsertNewSplittime(SplitTimeModel splittime)
        {
            if (ActualSplitimes != null && splittime.RaceDataId == raceDataForThisRaceRun.Id)
            {
                splittime.TimeOffsetToWinner = GetTimeOffsetToWinner(splittime.Time, splittime.SplitTimeNo);

                Application.Current.Dispatcher?.BeginInvoke((Action)(() =>
                {
	                AddSplitTime(splittime);
                }));
                return true;
            }

            return false;
        }

        private void AddSplitTime(SplitTimeModel splitTime)
        {
	        if (!actualStartListMember.Disqualified)
	        {
		        ActualSplitimes.Add(splitTime);
                Task.Run(() => new AdoSplitTimeDao(connectionFactory).Insert(splitTime.ToSplitTime()));
            }

            if (splitTime.SplitTimeNo == 1 && !actualStartListMember.Disqualified)
	        {
		        actualStartListMember.Running = true;
                Update(actualStartListMember, RaceControlModel.RaceModel.Id);
            }

            if (splitTime.SplitTimeNo == RaceControlModel.RaceModel.Splittimes && actualStartListMember.Running && !actualStartListMember.Disqualified)
	        {
		        actualStartListMember.Running = false;
                actualStartListMember.Finished = true;
		        Update(actualStartListMember, RaceControlModel.RaceModel.Id);
	        }
        }

        private TimeSpan GetTimeOffsetToWinner(DateTime time, int splitTimeNo)
        {
	        EvaluateWinnerSplittimes();
            var first = WinnerSplitimes.FirstOrDefault(model => model.SplitTimeNo == splitTimeNo);
            if (first != null)
                return WinnerSplitimes != null ? first.Time - time : time - time;
            return time - time; // when no first skier was found
        }

        public async Task<ICollection<SplitTimeModel>> GetSplittimesForSkier(int skierId, int runNo)
        {
	        return await Task.Run(() =>
            {
                var raceDatas = new AdoRaceDataDao(connectionFactory).FindAllBySkierId(skierId);
                raceDataForThisRaceRun = raceDatas.FirstOrDefault(data => data.RaceId == RaceControlModel.StartListModel.raceId);

                ActualSplitimes = new ObservableCollection<SplitTimeModel>();

                if (raceDataForThisRaceRun != null)
                {
                    var splittimes = new AdoSplitTimeDao(connectionFactory).FindByRaceDataId(raceDataForThisRaceRun.Id).Where(splittime => splittime.RunNo == runNo);

                    EvaluateWinnerSplittimes();

                    foreach (var splittime in splittimes)
                    {
                        ActualSplitimes.Add(new SplitTimeModel
                        {
                            RaceDataId = splittime.RaceDataId,
                            RunNo = splittime.RunNo,
                            SplitTimeNo = splittime.SplittimeNo,
                            Time = splittime.Time,
                            TimeOffsetToWinner = GetTimeOffsetToWinner(splittime.Time ,splittime.SplittimeNo)
                        });
                    }
                }

                return ActualSplitimes;
            });
        }

    }
}