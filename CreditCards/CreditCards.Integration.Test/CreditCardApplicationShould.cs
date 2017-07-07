using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Xunit;

namespace CreditCards.Integration.Test
{
    public class CreditCardApplicationShould
    {
        [Fact]
        public async Task RenderApplicationFormAsync()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(@"C:\Users\MIhsan\Documents\GitHub\CreditCard-Tests\CreditCards\CreditCards")
                .ConfigureAppConfiguration((hostingContext, config) =>  
                {
                    var env = hostingContext.HostingEnvironment;
                    ((ConfigurationBuilder)config)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .UseEnvironment("Development")
                .UseStartup<Startup>();
            //.UseApplicationInsights()

            var server = new TestServer(builder);

            var client = server.CreateClient();

            var response = await client.GetAsync("/apply");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("New Credit Card Application", responseString);
        }
    }
}
