using System.Diagnostics;
using System.Xml.Linq;

namespace site_machine_tracker.Data.Database
{
    /// <summary>
    /// In-memory database
    /// </summary>
    public class Repository
        : IRepository
#if DEBUG
        , IDebugRepository
#endif
    {
        private List<User> Users { get; } = [];
        private List<Machine> Machines { get; } = [];
        private List<Site> Sites { get; } = [];

        private int _lastUserId = 0;
        private int _lastMachineId = 0;
        private int _lastSiteId = 0;

        public IEnumerable<User> AllUsers => Users;
#if DEBUG
        public IEnumerable<Machine> AllMachines => Machines;
        public IEnumerable<Site> AllSites => Sites;
#endif

        public Repository()
        {
            InitializeSampleData();
            InitializeInvalidData();
        }

        public User? GetUser(int id)
        {
            return Users.FirstOrDefault(s => s.Id == id);
        }

        public Site? GetSite(int id)
        {
            return Sites.FirstOrDefault(s => s.Id == id);
        }

        public Machine? GetMachine(int id)
        {
            return Machines.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Machine> ListMachines(int siteId)
        {
            return Machines.Where(s => s.SiteId == siteId);
        }

        public Machine? UpdateMachineLocation(int id, Location newLocation)
        {
            var machine = GetMachine(id);
            if (machine is null)
                return null;

            machine.Location = newLocation;
            return machine;
        }

        public Site? AddSite(string name)
        {
            if (Sites.Any(s => s.Name == name))
                return null;

            var newSite = new Site() { Id = GetNextSiteId(), Name = name };
            Sites.Add(newSite);
            return newSite;
        }

        public User? AddUser(string name, int siteId)
        {
            if (Users.Any(u => u.Name == name))
                return null;

            var site = Sites.FirstOrDefault(s => s.Id == siteId);
            if (site is null)
                return null;

            var newUser = new User() { Id = GetNextUserId(), Name = name, SiteId = site.Id };
            Users.Add(newUser);
            return newUser;
        }

        public Machine? AddMachine(string name, MachineType type, int siteId, Location location)
        {
            if (Machines.Any(m => m.Name == name))
                return null;

            var site = Sites.FirstOrDefault(s => s.Id == siteId);
            if (site is null)
                return null;

            var newMachine = new Machine() { Id = GetNextMachineId(), Name = name, MachineType = type, SiteId = site.Id, Location = location };
            Machines.Add(newMachine);
            return newMachine;
        }

        private void InitializeSampleData()
        {
            var siteSthlm = AddSite("Stockholm");   // 2 users, 5 machines
            var siteGbg = AddSite("Göteborg");      // 1 user, 5 machines
            var siteM = AddSite("Malmö");           // 1 user, no machines

            Debug.Assert(siteSthlm != null);
            Debug.Assert(siteGbg != null);
            Debug.Assert(siteM != null);

            var userWilliam = AddUser("William", siteSthlm.Id);
            var userNoah = AddUser("Noah", siteSthlm.Id);
            var userAlice = AddUser("Alice", siteGbg.Id);
            var userCarl = AddUser("Carl", siteM.Id);

            Debug.Assert(userWilliam != null);
            Debug.Assert(userNoah != null);
            Debug.Assert(userAlice != null);
            Debug.Assert(userCarl != null);

            AddMachine("Björn", MachineType.Excavator, siteSthlm.Id, new Location() { Latitude = 59.3278264, Longitude = 18.052583 });
            AddMachine("Erik", MachineType.Dozer, siteSthlm.Id, new Location() { Latitude = 59.32473932, Longitude = 18.0644845963 });
            AddMachine("Astrid", MachineType.Excavator, siteSthlm.Id, new Location() { Latitude = 59.3280451819, Longitude = 18.0913925171 });
            AddMachine("Ragnar", MachineType.Dozer, siteSthlm.Id, new Location() { Latitude = 59.3200098459, Longitude = 18.0507087708 });
            AddMachine("Ulf", MachineType.Excavator, siteSthlm.Id, new Location() { Latitude = 59.293591519, Longitude = 18.0834960938 });

            AddMachine("Sigrid", MachineType.Dozer, siteGbg.Id, new Location() { Latitude = 57.682862602, Longitude = 11.9503355026 });
            AddMachine("Ivar", MachineType.Excavator, siteGbg.Id, new Location() { Latitude = 57.6984144583, Longitude = 11.971578598 });
            AddMachine("Harald", MachineType.Dozer, siteGbg.Id, new Location() { Latitude = 57.6897447777, Longitude = 11.9974136353 });
            AddMachine("Freya", MachineType.Excavator, siteGbg.Id, new Location() { Latitude = 57.6895153929, Longitude = 11.9493484497 });
            AddMachine("Thorsten", MachineType.Dozer, siteGbg.Id, new Location() { Latitude = 57.7106585671, Longitude = 11.9629096985 });
        }

        private void InitializeInvalidData()
        {
            // Invalid Users
            var userOlivia = new User() { Id = GetNextUserId(), Name = "Olivia", SiteId = 10 }; // Invalid SiteId (10)
            var userAstrid = new User() { Id = GetNextUserId(), Name = "Astrid", SiteId = 11 }; // Invalid SiteId (11)
            Users.Add(userOlivia);
            Users.Add(userAstrid);

            // Invalid Machines
            var machineGunnar = new Machine() { Id = GetNextMachineId(), Name = "Gunnar", MachineType = MachineType.Excavator, SiteId = 10, Location = new() { Latitude = 0, Longitude = 0 } }; // Invalid SiteId(10)
            var machineLeif = new Machine() { Id = GetNextMachineId(), Name = "Leif", MachineType = MachineType.Dozer, SiteId = 10, Location = new() { Latitude = 0, Longitude = 0 } }; // Invalid SiteId(10)
            Machines.Add(machineGunnar);
            Machines.Add(machineLeif);
        }

        private int GetNextSiteId()
        {
            return ++_lastSiteId;
        }

        private int GetNextUserId()
        {
            return ++_lastUserId;
        }

        private int GetNextMachineId()
        {
            return ++_lastMachineId;
        }
    }
}
