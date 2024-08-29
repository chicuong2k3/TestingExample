using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace FakeApi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var wiremockServer = WireMockServer.Start();

            Console.WriteLine($"Listening on {wiremockServer.Url}...");

            wiremockServer
                .Given(Request.Create()
                .WithPath("/users/nickchapsas")
                .UsingGet())
                .RespondWith(Response.Create()
                .WithBodyAsJson(
                """
                    {
                        "id": 1,
                        "username": "nickchapsas",
                        "email": "fsdfsa"
                    }
                """)
                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(200));

            Console.ReadLine();

            wiremockServer.Dispose();
        }
    }
}
