using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Interface
{
    public interface IRunDao
    {
        IEnumerable<Run> FindAll();
        Run FindById(int raceId, int runNo);
        bool Update(Run Run);
        int Insert(Run Run);
        bool Delete(Run Run);
    }
}