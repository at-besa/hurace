using System;

namespace Hurace.Dal.Domain
{

    // DTO/Domain Object
    public class Race
    {
        public int Id { get; set; }
        public RaceType Type { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Splittimes { get; set; }
        public string Sex { get; set;  }
        
        public override string ToString() =>
            $"Race(id:{Id}, Type:{Type}, Name:{Name}, Location:{Location}, Sex:{Sex}, Date:{Date:yyyy-MM-dd})";
    }
}