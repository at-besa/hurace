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
	public class RaceLogic : IRaceLogic
	{
		private IConnectionFactory connectionFactory;
		private ICollection<RaceModel> Races { get; set; }
		private ICollection<string> RaceTypes { get; set; } 
		private ICollection<string> RaceStates { get; set; }


		public RaceLogic()
		{
			var configuration = ConfigurationUtil.GetConfiguration();
			connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
		}

		public async Task<ICollection<RaceModel>> GetRaces()
		{
			return await Task.Run(() =>
			{
				Races = new Collection<RaceModel>();
				var racecollection = new AdoRaceDao(connectionFactory).FindAll();
				foreach (var race in racecollection)
				{
					Races.Add(new RaceModel
					{
						Race = race
					});
				}

				return Races;
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

		public async Task<bool> SaveRace(RaceModel race)
		{
			return await Task.Run(() =>
			{
				race.Race.Type = new AdoRaceTypeDao(connectionFactory).FindAll().FirstOrDefault(type => type.Type == race.Race.Type.Type);
				race.Race.Status = new AdoStatusDao(connectionFactory).FindAll().FirstOrDefault(status => status.Name == race.Race.Status.Name);

				var saved = new AdoRaceDao(connectionFactory).Update(race.Race);

				return saved;
			});
		}
		
		public async Task<bool> CreateRace(RaceModel race)
		{
			return await Task.Run(() =>
			{
				race.Race.Type = new AdoRaceTypeDao(connectionFactory).FindAll().FirstOrDefault(type => type.Type == race.Race.Type.Type);
				race.Race.Status = new AdoStatusDao(connectionFactory).FindAll().FirstOrDefault(status => status.Name == race.Race.Status.Name);

				var created = new AdoRaceDao(connectionFactory).Insert(race.Race);
				race.Race.Id = created;
				return created > 0;
			});
		}

		public async Task<ICollection<string>> GetRaceTypes()
		{
			return await Task.Run(() =>
			{
				var raceTypes = new AdoRaceTypeDao(connectionFactory).FindAll();
				RaceTypes = new Collection<string>();
				foreach (var raceType in raceTypes)
				{
					RaceTypes.Add(raceType.Type);
				}

				return RaceTypes;
			});
		}

		public async Task<ICollection<string>> GetRaceStates()
		{
			return await Task.Run(() =>
			{
				var raceStates = new AdoStatusDao(connectionFactory).FindAll();
				RaceStates = new Collection<string>();
				foreach (var raceState in raceStates)
				{
					RaceStates.Add(raceState.Name);
				}
				return RaceStates;
			});
		}
	}
}