using System.Collections;
using System.Collections.Generic;
using Api.Util;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Domain;
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
        public RankingController(ILogger<RankingController> logger)
        {
            _logger = logger;
            var connectionFactory = ConnectionFactoryHolder.getInstace().getConnectionFactory();
            _adoRaceDao = new AdoRaceDao(connectionFactory);
            _adoStartListDao = new AdoStartListDao(connectionFactory);
            _adoRaceDataDao = new AdoRaceDataDao(connectionFactory);
            _adoSplitTimeDao = new AdoSplitTimeDao(connectionFactory);
        }

        [HttpGet]
        public ActionResult<IList<RankingSkierOutDto>> Get(int runningRaceId)
        {
            Race runningRace = _adoRaceDao.FindById(runningRaceId);
            return Ok(runningRace.Name);
        }
    }
}