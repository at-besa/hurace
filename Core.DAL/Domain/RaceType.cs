namespace Hurace.Core.DAL.Domain
{
 
//        Downhill,
//        SuperG,
//        Slalom,
//        GiantSlalom,
//        AlpineCombined
    
    // DTO/Domain Object
    public class RaceType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int NumberOfRuns { get; set; }
        
        public override string ToString() =>
            $"Race(id:{Id}, Type:{Type}, NumberOfRuns:{NumberOfRuns})";
    }
}