﻿using System;
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

        private StartList MapRowToStartList(IDataRecord row)
        {
            return new StartList
            {
                Race = new AdoRaceDao(template.ConnectionFactory).FindById(Convert.ToInt32(row["raceId"])),
                SkierId = Convert.ToInt32(row["skierId"]),
                StartPos = Convert.ToInt32(row["startpos"])
            };
        }

        public IEnumerable<StartList> FindAll()
        {
            return template.Query("select * from startlist", MapRowToStartList);
        }

        public StartList FindByIds(int raceId, int skierId)
        {
            return template.QueryById(
                @"select * from startlist where raceId=@raceId and skierId=@skierId",
                MapRowToStartList,
                new QueryParameter("@raceId", raceId),
                new QueryParameter("@skierId", skierId));
        }

        internal IEnumerable<StartList> FindById(int raceId)
        {
            return template.Query(
                @"select * from startlist where raceId=@raceId",
                MapRowToStartList,
                new QueryParameter("@raceId", raceId));
        }

        public int Insert(StartList startList)
        {
            return template.Execute(
                       @"insert into startlist(raceId, skierId, startpos) values (@raceId, @skierId, @startpos); SELECT last_insert_rowid();",
                       new QueryParameter("@raceId", startList.Race.Id),
                       new QueryParameter("@skierId", startList.SkierId),
                       new QueryParameter("@startpos", startList.StartPos));
        }

        public bool Update(StartList startList)
        {
            return template.Execute(
                       @"update startlist set startpos=@startpos where raceId=@raceId and skierId=@skierId",
                       new QueryParameter("@raceId", startList.Race.Id),
                       new QueryParameter("@skierId", startList.SkierId),
                       new QueryParameter("@startpos", startList.StartPos)) == 1;
        }
    }
}