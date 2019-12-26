using Hurace.Core.DAL.Domain;
using System;

namespace Api
{
    public class SkierDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nation { get; set; }
        public string ProfileImage { get; set; }
        public string Sex { get; set; }

        public static SkierDto FromSkier(Skier skier)
        {
            return new SkierDto()
            {
                Id = skier.Id,
                FirstName = skier.FirstName,
                LastName = skier.LastName,
                DateOfBirth = skier.DateOfBirth,
                Nation = skier.ProfileImage,
                Sex = skier.Sex
            };
        }
    }
}
