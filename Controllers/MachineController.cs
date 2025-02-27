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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<UserItem>> GetAllUsers()
        {
            // TODO improve getting site name
            return Ok(repository.AllUsers.Select(u => new UserItem() { Id = u.Id, Name = u.Name, SiteName = repository.GetSite(u.SiteId)?.Name }));
        }

        [HttpGet("Types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MachineTypeItem>> GetMachineTypes()
        {
            return Ok(Enum.GetValues(typeof(MachineType))
                .Cast<MachineType>()
                .Select(enumValue => new MachineTypeItem() { EnumValue = enumValue, Name = enumValue.ToString() }));
        }

        [HttpGet("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                    Location = m.Location
                });
            return Ok(machines);
        }

        [HttpPost("{machineId:int}/{latitude:double}/{longitude:double}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateMachineLocation(int machineId, double latitude, double longitude)
        {
            if (repository.GetMachine(machineId) is null)
                return NotFound($"Machine (id:{machineId}) does not exist");

            repository.UpdateMachineLocation(machineId, new() { Latitude = latitude, Longitude = longitude });
            return Ok();
        }
    }
}
