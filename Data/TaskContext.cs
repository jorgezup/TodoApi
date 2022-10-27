using System.Data;

namespace TodoApi.Data;

public static class TaskContext
{
    public delegate Task<IDbConnection> GetConnection();
}