using Microsoft.AspNetCore.Mvc;
using site_machine_tracker.Data;
using site_machine_tracker.Data.Database;

namespace site_machine_tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController(
        ILogger<MachineController> logger,
        IRepository repository)
        : ControllerBase
    {
        [HttpGet("{userId:int}")]
        public ActionResult<IEnumerable<MachineListItem>> GetMachines(int userId, string? machineName = null, MachineType? machineType = null)
        {
            var user = repository.GetUser(userId);
            if (user is null)
                return NotFound($"User (id:{userId}) does not exist");

            var site = repository.GetSite(user.SiteId);
            if (site is null)
                return NotFound($"Site (id:{user.SiteId}) does not exist");

            var machines = repository.ListMachines(site.Id)
                .Where(m =>
                {
                    return
                        (machineName is null || m.Name.Contains(machineName, StringComparison.CurrentCultureIgnoreCase)) &&
                        (machineType is null || m.MachineType == machineType);
                })
                .Select(m => new MachineListItem()
                {
                    Name = m.Name,
                    MachineType = m.MachineType,
                    SiteName = site.Name,
                    Location = m.Location
                });
            return Ok(machines);
        }

        [HttpPost("{machineId:int}/{latitude:double}/{longitude:double}")]
        public ActionResult UpdateMachineLocation(int machineId, double latitude, double longitude)
        {
            if (repository.GetMachine(machineId) is null)
                return NotFound($"Machine (id:{machineId}) does not exist");

            repository.UpdateMachineLocation(machineId, new() { Latitude = latitude, Longitude = longitude });
            return Ok();
        }
    }
}
