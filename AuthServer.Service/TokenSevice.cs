﻿using AuthServer.Core.Configuration;
using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using AuthServer.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configuration;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service
{
    public class TokenSevice : ITokenService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly CustomTokenOptions _tokenOption;
        public TokenSevice(UserManager<UserApp>userManager,IOptions<CustomTokenOptions>options)
        {
            _userManager = userManager;
            _tokenOption = options.Value;
        }

        private string CreateRefreshToken()
        {
            // return Guid.NewGuid()
            var numberByte = new Byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);

        }

        private IEnumerable<Claim>GetClaim(UserApp userApp, List<string> audiences)//kullanıcı ile giren api
        {
            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,userApp.Id),
                new Claim(JwtRegisteredClaimNames.Email,userApp.Email),
                new Claim(ClaimTypes.Name, userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud,x)));
            return userList;

        }

        private IEnumerable<Claim>GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            new Claim(JwtRegisteredClaimNames.Sub, client.ClientId.ToString());

            return claims;
        }
        public TokenDto CreateToken(UserApp userApp)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccesTokenExpiration);//tokenın ömrü
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration);//tokenın ömrü
            var securityKey=SingService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);//tokenın imzası
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);//şifreleme yöntemi
            JwtSecurityToken jwtSecurityToken=new JwtSecurityToken(
                issuer: _tokenOption.Issuer, //yayınlayan
                expires:accessTokenExpiration,
                notBefore:DateTime.Now,
                claims:GetClaim(userApp,_tokenOption.Audience),
                signingCredentials:signingCredentials);
            var handler=new JwtSecurityTokenHandler();
            var token=handler.WriteToken(jwtSecurityToken);
            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration

            };
            return tokenDto;



        }

        public ClientTokenDto CreateTokenByClient(Client client)//Üyelik Sistemi olmayan Api
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccesTokenExpiration);//tokenın ömrü

            var securityKey = SingService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);//tokenın imzası
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);//şifreleme yöntemi
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOption.Issuer, //yayınlayan
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaimsByClient(client),
                signingCredentials: signingCredentials);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            var tokenDto = new ClientTokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration,

            };
            return tokenDto;
        }
     
    }
}
