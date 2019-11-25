using System;
using System.Linq;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace HuraceTest.Dal.Ado
{
	public class AdoSkierDaoTests
	{
		private IConfiguration configuration;
		private IConnectionFactory connectionFactory;
		private AdoSkierDao skierDao;
		[SetUp]
		public void Setup()
		{
			configuration = ConfigurationUtil.GetConfiguration();
			connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
			skierDao = new AdoSkierDao(connectionFactory);
		}

		[Test]
		public void AdoSkierDaoTest()
		{
			Assert.True(skierDao.FindAll().Any());
		}

		[Test]
		public void UpdateTest()
		{
			var skier = skierDao.FindById(5);
			skier.FirstName = "Franzbert";
			skierDao.Update(skier);
			Assert.True(skierDao.FindById(5).FirstName == "Franzbert");
		}

		[Test]
		public void InsertTest()
		{
			int id = skierDao.Insert(new Skier
				{
					Nation = "AUT", 
					Sex = "m", 
					FirstName = "Franzbert der zweite", 
					LastName = "Von dem Schlitten", 
					ProfileImage = "http://getmyimage.com/image2", 
					DateOfBirth = DateTime.Today
				});
			
			Assert.True(skierDao.FindById(id).Id == id);
		}

		[Test]
		public void FindAllTest()
		{
			Assert.True(skierDao.FindAll().Any());
		}

		[Test]
		public void FindByIdTest()
		{
			Assert.True(skierDao.FindById(4).Id == 4);
		}
	}
}