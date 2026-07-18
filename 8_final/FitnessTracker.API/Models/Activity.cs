namespace FitnessTracker.API.Models;

public class Activity
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public int Minutes { get; set; }
    public string? Notes { get; set; }
    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;
}
