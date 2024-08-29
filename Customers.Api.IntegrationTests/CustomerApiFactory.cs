using Customers.Api.Database;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Customers.Api.IntegrationTests
{
    public class CustomerApiFactory 
        : WebApplicationFactory<ApiAssembly>, IAsyncLifetime
    {
        private readonly IContainer dbContainer;
        // private readonly IContainer dbContainer = 
        // new PostgreSqlBuilder()
        // .WithImage("postgres:latest")
        // .WithDatabase("eventmanagement")
        // .WithUsername("postgres")
        // .WithPassword("postgres")
        // .Build();

        private readonly GithubApiServer githubApiServer = new();

        public const string GithubUser = "githubuser";
        public const string ThrottledGithubUser = "throttleduser";
        public CustomerApiFactory()
        {
            dbContainer = new ContainerBuilder()
            .WithImage("postgres:latest")
            .WithEnvironment("POSTGRES_USER", "postgres")
            .WithEnvironment("POSTGRES_PASSWORD", "postgres")
            .WithEnvironment("POSTGRES_DB", "mydb")
            .WithPortBinding(5555, 5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            });

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IHostedService));

                services.RemoveAll(typeof(IDbConnectionFactory));
                services.AddSingleton<IDbConnectionFactory>(_ =>
                    new NpgsqlConnectionFactory("Server=localhost;Port=5555;Database=mydb;User ID=postgres;Password=postgres;"));


                services.AddHttpClient("GitHub", httpClient =>
                {
                    httpClient.BaseAddress = new Uri(githubApiServer.Url);
                    httpClient.DefaultRequestHeaders.Add(
                        HeaderNames.Accept, "application/vnd.github.v3+json");
                    httpClient.DefaultRequestHeaders.Add(
                        HeaderNames.UserAgent, $"Course-{Environment.MachineName}");
                });

            });

        }
        public async Task InitializeAsync()
        {

            githubApiServer.Start();
            githubApiServer.Setup(GithubUser);
            githubApiServer.SetupThrottledUser(ThrottledGithubUser);

            await dbContainer.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            await dbContainer.DisposeAsync();
            githubApiServer.Dispose();
        }
    }
}
