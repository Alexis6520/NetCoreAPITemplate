namespace Domain.Settings
{
    public class JWTSettings
    {
        public string SecretKey { get; set; }
        public string ValidIssuer { get; set; }
        public List<string> ValidAudiences { get; set;}
    }
}
