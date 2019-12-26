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
    }
}