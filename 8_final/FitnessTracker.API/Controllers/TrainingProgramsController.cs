using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.API.Data;
using FitnessTracker.API.Dtos;
using FitnessTracker.API.Models;

namespace FitnessTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainingProgramsController : ControllerBase
{
    private readonly AppDbContext _context;
    public TrainingProgramsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainingProgram>>> GetAll()
        => await _context.TrainingPrograms.Include(p => p.Exercises).ToListAsync();

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrainingProgram>> GetById(Guid id)
    {
        var program = await _context.TrainingPrograms.Include(p => p.Exercises).FirstOrDefaultAsync(p => p.Id == id);
        return program == null ? NotFound() : program;
    }

    [HttpPost]
    public async Task<ActionResult<TrainingProgram>> Create(TrainingProgramDto dto)
    {
        var program = new TrainingProgram { Id = Guid.NewGuid(), Name = dto.Name, Type = dto.Type, IsActive = dto.IsActive };
        _context.TrainingPrograms.Add(program);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = program.Id }, program);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, TrainingProgramDto dto)
    {
        var program = await _context.TrainingPrograms.FindAsync(id);
        if (program == null) return NotFound();
        program.Name = dto.Name; program.Type = dto.Type; program.IsActive = dto.IsActive;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var program = await _context.TrainingPrograms.FindAsync(id);
        if (program == null) return NotFound();
        if (await _context.Exercises.AnyAsync(e => e.ProgramId == id))
            return BadRequest("Нельзя удалить программу, у которой есть упражнения.");
        _context.TrainingPrograms.Remove(program);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
