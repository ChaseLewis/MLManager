namespace MLManager
{
    public class JwtTokenConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public double ExpirationDuration { get; set; }
    }
}