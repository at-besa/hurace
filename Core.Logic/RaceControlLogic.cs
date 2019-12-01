using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic
{
    public class RaceControlLogic : IRaceControlLogic
    {
        private IConnectionFactory connectionFactory;
        public ICollection<RaceControlModel> RaceControls { get; set; }
        
        public RaceControlLogic()
        {
            var configuration = ConfigurationUtil.GetConfiguration();
            connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
        }
        
        public async Task<ICollection<RaceControlModel>> GetRaceControls()
        {
            return await Task.Run(() => {  
                RaceControls = new Collection<RaceControlModel>();
                var racecollection = new AdoRaceDao(connectionFactory).FindAll();
                foreach (var race in racecollection)
                {
                    RaceControls.Add(new RaceControlModel
                    {
                        Race = race
                    });
                }

                return RaceControls;
            });
        }
        
        public async Task<bool> DeleteRace(int raceId)
        {
            return await Task.Run(() =>
            {
                var deleted = new AdoRaceDao(connectionFactory).Delete(raceId);

                return deleted;
            });
        }
        
    }
}