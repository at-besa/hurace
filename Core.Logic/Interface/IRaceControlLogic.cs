using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic.Interface
{
    public interface IRaceControlLogic
    {
        Task<RaceControlModel> GetRaceControlForRaceId(int raceId, int runNo);
        Task<bool> StartRun(StartListMemberModel slm, int raceId);
        Task<bool> Clearance(StartListMemberModel slm, int raceId);
        Task<bool> Disqualify(StartListMemberModel slm, int raceId);
        Task<ICollection<SplitTimeModel>> GetSplittimesForSkier(int skierId, int runNo);
        Task<bool> SimulatorOnOff(bool onOff, int raceId);

    }
}