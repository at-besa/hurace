using System.Collections.Generic;
using System.Linq;
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
        private AdoRaceDao _adoRaceDao;

        public RunningRaceController(ILogger<RunningRaceController> logger)
        {
            _logger = logger;
            var configuration = ConfigurationUtil.GetConfiguration();
            var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
            _adoRaceDao = new AdoRaceDao(connectionFactory);
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