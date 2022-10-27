using Dapper.Contrib.Extensions;
using TodoApi.Data;
using static TodoApi.Data.TaskContext;
namespace TodoApi.Endpoints;

public static class TasksEndpoints
{
    public static void MapTasksEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => $"Welcome to the {nameof(TodoApi)} API!");

        app.MapGet("/tasks", async (GetConnection connectionGetter) =>
        {
            using var connection = await connectionGetter();
            var tasks = connection.GetAll<TaskRecord>().ToList();
            return tasks.Count == 0 ? Results.NotFound() : Results.Ok(tasks);
        });
        
        app.MapGet("/tasks/{id:int}", async (GetConnection connectionGetter, int id) =>
        {
            using var connection = await connectionGetter();
            var task = connection.Get<TaskRecord>(id);
            return task == null ? Results.NotFound() : Results.Ok(task);
        });
        
        app.MapPost("/tasks", async (GetConnection connectionGetter, TaskRecord task) =>
        {
            using var connection = await connectionGetter();
            var id = (int)connection.Insert(task);
            return Results.Created($"/tasks/{id}", task);
        });
        
        app.MapPut("/tasks/{id:int}", async (GetConnection connectionGetter, int id, TaskRecord task) =>
        {
            using var connection = await connectionGetter();
            var existingTask = connection.Get<TaskRecord>(id);
            
            if (existingTask == null) return Results.NotFound("Task not found");
            
            var taskToUpdate = task with { Id = id };
   
            await connection.UpdateAsync(taskToUpdate);
            return Results.Ok(taskToUpdate);
        });
        
        app.MapDelete("/tasks/{id:int}", async (GetConnection connectionGetter, int id) =>
        {
            using var connection = await connectionGetter();
            var task = connection.Get<TaskRecord>(id);
            
            if (task == null) return Results.NotFound("Task not found");
            
            await connection.DeleteAsync(task);
            return Results.NoContent();
        });
    }
}