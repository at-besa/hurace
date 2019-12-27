using System;
using System.Linq;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace HuraceTest.Dal.Ado
{
	public class AdoSplittimeDaoTests
	{
		private IConfiguration configuration;
		private IConnectionFactory connectionFactory;
		private AdoSplitTimeDao splitTimeDao;
		[SetUp]
		public void Setup()
		{
			configuration = ConfigurationUtil.GetConfiguration();
			connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
			splitTimeDao = new AdoSplitTimeDao(connectionFactory);
		}
		
		[Test]
		public void AdoSplittimeDaoTest()
		{
			Assert.True(splitTimeDao.FindAll().Any());
		}

		[Test]
		public void FindAllTest()
		{
			Assert.True(splitTimeDao.FindAll().Any());
		}

		[Test]
		public void FindByRaceRunTest()
		{
			Assert.True(splitTimeDao.FindByRaceDataId(4).Any());
		}

		[Test]
		public void FindByIdsTest()
		{
			Assert.True(splitTimeDao.FindByIds(4,1,1).SplittimeNo == 1);
		}
		
		[Test]
		public void InsertTest()
		{
			var test = new SplitTime
			{
				Time = DateTime.Now,
				RunNo = 2,
				SplittimeNo = 7,
				RaceDataId = 5
			};
			Assert.True(splitTimeDao.Insert(test) > 0);
		}

		[Test]
		public void UpdateTest()
		{
			var split = splitTimeDao.FindByIds(4,1,1);
			DateTime now = DateTime.Now;
			
			split.Time = now;
			splitTimeDao.Update(split);
			Assert.True(splitTimeDao.FindByIds(4,1,1).Time.Equals(now));
		}
	}
}