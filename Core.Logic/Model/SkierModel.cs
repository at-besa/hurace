using Hurace.Core.DAL.Domain;
using System;
using System.Windows.Input;
using Hurace.Core.Logic.Helpers;

namespace Hurace.Core.Logic.Model
{
    public class SkierModel : NotifyPropertyChanged, IComparable<SkierModel>
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
        
        private ICommand _addButtonCommand;
        public ICommand AddButtonCommand
        {
            get => _addButtonCommand;
            set => Set(ref _addButtonCommand, value);
        }

        public int CompareTo(SkierModel other)
        {
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
        
        public override string ToString() =>
            $"Skier(id:{Id}, FirstName:{FirstName}, LastName:{LastName}, Sex:{Sex}, Nation:{Nation}, DateOfBirth:{DateOfBirth:yyyy-MM-dd})";
    }
}