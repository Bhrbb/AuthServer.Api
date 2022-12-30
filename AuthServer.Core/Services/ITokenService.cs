using AuthServer.Core.Configuration;
using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface ITokenService
    {//ITokenService Kendi içinde kullanıyoruz o yuzden Responsa gerek yok 

        TokenDto CreateToken(UserApp userApp);
        ClientTokenDto CreateTokenByClient(Client client);


    }
}
