using System;

namespace Api
{
    public class RankingSkierOutDto
    {
        public int Ranking { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nation { get; set; }
        public string ProfileImage { get; set; }
        public DateTime EndTime { get; set; }
    }
}