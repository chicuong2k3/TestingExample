namespace Testing.Fundamentals
{
    public interface IActionResult { }
    public class NotFound : IActionResult { }
    public class Ok : IActionResult { }
    public class ProductsController
    {
        public IActionResult GetProductById(int id)
        {
            if (id == 0) return new NotFound();

            return new Ok();
        }

    }
}
