using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CreditCards.Integration.Test
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public static readonly string AntiForgeryFieldName = "__AFTField";
        public static readonly string AntiForgeryCookieName = "AFTCOokie";
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            // https://github.com/aspnet/Hosting/issues/959#issuecomment-286351703
            // Fix problem with 500 errors when running the test server
            var builder = new WebHostBuilder()
                .UseContentRoot(GetContentRootPath())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    ((ConfigurationBuilder)config)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .ConfigureServices(x =>
                {
                    x.AddAntiforgery(t =>
                    {
                        t.CookieName = AntiForgeryCookieName;
                        t.FormFieldName = AntiForgeryFieldName;
                    });
                });
            //.UseApplicationInsights()

            _testServer = new TestServer(builder);
            Client = _testServer.CreateClient();
        }

        public async Task<(string fieldValue, string cookieValue)> ExtractAntiForgeryValues(HttpResponseMessage response)
        {
            return (ExtractAntiForgeryToken(await response.Content.ReadAsStringAsync()), ExtractAntiForgeryCookieValueFrom(response));
        }

        private string GetContentRootPath()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToWebProject = @"..\..\..\..\CreditCards";
            return Path.Combine(testProjectPath, relativePathToWebProject);
        }
        
        private static string ExtractAntiForgeryCookieValueFrom(HttpResponseMessage response)
        {
            var antiForgeryCookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault(x => x.Contains(AntiForgeryCookieName));

            if (antiForgeryCookie is null)
                throw new ArgumentException($"Cookie '{AntiForgeryCookieName}' not found in HTTP response", nameof(response));

            var antiForgeryCookieValue = SetCookieHeaderValue.Parse(antiForgeryCookie).Value;

            return antiForgeryCookieValue.Value;
        }

        private string ExtractAntiForgeryToken(string htmlBody)
        {
            var requestVerificationTokenMatch = Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");

            if (requestVerificationTokenMatch.Success)
                return requestVerificationTokenMatch.Groups[1].Captures[0].Value;

            throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' not found in HTML", nameof(htmlBody));
        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}
