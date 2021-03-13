using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionsTable
{
   public class FakeData : TableEntity
    {
        public Int64 Fibonacci { get; set; }
        public Int64 LottoVinstChansProcent { get; set; }
        public string DeviceId { get; set; }
        public Int64 EpochTime { get; set; }
        public string school { get; set; }
        public string name { get; set; }


    }
}
