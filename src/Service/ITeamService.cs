using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Service;

public interface ITeamService
{
    bool AddTeam(TeamInsertDto team);
    bool UpdateTeam(TeamUpdateDto team);
    bool DeleteTeam(int idTeam);
    Team GetTeamByName(string name);
    bool AddTeamSeed(TeamInsertDto teamDto);
    List<Team> GetTeams();
    Team? GetTeamById(int idTeam);
}