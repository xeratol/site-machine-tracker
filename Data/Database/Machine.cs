namespace site_machine_tracker.Data.Database
{
    public class Machine
    {
        public required int Id { get; init; }
        public required string Name { get; set; }
        public required MachineType MachineType { get; set; }
        public required int SiteId { get; set; }
        public required Location Location { get; set; }
    }
}
