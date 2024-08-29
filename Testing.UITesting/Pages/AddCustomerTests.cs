using Bogus;
using Customers.Api.Contracts.Requests;
using Customers.Api.Domain;
using FluentAssertions;
using Microsoft.Playwright;

namespace Testing.UITesting.Pages
{
    [Collection("Test Collection")]
    public class AddCustomerTests
    {
        private readonly SharedTestContext _testContext;
        public AddCustomerTests(SharedTestContext testContext)
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
        public async Task Create_CreateCustomer_WhenDataIsValid()
        {
            // Arrange
            var customer = customerGenerator.Generate();

            var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
            {
                BaseURL = SharedTestContext.AppUrl
            });
            await page.GotoAsync("add-customer");

            // Act
            await page.FillAsync("input[id=fullname]", customer.FullName.Value);
            await page.FillAsync("input[id=email]", customer.Email.Value);
            await page.FillAsync("input[id=dob]", customer.DateOfBirth.Value.ToString("yyyy-MM-dd"));
            await page.FillAsync("input[id=github-username]", customer.GitHubUsername.Value);

            await page.ClickAsync("button[type=submit]");

            // Assert
            var linkElement = page.Locator("article>p>a").First;

            var link = await linkElement.GetAttributeAsync("href");
            await page.GotoAsync(link!);

            (await page.Locator("p[id=fullname-field]").InnerTextAsync())
                .Should().Be(customer.FullName.Value);
            (await page.Locator("p[id=email-field]").InnerTextAsync())
                .Should().Be(customer.Email.Value);
            (await page.Locator("p[id=github-username-field]").InnerTextAsync())
                .Should().Be(customer.GitHubUsername.Value);
            (await page.Locator("p[id=dob-field]").InnerTextAsync())
                .Should().Be(customer.DateOfBirth.Value.ToString("yyyy/MM/dd"));
        
            await page.CloseAsync();
        }

        [Fact]
        public async Task Create_ShowError_WhenEmailIsInvalid()
        {
            // Arrange
            var customer = customerGenerator.Generate();

            var page = await _testContext.Browser.NewPageAsync(new BrowserNewPageOptions
            {
                BaseURL = SharedTestContext.AppUrl
            });
            await page.GotoAsync("add-customer");

            // Act
            await page.FillAsync("input[id=fullname]", customer.FullName.Value);
            await page.FillAsync("input[id=email]", "invalidemail");
            await page.FillAsync("input[id=dob]", customer.DateOfBirth.Value.ToString("yyyy-MM-dd"));
            await page.FillAsync("input[id=github-username]", customer.GitHubUsername.Value);

            await page.ClickAsync("button[type=submit]");

            // Assert
            var element = page.Locator("li.validation-message").First;

            var text = await element.InnerTextAsync();

            text.Should().Be("invalid email format");
        }

    }
}
