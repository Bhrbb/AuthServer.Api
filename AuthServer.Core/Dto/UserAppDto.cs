using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Dto
{
    public class UserAppDto
    {
        //UserApp i dönmedik cunku onun ıcınde cok fazla bilgi var clientin sadece ihtiyacı oldugu seylerı buraya koyduk
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }



    }
}
