using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Interface
{
    public interface IStatusDao
    {
        IEnumerable<Status> FindAll();

        Status FindById(int id);

        bool Update(Status Status);
        int Insert(Status Status);
        bool Delete(Status Status);

    }
}
