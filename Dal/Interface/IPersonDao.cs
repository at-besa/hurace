﻿using System.Collections.Generic;
using System.Threading.Tasks;
 using Hurace.Domain;

 namespace Hurace.Dal.Interface
{
    public interface IPersonDao
    {
        IEnumerable<Person> FindAll();

        Person FindById(int id);

        bool Update(Person person);

    }
}
