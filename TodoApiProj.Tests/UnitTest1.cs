using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Services;
using TodoApi.Domain.Entities;
using Xunit;

namespace TodoApiProj.Tests;

public class TodoServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddTodo()
    {
        var context = GetInMemoryDbContext();
        var service = new TodoService(context);

        var todo = new TodoItem { Title = "Test Todo", Description = "Test Description" };

        var result = await service.CreateAsync(todo);

        Assert.NotNull(result);
        Assert.Equal("Test Todo", result.Title);
        Assert.False(result.IsCompleted);
        Assert.Single(context.TodoItems);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTodos()
    {
        var context = GetInMemoryDbContext();
        var service = new TodoService(context);

        await service.CreateAsync(new TodoItem { Title = "Todo 1" });
        await service.CreateAsync(new TodoItem { Title = "Todo 2" });

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetPendingAsync_ShouldReturnOnlyPendingTodos()
    {
        var context = GetInMemoryDbContext();
        var service = new TodoService(context);

        await service.CreateAsync(new TodoItem { Title = "Pending Todo" });
        var completedTodo = await service.CreateAsync(new TodoItem { Title = "Completed Todo" });
        await service.MarkCompleteAsync(completedTodo.Id);

        var pendingTodos = await service.GetPendingAsync();

        Assert.Single(pendingTodos);
        Assert.Equal("Pending Todo", pendingTodos.First().Title);
    }

    [Fact]
    public async Task MarkCompleteAsync_ShouldMarkTodoAsCompleted()
    {
        var context = GetInMemoryDbContext();
        var service = new TodoService(context);

        var todo = await service.CreateAsync(new TodoItem { Title = "Todo to Complete" });

        var updatedTodo = await service.MarkCompleteAsync(todo.Id);

        Assert.NotNull(updatedTodo);
        Assert.True(updatedTodo.IsCompleted);
    }

    [Fact]
    public async Task MarkCompleteAsync_WithInvalidId_ShouldReturnNull()
    {
        var context = GetInMemoryDbContext();
        var service = new TodoService(context);

        var result = await service.MarkCompleteAsync(999); // id that doesnt exist

        Assert.Null(result);
    }
}