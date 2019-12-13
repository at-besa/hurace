﻿using System;

 namespace Hurace.Core.DAL.Domain
{

    // DTO/Domain Object
    public class Skier : IComparable<Skier>
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

        public int CompareTo(Skier other) {
	        if (ReferenceEquals(this, other)) return 0;
	        if (ReferenceEquals(null, other)) return 1;
	        var idComparison = Id.CompareTo(other.Id);
	        if (idComparison != 0) return idComparison;
	        var firstNameComparison = string.Compare(FirstName, other.FirstName, StringComparison.Ordinal);
	        if (firstNameComparison != 0) return firstNameComparison;
	        var lastNameComparison = string.Compare(LastName, other.LastName, StringComparison.Ordinal);
	        if (lastNameComparison != 0) return lastNameComparison;
	        var dateOfBirthComparison = DateOfBirth.CompareTo(other.DateOfBirth);
	        if (dateOfBirthComparison != 0) return dateOfBirthComparison;
	        var nationComparison = string.Compare(Nation, other.Nation, StringComparison.Ordinal);
	        if (nationComparison != 0) return nationComparison;
	        var profileImageComparison = string.Compare(ProfileImage, other.ProfileImage, StringComparison.Ordinal);
	        if (profileImageComparison != 0) return profileImageComparison;
	        return string.Compare(Sex, other.Sex, StringComparison.Ordinal);
        }
    }
}
