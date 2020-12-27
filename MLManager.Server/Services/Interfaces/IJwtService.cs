using System;
using MLManager.Database;

namespace MLManager.Services
{
    public struct CreateJwtResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpirationTimestamp { get; set; }
    }
    public interface IJwtService
    {
        CreateJwtResponse CreateJwt(User user);
    }
}