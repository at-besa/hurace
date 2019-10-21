using System;

namespace Hurace.Dal.Domain
{
    public enum RaceType
    {
        Downhill,
        SuperG,
        Slalom,
        GiantSlalom,
        AlpineCombined
    }
    
    // DTO/Domain Object
    public class Race
    {
        public int Id { get; set; }
        public RaceType Type { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        
        public override string ToString() =>
            $"Race(id:{Id}, Type:{Type}, Name:{Name}, Location:{Location}, Description:{Description}, DateOfBirth:{Date:yyyy-MM-dd})";
    }
}