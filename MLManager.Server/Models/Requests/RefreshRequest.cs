using System;

namespace MLManager.Controllers
{
    public class RefreshRequest
    {
        public Guid DeviceId { get; set; }
        public Guid RefreshToken { get; set; }
    }
}