using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttandanceChecker
{
    [Serializable]
    public class Clients
    {

        public string deviceName { get; set; }
        public string clientName { get; set; }
        public string clientSurname { get; set; }
        public string clientAvtoName { get; set; }
        public string clientRegNumber { get; set; }
        public string clientDateBirth { get; set; }

        public Clients()
        {
            deviceName = "";
            clientName = "";
            clientSurname = "";
            clientAvtoName = "";
            clientRegNumber = "";
            clientDateBirth = "";
        }
    }

    public class ClientsStock : Clients
    {
        public ClientsStock()
        {
            deviceName = "";
            clientRegNumber = "";
        }
    }
}
