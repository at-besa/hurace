using System;
using System.Collections.Generic;
using System.Data;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.DAL.Interface;

namespace Hurace.Core.DAL.Ado
{
    public class AdoRunDao : IRunDao
    {
        private readonly AdoTemplate template;

        public AdoRunDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }
        
        private Run MapRowToRun(IDataRecord row)
        {
            return new Run
            {
                RaceId = Convert.ToInt32(row["raceId"]),
                RunNo = Convert.ToInt32(row["runNo"]),
                Running = Convert.ToBoolean(row["running"]),
                Finished = Convert.ToBoolean(row["finished"])
            };
        }
        
        public IEnumerable<Run> FindAll()
        {
            return template.Query("select * from Run", MapRowToRun);
        }

        public Run FindById(int raceId, int runNo)
        {
            return template.QueryById("select * from Status where raceId=@raceId and runNo=@runNo",
                MapRowToRun,
                new QueryParameter("@raceId", raceId),
                new QueryParameter("@runNo", runNo));
        }

        public bool Update(Run Run)
        {
            return template.Execute(
                       @"update Run set run=@run where raceId=@raceId and runNo=@runNo",
                       new QueryParameter("@raceId", Run.RaceId),
                       new QueryParameter("@runNo", Run.RunNo),
                       new QueryParameter("@run", Run)) == 1;
        }

        public int Insert(Run Run)
        {
            return template.Execute(
                @"insert into Run(raceId, runNo, running, finished)  values (@raceId, @runNo, @running, @finished); SELECT last_insert_rowid();",
                new QueryParameter("@raceId", Run.RaceId),
                new QueryParameter("@runNo", Run.RunNo),
                new QueryParameter("@running", Run.Running),
                new QueryParameter("@finished", Run.Finished));
        }

        public bool Delete(Run Run)
        {
            return template.Execute(
                       @"delete from Run where raceId=@raceId and runNo=@runNo",
                       new QueryParameter("@raceId", Run.RaceId),
                       new QueryParameter("@runNo", Run.RunNo)) >= 1;
        }
    }
}