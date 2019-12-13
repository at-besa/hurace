using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Core.Logic.Model
{
    public class StartListMemberModel       //TODO generate IComparable<StartListMemberModel>
    {
        public SkierModel Skier { get; set; }
        public int Startposition { get; set; }
    }
}
