#if DEBUG
namespace site_machine_tracker.Data.Database
{
    public interface IDebugRepository
    {
        public IEnumerable<User> AllUsers { get; }
        public IEnumerable<Machine> AllMachines { get; }
        public IEnumerable<Site> AllSites { get; }
    }
}
#endif