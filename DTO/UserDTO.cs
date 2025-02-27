namespace TaskManagerBackEnd.DTO;

public class UserInsertDTO
{
    public string IdMember { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Post { get; set; }
    public bool Enabled { get; set; }
}

public class UserUpdateDTO
{
    public string IdMember { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Post { get; set; }
    public bool Enabled { get; set; }
}