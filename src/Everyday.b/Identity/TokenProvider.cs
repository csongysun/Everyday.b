using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Everyday.b.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Everyday.b.Identity
{
    public interface ITokenProvider
    {
        string GenerateToken(User user, out DateTime expireTime);
    }
    public class JwtTokenProvider : ITokenProvider
    {
        private readonly JwtOptions _jwtOptions;
        public JwtTokenProvider(IOptions<JwtOptions> options )
        {
            _jwtOptions = options.Value;
        }

        public string GenerateToken(User user, out DateTime expireTime)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim("SecurityStamp",user.SecurityStamp), 
            };
            expireTime = DateTime.Now.AddMinutes(_jwtOptions.Expiration);
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expireTime,
                signingCredentials:
                new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SecretKey)),
                    SecurityAlgorithms.RsaSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
