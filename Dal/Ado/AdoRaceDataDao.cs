﻿using System;
 using System.Collections.Generic;
 using System.Data;
 using Hurace.Dal.Common;
 using Hurace.Dal.Domain;
 using Hurace.Dal.Interface;

 namespace Hurace.Dal.Ado
{
    public class AdoRaceDataDao : IRaceDataDao
    {
        private readonly AdoTemplate template;

        public AdoRaceDataDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }

        private RaceData MapRowToRaceData(IDataRecord row)
        {
            return new RaceData
            {
                Id = Convert.ToInt32(row["id"]),
                Race = new AdoRaceDao(template.ConnectionFactory).FindById(Convert.ToInt32(row["raceId"])),
                SkierId = Convert.ToInt32(row["skierId"]),
                Disqualified = Convert.ToBoolean(row["disqualified"])
            };
        }

        public bool Update(RaceData raceData)
        {
            return template.Execute(@"update RaceData set id=@id, raceId=@rid, skierId=@skid, disqualified=@dis
                                             where id=@id",
                                    new QueryParameter("@id", raceData.Id),
                                    new QueryParameter("@rid", raceData.Race.Id),
                                    new QueryParameter("@skid", raceData.SkierId),
                                    new QueryParameter("@dis", raceData.Disqualified)) == 1;
        }

        public bool Insert(RaceData raceData)
        {
            return template.Execute(@"insert into RaceData(id, raceId, skierId, disqualified) 
                                        values (null, @rid, @skid, @dis )",
                                    new QueryParameter("@id", raceData.Id),
                                    new QueryParameter("@rid", raceData.Race.Id),
                                    new QueryParameter("@skid", raceData.SkierId),
                                    new QueryParameter("@dis", raceData.Disqualified)) == 1;
        }
        
        public IEnumerable<RaceData> FindAll()
        {
            return template.Query("select * from RaceData", MapRowToRaceData);
        }

        public RaceData FindById(int id)
        {
            return template.QueryById("select * from RaceData where id=@id",
                MapRowToRaceData,
                new QueryParameter("@id", id));
        }
    }

}
