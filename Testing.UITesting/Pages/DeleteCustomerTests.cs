using Bogus;
using Customers.Api.Domain;
using FluentAssertions;
using Microsoft.Playwright;

namespace Testing.UITesting.Pages
{
    [Collection("Test Collection")]
    public class DeleteCustomerTests
    {
        private readonly SharedTestContext _testContext;
        public DeleteCustomerTests(SharedTestContext testContext)
        {
            _testContext = testContext;
        }

        private readonly Faker<Customer> customerGenerator =
            new Faker<Customer>()
            .RuleFor(x => x.Email.Value, faker => faker.Person.Email)
            .RuleFor(x => x.FullName.Value, faker => faker.Person.FullName)
            .RuleFor(x => x.GitHubUsername.Value, SharedTestContext.ValidGithubUsername)
            .RuleFor(x => x.DateOfBirth.Value, faker => DateOnly.FromDateTime(faker.Person.DateOfBirth.Date));



        [Fact]
        public async Task Delete_DeleteCustomer_WhenCustomerExists()
        {
            // Arrange
            var customer = CreateCustomer();

            var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
            {
                BaseURL = SharedTestContext.AppUrl
            });
            await page.GotoAsync($"customer/{customer.Id}");

            // Act
            page.Dialog += (_, dialog) => dialog.AcceptAsync();
            await page.ClickAsync("button.btn.btn-danger");

            // Assert
            await page.GotoAsync($"customer/{customer.Id}");

            (await page.Locator("p").First.InnerTextAsync())
                .Should().Be("No customer found with this id");

        }


        private async Task<Customer> CreateCustomer()
        {
            var customer = customerGenerator.Generate();

            var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
            {
                BaseURL = SharedTestContext.AppUrl
            });
            await page.GotoAsync("add-customer");

            await page.FillAsync("input[id=fullname]", customer.FullName.Value);
            await page.FillAsync("input[id=email]", "invalidemail");
            await page.FillAsync("input[id=dob]", customer.DateOfBirth.Value.ToString("yyyy-MM-dd"));
            await page.FillAsync("input[id=github-username]", customer.GitHubUsername.Value);

            await page.ClickAsync("button[type=submit]");


            return customer;
        }

    }
}
