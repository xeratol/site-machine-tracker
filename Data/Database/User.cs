namespace site_machine_tracker.Data.Database
{
    public class User
    {
        public required int Id { get; init; }
        public required string Name { get; set; }
        public required int SiteId { get; set; }
    }
}
