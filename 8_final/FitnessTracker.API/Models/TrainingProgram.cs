namespace FitnessTracker.API.Models;

public class TrainingProgram
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
