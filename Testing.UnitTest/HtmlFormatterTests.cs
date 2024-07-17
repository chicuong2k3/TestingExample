using NUnit.Framework;
using Testing.Fundamentals;
using Assert = NUnit.Framework.Assert;

namespace Testing.UnitTests
{
    [TestFixture]
    internal class HtmlFormatterTests
    {
        private HtmlFormatter htmlFormatter = default!;

        [SetUp]
        public void SetUp()
        {
            htmlFormatter = new HtmlFormatter();
        }

        [Test]
        public void GetBold_WhenCalled_ShouldEncloseTextWithStrongTag()
        {
            var result = htmlFormatter.GetBold("Some text");

            Assert.That(result, Is.EqualTo("<strong>Some text</strong>").IgnoreCase);
            Assert.That(result, Is.EqualTo("<strong>Some text</strong>").IgnoreCase);

            // more general
            Assert.That(result, Does.StartWith("<strong>"));
        }
    }
}
