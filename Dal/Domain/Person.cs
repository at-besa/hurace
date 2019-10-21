﻿using System;

namespace Hurace.Domain
{

    // DTO/Domain Object
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public override string ToString() =>
          $"Person(id:{Id}, FirstName:{FirstName}, LastName:{LastName}, DateOfBirth:{DateOfBirth:yyyy-MM-dd})";
    }
}
