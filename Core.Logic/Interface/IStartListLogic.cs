using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic.Interface
{
    public interface IStartListLogic
    {
        Task<StartListModel> GetStartListForRaceId(int raceId);
        Task<ICollection<SkierModel>> GetAllSkiersWithSameSex(string sex);
        Task<bool> UpdateStartListMemberStartPos(int raceId, int pos, int skierId);
        Task<bool> DeleteStartListMember(int skierId, int raceId, int startposition);
        Task<bool> IsStartListMemberInStartList(int raceId, int skierId);
    }
}