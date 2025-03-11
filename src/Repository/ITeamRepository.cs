using TaskManagerBackEnd;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Repository;

public interface ITeamRepository
{
    bool CreateTeam(Team team);
    Team GetTeamByName(string name);
}