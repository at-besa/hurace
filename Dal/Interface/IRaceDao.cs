using System.Collections.Generic;
using System.Threading.Tasks;
 using Hurace.Dal.Domain;

 namespace Hurace.Dal.Interface
{
    public interface IRaceDao
    {
        IEnumerable<Race> FindAll();

        Race FindById(int id);

        bool Update(Race race);
        int Insert(Race race);

    }
}
