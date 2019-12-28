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

namespace Api.Controllers
{
    [ApiController]
    [Route("RunningRace/{runningRaceId:int}/Skier/{skierId:int}/[Controller]")]
    public class SplitTimesController : ControllerBase
    {
        private readonly ILogger<StartListController> _logger;
        private readonly AdoSplitTimeDao _adoSplitTimeDao;
        private readonly AdoRaceDataDao _adoRaceDataDao;
        public SplitTimesController(ILogger<StartListController> logger)
        {
            _logger = logger;
            var connectionFactory = ConnectionFactoryHolder.getInstace().getConnectionFactory();
            _adoSplitTimeDao = new AdoSplitTimeDao(connectionFactory);
            _adoRaceDataDao = new AdoRaceDataDao(connectionFactory);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IList<SplitTimeOutDto>> Get(int runningRaceId, int skierId)
        {
            IEnumerable<RaceData> raceDatasByRaceId = _adoRaceDataDao.FindAllByRaceId(runningRaceId);
            if (raceDatasByRaceId == null)
            {
                return NotFound();
            }

            var raceDataByRaceIdAndSkierId = raceDatasByRaceId.FirstOrDefault(data => data.SkierId == skierId);
            if (raceDataByRaceIdAndSkierId == null)
            {
                return NotFound();
            }
            int raceDataId = raceDataByRaceIdAndSkierId.Id; 
            IEnumerable<SplitTime> splitTimes = _adoSplitTimeDao.FindByRaceDataId(raceDataId);
            if (splitTimes == null)
            {
                return NotFound();
            }
            IList<SplitTimeOutDto> splitTimeOutDtos = new List<SplitTimeOutDto>();
            foreach (var splitTime in splitTimes)
            {
                splitTimeOutDtos.Add(SplitTimeOutDto.FromSplitTime(splitTime));
            }
            return Ok(splitTimeOutDtos);
        }
    }
}