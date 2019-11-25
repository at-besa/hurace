using System.Collections.Generic;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Interface
{
    public interface ISkierDao
    {
        IEnumerable<Skier> FindAll();

        Skier FindById(int id);

        bool Update(Skier skier);
        int Insert(Skier skier);
        bool Delete(Skier skier);

    }
}
