using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Builders;
using Microsoft.Playwright;

namespace Testing.UITesting
{
    public class SharedTestContext : IAsyncLifetime
    {
        public const string ValidGithubUsername = "validuser";
        public const string AppUrl = "https://localhost:7780";
        public GithubApiServer GithubApiServer { get; } = new();
        private static readonly string dockerComposeFile =
            Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "docker-compose.integration.yml"));


        private IPlaywright playwright;
        public IBrowser Browser { get; private set; }


        private readonly ICompositeService dockerService = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(dockerComposeFile)
            .RemoveOrphans()
            .WaitForHttp("test-app", AppUrl)
            .Build();
        public async Task DisposeAsync()
        {
            GithubApiServer.Dispose();

            dockerService.Dispose();

            await Browser.DisposeAsync();
            playwright.Dispose();
        }

        public async Task InitializeAsync()
        {
            GithubApiServer.Start();
            GithubApiServer.Setup(ValidGithubUsername);

            dockerService.Start();

            playwright = await Playwright.CreateAsync();
            Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 1000
            });
        }
    }
}
