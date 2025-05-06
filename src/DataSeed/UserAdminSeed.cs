using src.TaskManagerBackEnd;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Service;
using UserService = src.TaskManagerBackEnd.UserService;

namespace TaskManagerBackEnd.DataSeed;

public class UserAdminSeed(IServiceScope serviceScope)
{
    private const string AdminTeam = "Admin Team";

    public void SeedUserAdmin()
    {
        IUserService userService = serviceScope.ServiceProvider.GetRequiredService<IUserService>();
        ITeamService teamService = serviceScope.ServiceProvider.GetRequiredService<ITeamService>();

        Team team = teamService.GetTeamByName(AdminTeam);
        
        
        if(team == null)
            throw new Exception(AdminTeam);

        UserInsertDTO userDto = new()
        {
            Email = "admin@example.com",
            Password = "admin123456",
            Name = "Admin User",
            Post = "Admin",
            Enabled = true,
            IdTeam = team.IdTeam
        };

        UserService? user = userService.GetUserByEmail(userDto.Email);
        
        if(user == null)
            userService.AddUserSeed(userDto);
    }

    public void SeedTeamAdmin()
    {
        ITeamService teamService = serviceScope.ServiceProvider.GetRequiredService<ITeamService>();

        TeamInsertDto teamDto = new()
        {
            Name = AdminTeam,
            Enabled = true
        };
        
        teamService.AddTeamSeed(teamDto);
    }




}