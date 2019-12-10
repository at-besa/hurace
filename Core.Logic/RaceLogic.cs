﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic
{
	public class RaceLogic : IRaceLogic
	{
		private IConnectionFactory connectionFactory;
		public ICollection<RaceModel> Races { get; set; }
		public ICollection<string> RaceTypes { get; set; }
		public ICollection<string> RaceStates { get; set; }


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
				var deleted = new AdoRaceDao(connectionFactory).Update(race.Race);

				return deleted;
			});
		}
		
		public async Task<ICollection<string>> GetRaceTypes()
		{
			return await Task.Run(() =>
			{
				RaceTypes = new Collection<string>();
				var racetypes = new AdoRaceTypeDao(connectionFactory).FindAll();
				foreach (var racetype in racetypes)
				{
					RaceTypes.Add(racetype.Type);
				}

				return RaceTypes;
			});
		}

		public async Task<IEnumerable<string>> GetRaceStates()
		{
			return await Task.Run(() =>
			{
				RaceStates = new Collection<string>();
				var racestates = new AdoStatusDao(connectionFactory).FindAll();
				foreach (var state in racestates)
				{
					RaceStates.Add(state.Name);
				}

				return RaceStates;
			});
		}
	}
}