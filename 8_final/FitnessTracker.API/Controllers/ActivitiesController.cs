using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.API.Data;
using FitnessTracker.API.Dtos;
using FitnessTracker.API.Models;

namespace FitnessTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly AppDbContext _context;
    public ActivitiesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Activity>>> GetAll(
        [FromQuery] DateTime? date, [FromQuery] int? year, [FromQuery] int? month)
    {
        IQueryable<Activity> query = _context.Activities.Include(a => a.Exercise).ThenInclude(e => e.Program);
        if (date.HasValue)
            query = query.Where(a => a.Date == DateOnly.FromDateTime(date.Value));
        else if (year.HasValue && month.HasValue)
            query = query.Where(a => a.Date.Year == year && a.Date.Month == month);
        return await query.ToListAsync();
    }

    [HttpGet("summary")]
    public async Task<ActionResult> GetDaySummary([FromQuery] DateTime date)
    {
        var day = DateOnly.FromDateTime(date);
        int total = await _context.Activities.Where(a => a.Date == day).SumAsync(a => (int?)a.Minutes) ?? 0;
        string sticker = total < 30 ? "yellow" : total <= 90 ? "green" : "red";
        return Ok(new { date = day, totalMinutes = total, sticker });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Activity>> GetById(Guid id)
    {
        var activity = await _context.Activities.Include(a => a.Exercise).FirstOrDefaultAsync(a => a.Id == id);
        return activity == null ? NotFound() : activity;
    }

    [HttpPost]
    public async Task<ActionResult<Activity>> Create(ActivityDto dto)
    {
        var exercise = await _context.Exercises.FindAsync(dto.ExerciseId);
        if (exercise == null || !exercise.IsActive)
            return BadRequest("Упражнение не найдено или неактивно.");

        var day = DateOnly.FromDateTime(dto.Date.Date);
        int currentSum = await _context.Activities.Where(a => a.Date == day).SumAsync(a => (int?)a.Minutes) ?? 0;
        if (currentSum + dto.Minutes > 1440)
            return BadRequest("Суммарное время за день не может превышать 1440 минут.");

        var activity = new Activity
        {
            Id = Guid.NewGuid(), Date = day, Minutes = dto.Minutes,
            Notes = dto.Notes, ExerciseId = dto.ExerciseId
        };
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = activity.Id }, activity);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, ActivityDto dto)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity == null) return NotFound();

        if (activity.ExerciseId != dto.ExerciseId)
        {
            var currentExercise = await _context.Exercises.FindAsync(activity.ExerciseId);
            if (currentExercise == null || !currentExercise.IsActive)
                return BadRequest("Нельзя изменить упражнение, так как текущее упражнение неактивно.");

            var newExercise = await _context.Exercises.FindAsync(dto.ExerciseId);
            if (newExercise == null || !newExercise.IsActive)
                return BadRequest("Новое упражнение не найдено или неактивно.");
        }

        var newDate = DateOnly.FromDateTime(dto.Date.Date);
        int currentSumWithoutThis = await _context.Activities
            .Where(a => a.Date == newDate && a.Id != id).SumAsync(a => (int?)a.Minutes) ?? 0;
        if (currentSumWithoutThis + dto.Minutes > 1440)
            return BadRequest("Суммарное время за день не может превышать 1440 минут.");

        activity.Date = newDate;
        activity.Minutes = dto.Minutes;
        activity.Notes = dto.Notes;
        activity.ExerciseId = dto.ExerciseId;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity == null) return NotFound();
        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
