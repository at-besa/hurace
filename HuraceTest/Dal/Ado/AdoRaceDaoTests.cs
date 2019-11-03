using System;
using System.Data;
using System.Linq;
using Hurace.Dal.Ado;
using Hurace.Dal.Common;
using Hurace.Dal.Domain;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace HuraceTest.Dal.Ado
{
	public class AdoRaceDaoTests
	{
		private IConfiguration configuration;
		private IConnectionFactory connectionFactory;
		private AdoRaceDao raceDao;
		
		[SetUp]
		public void Setup()
		{
			configuration = ConfigurationUtil.GetConfiguration();
			connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
			raceDao = new AdoRaceDao(connectionFactory);
		}
		[Test]
		public void AdoRaceDaoTest()
		{
			Assert.True(raceDao.FindAll().Any());
		}

		[Test]
		public void UpdateTest()
		{
			Race testRace = raceDao.FindById(3);
			DateTime time = DateTime.Today;
			testRace.Date = time;
			raceDao.Update(testRace);
			
			Assert.True(raceDao.FindById(3).Date == time);
		}

		[Test]
		public void InsertTest()
		{
			Race testRace = new Race
			{
				Date = DateTime.Today,
				Id = 9999,
				Location = "Here",
				Name = "meins",
				Sex = "m",
				Splittimes = 4,
				Type = new RaceType { Id = 3 }
			};
			testRace.Id = raceDao.Insert(testRace);
			Assert.True(raceDao.FindById(testRace.Id).Id == testRace.Id);
		}

		[Test]
		public void FindAllTest()
		{
			Assert.True(raceDao.FindAll().Any());
		}

		[Test]
		public void FindByIdTest()
		{
			Race testRace = raceDao.FindById(3);
			Assert.True(testRace.Id == 3);
		}
	}
}