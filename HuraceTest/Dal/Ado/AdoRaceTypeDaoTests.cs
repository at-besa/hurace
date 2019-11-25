using System.Linq;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace HuraceTest.Dal.Ado
{
	public class AdoRaceTypeDaoTests
	{
		private IConfiguration configuration;
		private IConnectionFactory connectionFactory;
		private AdoRaceTypeDao raceTypeDao;
		[SetUp]
		public void Setup()
		{
			configuration = ConfigurationUtil.GetConfiguration();
			connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
			raceTypeDao = new AdoRaceTypeDao(connectionFactory);
		}
		[Test]
		public void AdoRaceTypeDaoTest()
		{ 
			Assert.True(raceTypeDao.FindAll().Any());
		}

		[Test]
		public void UpdateTest()
		{
			raceTypeDao.Update(new RaceType {Id = 2, Type = "HugoHill", NumberOfRuns = 2});
			
			Assert.True(raceTypeDao.FindById(2).Type == "HugoHill");
		}

		[Test]
		public void InsertTest()
		{
			int id = raceTypeDao.Insert(new RaceType()
			{
				Type = "HugoHillNew",
				NumberOfRuns = 2
			});
			Assert.True(raceTypeDao.FindById(id).Id == id);
		}

		[Test]
		public void FindAllTest()
		{
			Assert.True(raceTypeDao.FindAll().Any());
		}

		[Test]
		public void FindByIdTest()
		{
			Assert.True(raceTypeDao.FindById(2).Id == 2);
		}
	}
}