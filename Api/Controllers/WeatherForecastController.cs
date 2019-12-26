using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConnectionFactory _connectionFactory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            var configuration = ConfigurationUtil.GetConfiguration();
            _connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
        }

        [HttpGet]
        public IEnumerable<SkierOutDto> Get()
        {
            
                return null;
        }
    }
}
