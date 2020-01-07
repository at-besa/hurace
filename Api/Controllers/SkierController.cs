using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class SkierController : ControllerBase
    {

        private readonly ILogger<SkierController> _logger;
        private readonly AdoSkierDao _adoSkierDao;

        public SkierController(ILogger<SkierController> logger)
        {
            _logger = logger;
            _adoSkierDao = new AdoSkierDao(ConnectionFactoryHolder.getInstace().getConnectionFactory());
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            return Ok(SkierOutDto.FromSkier(updatedSkier));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<SkierOutDto> Insert(SkierInDto skierInDto)
        {
            //TODO does not work properly
            Skier skier = SkierInDto.ToSkier(skierInDto);
            var rowid = _adoSkierDao.Insert(skier);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
