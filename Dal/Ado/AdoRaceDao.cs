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

        public bool Update(Race race)
        {
            return template.Execute(@"update Race set name=@nam, location=@loc, date=@dat, description=@desc, splittimes=@spl 
                                             where id=@id",
                                    new QueryParameter("@id", race.Id),
                                    new QueryParameter("@nam", race.Name),
                                    new QueryParameter("@loc", race.Location),
                                    new QueryParameter("@dat", race.Date),
                                    new QueryParameter("@spl", race.Splittimes),
                                    new QueryParameter("@desc", race.Description)) == 1;
        }

        public bool Insert(Race race)
        {
            return template.Execute(@"insert into Race(id, name, location, date, description, splittimes) 
                                        values (null, @fn, @ln, @dob , @nat, null)",
                                    new QueryParameter("@id", race.Id),
                                    new QueryParameter("@nam", race.Name),
                                    new QueryParameter("@loc", race.Location),
                                    new QueryParameter("@dat", race.Date),
                                    new QueryParameter("@spl", race.Splittimes),
                                    new QueryParameter("@desc", race.Description)) == 1;
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
