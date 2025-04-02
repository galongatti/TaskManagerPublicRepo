using TaskManagerBackEnd;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Repository;

public interface ITeamRepository
{
    bool CreateTeam(Team team);
    Team? GetTeamByName(string name);
    bool UpdateTeam(Team team);
    List<Team> GetTeams();
    Team? GetTeamById(int idTeam);
    bool DeleteTeam(int idTeam);
}