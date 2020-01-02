using Hurace.Core.DAL.Domain;
using System;
using System.Linq;

namespace Api
{
    public class RaceOutDto
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Sex { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }

        public static RaceOutDto FromRace(IGrouping<RaceType, Race> grouping)
        {
            throw new NotImplementedException();
        }
    }
}
