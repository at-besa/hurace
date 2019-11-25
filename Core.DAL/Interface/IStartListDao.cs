using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Interface
{
    interface IStartListDao
    {
        IEnumerable<StartList> FindAll();
        StartList FindByIds(int raceId, int skierId);
        int Insert(StartList startList);
        bool Update(StartList startList);
    }
}
