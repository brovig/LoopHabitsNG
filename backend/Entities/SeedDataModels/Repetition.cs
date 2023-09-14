using System;
using System.Collections.Generic;

namespace Entities.SeedDataModels;

public partial class Repetition
{
    public long Id { get; set; }

    public long Habit { get; set; }

    public long Timestamp { get; set; }

    public long Value { get; set; }

    public virtual Habit HabitNavigation { get; set; } = null!;
}
