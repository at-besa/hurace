using System;
using Hurace.Core.DAL.Domain;

namespace Api
{
    public class RunningRaceOutDto
    {
        public int Id { get; set; }
        public RaceType Type { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Splittimes { get; set; }
        public string Sex { get; set;  }

        public static RunningRaceOutDto FromRace(Race runningRace)
        {
            return new RunningRaceOutDto()
            {
                Id = runningRace.Id,
                Type = runningRace.Type,
                Status =  runningRace.Status,
                Date = runningRace.Date,
                Name = runningRace.Name,
                Location = runningRace.Location,
                Splittimes = runningRace.Splittimes,
                Sex = runningRace.Sex
            };
        }
    }
}