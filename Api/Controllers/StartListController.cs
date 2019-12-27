﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventSource;

namespace Api.Controllers
{
    [ApiController]
    [Route("RunningRace/{id:int}/[controller]")]
    public class StartListController : ControllerBase
    {
        private readonly ILogger<StartListController> _logger;
        private AdoStartListDao _adoStartListDao;
        private AdoRaceDataDao _adoRaceDataDao;
        private AdoSkierDao _adoSkierDao;
        public StartListController(ILogger<StartListController> logger)
        {
            _logger = logger;
            var configuration = ConfigurationUtil.GetConfiguration();
            var connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
            _adoStartListDao = new AdoStartListDao(connectionFactory);
            _adoRaceDataDao = new AdoRaceDataDao(connectionFactory);
            _adoSkierDao = new AdoSkierDao(connectionFactory);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IList<StartListSkierOutDto>> Get(int id)
        {
            IList<StartListSkierOutDto> startListSkierOutDtos = new List<StartListSkierOutDto>();
            IEnumerable<StartListMember> startListMembers = _adoStartListDao.FindAllByRaceId(id);
            if (startListMembers == null)
            {
                return NotFound();
            }
            IEnumerable<RaceData> raceDatas = _adoRaceDataDao.FindAllByRaceId(id);
            if (raceDatas == null)
            {
                return NotFound();
            }
            foreach (var startListMember in startListMembers)
            {
                var raceData = raceDatas.FirstOrDefault(data => data.SkierId == startListMember.SkierId);
                var skier = _adoSkierDao.FindById(raceData.SkierId);
                startListSkierOutDtos.Add(StartListSkierOutDto.FromSkierRaceDataAndStartListMember(skier, raceData, startListMember));
            }
            
            return Ok(startListSkierOutDtos);
        }
    }
}