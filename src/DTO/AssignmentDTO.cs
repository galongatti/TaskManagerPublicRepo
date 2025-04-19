using System.ComponentModel.DataAnnotations;
using TaskManagerBackEnd.Enum;

namespace TaskManagerBackEnd.DTO;

public class AssignmentDTOInsert
{
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }

    [Required] public DateTime Deadline { get; set; }

    [Required]
    [AllowedValues(1, int.MaxValue)]
    public int? IdUser { get; set; }

    [Required] [AllowedValues(1, 2, 3, 4)] 
    public StatusAssignmet Status { get; set; }
}

public class AssignmentDTOUpdate
{
    
    [Required]
    [AllowedValues(1, int.MaxValue)]
    public int IdTask { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public DateTime Deadline { get; set; }
    
    [Required]
    [AllowedValues(1, int.MaxValue)]
    public int? IdUser { get; set; }
    
    [Required] [AllowedValues(1, 2, 3, 4)] 
    public StatusAssignmet Status { get; set; }
    
    [Required]
    public DateTime? DateConclusion { get; set; }
}