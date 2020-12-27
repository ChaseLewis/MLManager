using System;

namespace MLManager.Services
{
    public class JwtResponse
    {
        public string Token { get; set; }
        public Guid RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}