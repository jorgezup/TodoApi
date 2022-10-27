using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Data;

[Table("task_db")]
public record TaskRecord(int Id, string Task, string Status);