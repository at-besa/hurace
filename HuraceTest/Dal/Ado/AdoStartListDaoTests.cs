using System;
using System.Linq;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace HuraceTest.Dal.Ado
{
	public class AdoStartListDaoTests
	{
		private IConfiguration configuration;
		private IConnectionFactory connectionFactory;
		private AdoStartListDao startlistDao;
		[SetUp]
		public void Setup()
		{
			configuration = ConfigurationUtil.GetConfiguration();
			connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
			startlistDao = new AdoStartListDao(connectionFactory);
		}
		[Test]
		public void AdoStartListDaoTest()
		{
			Assert.True(startlistDao.FindAll().Any());
		}

		[Test]
		public void FindAllTest()
		{
			Assert.True(startlistDao.FindAll().Any());
		}

		[Test]
		public void FindByIdsTest()
		{
			Assert.True(startlistDao.FindByIds(4,4).SkierId == 4);
		}

		[Test]
		public void InsertTest()
		{
			var startlist = new StartListMember
			{
				Race = new Race {Id = 14},
				SkierId = 44,
				StartPos = 2
			};
			Assert.True(startlistDao.Insert(startlist) > 0);
		}

		[Test]
		public void UpdateTest()
		{
			var test = startlistDao.FindByIds(6, 2);
			test.StartPos = 66;
			startlistDao.Update(test);
			Assert.True(startlistDao.FindByIds(6,2).StartPos == 66);
		}
	}
}