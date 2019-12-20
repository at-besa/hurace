using System;
using System.Collections.Generic;
using System.Text;
using Hurace.Core.DAL.Domain;

namespace HuraceTest
{
	public static class ObjectGenerator
	{
		private static DateTime now = DateTime.Now;

		public static Race GetRace(int id)
		{
			return new Race
			{
				Id = id,
				Type = GetRaceType(),
				Status = GetStatus(),
				Date = now,
				Name = "TestRace" + id,
				Location = "TestLocation" + id,
				Splittimes = 5,
				Sex = "t"
			};
		}

		public static RaceType GetRaceType()
		{
			return new RaceType
			{
				Id = 1,
				Type = "testType",
				NumberOfRuns = 2
			};
		}

		public static Status GetStatus()
		{
			return new Status
			{
				Id = 0,
				Name = null
			};
		}

		public static DateTime GetDate()
		{
			return now;
		}
	}
}
