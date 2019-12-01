using System;
using System.Collections.Generic;
using System.Data;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.DAL.Interface;

namespace Hurace.Core.DAL.Ado
{
    public class AdoStatusDao : IStatusDao
    {
        private readonly AdoTemplate template;

        public AdoStatusDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }

        private Status MapRowToStatus(IDataRecord row)
        {
            return new Status
            {
                Id = Convert.ToInt32(row["id"]),
                Name = Convert.ToString(row["status"]),
            };
        }

        public bool Update(Status Status)
        {
            return template.Execute(
                       @"update Status set status=@status where id=@id",
                       new QueryParameter("@id", Status.Id),
                       new QueryParameter("@status", Status.Name)) == 1;
        }

        public int Insert(Status Status)
        {
            return template.Execute(
                       @"insert into Status(id, status)  values (null, @status); SELECT last_insert_rowid();",
                       new QueryParameter("@id", Status.Id),
                       new QueryParameter("@status", Status.Name));
        }

        public bool Delete(Status Status)
        {
            return template.Execute(
                       @"delete from Status where id=@id",
                       new QueryParameter("@id", Status.Id)) >= 0;
        }

        public IEnumerable<Status> FindAll()
        {
            return template.Query("select * from Status", MapRowToStatus);
        }

        public Status FindById(int id)
        {
            return template.QueryById("select * from Status where id=@id",
                MapRowToStatus,
                new QueryParameter("@id", id));
        }
    }
}