using System;
using System.Collections.Generic;
using System.Data;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.DAL.Interface;

namespace Hurace.Core.DAL.Ado
{
    public class AdoStartListDao : IStartListDao
    {
        private readonly AdoTemplate template;

        public AdoStartListDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }

        private StartListMember MapRowToStartList(IDataRecord row)
        {
            return new StartListMember
            {
                Race = new AdoRaceDao(template.ConnectionFactory).FindById(Convert.ToInt32(row["raceId"])),
                SkierId = Convert.ToInt32(row["skierId"]),
                StartPos = Convert.ToInt32(row["startpos"])
            };
        }

        public IEnumerable<StartListMember> FindAll()
        {
            return template.Query("select * from startlist", MapRowToStartList);
        }

        public StartListMember FindByIds(int raceId, int skierId)
        {
            return template.QueryById(
                @"select * from startlist where raceId=@raceId and skierId=@skierId",
                MapRowToStartList,
                new QueryParameter("@raceId", raceId),
                new QueryParameter("@skierId", skierId));
        }
        
        public IEnumerable<StartListMember> FindAllByRaceId(int raceId)
        {
            return template.Query(
                @"select * from startlist where raceId=@raceId",
                MapRowToStartList,
                new QueryParameter("@raceId", raceId));
        }

        public IEnumerable<StartListMember> FindAllBySkierId(int skierId)
        {
            return template.Query(
                @"select * from startlist where skierId=@skierId",
                MapRowToStartList,
                new QueryParameter("@skierId", skierId));
        }

        internal IEnumerable<StartListMember> FindById(int raceId)
        {
            return template.Query(
                @"select * from startlist where raceId=@raceId",
                MapRowToStartList,
                new QueryParameter("@raceId", raceId));
        }

        public int Insert(StartListMember startListMember)
        {
            return template.Execute(
                       @"insert into startlist(raceId, skierId, startpos) values (@raceId, @skierId, @startpos); SELECT last_insert_rowid();",
                       new QueryParameter("@raceId", startListMember.Race.Id),
                       new QueryParameter("@skierId", startListMember.SkierId),
                       new QueryParameter("@startpos", startListMember.StartPos));
        }

        public bool Update(StartListMember startListMember)
        {
            return template.Execute(
                       @"update startlist set startpos=@startpos where raceId=@raceId and skierId=@skierId",
                       new QueryParameter("@raceId", startListMember.Race.Id),
                       new QueryParameter("@skierId", startListMember.SkierId),
                       new QueryParameter("@startpos", startListMember.StartPos)) == 1;
        }

        public bool Delete(StartListMember startListMember)
        {
            return template.Execute(
                       @"delete from startlist where raceId=@raceId and skierId=@skierId and startposition=@startposition",
                       new QueryParameter("@raceId", startListMember.Race.Id),
                       new QueryParameter("@skierId", startListMember.SkierId),
                       new QueryParameter("@startposition", startListMember.StartPos)) == 1;
        }
    }
}