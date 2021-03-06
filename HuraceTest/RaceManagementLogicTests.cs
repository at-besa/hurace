﻿using Xunit;
using Hurace.Core.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.DAL.Interface;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;
using HuraceTest;
using Moq;

namespace Hurace.Core.Logic.Tests
{
	public class RaceManagementFixture : IDisposable
	{
		public RaceManagementFixture() { }
		public void Dispose() { }
	}

	public class RaceManagementLogicTests : IClassFixture<RaceManagementFixture>
	{
		private IRaceDao raceDao;
		private IRaceTypeDao raceTypeDao;
		private IStatusDao statusDao;

		RaceManagementLogic rcm = RaceManagementLogic.Instance;

		public RaceManagementLogicTests(RaceManagementFixture data)
		{
			var raceDaoMock = new Mock<IRaceDao>();
			raceDaoMock.Setup(dao => dao.FindAll()).Returns(new List<Race>() {ObjectGenerator.GetRace(1), ObjectGenerator.GetRace(2)});
			raceDaoMock.Setup(dao => dao.Delete(It.IsAny<int>())).Returns<int>(i => i > 0);
			raceDaoMock.Setup(dao => dao.Insert(It.IsAny<Race>())).Returns<Race>(race => 1 );
			raceDaoMock.Setup(dao => dao.Update(It.IsAny<Race>())).Returns<Race>(race => true);
			raceDaoMock.Setup(dao => dao.FindById(It.IsAny<int>())).Returns<Race>(race => ObjectGenerator.GetRace(race.Id));

			raceDao = raceDaoMock.Object;

			var raceTypeDaoMock = new Mock<IRaceTypeDao>();
			raceTypeDaoMock.Setup(dao => dao.FindAll()).Returns(
				new List<RaceType>()
				{
					ObjectGenerator.GetRaceType(),
					ObjectGenerator.GetRaceType()
				});
			
			raceTypeDao = raceTypeDaoMock.Object;

			var statusDaoMock = new Mock<IStatusDao>();
			statusDaoMock.Setup(dao => dao.FindAll()).Returns(
				new List<Status>()
				{
					ObjectGenerator.GetStatus(),
					ObjectGenerator.GetStatus()
				});
			statusDao = statusDaoMock.Object;

			rcm.MockSetup(raceDao, raceTypeDao, statusDao);
		}


		[Fact()]
		public async void GetRacesTest()
		{
			var races = await rcm.GetRaces();
			Assert.True(races.Count == 2);
			Assert.True(races.First().Id == 1);
		}

		[Fact()]
		public async void DeleteRaceTest()
		{
			Assert.True(await rcm.DeleteRace(1));
		}

		[Fact()]
		public async void SaveRaceTest()
		{
			;
			Assert.True(await rcm.SaveRace(new RaceModel(ObjectGenerator.GetRace(1))));
		}

		[Fact()]
		public async  void CreateRaceTest()
		{
			Assert.True(await rcm.CreateRace(new RaceModel(ObjectGenerator.GetRace(3))));
		}

		[Fact()]
		public async void GetRaceTypesTest()
		{
			var types = await rcm.GetRaceTypes();
			Assert.True(types.Count == 2);
		}

		[Fact()]
		public async void GetRaceStatesTest()
		{
			var states = await rcm.GetRaceStates();
			Assert.True(states.Count == 2);
		}

		[Fact()]
		public async void GetRunningRaceTest()
		{
			var race = await rcm.GetRunningRace();
			Assert.True(race != null);
		
		}
	}
}