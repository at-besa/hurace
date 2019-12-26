using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkierController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private AdoSkierDao _adoSkierDao;

        public SkierController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            var configuration = ConfigurationUtil.GetConfiguration();
            var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
            _adoSkierDao = new AdoSkierDao(connectionFactory);
        }

        [HttpGet]
        public ActionResult<IEnumerable<SkierDto>> GetAll()
        {
            IEnumerable<Skier> skiers = _adoSkierDao.FindAll();
            if(skiers == null)
            {
                return NotFound();
            }
            IList<SkierDto> skierDtos = new List<SkierDto>();
            foreach (var skier in skiers)
            {
                skierDtos.Add(SkierDto.FromSkier(skier));
            }
            return Ok(skierDtos);
        }

        [HttpGet("{id:int}")]
        public ActionResult<SkierDto> GetById(int id)
        {
            Skier skier = _adoSkierDao.FindById(id);
            if(skier == null)
            {
                return NotFound();
            }
            return Ok(SkierDto.FromSkier(skier));
        }
    }
}
