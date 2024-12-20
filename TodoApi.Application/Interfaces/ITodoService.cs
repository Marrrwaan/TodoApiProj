using TodoApi.Domain.Entities;

namespace TodoApi.Application.Interfaces;

public interface ITodoService
{
    Task<TodoItem> CreateAsync(TodoItem todo);
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task<IEnumerable<TodoItem>> GetPendingAsync();
    Task<TodoItem?> MarkCompleteAsync(int id);
}
