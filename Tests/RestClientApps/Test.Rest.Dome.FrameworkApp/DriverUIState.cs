using RACI.Client;
using RACI.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAscom.App.ControlPanel
{
    public class DriverUIState
    {
        public DriverUIState()
        {
            Driver = new RestDriver();

        }
        public RestDriver Driver { get; set; } = new RestDriver();
        public ObservableCollection<RaciEndpoint> Endpoints { get; set; } = new ObservableCollection<RaciEndpoint>();
        public ObservableCollection<RaciEndpointDriver> Drivers { get; set; } = new ObservableCollection<RaciEndpointDriver>();

    }
}
