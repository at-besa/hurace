using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Interface
{
    public interface IRaceTypeDao
    {
        IEnumerable<RaceType> FindAll();

        RaceType FindById(int id);

        bool Update(RaceType raceType);
        int Insert(RaceType raceType);

    }
}
