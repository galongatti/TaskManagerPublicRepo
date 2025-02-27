namespace TaskManagerBackEnd;

public class User
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Post { get; set; }
    public DateTime DateCreation { get; set; }
    public bool Enabled { get; set; }
}
