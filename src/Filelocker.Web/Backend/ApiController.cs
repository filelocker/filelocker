using Microsoft.AspNetCore.Mvc;

namespace Filelocker.Backend
{
    public class ApiController : Controller
    {
        [HttpGet]
        [Route("/api/hello")]
        public string Hello(string name)
        {
            return $"Hello {name}";
        }
    }
}