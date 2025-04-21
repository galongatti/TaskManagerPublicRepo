using src.TaskManagerBackEnd;
using TaskManagerBackEnd.DTO;

namespace TaskManagerBackEnd.Mappers;

public static class UserMapper
{
    public static UserGetDto MapToGetDto(this User user)
    {
        return new UserGetDto
        {
            IdTeam = user.IdTeam,
            Name = user.Name,
            Email = user.Email,
            IdUser = user.IdUser,
            Post = user.Post,
            Enabled =  user.Enabled,
            Team =  user.Team?.MapToGetDto()
        };
    }
}