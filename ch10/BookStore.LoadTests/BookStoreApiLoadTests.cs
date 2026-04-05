using NBomber.CSharp;
using NBomber.Http.CSharp;

namespace BookStore.LoadTests;

public class BookStoreApiLoadTests
{
    [Fact]
    public void GetAllBooksTest()
    {
        HttpClient httpClient = Http.CreateDefaultClient();
        httpClient.BaseAddress = new Uri("http://localhost:5237");

        var scenario = Scenario.Create("get_all_books", async context =>
        {
            var response = await httpClient.GetAsync("/api/books");
            return response.IsSuccessStatusCode ? Response.Ok() :
              Response.Fail();
        })
        .WithLoadSimulations(
            // Phase 1: Ramp up from 0 to 50 users over 10 seconds
            Simulation.RampingConstant(
                copies: 50,
                during: TimeSpan.FromSeconds(10)),
            // Phase 2: Maintain 50 concurrent users for 30 seconds
            Simulation.KeepConstant(
                copies: 50,
                during: TimeSpan.FromSeconds(30)),
            // Phase 3: Ramp down to 0 over 10 seconds
            Simulation.RampingConstant(
                copies: 0,
                during: TimeSpan.FromSeconds(10)));

        NBomberRunner
          .RegisterScenarios(scenario)
          .Run();

    }
}
