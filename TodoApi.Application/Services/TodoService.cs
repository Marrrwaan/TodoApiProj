using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Interfaces;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Services;

public class TodoService : ITodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem> CreateAsync(TodoItem todo)
    {
        todo.CreatedDate = DateTime.UtcNow;
        _context.TodoItems.Add(todo);
        await _context.SaveChangesAsync();
        return todo;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync() =>
        await _context.TodoItems.ToListAsync();

    public async Task<IEnumerable<TodoItem>> GetPendingAsync() =>
        await _context.TodoItems.Where(t => !t.IsCompleted).ToListAsync();

    public async Task<TodoItem?> MarkCompleteAsync(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo == null) return null;

        todo.IsCompleted = true;
        await _context.SaveChangesAsync();
        return todo;
    }
}