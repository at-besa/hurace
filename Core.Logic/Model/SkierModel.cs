using System;

namespace Hurace.Core.Logic.Model
{
    public class SkierModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nation { get; set; }
        public string ProfileImage { get; set; }
        public string Sex { get; set; }
        
        
        // BeSa predefined data for WorldCup
//        public int Rank { get; set; }
//        public int Points { get; set; }
        
        public override string ToString() =>
            $"Skier(id:{Id}, FirstName:{FirstName}, LastName:{LastName}, Sex:{Sex}, Nation:{Nation}, DateOfBirth:{DateOfBirth:yyyy-MM-dd})";
    }
}