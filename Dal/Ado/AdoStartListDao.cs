using Hurace.Dal.Common;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Hurace.Dal.Ado
{
    public class AdoStartListDao : IStartListDao
    {
        private readonly AdoTemplate template;
        public AdoStartListDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }
        private StartList MapRowToStartList(IDataRecord row)
        {
            return new StartList
            {
                Race = new Race { Id = (int)(long)row["raceId"] },
                SkierId = (int)(long)row["skierId"],
                StartPos = (int)(long)row["startpos"]
            };
        }
        public IEnumerable<StartList> FindAll()
        {
            return template.Query("select * from startlist", MapRowToStartList);
        }

        public StartList FindByIds(int raceId, int skierId)
        {
            return template.QueryById(@"select * from startlist 
                                    where raceId=@raceId and skierId=@skierId", 
                                MapRowToStartList,
                                new QueryParameter("@raceId", raceId),
                                new QueryParameter("@skierId", skierId));
        }

        internal IEnumerable<StartList> FindById(int raceId)
        {
            return template.Query(@"select * from startlist 
                                    where raceId=@raceId",
                                MapRowToStartList,
                                new QueryParameter("@raceId", raceId));
        }
        public bool Insert(StartList startList)
        {
            return template.Execute(@"insert into startlist(raceId, skierId, startpos) 
                                        values (@raceId, @skierId, @startpos)",
                                       new QueryParameter("@raceId", startList.Race.Id),
                                       new QueryParameter("@skierId", startList.SkierId),
                                       new QueryParameter("@startpos", startList.StartPos)) == 1;
        }

        public bool Update(StartList startList)
        {
            return template.Execute(@"update startlist set startpos=@startpos 
                                        where raceId=@raceId and skierId=@skierId",
                                    new QueryParameter("@raceId", startList.Race.Id),
                                    new QueryParameter("@skierId", startList.SkierId),
                                    new QueryParameter("@startpos", startList.StartPos)) == 1;
        }

    }
}
