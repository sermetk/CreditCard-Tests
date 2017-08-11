using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CreditCards.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id < 1)
                return BadRequest($"Invalid request for id {id}");

            return Content($"Value {id}");
        }

        [HttpPost("startJob")]
        public IActionResult StartJob()
        {
            return Ok("Batch Job Started");
        }
    }
}
