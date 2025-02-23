﻿using site_machine_tracker.Data.Database;

namespace site_machine_tracker.Data
{
    public class MachineListItem
    {
        public required string Name { get; set; }
        public required MachineType MachineType { get; set; }
        public required string SiteName { get; set; }
        public required Location Location { get; set; }
    }
}
