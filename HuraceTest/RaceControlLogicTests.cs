using Xunit;
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
	public class RaceControlFixture : IDisposable
	{
		public RaceControlFixture() { }
		public void Dispose() { }
	}

	public class RaceControlLogicTests : IClassFixture<RaceManagementFixture>
	{
		private IRaceDao raceDao;
		private IRaceTypeDao raceTypeDao;
		private IStatusDao statusDao;

		RaceControlLogic rcl = RaceControlLogic.Instance;

		public RaceControlLogicTests(RaceManagementFixture data)
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

			// rcl.MockSetup(raceDao, raceTypeDao, statusDao);
		}


		[Fact()]
		public void GetRacesTest()
		{
			// var races = await rcl.GetRaces();
			// Assert.True(races.Count == 2);
			// Assert.True(races.First().Id == 1);
		}

		[Fact()]
		public void DeleteRaceTest()
		{
			// Assert.True(await rcl.DeleteRace(1));
		}

		[Fact()]
		public void SaveRaceTest()
		{
			;
			// Assert.True(await rcl.SaveRace(new RaceModel(ObjectGenerator.GetRace(1))));
		}

		[Fact()]
		public  void CreateRaceTest()
		{
			// Assert.True(await rcl.CreateRace(new RaceModel(ObjectGenerator.GetRace(3))));
		}

		[Fact()]
		public void GetRaceTypesTest()
		{
			// var types = await rcl.GetRaceTypes();
			// Assert.True(types.Count == 2);
		}

		[Fact()]
		public void GetRaceStatesTest()
		{
			// var states = await rcl.GetRaceStates();
			// Assert.True(states.Count == 2);
		}

		[Fact()]
		public void GetRunningRaceTest()
		{
			// var race = await rcl.GetRunningRace();
			// Assert.True(race != null);
		
		}
	}
}