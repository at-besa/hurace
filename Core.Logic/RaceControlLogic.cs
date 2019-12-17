using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
        private readonly RaceManagementLogic raceManagementLogic = RaceManagementLogic.Instance;
        public RaceControlModel RaceControlModel { get; private set; }
        
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
        
    }
}