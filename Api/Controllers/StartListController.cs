using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Api.Util;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventSource;

namespace Api.Controllers
{
    [ApiController]
    [Route("RunningRace/{runningRaceId:int}/Run/{runNo:int}/[controller]")]
    public class StartListController : ControllerBase
    {
        private readonly ILogger<StartListController> _logger;
        private readonly AdoStartListDao _adoStartListDao;
        private readonly AdoRaceDataDao _adoRaceDataDao;
        private readonly AdoSkierDao _adoSkierDao;
        public StartListController(ILogger<StartListController> logger)
        {
            _logger = logger;
            var connectionFactory = ConnectionFactoryHolder.getInstace().getConnectionFactory();
            _adoStartListDao = new AdoStartListDao(connectionFactory);
            _adoRaceDataDao = new AdoRaceDataDao(connectionFactory);
            _adoSkierDao = new AdoSkierDao(connectionFactory);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IList<StartListSkierOutDto>> Get(int runningRaceId, int runNo)
        {
            IList<StartListSkierOutDto> startListSkierOutDtos = new List<StartListSkierOutDto>();
            IEnumerable<StartListMember> startListMembers = _adoStartListDao.FindAllByRaceId(runningRaceId);
            if (startListMembers == null)
            {
                return NotFound();
            }
            
            startListMembers = startListMembers.Where(member => member.RunNo == runNo);
            IEnumerable<RaceData> raceDatas = _adoRaceDataDao.FindAllByRaceId(runningRaceId);
            if (raceDatas == null)
            {
                return NotFound();
            }
            foreach (var startListMember in startListMembers)
            {
                var raceData = raceDatas.FirstOrDefault(data => data.SkierId == startListMember.SkierId);
                if (raceData == null)
                {
                    continue;
                }
                var skier = _adoSkierDao.FindById(raceData.SkierId);
                startListSkierOutDtos.Add(StartListSkierOutDto.FromSkierRaceDataAndStartListMember(skier, raceData, startListMember));
            }
            
            return Ok(startListSkierOutDtos);
        }
    }
}