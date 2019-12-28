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
        public RankingController(ILogger<RankingController> logger)
        {
            _logger = logger;
            _adoRaceDao = new AdoRaceDao(ConnectionFactoryHolder.getInstace().getConnectionFactory());
        }

        [HttpGet]
        public ActionResult Get(int runningRaceId)
        {
            Race runningRace = _adoRaceDao.FindById(runningRaceId);
            return Ok(runningRace.Name);
        }
    }
}