namespace src.TaskManagerBackEnd.DTO;

public class UserInsertDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Post { get; set; }
    public bool Enabled { get; set; }
}

public class UserUpdateDTO
{
    public int IdUser { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Post { get; set; }
    public bool Enabled { get; set; }
}