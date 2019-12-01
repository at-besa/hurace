using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
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

        public async Task<ICollection<StartListModel>> GetStartLists()
        {
            return await Task.Run(() => {  StartLists = new Collection<StartListModel>();
                var racecollection = new AdoRaceDao(connectionFactory).FindAll();
                var startlists = new AdoStartListDao(connectionFactory).FindAll();
                foreach (var race in racecollection)
                {
                    foreach (var startlist in startlists)
                    {
                        StartLists.Add(new StartListModel
                        {
                            Location = race.Location,
                            StartPos = startlist.StartPos,
                            Name = race.Name,
                            Sex = race.Sex,
                            Splittimes = race.Splittimes,
                            Status = "running",
                            Type = race.Type.Type
                        });
                        
                    }
         
                }

                return StartLists;
            });
        }
    }
}