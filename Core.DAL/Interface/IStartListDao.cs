using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Interface
{
    interface IStartListDao
    {
        IEnumerable<StartListMember> FindAll();
        StartListMember FindByIds(int raceId, int skierId);
        int Insert(StartListMember startListMember);
        bool Update(StartListMember startListMember);
        bool Delete(StartListMember startListMember);
    }
}
