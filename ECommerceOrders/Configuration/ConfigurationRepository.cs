using Microsoft.Extensions.Configuration;

namespace ECommerceOrders.Configuration
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly IConfiguration _configuration;
        public ConfigurationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetBaseUrl()
        {
            return $"{_configuration.GetValue<string>("CustomerDetails:BaseUrl")}";
        }

        public string GetApiKey()
        {
            return $"{_configuration.GetValue<string>("CustomerDetails:ApiKey")}";
        }

    }
}
