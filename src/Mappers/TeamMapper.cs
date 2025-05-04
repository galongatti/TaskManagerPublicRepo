using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Mappers;

public static class TeamMapper
{

    public static TeamDto MapToGetDto(this Team team)
    {
        return new TeamDto()
        {
            Name = team.Name,
            IdTeam = team.IdTeam,
            Enabled = team.Enabled,
            DateCreation = team.DateCreation
        };
    }
    
}