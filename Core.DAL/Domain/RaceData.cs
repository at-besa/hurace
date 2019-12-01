using System;
using System.Collections.Generic;

namespace Hurace.Core.DAL.Domain
{
    // DTO/Domain Object
    public class RaceData
    {
        private const string Seperator = " - ";
        private Func<IEnumerable<Splittime>, string> splitter = splittimes => String.Join(Seperator, splittimes); 
        public int Id { get; set; }
        public Race Race { get; set; }
        public int SkierId { get; set; }
        public bool Disqualified { get; set; }
        public IEnumerable<Splittime>[] Splittime { get; set; }
        
        public override string ToString() =>
            $"RaceData(id:{SkierId}, Race:{Race}, Runs: [1: {splitter(Splittime[0])} 2: {splitter(Splittime[1])}], Disqualified:{Disqualified})";
    }
}