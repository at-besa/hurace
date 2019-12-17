using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Hurace.Core.DAL.Ado;
using Hurace.Core.DAL.Common;
using Hurace.Core.Logic.Interface;
using Hurace.Core.Logic.Model;

namespace Hurace.Core.Logic
{
    public class ScreenControlLogic : IScreenControl
    {
        public static ScreenControlLogic Instance = new ScreenControlLogic();
        private IConnectionFactory connectionFactory;
        public ICollection<ScreenControlModel> ScreenControls { get; set; }
        
        private ScreenControlLogic()
        {
            var configuration = ConfigurationUtil.GetConfiguration();
            connectionFactory = DefaultConnectionFactory.FromConfiguration(configuration, "HuraceDbConnection");
        }
        
        public async Task<ICollection<ScreenControlModel>> GetScreenControls()
        {
            return await Task.Run(() => {  ScreenControls = new Collection<ScreenControlModel>();
                var racecollection = new AdoRaceDao(connectionFactory).FindAll();
                foreach (var race in racecollection)
                {
                    ScreenControls.Add(new ScreenControlModel
                    {
                        Date = race.Date,
                        Location = race.Location,
                        Name = race.Name,
                        Sex = race.Sex,
                        Splittimes = race.Splittimes,
                        Status = "running",
                        Type = race.Type.Type
                    });
                }

                return ScreenControls;
            });
        }
    }
}