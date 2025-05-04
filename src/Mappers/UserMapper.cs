using src.TaskManagerBackEnd;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Mappers;

public static class UserMapper
{
    public static UserGetDto MapToGetDto(this User user)
    {
        return new UserGetDto
        {
            Name = user.Name,
            Email = user.Email,
            IdUser = user.IdUser,
            Post = user.Post,
            Enabled = user.Enabled,
            Team = user.Team is not null ? user.Team.MapToGetDto() : null
        };
    }

    public static User MapToModel(this UserInsertDTO userDto)
    {
        return new User
        {
            Email = userDto.Email,
            Post = userDto.Post,
            DateCreation = DateTime.Today,
            Enabled = userDto.Enabled,
            Name = userDto.Name,
            Team = userDto.IdTeam is not null ? new Team() { IdTeam = userDto.IdTeam.Value } : null
        };
    }
    
    public static User MapToModel(this UserUpdateDto userDto)
    {
        return new User
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