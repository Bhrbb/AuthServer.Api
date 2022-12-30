using AuthServer.Core.Configuration;
using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using AuthServer.Core.Reposityory;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using SharedLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthentictionService : IAuthenticationService
    {

        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepostory<UserRefreshToken> _userRefreshTokenService;
        public AuthentictionService(IOptions<List<Client>>optionClient,ITokenService tokenService,UserManager<UserApp>userManager,
            IUnitOfWork unitOfWork,IGenericRepostory<UserRefreshToken>userRefreshTokenService)
        {
            _clients = optionClient.Value;
            _tokenService = tokenService;
            _userService = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;

        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));
            var user =await _userService.FindByEmailAsync(loginDto.Email);
            if (user == null) return Response<TokenDto>.Fail("Email or  Password is wrong", 400, true);
            if (!await _userService.CheckPasswordAsync(user,loginDto.Password))
            {
                return Response<TokenDto>.Fail("Email or  Password is wrong", 400, true);
            }
            var token = _tokenService.CreateToken(user);
            var userRefreshToken = await _userRefreshTokenService.Where(x=> x.UserID==user.Id).SingleOrDefaultAsync();
            if (userRefreshToken==null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken { UserID = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.Code= token.RefreshToken;
                userRefreshToken.Expiration= token.RefreshTokenExpiration;
            }
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Succes(token,200);

        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client=_clients.SingleOrDefault(x=>x.ClientId==clientLoginDto.ClientId && x.ClientSecret==clientLoginDto.ClientSecret);
            if (client==null)
            {
                return Response<ClientTokenDto>.Fail("Email or  Password is wrong", 400, true);
            }
            var token=_tokenService.CreateTokenByClient(client);
            return Response<ClientTokenDto>.Succes(token, 200);

        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existrefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existrefreshToken == null)
            {
                return Response<TokenDto>.Fail("RefreshToken not Found", 404, true);

            }
            var user = await _userService.FindByIdAsync(existrefreshToken.UserID);
            if (user == null)
            {
                return Response<TokenDto>.Fail("User ıd not found",404,true);
            }
            var tokenDto=_tokenService.CreateToken(user);
            existrefreshToken.Code = tokenDto.RefreshToken;
            existrefreshToken.Expiration = tokenDto.RefreshTokenExpiration;
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Succes(tokenDto,200);


           
        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var existrefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existrefreshToken == null)
            {
                return Response<NoDataDto>.Fail("RefreshToken not Found", 404, true);
            }
            _userRefreshTokenService.Remove(existrefreshToken);
            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Succes(200);
        }

   
    }
}
