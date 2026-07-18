namespace FitnessTracker.API.Dtos;

public class ActivityDto
{
    public DateTime Date { get; set; }
    public int Minutes { get; set; }
    public string? Notes { get; set; }
    public Guid ExerciseId { get; set; }
}
