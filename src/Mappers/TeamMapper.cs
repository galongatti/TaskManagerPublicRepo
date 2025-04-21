using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Mappers;

public static class TeamMapper
{

    public static TeamGetDto MapToGetDto(this Team team)
    {
        return new TeamGetDto()
        {
            Name = team.Name,
            IdTeam = team.IdTeam,
            Enabled = team.Enabled
        };
    }
    
}