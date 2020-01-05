using System.Collections.Generic;
using System.Linq;
using Api.Util;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Domain;
using Hurace.Core.DAL.Importer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("RunningRace/{runningRaceId:int}/[controller]")]
    public class RunningRunController : ControllerBase
    {
        private readonly ILogger<RaceController> _logger;
        private readonly AdoRunDao _adoRunDao;
        private readonly AdoRaceDao _adoRaceDao;
        public RunningRunController(ILogger<RunningRunController> _logger)
        {
            var connectionFactory = ConnectionFactoryHolder.getInstace().getConnectionFactory();
            _adoRunDao = new AdoRunDao(connectionFactory);
            _adoRaceDao = new AdoRaceDao(connectionFactory);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RunningRunOutDto> Get(int runningRaceId)
        {
            //new RunImporter(ConnectionFactoryHolder.getInstace().getConnectionFactory()).Import();
            RunningRunOutDto runningRun = null;
            Race race = _adoRaceDao.FindById(runningRaceId);
            if (race == null)
            {
                return NotFound();
            }
            if (race.Status?.Id != 2 || race.Status?.Name != "running")
            {
                return NotFound();
            }

            for (int i = 0; i < race.Type?.NumberOfRuns; i++)
            {
                var run = _adoRunDao.FindById(race.Id, i + 1);
                if (run.Running == true && run.Finished == false)
                {
                    runningRun = RunningRunOutDto.FromRun(run);
                }
            }

            if (runningRun == null)
            {
                return NotFound();
            }
            return Ok(runningRun);
        }
        
    }
}