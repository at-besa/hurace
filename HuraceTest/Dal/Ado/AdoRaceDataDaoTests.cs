using System.Linq;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace HuraceTest.Dal.Ado
{
	public class AdoRaceDataDaoTests
	{
		private IConfiguration configuration;
		private IConnectionFactory connectionFactory;
		private AdoRaceDataDao raceDataDao;
		[SetUp]
		public void Setup()
		{
			configuration = ConfigurationUtil.GetConfiguration();
			connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
			raceDataDao = new AdoRaceDataDao(connectionFactory);
		}
		[Test]
		public void AdoRaceDataDaoTest()
		{
			Assert.True(raceDataDao.FindAll().Any());
		}

		[Test]
		public void UpdateTest()
		{
			RaceData testRaceData = raceDataDao.FindById(3);
			testRaceData.SkierId = 45;
			raceDataDao.Update(testRaceData);
			
			Assert.True(raceDataDao.FindById(3).SkierId == 45);
		}

		[Test]
		public void InsertTest()
		{
			//RaceData raceData = new RaceData
			//{
			//	Disqualified = false,
			//	RaceId = new AdoRaceDao(connectionFactory).FindById(15),
			//	SplitTime = new []{
			//		new AdoSplitTimeDao(connectionFactory).FindByRaceDataId(45, 1), 
			//		new AdoSplitTimeDao(connectionFactory).FindByRaceDataId(45, 2)},
			//	SkierId = 5
			//};
			//raceData.SkierId = 45;
			//raceData.Id = raceDataDao.Insert(raceData);
			//Assert.True(raceDataDao.FindById(raceData.Id).Id == raceData.Id);

		}

		[Test]
		public void FindAllTest()
		{
			Assert.True(raceDataDao.FindAll().Any());
		}

		[Test]
		public void FindByIdTest()
		{
			Assert.True(raceDataDao.FindById(4).Id == 4);
		}
	}

}