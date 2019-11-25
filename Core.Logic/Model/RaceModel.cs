using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.Logic.Model
{
    public class RaceModel
    {
        private IConnectionFactory connectionFactory;
        private AdoRaceDao raceDao;
        
        public ObservableCollection<Race> Races { get; set; }
        
        public RaceModel()
        {
            var configuration = ConfigurationUtil.GetConfiguration();
            connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
        }
        
        public ObservableCollection<Race> GetRaces()
        {
            Races = new ObservableCollection<Race>(new AdoRaceDao(connectionFactory).FindAll());
            return Races;
        }
    }
}