using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Api.Util;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RaceController : ControllerBase
    {
        private readonly ILogger<RaceController> _logger;
        private readonly AdoRaceDao _adoRaceDao;
        public RaceController(ILogger<RaceController> logger)
        {
            _logger = logger;
            IConnectionFactory connectionFactory = ConnectionFactoryHolder.getInstace().getConnectionFactory(); 
            _adoRaceDao = new AdoRaceDao(connectionFactory);
        }
        [HttpGet]
        public ActionResult<IList<RaceOutDto>> GetAll()
        {
            IList<RaceOutDto> raceOutDtos = new List<RaceOutDto>();
            IEnumerable<Race> races = _adoRaceDao.FindAll();
            if (races == null)
            {
                return NotFound();
            }
            foreach (var race in races)
            {
                raceOutDtos.Add(new RaceOutDto()
                {
                    Location = race.Location,
                    Name = race.Name,
                    Sex = race.Sex,
                    TypeId = race.Type.Id,
                    TypeName = race.Type.Type
                });
            }
            return Ok(raceOutDtos);
        }
    }
}