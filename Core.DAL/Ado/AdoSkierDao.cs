using System;
using System.Collections.Generic;
using System.Data;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.DAL.Interface;

namespace Hurace.Core.DAL.Ado
{
    public class AdoSkierDao : ISkierDao
    {
        private readonly AdoTemplate template;

        public AdoSkierDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }

        private Skier MapRowToSkier(IDataRecord row)
        {
            return new Skier
            {
                Id = Convert.ToInt32(row["id"]),
                FirstName = Convert.ToString(row["firstname"]),
                LastName = Convert.ToString(row["lastname"]),
                DateOfBirth = Convert.ToDateTime(row["dateofbirth"]),
                Nation = Convert.ToString(row["nation"]),
                ProfileImage = Convert.ToString(row["profileimage"]),
                Sex = Convert.ToString(row["sex"])
            };
        }

        public bool Update(Skier skier)
        {
            return template.Execute(
                       @"update skier set firstname=@fn, lastname=@ln, dateofbirth=@dob , nation=@nat, sex=@sex where id=@id",
                       new QueryParameter("@id", skier.Id),
                       new QueryParameter("@fn", skier.FirstName),
                       new QueryParameter("@ln", skier.LastName),
                       new QueryParameter("@dob", skier.DateOfBirth.ToString("yyyy-M-d")),
                       new QueryParameter("@nat", skier.Nation),
                       new QueryParameter("@sex", skier.Sex)) == 1;
        }

        public int Insert(Skier skier)
        {
            return template.Execute(
                       @"insert into skier(id, firstname, lastname, dateofbirth, nation, profileimage, sex) values (null, @fn, @ln, @dob , @nat, null, @sex); SELECT last_insert_rowid();",
                       new QueryParameter("@id", skier.Id),
                       new QueryParameter("@fn", skier.FirstName),
                       new QueryParameter("@ln", skier.LastName),
                       new QueryParameter("@dob", skier.DateOfBirth.ToString("yyyy-M-d")),
                       new QueryParameter("@nat", skier.Nation),
                       new QueryParameter("@sex", skier.Sex));
        }

        public bool Delete(Skier skier)
        {
            return template.Execute(
                       @"delete from skier where id=@id",
                       new QueryParameter("@id", skier.Id)) >= 0;
        }

        public IEnumerable<Skier> FindAll()
        {
            return template.Query("select * from skier", MapRowToSkier);
        }

        public Skier FindById(int id)
        {
            return template.QueryById("select * from skier where id=@id",
                MapRowToSkier,
                new QueryParameter("@id", id));
        }
    }
}