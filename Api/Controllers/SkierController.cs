using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [ProducesResponseType(typeof(IEnumerable<SkierOutDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<SkierOutDto>> GetAll()
        {
            IEnumerable<Skier> skiers = _adoSkierDao.FindAll();
            if(skiers == null)
            {
                return NotFound();
            }
            IList<SkierOutDto> skierDtos = new List<SkierOutDto>();
            foreach (var skier in skiers)
            {
                skierDtos.Add(SkierOutDto.FromSkier(skier));
            }
            return Ok(skierDtos);
        }

        [HttpGet("{id:int}")]
        public ActionResult<SkierOutDto> GetById(int id)
        {
            Skier skier = _adoSkierDao.FindById(id);
            if(skier == null)
            {
                return NotFound();
            }
            return Ok(SkierOutDto.FromSkier(skier));
        }

        [HttpPut("{id:int}")]
        public ActionResult<SkierOutDto> Update(int id, SkierInDto skierInDto)
        {
            Skier skier = SkierInDto.ToSkier(skierInDto);
            skier.Id = id;
            _adoSkierDao.Update(skier);
            var updatedSkier = _adoSkierDao.FindById(id);
            if(updatedSkier == null)
            {
                return NotFound();
            }
            if(skier.CompareTo(updatedSkier) != 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(SkierOutDto.FromSkier(updatedSkier));
        }

        [HttpPost]
        public ActionResult<SkierOutDto> Insert(SkierInDto skierInDto)
        {
            //TODO does not work properly
            Skier skier = SkierInDto.ToSkier(skierInDto);
            var rowid = _adoSkierDao.Insert(skier);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            Skier skier = _adoSkierDao.FindById(id);
            if(skier == null)
            {
                return NotFound();
            }
            _adoSkierDao.Delete(skier);
            return NoContent();
        }
    }
}
