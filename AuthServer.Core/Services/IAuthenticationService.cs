using AuthServer.Core.Dto;
using SharedLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        //username ve password dogruysa token uret
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        Task<Response<TokenDto>>CreateTokenByRefreshToken(string refreshToken);
        //kullanıcı logout yapınca null tutmak ıcın 
        //yada kullanıcı refreshtokenı ele geçirip sürekli istek yapmasın diye
        Task<Response<NoDataDto>>RevokeRefreshToken(string refreshToken);
        Response<ClientTokenDto>CreateTokenByClient(ClientLoginDto clientLoginDto);


    }
}
