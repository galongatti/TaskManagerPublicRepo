using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.Extensions;
using src.TaskManagerBackEnd;
using TaskManagerBackEnd.Enum;

namespace TaskManagerBackEnd.DTO;

public class AssignmentInsertDto
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

public class AssignmentUpdateDto
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


public class AssignmentGetDto
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