using Hurace.Dal.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Dal.Interface
{
    interface ISplittimeDao
    {
        IEnumerable<Splittime> FindAll();
        IEnumerable<Splittime> FindByRaceRun(int raceDataId, int runNo);
        Splittime FindByIds(int raceDataId, int runNo, int splittimeNo);
        bool Insert(Splittime splittime);
        bool Update(Splittime splittime);
    }
}
