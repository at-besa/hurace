using System;
using System.Collections.Generic;
using System.Data;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.DAL.Interface;

namespace Hurace.Core.DAL.Ado
{
    public class AdoSplittimeDao : ISplittimeDao
    {
        private readonly AdoTemplate template;

        public AdoSplittimeDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }

        private Splittime MapRowToSplittime(IDataRecord row)
        {
            return new Splittime
            {
                RaceDataId = Convert.ToInt32(row["racedataId"]),
                RunNo = Convert.ToInt32(row["runNo"]),
                SplittimeNo = Convert.ToInt32(row["splittimeNo"]),
                Time = Convert.ToDateTime(row["splittime"])
            };
        }

        public IEnumerable<Splittime> FindAll()
        {
            return template.Query("select * from Splittime", MapRowToSplittime);
        }

        public IEnumerable<Splittime> FindByRaceDataId(int raceDataId)
        {
            return template.Query(
                @"select * from Splittime where racedataId=@raceDataId",
                MapRowToSplittime,
                new QueryParameter("@raceDataId", raceDataId));
        }

        public Splittime FindByIds(int raceDataId, int runNo, int splittimeNo)
        {
            return template.QueryById(
                @"select * from Splittime where racedataId=@raceDataId and runNo=@runNo and splittimeNo=@splittimeNo",
                MapRowToSplittime,
                new QueryParameter("@raceDataId", raceDataId),
                new QueryParameter("@runNo", runNo),
                new QueryParameter("@splittimeNo", splittimeNo));
        }

        public int Insert(Splittime splittime)
        {
            return template.Execute(
                       @"insert into Splittime(racedataId, runNo, splittimeNo, splittime) values (@racedataId, @runNo, @splittimeNo, @splittime); SELECT last_insert_rowid();",
                       new QueryParameter("@racedataId", splittime.RaceDataId),
                       new QueryParameter("@runNo", splittime.RunNo),
                       new QueryParameter("@splittimeNo", splittime.SplittimeNo),
                       new QueryParameter("@splittime", splittime.Time.ToLongTimeString()));
        }
        
        public bool Update(Splittime splittime)
        {
            return template.Execute(
                       @"update Splittime set splittime=@splittime where racedataId=@racedataId and runNo=@runNo and splittimeNo=@splittimeNo",
                       new QueryParameter("@splittime", splittime.Time),
                       new QueryParameter("@racedataId", splittime.RaceDataId),
                       new QueryParameter("@runNo", splittime.RunNo),
                       new QueryParameter("@splittimeNo", splittime.SplittimeNo)) == 1;
        }
        
        public bool Delete(Splittime splittime)
        {
            return template.Execute(
                       @"delete from Splittime where racedataId=@racedataId and runNo=@runNo and splittimeNo=@splittimeNo",
                       new QueryParameter("@racedataId", splittime.RaceDataId),
                       new QueryParameter("@runNo", splittime.RunNo),
                       new QueryParameter("@splittimeNo", splittime.SplittimeNo)) >= 1;
        }
    }
}