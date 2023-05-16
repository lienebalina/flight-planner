using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class CleanupController : ControllerBase
    {
        private readonly FlightStorage _storage;
        public CleanupController(FlightStorage storage)
        {
            _storage = storage;
        }

        [HttpPost]
        [Route("clear")]
        public IActionResult Clear()
        {
            _storage.Clear();
            return Ok();
        }
    }
}
