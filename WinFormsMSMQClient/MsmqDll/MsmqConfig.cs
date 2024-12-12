using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsmqDll
{
    public class MsmqConfig
    {
        public string IpAddress { get; set; }
        public string Message { get; set; }
        public string Label { get; set; }
        public string QueueName { get; set; }
        public string Type { get; set; }
        public string Command { get; set; }
    }
}
