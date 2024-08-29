using Bogus;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace Customers.Api.IntegrationTests
{
    //[Collection("CustomerApi Collection")]
    public class CustomersControllerTests
        : IClassFixture<CustomerApiFactory>

    {
        //private readonly WebApplicationFactory<ApiAssembly> _webApplicationFactory;
        private readonly HttpClient _httpClient;
        //private readonly Faker<CustomerRequest> _customerGenerator
        //    = new Faker<CustomerRequest>()
        //    .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        //    .RuleFor(x => x.Email, faker => faker.Person.Email)
        //    .RuleFor(x => x.GitHubUsername, "chicuong2k3")
        //    .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

        //private readonly List<Guid> _createdIds = [];



        private readonly Faker<CustomerRequest> customerGenerator =
            new Faker<CustomerRequest>()
            .RuleFor(x => x.Email, faker => faker.Person.Email)
            .RuleFor(x => x.FullName, faker => faker.Person.FullName)
            .RuleFor(x => x.GitHubUsername, CustomerApiFactory.GithubUser)
            .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);
        public CustomersControllerTests(
            //WebApplicationFactory<ApiAssembly> webApplicationFactory
            CustomerApiFactory customerApiFactory
        )
        {
            //_webApplicationFactory = webApplicationFactory;
            _httpClient = customerApiFactory.CreateClient();
        }

        [Fact]
        public async Task Create_UserCreated_WhenDataIsValid()
        {
            var customer = customerGenerator.Generate();

            var response = await _httpClient.PostAsJsonAsync("customers", customer);

            var customerResponse =await response.Content.ReadFromJsonAsync<CustomerResponse>();

            customerResponse.Should().BeEquivalentTo(customer);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            response.Headers.Location
                !.ToString()
                .Should()
                .Be($"http://localhost/customers/{customerResponse!.Id}");   
        }

        [Fact]
        public async Task Create_ReturnsValidationError_WhenEmailInvalid()
        {
            const string invalidEmail = "abc";
            var customer = customerGenerator.Clone()
                .RuleFor(x => x.Email, invalidEmail).Generate();

            var response = await _httpClient.PostAsJsonAsync("customers", customer);


            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            error!.Status.Should().Be(400);
            error!.Title.Should().Be("One or more validation errors occurred.");
            error!.Errors["Email"][0].Should().Be($"{invalidEmail} is not a valid email address");
        }

        [Fact]
        public async Task Get_ReturnsCustomer_WhenCustomerExists()
        {
            // Arrange
            var customer = customerGenerator.Generate();
            var postResponse = await _httpClient.PostAsJsonAsync("customers", customer);
            var createdCustomer = await postResponse.Content.ReadFromJsonAsync<CustomerResponse>();
            // Act
            var response = await _httpClient.GetAsync($"customers/{createdCustomer!.Id}");

            var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            customerResponse.Should().BeEquivalentTo(createdCustomer);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            var response = await _httpClient.GetAsync($"customers/{Guid.NewGuid()}");

            //var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAll_ReturnsCustomers_WhenCustomersExist()
        {
            // Arrange
            var customer = customerGenerator.Generate();
            var postResponse = await _httpClient.PostAsJsonAsync("customers", customer);
            var createdCustomer = await postResponse.Content.ReadFromJsonAsync<CustomerResponse>();
            // Act
            var response = await _httpClient.GetAsync($"customers");
            var responseContent = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent!.Customers.Single().Should().BeEquivalentTo(createdCustomer);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyResult_WhenNoCustomer()
        {
            // Arrange
            
            // Act
            var response = await _httpClient.GetAsync($"customers");
            var responseContent = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent!.Customers.Should().BeEmpty();
        }

        [Fact]
        public async Task Create_ReturnsInternalServerError_WhenGithubIsThrottled()
        {
            var customer = customerGenerator.Clone()
                .RuleFor(x => x.GitHubUsername, CustomerApiFactory.ThrottledGithubUser)
                .Generate();

            var response = await _httpClient.PostAsJsonAsync("customers", customer);

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        //[Fact]
        //public async Task Get_ReturnsNotFound_WhenCustomerDoesNotExist()
        //{
        //    var response = await _httpClient.GetAsync($"customers/{Guid.NewGuid()}");

        //    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        //    var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        //    problemDetails.Should().NotBeNull();
        //    problemDetails!.Title.Should().Be("Not Found");
        //    problemDetails.Status.Should().Be((int)HttpStatusCode.NotFound);

        //    response.Headers.Location.Should().BeNull();    
        //}

        //[Fact]
        //public async Task Create_ReturnsCreated_WhenCustomerIsCreated()
        //{
        //    // Arrange
        //    var customer = _customerGenerator.Generate();

        //    // Act
        //    var response = await _httpClient.PostAsJsonAsync("customers", customer);

        //    var responseBody = await response.Content.ReadFromJsonAsync<CustomerResponse>();

        //    // Assert 
        //    responseBody.Should().BeEquivalentTo(customer);

        //    response.StatusCode.Should().Be(HttpStatusCode.Created);

        //    _createdIds.Add(responseBody!.Id);

        //}

        //public Task InitializeAsync()
        //{
        //    return Task.CompletedTask;
        //}

        //public async Task DisposeAsync()
        //{
        //    foreach (var createdId in _createdIds)
        //    {
        //        await _httpClient.DeleteAsync($"customers/{createdId}");

        //    }
        //}
    }
}