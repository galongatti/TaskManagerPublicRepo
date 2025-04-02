namespace TaskManagerBackEnd.Model;

public class Team
{
    public int IdTeam { get; set; }
    public string Name { get; set; }
    public DateTime DateCreation { get; set; }
    public bool Enabled { get; set; }
}