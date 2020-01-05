using Hurace.Core.DAL.Domain;

namespace Api
{
    public class RunningRunOutDto
    {
        public int RaceId { get; set; }
        public int RunNo { get; set; }
        public bool Running { get; set; }
        public bool Finnished { get; set; }

        public static RunningRunOutDto FromRun(Run run)
        {
            return new RunningRunOutDto()
            {
                RaceId = run.RaceId,
                RunNo = run.RunNo,
                Running = run.Running,
                Finnished = run.Finished
            };
        }
    }
}