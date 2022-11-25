using EKupi.Infrastructure.Interfaces;

namespace EKupi.Infrastructure.AppSettings
{
    public class TokenSettings : ITokenSettings
    {
        public int ExpirationHours { get; set; }
        public string Secret { get; set; }
    }
}
