using Hurace.Dal.Ado;
using Hurace.Dal.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hurace.Dal.Importer
{
    class SplittimesImporter : IImporter
    {
        private AdoRaceDataDao adoRaceDataDao;
        private AdoRaceDao adoRaceDao;
        private AdoRaceTypeDao adoRaceTypeDao;
        public SplittimesImporter(IConnectionFactory connectionfactroy)
        {
            adoRaceDataDao = new AdoRaceDataDao(connectionfactroy);
            adoRaceDao = new AdoRaceDao(connectionfactroy);
            adoRaceTypeDao = new AdoRaceTypeDao(connectionfactroy);
        }
        public void Import()
        {
            //get all racedata
            //join with race on raceId for typeId and splittimes 
            //join with racetype on typeId for numberOf runs

                //insert into splittime foreach racedata
                    //check if skier is disqualified for this race
                        // if so calculate random splittime of random run when he is disqualified
                        //insert numberOf runs times
                            //insert number of splittimes
                                //get splittime with random variance
            throw new NotImplementedException();
        }


    }
}
