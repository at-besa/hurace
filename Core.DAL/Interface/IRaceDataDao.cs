using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Interface
{
    public interface IRaceDataDao
    {
        IEnumerable<RaceData> FindAll();

        RaceData FindById(int id);

        bool Update(RaceData raceData);
        int Insert(RaceData raceData);
        bool Delete(RaceData raceData);

    }
}
