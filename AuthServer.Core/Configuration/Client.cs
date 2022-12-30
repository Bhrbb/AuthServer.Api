using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Configuration
{
    public  class Client
    {
        public string ClientId { get; set; }//kullanıcı adı ve şifre gibi
        public string ClientSecret { get; set; }
        public List<string> Audiences { get; set; }
        //apilerden hangisine erişebilir onu belirtmek için 


    }
}
