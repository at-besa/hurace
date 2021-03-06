﻿using System;
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
                Status = new AdoStatusDao(template.ConnectionFactory).FindById(Convert.ToInt32(row["statusId"])),
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
                       @"update Race set typeId=@type, statusId=@status, name=@nam, location=@loc, date=@dat, splittimes=@spl, sex=@sex, deleted = 0 where id=@id",
                       new QueryParameter("@id", race.Id),
                       new QueryParameter("@type", race.Type.Id),
                       new QueryParameter("@status", race.Status.Id),
                       new QueryParameter("@nam", race.Name),
                       new QueryParameter("@loc", race.Location),
                       new QueryParameter("@dat", race.Date.ToString("s")),
                       new QueryParameter("@spl", race.Splittimes),
                       new QueryParameter("@sex", race.Sex)) == 1;
        }

        public int Insert(Race race)
        {
            return template.Execute(
                       @"insert into Race(id, typeId, statusId, name, location, date, splittimes, sex, deleted) values (null, @type, @status, @nam, @loc, @dat , @spl, @sex, 0); SELECT last_insert_rowid();",
                       new QueryParameter("@type", race.Type.Id),
                       new QueryParameter("@status", race.Status.Id),
                       new QueryParameter("@nam", race.Name),
                       new QueryParameter("@loc", race.Location),
                       new QueryParameter("@dat", race.Date.ToString("s")),
                       new QueryParameter("@spl", race.Splittimes),
                       new QueryParameter("@sex", race.Sex));
        }

        public bool Delete(int raceId)
        {
            return template.Execute(
                    @"update Race set deleted=1 where id=@id",
                    new QueryParameter("@id", raceId)) >= 0;
        }

        public IEnumerable<Race> FindAll()
        {
            return template.Query("select * from race where deleted = 0", MapRowToRace);
        }

        public Race FindById(int id)
        {
            return template.QueryById("select * from race where id=@id and deleted = 0",
                MapRowToRace,
                new QueryParameter("@id", id));
        }
    }
}