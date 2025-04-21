using System.ComponentModel.DataAnnotations;

namespace TaskManagerBackEnd.DTO;

public struct UserInsertDTO
{
    [Required, EmailAddress]
    public string Email { get; set; }
    
    [Required, MinLength(8)]
    public string Password { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Post { get; set; }
    
    [Required, AllowedValues(true, false)]
    public bool Enabled { get; set; }
    
    [Required]
    public int? IdTeam { get; set; }
}

public struct UserUpdateDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Post { get; set; }
    
    [Required, AllowedValues(true, false)]
    public bool Enabled { get; set; }
    
    [Required]
    public int? IdTeam { get; set; }
}

public struct UserUpdatePasswordDto
{
    [Required]
    public int IdUser { get; set; }
    
    [Required, MinLength(8)]
    public string NewPassword { get; set; }
}

public struct UserLoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
    
    [Required, MinLength(8)]
    public string Password { get; set; }
}

public struct UserGetDto
{
    public int IdUser { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Post { get; set; }
    public bool Enabled { get; set; }
    public int? IdTeam { get; set; }
    public TeamGetDto? Team { get; set; }
}