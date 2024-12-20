﻿namespace TodoApi.Domain.Entities;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
