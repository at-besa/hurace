using Hurace.Dal.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Dal.Interface
{
    interface IStartListDao
    {
        IEnumerable<StartList> FindAll();
        StartList FindByIds(int raceId, int skierId);
        bool Insert(StartList startList);
        bool Update(StartList startList);
    }
}
