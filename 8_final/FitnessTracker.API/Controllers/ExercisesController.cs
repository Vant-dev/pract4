using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.API.Data;
using FitnessTracker.API.Dtos;
using FitnessTracker.API.Models;

namespace FitnessTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly AppDbContext _context;
    public ExercisesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Exercise>>> GetAll()
        => await _context.Exercises.Include(e => e.Program).ToListAsync();

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Exercise>> GetById(Guid id)
    {
        var exercise = await _context.Exercises.Include(e => e.Program).FirstOrDefaultAsync(e => e.Id == id);
        return exercise == null ? NotFound() : exercise;
    }

    [HttpPost]
    public async Task<ActionResult<Exercise>> Create(ExerciseDto dto)
    {
        if (!await _context.TrainingPrograms.AnyAsync(p => p.Id == dto.ProgramId))
            return BadRequest("Программа не найдена.");
        var exercise = new Exercise { Id = Guid.NewGuid(), Name = dto.Name, ProgramId = dto.ProgramId, IsActive = dto.IsActive };
        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = exercise.Id }, exercise);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, ExerciseDto dto)
    {
        var exercise = await _context.Exercises.FindAsync(id);
        if (exercise == null) return NotFound();
        exercise.Name = dto.Name; exercise.ProgramId = dto.ProgramId; exercise.IsActive = dto.IsActive;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var exercise = await _context.Exercises.FindAsync(id);
        if (exercise == null) return NotFound();
        if (await _context.Activities.AnyAsync(a => a.ExerciseId == id))
            return BadRequest("Нельзя удалить упражнение, у которого есть активности.");
        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
