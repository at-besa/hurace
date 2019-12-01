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
        private IConnectionFactory connectionFactory;
        public RaceControlModel RaceControl { get; private set; }
        
        public RaceControlLogic()
        {
            var configuration = ConfigurationUtil.GetConfiguration();
            connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
        }
        
        public async Task<RaceControlModel> GetRaceControlForRaceId(int raceId)
        {
            return await Task.Run(() =>
            {
                RaceControl = new RaceControlModel
                {
                    Race = new AdoRaceDao(connectionFactory).FindById(raceId)
                };

                return RaceControl;
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
        
        public async Task<bool> SetRaceStatus(int raceId, int statusId)
        {
            return await Task.Run(() =>
            {
                var race = RaceControl.Race;
                race.Status = new AdoStatusDao(connectionFactory).FindById(statusId);
                
                var done = new AdoRaceDao(connectionFactory).Update(race);

                return done;
            });
        }

    }
}