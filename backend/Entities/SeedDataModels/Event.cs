using System;
using System.Collections.Generic;

namespace Entities.SeedDataModels;

public partial class Event
{
    public long Id { get; set; }

    public long? Timestamp { get; set; }

    public string? Message { get; set; }

    public long? ServerId { get; set; }
}
