using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic
{
    public class RaceLogic : IRaceLogic
    {
        private IConnectionFactory connectionFactory;
        public ICollection<RaceModel> Races { get; set; }
        
        public RaceLogic()
        {
            var configuration = ConfigurationUtil.GetConfiguration();
            connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
        }
        
        public ICollection<RaceModel> GetRaces()
        {
            Races = new Collection<RaceModel>();
            var racecollection = new AdoRaceDao(connectionFactory).FindAll();
            foreach (var race in racecollection)
            {
                Races.Add(new RaceModel
                {
                    Date = race.Date,
                    Location = race.Location,
                    Name = race.Name,
                    Sex = race.Sex,
                    Splittimes = race.Splittimes,
                    Status = "running",
                    Type = race.Type.Type
                });
            }
            
            return Races;
        }
    }
}