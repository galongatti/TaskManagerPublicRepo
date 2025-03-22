using Microsoft.OpenApi.Extensions;
using TaskManagerBackEnd.Enum;

namespace TaskManagerBackEnd.Model;

public class Assignment
{
    public int IdTask { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime DateCreation { get; set; }
    public DateTime? DateConclusion { get; set; }
    public int? IdUser { get; set; }
    public StatusAssignmet Status { get; set; }

    public string StatusName => Status.GetDisplayName();
}