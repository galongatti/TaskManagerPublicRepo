using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Service;

public interface ITeamService
{
    public  bool AddTeam(TeamInsertDto team);
    public  bool UpdateTeam(Team team);
    public  bool DeleteTeam(Team team);
    public Team GetTeamByName(string name);
}