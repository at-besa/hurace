using System;

namespace Hurace.Dal.Domain
{
    // DTO/Domain Object
    public class RaceData 
    {
        public int Id { get; set; }
        public Race Race { get; set; }
        public int SkierId { get; set; }
        public bool Disqualified { get; set; }
        
        public Splittime[] Splittime { get; set; } = new Splittime[2];
        
        
        public override string ToString() =>
            $"RaceData(id:{SkierId}, Race:{Race}, Run1:( {Splittime[0]}), Run2:({Splittime[1]}), Disqualified:{Disqualified})";
    }
}