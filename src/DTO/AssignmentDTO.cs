using TaskManagerBackEnd.Enum;

namespace TaskManagerBackEnd.DTO;

public class AssignmentDTOInsert
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public int? IdUser { get; set; }
    public StatusAssignmet Status { get; set; }
}

public class AssignmentDTOUpdate
{
    public int IdTask {get; set;}
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public int? IdUser { get; set; }
    public StatusAssignmet Status { get; set; }
    public DateTime? DateConclusion { get; set; }
}
