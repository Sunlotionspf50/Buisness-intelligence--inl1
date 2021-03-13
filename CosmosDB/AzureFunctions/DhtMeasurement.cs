using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctions
{
    public class DhtMeasurement
    {
        public string epochtime { get; set; }
        public string DeviceId { get; set; }
        public string school { get; set; }
        public string name { get; set; }
        public string temperature { get; set; }
        public string humidity { get; set; }

    }
}