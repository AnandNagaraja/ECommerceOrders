using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ECommerceOrders.Exception;
using ECommerceOrders.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ECommerceOrders.Services
{
    public class UserService : IUserService
    {

        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private string _baseUrl;
        private string _apiCode;
        private const string GetUserDetailsRoute = "GetUserDetails";

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient();
        }

        public async Task<Customer> GetCustomerAsync(string customerEmailId)
        {

            var response = await _client.GetAsync(BuildCustomerApiUri(customerEmailId));

            if (!response.IsSuccessStatusCode)
                throw new ECommerceValidationException("Customer Email does not exists", HttpStatusCode.NotFound);

            return JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
        }


        public string BuildCustomerApiUri(string customerEmail)
        {
            _baseUrl ??= $"{_configuration.GetValue<string>("CustomerDetails:BaseUrl")}";
            _apiCode ??= $"{_configuration.GetValue<string>("CustomerDetails:ApiKey")}";

            return $"{_baseUrl}/{GetUserDetailsRoute}?code={_apiCode}&email={customerEmail}";
        }

    }
}
