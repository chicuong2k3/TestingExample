using Moq;
using NUnit.Framework;
using Testing.Services;
using Assert = NUnit.Framework.Assert;

namespace Testing.UnitTesting.NUnit
{
    [TestFixture]
    public class BankAccountTests
    {
        //private BankAccount bankAccount;
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void Deposit_AmountIs100_ShouldIncrease100()
        {
            // Arrange
            var mockLogger = new Mock<ICustomLogger>();
            mockLogger.Setup(x => x.LogInformation(""));


            var bankAccount = new BankAccount(mockLogger.Object);
            var initialBalance = bankAccount.Balance;
            var amount = 100;

            // Act
            bankAccount.Deposit(amount);

            // Assert
            Assert.That(bankAccount.Balance, Is.EqualTo(initialBalance + amount));
        }

        [Test]
        public void Withdraw_Withdraw100With200Balance_ReturnsTrue()
        {
            // Arrange
            var mockLogger = new Mock<ICustomLogger>();
            mockLogger.Setup(x => 
                x.LogInformation(It.IsAny<string>())
            ).Returns(true);

            mockLogger.Setup(x =>
                x.LogInformationToDatabase(It.IsAny<double>(), It.IsAny<double>())
            ).Returns(true);


            var bankAccount = new BankAccount(mockLogger.Object);
            bankAccount.Deposit(200);
            var initialBalance = bankAccount.Balance;
            var amount = 100;

            // Act
            var result = bankAccount.Withdraw(amount);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(bankAccount.Balance, Is.EqualTo(initialBalance - amount));
        }

        [Test]
        public void Withdraw_Withdraw300With200Balance_ReturnsTrue()
        {
            // Arrange
            var mockLogger = new Mock<ICustomLogger>();

            mockLogger.Setup(x =>
                x.LogInformationToDatabase(It.IsAny<double>(), It.IsAny<double>())
            ).Returns((double amount, double balance) => amount <= balance); 


            var bankAccount = new BankAccount(mockLogger.Object);
            bankAccount.Deposit(200);
            var initialBalance = bankAccount.Balance;
            var amount = 300;

            // Act
            var result = bankAccount.Withdraw(amount);

            // Assert
            Assert.That(result, Is.False);
        }



        [Test]
        public void LogMockWithOutput()
        {
            // Arrange
            var mockLogger = new Mock<ICustomLogger>();
            string exptectedOutput = string.Empty;

            mockLogger.Setup(x =>
                x.LogWithOuput(It.IsAny<string>(), out exptectedOutput)
            );

            // Act
            string result = string.Empty;
            mockLogger.Object.LogWithOuput("hello", out result);

            // Assert
            Assert.That(
                result, 
                Is.EqualTo(exptectedOutput)
            );
        }

        //[Test]
        //public void LogMockWithRef()
        //{
        //    // Arrange
        //    var mockLogger = new Mock<ICustomLogger>();
        //    var customer = new Customer();

        //    mockLogger.Setup(x =>
        //        x.LogWithOuput(ref customer)
        //    );

        //    // Assert
        //    Assert.That(
        //        mockLogger.Object.LogWithOuput(ref customer),
        //        Is.False
        //    );
        //}

        [Test]
        public void LogMockProperties()
        {
            // Arrange
            var mockLogger = new Mock<ICustomLogger>();

            mockLogger.Setup(x =>
                x.LogType
            ).Returns("information");

            //mockLogger.SetupAllProperties();
            //mockLogger.Object.LogType = "information";

            // Act
            // Assert
            Assert.That(
                mockLogger.Object.LogType,
                Is.EqualTo("information")
            );


            // callback
            double remain = 0;
            mockLogger.Setup(x =>
                x.LogInformationToDatabase(It.IsAny<double>(), It.IsAny<double>())
            ).Returns((double a, double b) => a <= b)
            .Callback((double a, double b) => remain = b - a);

            var bankAccount = new BankAccount(mockLogger.Object);
            var initialBalance = bankAccount.Balance;
            var amount = 100;
            bankAccount.Deposit(amount);

            Assert.That(remain, Is.EqualTo(initialBalance - amount));
        }

        [Test]
        public void MockVerification()
        {
            // Arrange
            var mockLogger = new Mock<ICustomLogger>();
            
            mockLogger.Verify(x =>
                x.LogInformation(It.IsAny<string>()), Times.Exactly(2)
            );

            
        }
    }
}
