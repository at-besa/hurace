using System;
using System.Collections.Generic;
using System.Data;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.DAL.Interface;

namespace Hurace.Core.DAL.Ado
{
    public class AdoRaceTypeDao : IRaceTypeDao
    {
        private readonly AdoTemplate template;

        public AdoRaceTypeDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }

        private RaceType MapRowToRaceType(IDataRecord row)
        {
            return new RaceType
            {
                Id = Convert.ToInt32(row["id"]),
                Type = Convert.ToString(row["type"]),
                NumberOfRuns = Convert.ToInt32(row["numberOfRuns"])
            };
        }

        public bool Update(RaceType raceType)
        {
            return template.Execute(
                       @"update racetype set type=@typ, numberOfRuns=@num where id=@id",
                       new QueryParameter("@id", raceType.Id),
                       new QueryParameter("@typ", raceType.Type),
                       new QueryParameter("@num", raceType.NumberOfRuns)) == 1;
        }

        public int Insert(RaceType raceType)
        {
            return template.Execute(
                       @"insert into racetype(id, type, numberOfRuns)  values (null, @typ, @num); SELECT last_insert_rowid();",
                       new QueryParameter("@id", raceType.Id),
                       new QueryParameter("@typ", raceType.Type),
                       new QueryParameter("@num", raceType.NumberOfRuns));
        }

        public bool Delete(RaceType raceType)
        {
            return template.Execute(
                       @"delete from racetype where id=@id",
                       new QueryParameter("@id", raceType.Id)) >= 0;
        }

        public IEnumerable<RaceType> FindAll()
        {
            return template.Query("select * from racetype", MapRowToRaceType);
        }

        public RaceType FindById(int id)
        {
            return template.QueryById("select * from racetype where id=@id",
                MapRowToRaceType,
                new QueryParameter("@id", id));
        }
    }
}