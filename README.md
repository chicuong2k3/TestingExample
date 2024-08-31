# The Types of Testing

- Unit Testing
- Component Testing
- Integration Testing (prefered than Unit Testing if you need choose one)
- End-to-End Testing (System Testing)
- Performance Testing
  - Load Testing
  - Soak Testing
  - Spike Testing
  - Stress Testing

**Unit Testing, Component Testing, and Integration Testing are done before deployment.**

# Test Pyramid

# Libraries

[WireMock.Net (Mock External Services)](https://wiremock.org/docs/solutions/dotnet/)

[Moq](https://github.com/devlooped/moq)

[Bogus (Fake Data)](https://github.com/bchavez/Bogus)

[Testcontainers](https://testcontainers.com/)

[FluentAssertions](https://fluentassertions.com/)

[Playwright (UI Testing)](https://playwright.dev/dotnet/)

[FluentDocker (Control Docker)](https://github.com/mariotoffia/FluentDocker)

# Unit Testing

## Method Naming Convention
- the first part: the name of the method need to be tested.
-  the second part: the scenario.
- the third part: the expected behaviour.

## NUnit

### Global Initialization
```c#
private Calculator calculator = default!;

// This method will be invoked before the start of any test
[SetUp]
public void SetUp()
{
    calculator = new Calculator();
}
```

### Normal Test
```c#
[Test]
public void Max_TheFirstArgIsGreater_ReturnsTheFirstArg()
{
    // Act
    int result = calculator.Max(2, 1);

    // Assert
    Assert.That(result, Is.EqualTo(2));
}
```

### Test Multiple Values
```c#
[Test]
[TestCase(2, 1, ExpectedResult = 2)]
public int Max_WhenCalled_ReturnsTheGreaterArg(int a, int b)
{
    var result = calculator.Max(a, b);

    return result;
}
```

### Test with Expected Result

```c#
[Test]
[TestCase(2, 1, 2)]
[TestCase(1, 2, 2)]
[TestCase(1, 1, 1)]
public void Max_WhenCalled_ReturnsTheGreaterArg(int a, int b, int expectedResult)
{
    var result = calculator.Max(a, b);

    Assert.That(result, Is.EqualTo(expectedResult));
}
```


### Test Double Values
```c#
[Test]
[TestCase(2.1, 1.2)]
public double Add_TwoDoubleValues_ReturnsCorrectResult(double a, double b)
{
    var result = calculator.Max(a, b);

    Assert.AreEqual(3.3, result, 0.1);
}
```

### Test String Values
```c#
[Test]
public void GetBold_WhenCalled_ShouldEncloseTextWithStrongTag()
{
    // Act
    var result = htmlFormatter.GetBold("Some text");

    // Assert
    Assert.That(result, Is.EqualTo("<strong>Some text</strong>").IgnoreCase);
    Assert.That(result, Is.EqualTo("<strong>Some text</strong>").IgnoreCase);

    // More general
    Assert.That(result, Does.StartWith("<strong>"));

    // Regular expression
    Assert.That(result, Does.Match("<strong>.+</strong>"));
}
```

### Test Collections
```c#
[Test]
public void GetEvenNumbers_LimitIsNotNegative_ReturnsEvenNumbersUpToLimit()
{
    // Act
    var result = calculator.GetEvenNumbers(5);

    // Assert
    // Too general
    Assert.That(result, Is.Not.Empty);

    Assert.That(result.Count(), Is.EqualTo(3));

    Assert.That(result, Is.EquivalentTo(new[] { 0, 2, 4 }));

    Assert.That(result, Does.Contain(2));
    Assert.That(result, Has.No.Member(3));
    Assert.That(result, Is.Ordered.Descending);
    Assert.That(result, Is.Unique);
}
```

### Test if a Value is in A Range
```c#
[Test]
public double Add_TwoDoubleValues_ReturnsCorrectResult()
{
    var result = calculator.GetRandom(1, 10);

    Assert.That(result, Is.InRange(1, 10));
}
```

### Test Exceptions
```c#
[Test]
public void SetSpeed_InvalidSpeed_ThrowArgumentException()
{
    // Assert
    var exceptionDetails = Assert.Throws<ArgumentException>(() => calculator.SetSpeed(-10));
    Assert.That(exceptionDetails.Message, Does.StartWith("Speed must be greater than 0"));
    // Assert.That(, Throws.ArgumentException);
}
```

### Multiple Assert
```c#
[Test]
public void GetBold_WhenCalled_ShouldEncloseTextWithStrongTag()
{
    // Act
    var result = htmlFormatter.GetBold("Some text");

    // Assert
    Assert.Multiple(() => {
        Assert.That(result, Is.EqualTo("<strong>Some text</strong>").IgnoreCase);
        Assert.That(result, Is.EqualTo("<strong>Some text</strong>").IgnoreCase);
        Assert.That(result, Does.StartWith("<strong>"));
        Assert.That(result, Does.Match("<strong>.+</strong>"));
    
    });
}
```

### Test Object Type
```c#
[Test]
public void GetCustomer_WhenCalled_ReturnsCustomerObject()
{
	var result = customerService.GetCustomer(1);

	Assert.That(result, Is.TypeOf<PlatinumCustomer>());
    // Assert.That(result, Is.InstanceOf<Customer>());
}
```

### Test Event Handler
```c#
[Test]
public void SetSpeed_ValidSpeed_RaiseSpeedSetEvent()
{
    // Arrange
    var speed = -1;

    calculator.SpeedSet += (sender, args) =>
    {
        speed = args;
    };

    // Act
    calculator.SetSpeed(10);

    // Assert
    Assert.That(speed, Is.Not.EqualTo(-1));
}
```

# Integration Testing

1. Setup
2. Dependency Mocking
3. Execution
4. Assertion
5. Cleanup

```c#
// Setup Code is in Constructor

public async Task Test()
{
    var httplClient = new HttpClient() 
    {
        BaseAddress = new Uri("http://localhost:5000")
    };

    var response = await httplClient.GetAsync($"customers/{Guid.NewGuid()}");

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
}

public void Dispose()
{
	// Cleanup Code
}
```

## Test Internal Values

```c#
[assembly: InternalsVisibleTo("TestProjectFullName")]]
```

```xml
<ItemGroup>
    <InternalsVisibleTo Include="TestProjectFullName" />
</ItemGroup>
```
