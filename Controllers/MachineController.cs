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
        [HttpGet("Users")]
        public ActionResult<IEnumerable<UserItem>> GetAllUsers()
        {
            return Ok(repository.AllUsers.Select(u => new UserItem() { Id = u.Id, Name = u.Name }));
        }

        [HttpGet("Types")]
        public ActionResult<IEnumerable<MachineTypeItem>> GetMachineTypes()
        {
            return Ok(Enum.GetValues(typeof(MachineType))
                .Cast<MachineType>()
                .Select(enumValue => new MachineTypeItem() { EnumValue = enumValue, Name = enumValue.ToString() }));
        }

        [HttpGet("{userId:int}")]
        public ActionResult<IEnumerable<MachineItem>> GetMachines(int userId, string? machineName = null, MachineType? machineType = null)
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
                .Select(m => new MachineItem()
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
