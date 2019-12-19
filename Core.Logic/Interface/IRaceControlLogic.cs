using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic.Interface
{
    public interface IRaceControlLogic
    {
        Task<RaceControlModel> GetRaceControlForRaceId(int raceId);
        Task<bool> StartRun(StartListMemberModel slm, int raceId);
        Task<ICollection<SplittimeModel>> GetSplittimesForSkier(int skierId, int runNo);
    }
}