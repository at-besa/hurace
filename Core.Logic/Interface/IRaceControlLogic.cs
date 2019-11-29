using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic.Interface
{
    public interface IRaceControlLogic
    {
        Task<ICollection<RaceControlModel>> GetRaceControls();
    }
}