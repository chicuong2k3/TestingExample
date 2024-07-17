using NUnit.Framework;
using Testing.Fundamentals;
using Assert = NUnit.Framework.Assert;
using IgnoreAttribute = NUnit.Framework.IgnoreAttribute;

namespace Testing.UnitTests
{
    [TestFixture]
    internal class CalculatorTests
    {
        private Calculator calculator = default!;

        // This method will be invoked before the starting of any Test
        [SetUp]
        public void SetUp()
        {
            calculator = new Calculator();
        }

        [Test]
        [Ignore("Because I don't need it anymore.")]
        public void Add_IsCalled_ReturnsSumOfArgs()
        {
            // Act
            int sum = calculator.Add(1, 2);

            // Assert
            Assert.That(sum, Is.EqualTo(3));
        }

        //[Test]
        //public void Max_TheFirstArgIsGreater_ReturnsTheFirstArg()
        //{
        //    // Act
        //    int result = calculator.Max(2, 1);

        //    // Assert
        //    Assert.That(result, Is.EqualTo(2));
        //}

        //[Test]
        //public void Max_TheFirstArgIsLesser_ReturnsTheSecondArg()
        //{
        //    // Act
        //    int result = calculator.Max(1, 2);

        //    // Assert
        //    Assert.That(result, Is.EqualTo(2));
        //}

        //[Test]
        //public void Max_ArgsAreEqual_ReturnsTheSameArg()
        //{
        //    // Act
        //    int result = calculator.Max(1, 1);

        //    // Assert
        //    Assert.That(result, Is.EqualTo(1));
        //}

        [Test]
        [TestCase(2, 1, 2)]
        [TestCase(1, 2, 2)]    
        [TestCase(1, 1, 1)]    
        
        public void Max_WhenCalled_ReturnsTheGreaterArg(int a, int b, int expectedResult)
        {
            var result = calculator.Max(a, b);

            Assert.That(result, Is.EqualTo(expectedResult));


        }

        public void GetEvenNumbers_LimitIsNotNegative_ReturnsEvenNumbersUpToLimit()
        {
            var result = calculator.GetEvenNumbers(5);

            // too general
            Assert.That(result, Is.Not.Empty);

            Assert.That(result.Count(), Is.EqualTo(3));

            Assert.That(result, Is.EquivalentTo(new[] { 0, 2, 4 }));
        }

        [Test]
        public void SetSpeed_ValidSpeed_SetTheSpeedProperty()
        {
            calculator.SetSpeed(10);

            Assert.That(calculator.Speed, Is.EqualTo(10));
        }

        [Test]
        public void SetSpeed_InvalidSpeed_ThrowArgumentException()
        {
            Assert.That(() => calculator.SetSpeed(-10), Throws.ArgumentException);
        }

        [Test]
        public void SetSpeed_ValidSpeed_RaiseSpeedSetEvent()
        {
            var speed = -1;

            calculator.SpeedSet += (sender, args) =>
            {
                speed = args;
            };

            calculator.SetSpeed(10);

            Assert.That(speed, Is.Not.EqualTo(-1));

            var x = new int[] { 1, 2, 3 };
        }
    }
}
