using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Services
{
    public static class SingService//yardımcı bir class
    {
        //aynı key ıle ımzalayıp aynı key ıle dogrularsan simetrik
        //private key ile imza public farkllı key ile dogrularsan asimetetrik
        public static SecurityKey GetSymmetricSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
