﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Linq;
using Hurace.Dal.Common;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Dal.Ado
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
            return template.Execute(@"update racetype set type=@typ, numberOfRuns=@num
                                             where id=@id",
                                    new QueryParameter("@id", raceType.Id),
                                    new QueryParameter("@typ", raceType.Type),
                                    new QueryParameter("@num", raceType.NumberOfRuns)) == 1;
        }

        public bool Insert(RaceType raceType)
        {
            return template.Execute(@"insert into racetype(id, type, numberOfRuns) 
                                        values (null, @typ, @num)",
                                    new QueryParameter("@id", raceType.Id),
                                    new QueryParameter("@typ", raceType.Type),
                                    new QueryParameter("@num", raceType.NumberOfRuns)) == 1;
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
