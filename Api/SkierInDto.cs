using Hurace.Core.DAL.Domain;
using System;

namespace Api.Controllers
{
    public class SkierInDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nation { get; set; }
        public string ProfileImage { get; set; }
        public string Sex { get; set; }

        public static Skier ToSkier(SkierInDto skierInDto)
        {
            return new Skier()
            {
                FirstName = skierInDto.FirstName,
                LastName = skierInDto.LastName,
                DateOfBirth = skierInDto.DateOfBirth,
                Nation = skierInDto.Nation,
                ProfileImage = skierInDto.ProfileImage,
                Sex = skierInDto.Sex
            };
        }
    }
}