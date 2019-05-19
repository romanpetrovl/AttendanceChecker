using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttandanceChecker
{
    public class Clients
    {

        public string deviceName { get; set; }
        public string clientName { get; set; }
        public string clientSurname { get; set; }

        public Clients()
        {
            deviceName = "";
            clientName = "";
            clientSurname = "";
        }
    }
}
