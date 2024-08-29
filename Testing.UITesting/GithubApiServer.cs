using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Testing.UITesting
{
    public class GithubApiServer : IDisposable
    {
        private WireMockServer _wiremockServer;
        public string Url => _wiremockServer.Url!;
        public void Start()
        {
            _wiremockServer = WireMockServer.Start(9850);
        }

        public void Setup(string username)
        {
            _wiremockServer
                .Given(Request.Create()
                .WithPath($"/users/{username}")
                .UsingGet())
                .RespondWith(Response.Create()
                .WithBodyAsJson(
                $@"""
                    {{
                        ""id"": 1,
                        ""username"": ""{username}"",
                        ""email"": ""{username}@gmail.com""
                    }}
                """)
                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(200));
        }
        public void SetupThrottledUser(string username)
        {
            _wiremockServer
                .Given(Request.Create()
                .WithPath($"/users/{username}")
                .UsingGet())
                .RespondWith(Response.Create()
                .WithBodyAsJson(
                $@"""
                    {{
                        ""id"": 1,
                        ""username"": ""{username}"",
                        ""email"": ""{username}@gmail.com""
                    }}
                """)
                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(403));
        }

        public void Dispose()
        {
            _wiremockServer.Stop();
            _wiremockServer.Dispose();
        }
    }
}
