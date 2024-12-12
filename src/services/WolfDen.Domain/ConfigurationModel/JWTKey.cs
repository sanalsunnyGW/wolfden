namespace WolfDen.Domain.ConfigurationModel
{
    public class JwtKey
    {
        public string Key { get; set; }
        public string Issuer {  get; set; }
        public string Audience { get; set; }
    }
}
