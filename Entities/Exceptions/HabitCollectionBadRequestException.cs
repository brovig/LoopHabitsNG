﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;
public sealed class HabitCollectionBadRequestException : BadRequestException
{
    public HabitCollectionBadRequestException() : base("Habit collection sent from a client is null.")
    {
    }
}
