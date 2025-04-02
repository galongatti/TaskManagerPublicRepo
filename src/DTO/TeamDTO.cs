namespace TaskManagerBackEnd.DTO;

public class TeamInsertDto
{
    public string Name { get; set; }
    public bool Enabled { get; set; }

}

public class TeamUpdateDto
{
    public int IdTeam { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
}