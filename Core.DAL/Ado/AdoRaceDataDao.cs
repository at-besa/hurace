using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            IEnumerable<SplitTime>[] runs = new IEnumerable<SplitTime>[2];
            for (int i = 0; i <= 1; i++)
            {
                var i1 = i;
                runs[i] = new AdoSplitTimeDao(template.ConnectionFactory).FindByRaceDataId(id).Where(splittime => splittime.RunNo == i1+1);
            }

            return new RaceData
            {
                Id = id,
                RaceId = Convert.ToInt32(row["raceId"]),
                SkierId = Convert.ToInt32(row["skierId"]),
                Disqualified = Convert.ToBoolean(row["disqualified"]),
                Running = Convert.ToBoolean(row["running"]),
                Blocked = Convert.ToBoolean(row["blocked"]),
                Finished = Convert.ToBoolean(row["finished"]),
                Splittime = runs
            };
        }

        public bool Update(RaceData raceData)
        {
            return template.Execute(
                       @"update RaceData set id=@id, raceId=@rid, skierId=@skid, disqualified=@dis, running=@running, blocked=@blocked, finished=@finished where id=@id",
                       new QueryParameter("@id", raceData.Id),
                       new QueryParameter("@rid", raceData.RaceId),
                       new QueryParameter("@skid", raceData.SkierId),
                       new QueryParameter("@dis", raceData.Disqualified),
                       new QueryParameter("@running", raceData.Running),
                       new QueryParameter("@blocked", raceData.Blocked),
                       new QueryParameter("@finished", raceData.Finished)) >= 0;
        }

        public int Insert(RaceData raceData)
        {
            return template.Execute(
                       @"insert into RaceData(raceId, skierId, disqualified, running, blocked, finished) values (null, @rid, @skid, @dis, @running, @blocked, @finished); SELECT last_insert_rowid();",
                       new QueryParameter("@rid", raceData.RaceId),
                       new QueryParameter("@skid", raceData.SkierId),
                       new QueryParameter("@dis", raceData.Disqualified),
                       new QueryParameter("@running", raceData.Running),
                       new QueryParameter("@blocked", raceData.Blocked),
                       new QueryParameter("@finished", raceData.Finished));
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
        
        public IEnumerable<RaceData> FindAllBySkierId(int id)
        {
            return template.Query("select * from RaceData where skierId=@id",
                MapRowToRaceData,
                new QueryParameter("@id", id));
        }

        public IEnumerable<RaceData> FindAllByRaceId(int id)
        {
            return template.Query("select * from RaceData where raceId=@id",
                MapRowToRaceData,
                new QueryParameter("@id", id));
        }
        
        public RaceData FindById(int id)
        {
            return template.QueryById("select * from RaceData where id=@id",
                MapRowToRaceData,
                new QueryParameter("@id", id));
        }
    }
}