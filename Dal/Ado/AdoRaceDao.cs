﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
 using System.Data;
using System.Data.Common;
using System.Linq;
 using Hurace.Dal.Common;
 using Hurace.Dal.Domain;
 using Hurace.Dal.Interface;

 namespace Hurace.Dal.Ado
{
    public class AdoRaceDao : IRaceDao
    {
        private readonly AdoTemplate template;

        public AdoRaceDao(IConnectionFactory connectionFactory)
        {
            template = new AdoTemplate(connectionFactory);
        }

        private Race MapRowToRace(IDataRecord row)
        {
            return new Race
            {
                Id = (int)row["id"],
                Name = (string)row["name"],
                Location = (string)row["location"],
                Date = (DateTime)row["date"],
                Splittimes = (string)row["splittimes"],
                Description = (string)row["description"]
            };
        }

        public bool Update(Race Race)
        {
            return template.Execute(@"update Race set name=@nam, location=@loc, date=@dat, description=@desc, splittimes=@spl 
                                             where id=@id",
                                    new QueryParameter("@id", Race.Id),
                                    new QueryParameter("@nam", Race.Name),
                                    new QueryParameter("@loc", Race.Location),
                                    new QueryParameter("@dat", Race.Date),
                                    new QueryParameter("@spl", Race.Splittimes),
                                    new QueryParameter("@desc", Race.Description)) == 1;
        }

        public bool Insert(Race Race)
        {
            return template.Execute(@"insert into Race(id, name, location, date, description, splittimes) 
                                        values (null, @fn, @ln, @dob , @nat, null)",
                                    new QueryParameter("@id", Race.Id),
                                    new QueryParameter("@nam", Race.Name),
                                    new QueryParameter("@loc", Race.Location),
                                    new QueryParameter("@dat", Race.Date),
                                    new QueryParameter("@spl", Race.Splittimes),
                                    new QueryParameter("@desc", Race.Description)) == 1;
        }
        
        public IEnumerable<Race> FindAll()
        {
            return template.Query("select * from race", MapRowToRace);
        }

        public Race FindById(int id)
        {
            return template.QueryById("select * from race where id=@id",
                MapRowToRace,
                new QueryParameter("@id", id));
        }
    }

}
