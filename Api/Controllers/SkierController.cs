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
        private IConnectionFactory _connectionFactory;

        public SkierController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            var configuration = ConfigurationUtil.GetConfiguration();
            _connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
        }

        [HttpGet]
        public IEnumerable<SkierDto> Get()
        {
            var adoSkierDao = new AdoSkierDao(_connectionFactory);
            IEnumerable<Skier> skiers = adoSkierDao.FindAll();
            IEnumerable<SkierDto> skierDtos = new List<SkierDto>();
            foreach (var skier in skiers)
            {
                skierDtos.Append(SkierDto.FromSkier(skier));
            }
            return skierDtos;
        }
    }
}
