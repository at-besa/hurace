using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

 namespace Hurace.Dal.Interface
{
    public interface IRaceDataDao
    {
        IEnumerable<RaceData> FindAll();

        RaceData FindById(int id);

        bool Update(RaceData raceData);
        int Insert(RaceData raceData);

    }
}
