using System;

namespace MLManager.Controllers
{
    public class AuthenticationRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid DeviceId { get; set; }
    }
}