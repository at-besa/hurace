using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Interface
{
    interface ISplittimeDao
    {
        IEnumerable<SplitTime> FindAll();
        IEnumerable<SplitTime> FindByRaceDataId(int raceDataId);
        SplitTime FindByIds(int raceDataId, int runNo, int splittimeNo);
        bool Update(SplitTime splitTime);
        int Insert(SplitTime splitTime);
        bool Delete(SplitTime splitTime);
    }
}
