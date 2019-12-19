﻿using System;
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
        }

        public async Task<StartListModel> GetStartListForRaceId(int raceId)
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

                IEnumerable<StartList> startListMembers = 
                    new AdoStartListDao(connectionFactory)
                    .FindAllByRaceId(raceId)
                    .OrderBy(startListMember => startListMember.StartPos);

                foreach (var startListMember in startListMembers)
                {
                    var skierdata = racedata.FirstOrDefault(data => data.SkierId == startListMember.SkierId);
                    
                    StartList.StartListMembers.Add(
                        new StartListMemberModel()
                        {
                            Skier = new SkierModel(skiers.FirstOrDefault(skier => skier.Id == startListMember.SkierId)),
                            Startposition = startListMember.StartPos,
                            Blocked = skierdata != null && skierdata.Blocked,
                            Disqualified = skierdata != null && skierdata.Disqualified,
                            Finished = skierdata != null && skierdata.Finished,
                            Running = skierdata != null && skierdata.Running
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

        public async Task<bool> UpdateSkierStartPos(int raceId, int pos, int skierId)
        {
            return await Task.Run(() =>
            {
                var startlistAdo = new AdoStartListDao(connectionFactory);
                
                var startList = new StartList
                {
                    Race = new AdoRaceDao(connectionFactory).FindById(raceId),
                    SkierId = skierId,
                    StartPos = pos
                };

                var result = startlistAdo.Update(startList);
                
                return result;
            });
        }
    }
}