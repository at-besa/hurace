using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.Logic.Model
{
    public class RaceModel
    {
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Splittimes { get; set; }
        public string Sex { get; set; }
        public string Status { get; set; }
    }
}