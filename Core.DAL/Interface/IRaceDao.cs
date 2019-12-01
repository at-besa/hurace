using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Interface
{
    public interface IRaceDao
    {
        IEnumerable<Race> FindAll();

        Race FindById(int id);

        bool Update(Race race);
        int Insert(Race race);
        bool Delete(int raceId);

    }
}
