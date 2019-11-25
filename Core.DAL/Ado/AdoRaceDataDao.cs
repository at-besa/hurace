using System;
using System.Collections.Generic;
using System.Data;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.DAL.Interface;

namespace Hurace.Core.DAL.Ado
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
            int id = Convert.ToInt32(row["id"]);
            IEnumerable<Splittime>[] runs = new IEnumerable<Splittime>[2];
            for (int i = 0; i <= 1; i++)
            {
                runs[i] = new AdoSplittimeDao(template.ConnectionFactory).FindByRaceRun(id, i + 1);
            }

            return new RaceData
            {
                Id = id,
                Race = new AdoRaceDao(template.ConnectionFactory).FindById(Convert.ToInt32(row["raceId"])),
                SkierId = Convert.ToInt32(row["skierId"]),
                Disqualified = Convert.ToBoolean(row["disqualified"]),
                Splittime = runs
            };
        }

        public bool Update(RaceData raceData)
        {
            return template.Execute(
                       @"update RaceData set id=@id, raceId=@rid, skierId=@skid, disqualified=@dis where id=@id",
                       new QueryParameter("@id", raceData.Id),
                       new QueryParameter("@rid", raceData.Race.Id),
                       new QueryParameter("@skid", raceData.SkierId),
                       new QueryParameter("@dis", raceData.Disqualified)) >= 0;
        }

        public int Insert(RaceData raceData)
        {
            return template.Execute(
                       @"insert into RaceData(id, raceId, skierId, disqualified) values (null, @rid, @skid, @dis); SELECT last_insert_rowid();",
                       new QueryParameter("@id", raceData.Id),
                       new QueryParameter("@rid", raceData.Race.Id),
                       new QueryParameter("@skid", raceData.SkierId),
                       new QueryParameter("@dis", raceData.Disqualified));
        }

        public bool Delete(RaceData raceData)
        {
            return template.Execute(
                       @"delete from RaceData where id=@id",
                       new QueryParameter("@id", raceData.Id)) >= 0;
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