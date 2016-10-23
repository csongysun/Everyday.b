using Everyday.b.Common;
using Everyday.b.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Everyday.Test.Identity
{
    public class TodoManagerTest
    {

        public async void TokenRefreshTest()
        {
            var _tokenProvider = new JwtTokenProvider(new OptionsWrapper<JwtOptions>(new JwtOptions()));
            var refreshToken = "";
            ClaimsPrincipal principal;
            principal = _tokenProvider.ValidateRefreshToken(refreshToken);

            var securityStamp = principal.Claims.Where(c => c.Type == "SecurityStamp")
                .Select(c => c.Value).FirstOrDefault();
        }


    }
}
