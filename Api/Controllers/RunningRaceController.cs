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
    [Route("[controller]")]
    public class RunningRaceController : ControllerBase    
    {
        private readonly ILogger<RunningRaceController> _logger;
        private readonly AdoRaceDao _adoRaceDao;

        public RunningRaceController(ILogger<RunningRaceController> logger)
        {
            _logger = logger;
            _adoRaceDao = new AdoRaceDao(ConnectionFactoryHolder.getInstace().getConnectionFactory());
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RunningRaceOutDto> Get()
        {
            IEnumerable<Race> races = _adoRaceDao.FindAll();
            if (races == null)
            {
                return NotFound();
            }
            var runningRace = races.FirstOrDefault(race => race.Status.Id == 2 && race.Status.Name == "running");
            if (runningRace == null)
            {
                return NotFound();
            }
            return Ok(RunningRaceOutDto.FromRace(runningRace));
        }
    }
}