using site_machine_tracker.Data.Database;

namespace site_machine_tracker.Data
{
    public class MachineTypeItem
    {
        public required MachineType EnumValue { get; set; }
        public required string Name { get; set; }
    }
}
