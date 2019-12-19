using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.DAL.Domain;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic.Interface
{
    public interface IRaceManagementLogic
    {
        Task<bool> DeleteRace(int raceId);
        Task<bool> SaveRace(RaceModel race);
        Task<bool> CreateRace(RaceModel race);
        Task<ICollection<RaceModel>> GetRaces();
        Task<ICollection<string>> GetRaceTypes();
        Task<ICollection<string>> GetRaceStates();
        Task<RaceModel> GetRunningRace();
    }
}