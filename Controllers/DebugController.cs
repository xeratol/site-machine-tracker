#if DEBUG
using Microsoft.AspNetCore.Mvc;
using site_machine_tracker.Data.Database;

namespace site_machine_tracker.Controllers
{
    [Route("debug/[controller]")]
    [ApiController]
    public class DebugController(
        ILogger<MachineController> logger,
        IDebugRepository repository)
        : ControllerBase
    {
        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return Ok(repository.AllUsers);
        }

        [HttpGet("sites")]
        public ActionResult<IEnumerable<Site>> GetAllSites()
        {
            return Ok(repository.AllSites);
        }

        [HttpGet("machines")]
        public ActionResult<IEnumerable<Machine>> GetAllMachines()
        {
            return Ok(repository.AllMachines);
        }
    }
}
#endif