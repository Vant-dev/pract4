namespace FitnessTracker.API.Dtos;

public class ExerciseDto
{
    public string Name { get; set; } = string.Empty;
    public Guid ProgramId { get; set; }
    public bool IsActive { get; set; }
}
