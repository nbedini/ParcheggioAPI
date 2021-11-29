using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParcheggioAPI;

namespace ParcheggioAPI
{
    public class SecurityKeyGenerator
    {
        public static SecurityKey GetSecurityKey()
        {
            //prendo la chiave e la trasformo in un array di bytes
            var key = Encoding.ASCII.GetBytes(Startup.MasterKey);

            //la passo all'istanza che mi crea e ritorna la chiave di sicurezza
            return new SymmetricSecurityKey(key);
        }
    }
}
