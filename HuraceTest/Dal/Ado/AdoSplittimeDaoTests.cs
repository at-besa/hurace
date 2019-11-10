using System;
using System.Linq;
using Hurace.Dal.Ado;
using Hurace.Dal.Common;
using Hurace.Dal.Domain;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace HuraceTest.Dal.Ado
{
	public class AdoSplittimeDaoTests
	{
		private IConfiguration configuration;
		private IConnectionFactory connectionFactory;
		private AdoSplittimeDao splittimeDao;
		[SetUp]
		public void Setup()
		{
			configuration = ConfigurationUtil.GetConfiguration();
			connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
			splittimeDao = new AdoSplittimeDao(connectionFactory);
		}
		
		[Test]
		public void AdoSplittimeDaoTest()
		{
			Assert.True(splittimeDao.FindAll().Any());
		}

		[Test]
		public void FindAllTest()
		{
			Assert.True(splittimeDao.FindAll().Any());
		}

		[Test]
		public void FindByRaceRunTest()
		{
			Assert.True(splittimeDao.FindByRaceRun(4,1).Any());
		}

		[Test]
		public void FindByIdsTest()
		{
			Assert.True(splittimeDao.FindByIds(4,1,1).SplittimeNo == 1);
		}
		
		[Test]
		public void InsertTest()
		{
			var test = new Splittime
			{
				Time = DateTime.Now,
				RunNo = 2,
				SplittimeNo = 7,
				RaceDataId = 5
			};
			Assert.True(splittimeDao.Insert(test) > 0);
		}

		[Test]
		public void UpdateTest()
		{
			var split = splittimeDao.FindByIds(4,1,1);
			DateTime now = DateTime.Now;
			
			split.Time = now;
			splittimeDao.Update(split);
			Assert.True(splittimeDao.FindByIds(4,1,1).Time.Equals(now));
		}
	}
}