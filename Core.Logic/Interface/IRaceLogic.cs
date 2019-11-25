using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic.Interface
{
    public interface IRaceLogic
    {
        ICollection<RaceModel> GetRaces();
    }
}