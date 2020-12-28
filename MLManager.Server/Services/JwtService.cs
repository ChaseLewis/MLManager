using System;
using System.Text;
using System.Text.Json;  
using MLManager.Database;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace MLManager.Services
{
    public class JwtService : IJwtService
    {
        public double _expirationDuration;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly SigningCredentials _signingCredentials;

        public JwtService(IConfiguration config)
        {
            var jwtConfig = config.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
            
            _issuer = jwtConfig.Issuer;
            _audience = jwtConfig.Audience;
            _expirationDuration = jwtConfig.ExpirationDuration;
            _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),SecurityAlgorithms.HmacSha256);
        }

        public CreateJwtResponse CreateJwt(User user)
        {
            var claims = new Claim[]
            {
                new Claim("UserId",JsonSerializer.Serialize(user.UserId)),
                new Claim("FirstName",JsonSerializer.Serialize(user.FirstName)),
                new Claim("LastName",JsonSerializer.Serialize(user.LastName)),
                new Claim("Username",JsonSerializer.Serialize(user.Username))
                //Probably should include 'deviceId'
            };

            DateTime expirationTimestamp = DateTime.Now.AddMinutes(_expirationDuration);

            var jwtSecurityToken = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                _signingCredentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return new CreateJwtResponse
            {
                AccessToken = jwt,
                ExpirationTimestamp = expirationTimestamp.ToUniversalTime()
            };
        }
    }
}