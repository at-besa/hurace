using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Api.Util;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("RunningRace/{runningRaceId:int}/[Controller]")]
    public class RankingController : ControllerBase
    {
        private readonly ILogger<RankingController> _logger;
        private readonly AdoRaceDao _adoRaceDao;
        private readonly AdoStartListDao _adoStartListDao;
        private readonly AdoRaceDataDao _adoRaceDataDao;
        private readonly AdoSplitTimeDao _adoSplitTimeDao;
        private readonly AdoSkierDao _adoSkierDao;
        public RankingController(ILogger<RankingController> logger)
        {
            _logger = logger;
            var connectionFactory = ConnectionFactoryHolder.getInstace().getConnectionFactory();
            _adoRaceDao = new AdoRaceDao(connectionFactory);
            _adoStartListDao = new AdoStartListDao(connectionFactory);
            _adoRaceDataDao = new AdoRaceDataDao(connectionFactory);
            _adoSplitTimeDao = new AdoSplitTimeDao(connectionFactory);
            _adoSkierDao = new AdoSkierDao(connectionFactory);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IList<RankingSkierOutDto>> Get(int runningRaceId)
        {
            IList<RankingSkierOutDto> rankingSkierOutDtos = new List<RankingSkierOutDto>();
            IList<RaceData> finishedStartListMemberRaceDatas = new List<RaceData>();
            Race runningRace = _adoRaceDao.FindById(runningRaceId);
            if (runningRace == null)
            {
                return NotFound();
            }
            IEnumerable<StartListMember> startListMembers = _adoStartListDao.FindAllByRaceId(runningRaceId);
            if (startListMembers == null)
            {
                return NotFound();
            }
            foreach (var startListMember in startListMembers)
            {
                RaceData raceData = _adoRaceDataDao.FindAllBySkierId(startListMember.SkierId)
                    .FirstOrDefault(data => data.RaceId == runningRaceId);
                if (raceData.Finished && !raceData.Disqualified)
                {
                    finishedStartListMemberRaceDatas.Add(raceData);
                }
            }

            foreach (var finishedStartListMemberRaceData in finishedStartListMemberRaceDatas)
            {
                var lastSplitTime = _adoSplitTimeDao.FindByRaceDataId(finishedStartListMemberRaceData.Id)
                    .FirstOrDefault(splitTimeEntry => splitTimeEntry.SplittimeNo == runningRace.Splittimes);
                var skier = _adoSkierDao.FindById(finishedStartListMemberRaceData.SkierId);
                rankingSkierOutDtos.Add(new RankingSkierOutDto()
                {
                    FirstName = skier.FirstName,
                    LastName = skier.LastName,
                    Nation = skier.Nation,
                    ProfileImage = skier.ProfileImage,
                    EndTime = lastSplitTime.Time
                });
            }
            var orderedRankingSkierOutDtos = rankingSkierOutDtos.OrderBy(dto => dto.EndTime);
            rankingSkierOutDtos = orderedRankingSkierOutDtos.ToList();
            for (int i = 1; i <= rankingSkierOutDtos.Count; i++)
            {
                rankingSkierOutDtos[i - 1].Ranking = i;
            }
            return Ok(rankingSkierOutDtos);
        }
    }
}