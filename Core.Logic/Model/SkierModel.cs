using Hurace.Core.DAL.Domain;
using System;

namespace Hurace.Core.Logic.Model
{
    public class SkierModel        //TODO generate IComparable<SkierModel>
    {
        public SkierModel(Skier skier)
        {
            Id = skier.Id;
            FirstName = skier.FirstName;
            LastName = skier.LastName;
            DateOfBirth = skier.DateOfBirth;
            Nation = skier.Nation;
            ProfileImage = skier.ProfileImage;
            Sex = skier.Sex;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nation { get; set; }
        public string ProfileImage { get; set; }
        public string Sex { get; set; }

        public override string ToString() =>
            $"Skier(id:{Id}, FirstName:{FirstName}, LastName:{LastName}, Sex:{Sex}, Nation:{Nation}, DateOfBirth:{DateOfBirth:yyyy-MM-dd})";
    }
}