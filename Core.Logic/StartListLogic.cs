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
        private IConnectionFactory connectionFactory;
        public ICollection<StartListModel> StartLists { get; set; }
        
        public StartListLogic()
        {
            var configuration = ConfigurationUtil.GetConfiguration();
            connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
        }

        public async Task<ICollection<StartListModel>> GetStartListForRaceId(int raceId)
        {
            return await Task.Run(() => {  
                StartLists = new Collection<StartListModel>();
                
                var skiers = new AdoSkierDao(connectionFactory).FindAll();
                var startlist = new AdoStartListDao(connectionFactory).FindAllByRaceId(raceId);
                var raceDatas = new AdoRaceDataDao(connectionFactory).FindAllByRaceId(raceId); 
                
                
                foreach (var raceData in raceDatas)
                {
                    var skier = skiers.SingleOrDefault(s => s.Id == raceData.SkierId);
                    var start = startlist.SingleOrDefault(sl => sl.SkierId == skier?.Id);
                    
                    // TODO
//                    StartLists.Add(new StartListModel
//                    {
//                        Location = raceData.RaceId.Location,
//                        StartPos = start.StartPos,
//                        Name = raceData.RaceId.Name,
//                        Sex = raceData.RaceId.Sex,
//                        RaceType = raceData.RaceId.Type.Type,
//                        Skier = $"{skier?.FirstName} {skier?.LastName}",
//                    });
                }
                return StartLists;
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