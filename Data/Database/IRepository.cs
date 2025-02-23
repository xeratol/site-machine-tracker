namespace site_machine_tracker.Data.Database
{
    public interface IRepository
    {
        Machine? GetMachine(int id);
        Site? GetSite(int id);
        User? GetUser(int id);
        IEnumerable<Machine> ListMachines(int siteId);
        Machine? UpdateMachineLocation(int id, Location newLocation);
    }
}
