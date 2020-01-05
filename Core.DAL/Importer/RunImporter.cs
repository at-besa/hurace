using System.Collections.Generic;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.DAL.Domain;

namespace Hurace.Core.DAL.Importer
{
    public class RunImporter : IImporter
    {
        private AdoRunDao _adoRunDao;
        private AdoRaceDao _adoRaceDao;
        private AdoRaceTypeDao _adoRaceTypeDao;
        
        public RunImporter(IConnectionFactory connectionFactory)
        {
            _adoRunDao = new AdoRunDao(connectionFactory);
            _adoRaceDao = new AdoRaceDao(connectionFactory);
            _adoRaceTypeDao = new AdoRaceTypeDao(connectionFactory);
        }
        public void Import()
        {
            IEnumerable<Race> races = _adoRaceDao.FindAll();
            foreach (var race in races)
            {
                for (int i = 0; i < race.Type.NumberOfRuns; i++)
                {
                    _adoRunDao.Insert(new Run()
                    {
                        RaceId = race.Id,
                        RunNo = i + 1,
                        Running = (i == 0 && race.Status.Id == 2) ? true : false,
                        Finished = race.Status.Id == 4 ? true : false
                    });
                }
            }
            
        }
    }
}