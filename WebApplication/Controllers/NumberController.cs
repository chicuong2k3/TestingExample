using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class NumberController : Controller
    {
        private readonly ICalculateService calculateService;

        public NumberController(ICalculateService calculateService)
        {
            this.calculateService = calculateService;
        }
        public IActionResult GetSum(int a, int b)
        {
            return Ok(calculateService.GetSum(a, b));
        }
    }
}
