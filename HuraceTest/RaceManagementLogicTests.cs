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
using HuraceTest;
using Moq;

namespace Hurace.Core.Logic.Tests
{
	public class RaceManagementFixture : IDisposable
	{
		private readonly IConnectionFactory connectionFactory;

		public RaceManagementFixture()
		{
			var configuration = ConfigurationUtil.GetConfiguration();
			connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");


			var raceDaoMock = new Mock<IRaceDao>();
			raceDaoMock.Setup(dao => dao.FindAll()).Returns(
				new List<Race>()
				{
					ObjectGenerator.GetRace(1),
					ObjectGenerator.GetRace(2)
				});
			
			var raceTypeMockDao = new Mock<IRaceTypeDao>();
			raceTypeMockDao.Setup(dao => dao.FindAll()).Returns(
				new List<RaceType>()
				{
					ObjectGenerator.GetRaceType(),
					ObjectGenerator.GetRaceType()
				});
		}

		public void Dispose()
		{
			// Do "global" teardown here; Only called once.
		}
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
			raceDaoMock.Setup(dao => dao.FindAll()).Returns(
				new List<Race>()
				{
					ObjectGenerator.GetRace(1),
					ObjectGenerator.GetRace(2)
				});
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

			//raceTypeDao = new AdoRaceTypeDao(connectionFactory);
			//statusDao = new AdoStatusDao(connectionFactory);

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
		public void DeleteRaceTest()
		{
			Assert.True(false, "This test needs an implementation");
		}

		[Fact()]
		public void SaveRaceTest()
		{
			Assert.True(false, "This test needs an implementation");
		}

		[Fact()]
		public void CreateRaceTest()
		{
			Assert.True(false, "This test needs an implementation");
		}

		[Fact()]
		public void GetRaceTypesTest()
		{
			Assert.True(false, "This test needs an implementation");
		}

		[Fact()]
		public void GetRaceStatesTest()
		{
			Assert.True(false, "This test needs an implementation");
		}

		[Fact()]
		public void GetRunningRaceTest()
		{
			Assert.True(false, "This test needs an implementation");
		}
	}
}