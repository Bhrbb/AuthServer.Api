using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Configuration;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Extensions
{
    public static class CustomTokenAuth
    {
        public static void AddCustomTokenAuth(this IServiceCollection services,CustomTokenOptions customTokenOptions)
        {
            services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                

                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {

                    ValidIssuer = customTokenOptions.Issuer,
                    ValidAudience = customTokenOptions.Audience[0],
                    IssuerSigningKey = SingService.GetSymmetricSecurityKey(customTokenOptions.SecurityKey),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero//omrunu kontrol ederken senın verdiğinin üstüne default
                                             //5 dk ekler farklı serverlardan istek yaparken geciklemeye tolerans tanıyor


                };
            });

        }
    }
}
