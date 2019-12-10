﻿using System.Collections.Generic;
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
		private IEnumerable<RaceType> RaceTypes { get; set; }
		private IEnumerable<Status> RaceStates { get; set; }


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
				var status = race.Race.Status;
				var type = race.Race.Type;
				
				race.Race.Status = RaceStates.First(s => s.Name == status.Name);
				race.Race.Type = RaceTypes.First(t => t.Type == type.Type);
				var saved = new AdoRaceDao(connectionFactory).Update(race.Race);

				return saved;
			});
		}

		public async Task<ICollection<string>> GetRaceTypes()
		{
			return await Task.Run(() =>
			{
				RaceTypes = new AdoRaceTypeDao(connectionFactory).FindAll();
				var racetypes = new Collection<string>();
				foreach (var racetype in RaceTypes)
				{
					racetypes.Add(racetype.Type);
				}

				return racetypes;
			});
		}

		public async Task<ICollection<string>> GetRaceStates()
		{
			return await Task.Run(() =>
			{
				RaceStates = new AdoStatusDao(connectionFactory).FindAll();
				var racestates = new Collection<string>();
				foreach (var state in RaceStates)
				{
					racestates.Add(state.Name);
				}

				return racestates;
			});
		}
	}
}