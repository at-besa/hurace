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
	public class RaceManagementLogic : IRaceLogic
	{
		public static RaceManagementLogic Instance = new RaceManagementLogic();
		private readonly IConnectionFactory connectionFactory;
		private ICollection<RaceModel> Races { get; set; }
		private ICollection<string> RaceTypes { get; set; } 
		private ICollection<string> RaceStates { get; set; }


		private RaceManagementLogic()
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
					Races.Add(new RaceModel(race));
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
				race.Type = new AdoRaceTypeDao(connectionFactory).FindAll().FirstOrDefault(type => type.Type == race.Type.Type);
				race.Status = new AdoStatusDao(connectionFactory).FindAll().FirstOrDefault(status => status.Name == race.Status.Name);

				var saved= new AdoRaceDao(connectionFactory).Update(race.ToRace());

				return saved;
			});
		}
		
		public async Task<bool> CreateRace(RaceModel race)
		{
			return await Task.Run(() =>
			{
				race.Type = new AdoRaceTypeDao(connectionFactory).FindAll().FirstOrDefault(type => type.Type == race.Type.Type);
				race.Status = new AdoStatusDao(connectionFactory).FindAll().FirstOrDefault(status => status.Name == race.Status.Name);

				var created = new AdoRaceDao(connectionFactory).Insert(race.ToRace());
				race.Id = created;
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
		
		public async Task<RaceModel> GetRunningRace()
		{
			Races = await GetRaces();
			var runningRaceModel = Races.FirstOrDefault(raceModel => raceModel.Status.Name.Equals("running"));

			return runningRaceModel;
		}
	}
}