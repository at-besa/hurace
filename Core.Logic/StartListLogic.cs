using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic
{
    public class StartListLogic : IStartListLogic
    {
        public static StartListLogic Instance = new StartListLogic();
        private IConnectionFactory connectionFactory;
        public StartListModel StartList { get; set; }
        
        private StartListLogic()
        {
            var configuration = ConfigurationUtil.GetConfiguration();
            connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
            
            // AdoSplitTimeDao std = new AdoSplitTimeDao(connectionFactory);
            // var data = std.FindAll();
            //
            // foreach (var splitTime in data)
            // {
            //     var random = new Random();
            //     splitTime.Time = splitTime.Time.AddMilliseconds(random.Next(0, 999));
            //     std.Update(splitTime);
            //     Console.WriteLine($"Updated {splitTime}");
            // }

        }

        public async Task<StartListModel> GetStartListForRaceId(int raceId, int runNo)
        {
            return await Task.Run(() => {  
                StartList = new StartListModel();
                StartList.raceId = raceId;
                StartList.StartListMembers = new ObservableCollection<StartListMemberModel>();

                IEnumerable<Skier> skiers = new AdoSkierDao(connectionFactory).FindAll();
                IEnumerable<RaceData> racedata = new AdoRaceDataDao(connectionFactory).FindAllByRaceId(raceId);

                if (skiers == null)
                {
                    throw new NullReferenceException("No skiers found");
                }

                IEnumerable<StartListMember> startListMembers = 
                    new AdoStartListDao(connectionFactory)
                    .FindAllByRaceIdAndRunNo(raceId, runNo)
                    .OrderBy(startListMember => startListMember.StartPos);


                foreach (var startListMember in startListMembers)
                {
                    var skierdata = racedata.FirstOrDefault(data => data.SkierId == startListMember.SkierId);
                    
                    StartList.StartListMembers.Add(
                        new StartListMemberModel()
                        {
                            Skier = new SkierModel(skiers.FirstOrDefault(skier => skier.Id == startListMember.SkierId)),
                            Startposition = startListMember.StartPos,
                            RunNo = startListMember.RunNo,
                            Blocked = skierdata != null && skierdata.Blocked,
                            Disqualified = skierdata != null && skierdata.Disqualified,
                            Finished = skierdata != null && skierdata.Finished,
                            Running = skierdata != null && skierdata.Running,
                            RaceDataId = skierdata?.Id ?? 0
                        }
                    );
                    
                }

                return StartList;
            });
        }

        public async Task<ICollection<SkierModel>> GetAllSkiersWithSameSex(string sex)
        {
            return await Task.Run(() => {
                IEnumerable<Skier> allSkiers = new AdoSkierDao(connectionFactory).FindAll();
                IEnumerable<Skier> allSkiersWithSameSex = allSkiers.Where(skier => skier.Sex == sex);
                var allSkierModelsWithSameSex = new ObservableCollection<SkierModel>();
                foreach (var skierWithSameSex in allSkiersWithSameSex)
                {
                    allSkierModelsWithSameSex.Add(new SkierModel(skierWithSameSex));
                }
                return allSkierModelsWithSameSex;
            });
        }

        public async Task<bool> UpdateStartListMemberStartPosition(int raceId, int skierId, int runNo, int startPosition)
        {
            return await Task.Run(() =>
            {
                var startlistAdo = new AdoStartListDao(connectionFactory);
                var race = new Race
                {
                    Id = raceId
                };               
                var startList = new StartListMember
                {
                    Race = race,
                    SkierId = skierId,
                    RunNo = runNo,
                    StartPos = startPosition
                };

                var result = startlistAdo.Update(startList);
                
                return result;
            });
        }

        public async Task<bool> DeleteStartListMember(int raceId, int skierId, int runNo)
        {
            return await Task.Run(() =>
            {
                var startlistAdo = new AdoStartListDao(connectionFactory);
                var race = new Race
                {
                    Id = raceId
                };
                var startList = new StartListMember
                {
                    Race = race,
                    SkierId = skierId,
                    RunNo = runNo
                };
                var result = startlistAdo.Delete(startList);
                return result;
            });
        }

        public async Task<bool> InsertStartListMember(int raceId, int skierId, int runNo, int startPosition)
        {
            return await Task.Run(() =>
            {
                var startListAdo = new AdoStartListDao(connectionFactory);
                var race = new Race
                {
                    Id = raceId
                };
                var startListMember = new StartListMember
                {
                    Race = race,
                    SkierId = skierId,
                    RunNo = runNo,
                    StartPos = startPosition
                };
                var result = startListAdo.Insert(startListMember);
                return result != 0;
            });
        }

        public async Task<bool> IsStartListMemberInStartList(int raceId, int skierId, int runNo)
        {
            return await Task.Run(() => {
                var startListAdo = new AdoStartListDao(connectionFactory);
                StartListMember startListMemberMember = startListAdo.FindByIds(raceId, skierId, runNo);
                if(startListMemberMember != null)
                {
                    return true;
                }
                return false;
            });
        }
    }
}