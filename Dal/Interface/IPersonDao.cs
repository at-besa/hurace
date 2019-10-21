﻿using System.Collections.Generic;
using System.Threading.Tasks;
 using Hurace.Dal.Domain;

 namespace Hurace.Dal.Interface
{
    public interface IPersonDao
    {
        IEnumerable<Skier> FindAll();

        Skier FindById(int id);

        bool Update(Skier skier);

    }
}
