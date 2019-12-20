using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.DAL.Interface;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic
{
	public class RaceManagementLogic : IRaceManagementLogic
	{
		public static RaceManagementLogic Instance = new RaceManagementLogic();
		private readonly IConnectionFactory connectionFactory;
		private ICollection<RaceModel> Races { get; set; }
		private ICollection<string> RaceTypes { get; set; } 
		private ICollection<string> RaceStates { get; set; }
		private IRaceDao raceDao;
		private IRaceTypeDao raceTypeDao;
		private IStatusDao statusDao;
		
		private RaceManagementLogic()
		{
			var configuration = ConfigurationUtil.GetConfiguration();
			connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
			raceDao = new AdoRaceDao(connectionFactory);
			raceTypeDao = new AdoRaceTypeDao(connectionFactory);
			statusDao = new AdoStatusDao(connectionFactory);
		}

		public void MockSetup(IRaceDao rd, IRaceTypeDao rtd, IStatusDao sd)
		{
			raceDao = rd;
			raceTypeDao = rtd;
			statusDao = sd;
		}
		
		public async Task<ICollection<RaceModel>> GetRaces()
		{
			return await Task.Run(() =>
			{
				Races = new Collection<RaceModel>();
				var racecollection = raceDao.FindAll();
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
				var deleted = raceDao.Delete(raceId);

				return deleted;
			});
		}

		public async Task<bool> SaveRace(RaceModel race)
		{
			return await Task.Run(() =>
			{
				race.Type = raceTypeDao.FindAll().FirstOrDefault(type => type.Type == race.Type.Type);
				race.Status = statusDao.FindAll().FirstOrDefault(status => status.Name == race.Status.Name);

				var saved= raceDao.Update(race.ToRace());

				return saved;
			});
		}
		
		public async Task<bool> CreateRace(RaceModel race)
		{
			return await Task.Run(() =>
			{
				race.Type = raceTypeDao.FindAll().FirstOrDefault(type => type.Type == race.Type.Type);
				race.Status = statusDao.FindAll().FirstOrDefault(status => status.Name == race.Status.Name);

				var created = raceDao.Insert(race.ToRace());
				race.Id = created;
				return created > 0;
			});
		}

		public async Task<ICollection<string>> GetRaceTypes()
		{
			return await Task.Run(() =>
			{
				var raceTypes = raceTypeDao.FindAll();
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
				var raceStates = statusDao.FindAll();
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