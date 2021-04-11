namespace ECommerceOrders.Configuration
{
    public interface IConfigurationRepository
    {
        string GetBaseUrl();
        string GetApiKey();
    }
}