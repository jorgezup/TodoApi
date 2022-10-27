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
    }
}