using System.ComponentModel.DataAnnotations;

namespace TaskManagerBackEnd.DTO;

public class TeamInsertDto
{
    [Required]
    public string Name { get; set; }
    
    [Required, AllowedValues(true, false)]
    public bool Enabled { get; set; }

}

public class TeamUpdateDto
{
    [Required]
    [AllowedValues(1, int.MaxValue)]
    public int IdTeam { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required, AllowedValues(true, false)]
    public bool Enabled { get; set; }
}

public struct TeamDto
{
    public int IdTeam { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
    
    public DateTime DateCreation { get; set; }
}