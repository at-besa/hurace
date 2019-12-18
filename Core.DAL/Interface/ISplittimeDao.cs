using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Interface
{
    interface ISplittimeDao
    {
        IEnumerable<Splittime> FindAll();
        IEnumerable<Splittime> FindByRaceDataId(int raceDataId);
        Splittime FindByIds(int raceDataId, int runNo, int splittimeNo);
        bool Update(Splittime splittime);
        int Insert(Splittime splittime);
        bool Delete(Splittime splittime);
    }
}
