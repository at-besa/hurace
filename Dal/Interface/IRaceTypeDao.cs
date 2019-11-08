﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

 namespace Hurace.Dal.Interface
{
    public interface IRaceTypeDao
    {
        IEnumerable<RaceType> FindAll();

        RaceType FindById(int id);

        bool Update(RaceType raceType);
        int Insert(RaceType raceType);

    }
}
