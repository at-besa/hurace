using System;
using System.Collections.Generic;
using System.Data;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Hurace.Core.DAL.Interface;

namespace Hurace.Core.DAL.Ado
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
                Id = Convert.ToInt32(row["id"]),
                Type = new AdoRaceTypeDao(template.ConnectionFactory).FindById(Convert.ToInt32(row["typeId"])),
                Name = Convert.ToString(row["name"]),
                Location = Convert.ToString(row["location"]),
                Date = Convert.ToDateTime(row["date"]),
                Splittimes = Convert.ToInt32(row["splittimes"]),
                Sex = row.IsDBNull(row.GetOrdinal("sex")) ? "" : Convert.ToString(row["sex"])
            };
        }

        public bool Update(Race race)
        {
            return template.Execute(
                       @"update Race set typeId=@type, name=@nam, location=@loc, date=@dat, splittimes=@spl, sex=@sex where id=@id",
                       new QueryParameter("@id", race.Id),
                       new QueryParameter("@type", race.Type.Id),
                       new QueryParameter("@nam", race.Name),
                       new QueryParameter("@loc", race.Location),
                       new QueryParameter("@dat", race.Date.ToString("s")),
                       new QueryParameter("@spl", race.Splittimes),
                       new QueryParameter("@sex", race.Sex)) == 1;
        }

        public int Insert(Race race)
        {
            return template.Execute(
                       @"insert into Race(id, typeId, name, location, date, splittimes, sex) values (null, @type, @nam, @loc, @dat , @spl, @sex); SELECT last_insert_rowid();",
                       new QueryParameter("@id", race.Id),             // TODO check the insertion of the ID 
                       new QueryParameter("@type", race.Type.Id),
                       new QueryParameter("@nam", race.Name),
                       new QueryParameter("@loc", race.Location),
                       new QueryParameter("@dat", race.Date.ToString("s")),
                       new QueryParameter("@spl", race.Splittimes),
                       new QueryParameter("@sex", race.Sex));
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