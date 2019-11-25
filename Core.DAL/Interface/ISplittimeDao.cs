using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Interface
{
    interface ISplittimeDao
    {
        IEnumerable<Splittime> FindAll();
        IEnumerable<Splittime> FindByRaceRun(int raceDataId, int runNo);
        Splittime FindByIds(int raceDataId, int runNo, int splittimeNo);
        int Insert(Splittime splittime);
        bool Update(Splittime splittime);
    }
}
