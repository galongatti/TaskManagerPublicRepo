using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Service;

public interface ITeamService
{
    bool AddTeam(TeamInsertDto team);
    bool UpdateTeam(TeamUpdateDto team);
    bool DeleteTeam(Team team);
    Team GetTeamByName(string name);
}