using Hurace.Core.DAL.Domain;

namespace Api
{
    public class StartListSkierOutDto
    {
        public bool Blocked { get; set; }
        public bool Running { get; set; }
        public bool Disqualified { get; set; }
        public bool Finished { get; set; }
        public int Startposition { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nation { get; set; }
        public string ProfileImage { get; set; }

        public static StartListSkierOutDto FromSkierRaceDataAndStartListMember(Skier skier, RaceData raceData, StartListMember startListMember)
        {
            return new StartListSkierOutDto()
            {
                Blocked = raceData.Blocked,
                Running = raceData.Running,
                Disqualified = raceData.Disqualified,
                Finished = raceData.Finished,
                Startposition = startListMember.StartPos,
                FirstName = skier.FirstName,
                LastName = skier.LastName,
                Nation = skier.Nation,
                ProfileImage = skier.ProfileImage
            };
        }
    }
}