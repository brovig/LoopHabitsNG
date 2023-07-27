using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models;

public class Repetition
{
    [Column("RepetitionId")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Timestamp is a required field.")]
    public DateTime Timestamp { get; set; }

    [Required(ErrorMessage = "Value is a required field.")]
    public double Value { get; set; }

    [ForeignKey(nameof(Habit))]
    public Guid HabitId { get; set; }
    public Habit? Habit { get; set; }
}
