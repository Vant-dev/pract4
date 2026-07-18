namespace FitnessTracker.API.Models;

public class Exercise
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public Guid ProgramId { get; set; }
    public TrainingProgram Program { get; set; } = null!;
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
