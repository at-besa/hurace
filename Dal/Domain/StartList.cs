using System;
using System.Collections.Generic;

namespace Hurace.Dal.Domain
{
    // DTO/Domain Object
    public class StartList
    {
        public Race Race { get; set; }
        public int SkierId { get; set; }
        public int StartPos { get; set; }
        
        public override string ToString() =>
            $"StartList(id:{SkierId}, Name:{Race.Name}, StartPos:{StartPos})";
    }
}