﻿﻿using System;

 namespace Hurace.Dal.Domain
{

    // DTO/Domain Object
    public class Skier
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nation { get; set; }
        public Object ProfileImage { get; set; }
        
        
        // BeSa predefined data for WorldCup
//        public int Rank { get; set; }
//        public int Points { get; set; }
        
        public override string ToString() =>
          $"Skier(id:{Id}, FirstName:{FirstName}, LastName:{LastName}, Nation:{Nation}, DateOfBirth:{DateOfBirth:yyyy-MM-dd})";
    }
}