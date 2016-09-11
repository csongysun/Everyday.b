﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Everyday.b.Identity
{
    public class JwtOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }

        public double Expiration { get; set; }

        //public SigningCredentials SigningCredentials { get; set; } = new SigningCredentials();
    }

}
