using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
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
                .ConfigureLogging((hostingContext, logging) => 
                {
                    logging.UseConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
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
