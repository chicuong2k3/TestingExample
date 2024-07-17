using NUnit.Framework;
using Testing.Fundamentals;
using Assert = NUnit.Framework.Assert;

namespace Testing.UnitTests
{
    [TestFixture]
    internal class ProductsControllerTests
    {
        [Test]
        public void GetProductById_IdIsZero_ReturnsNotFound()
        {
            var controller = new ProductsController();

            var result = controller.GetProductById(0);

            Assert.That(result, Is.TypeOf<NotFound>());

            Assert.That(result, Is.InstanceOf<NotFound>());
        }
    }
}
