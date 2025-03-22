namespace TaskManagerBackEnd.DTO;

public struct UserInsertDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Post { get; set; }
    public bool Enabled { get; set; }
    public int IdTeam { get; set; }
}

public struct UserUpdateDto
{
    public int IdUser { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Post { get; set; }
    public bool Enabled { get; set; }
    
    public int IdTeam { get; set; }
}

public struct UserUpdatePasswordDto
{
    public int IdUser { get; set; }
    public string NewPassword { get; set; }
}

public struct UserLoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}