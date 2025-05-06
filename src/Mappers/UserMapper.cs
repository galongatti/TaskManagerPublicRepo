using src.TaskManagerBackEnd;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Mappers;

public static class UserMapper
{
    public static UserGetDto MapToGetDto(this UserService userService)
    {
        return new UserGetDto
        {
            Name = userService.Name,
            Email = userService.Email,
            IdUser = userService.IdUser,
            Post = userService.Post,
            Enabled = userService.Enabled,
            Team = userService.Team is not null ? userService.Team.MapToGetDto() : null
        };
    }

    public static UserService MapToModel(this UserInsertDTO userDto)
    {
        return new UserService
        {
            Email = userDto.Email,
            Post = userDto.Post,
            DateCreation = DateTime.Today,
            Enabled = userDto.Enabled,
            Name = userDto.Name,
            Team = userDto.IdTeam is not null ? new Team() { IdTeam = userDto.IdTeam.Value } : null
        };
    }
    
    public static UserService MapToModel(this UserUpdateDto userDto)
    {
        return new UserService
        {
            Email = userDto.Email,
            Post = userDto.Post,
            DateCreation = DateTime.Today,
            Enabled = userDto.Enabled,
            Name = userDto.Name,
            Team = userDto.IdTeam is not null ? new Team() { IdTeam = userDto.IdTeam.Value } : null
        };
    }
}