using Microsoft.AspNetCore.Mvc;
using TodoApi.Application.Interfaces;
using TodoApi.Domain.Entities;

namespace TodoApiProj.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoItem todo)
    {
        if (string.IsNullOrWhiteSpace(todo.Title))
            return BadRequest("Title is required.");

        var createdTodo = await _todoService.CreateAsync(todo);
        return CreatedAtAction(nameof(GetAll), new { id = createdTodo.Id }, createdTodo);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var todos = await _todoService.GetAllAsync();
        return Ok(todos);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var pendingTodos = await _todoService.GetPendingAsync();
        return Ok(pendingTodos);
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> MarkComplete(int id)
    {
        var todo = await _todoService.MarkCompleteAsync(id);
        if (todo == null)
            return NotFound();

        return Ok(todo);
    }
}
