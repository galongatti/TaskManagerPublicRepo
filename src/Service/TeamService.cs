using src.TaskManagerBackEnd;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Repository;

namespace TaskManagerBackEnd.Service;

public class TeamService(ITeamRepository repository, IServiceProvider serviceProvider) : ITeamService
{
    public bool AddTeam(TeamInsertDto teamDto)
    {
        Team? existingTeam = repository.GetTeamByName(teamDto.Name);
        
        if(existingTeam is not null)
            throw new ArgumentException("Team already exists");

        Team team = new()
        {
            Name = teamDto.Name,
            DateCreation = DateTime.Now,
            Enabled = teamDto.Enabled,
        };

        return repository.CreateTeam(team);
    }

    public bool UpdateTeam(TeamUpdateDto teamDto)
    {
        Team? existingTeam = repository.GetTeamById(teamDto.IdTeam);
        
        if(existingTeam is null)
            throw new ArgumentException("Team does not exists");
        
        Team team = new()
        {
            Name = teamDto.Name,
            Enabled = teamDto.Enabled,
            IdTeam = teamDto.IdTeam
        };
        return repository.UpdateTeam(team);
    }

    public bool DeleteTeam(int idTeam)
    {
        IUserService? userService = serviceProvider.GetRequiredService<IUserService>();
        
        if(userService is null)
            throw new ArgumentNullException(nameof(UserService), "UserService not found");
        
        List<src.TaskManagerBackEnd.UserService> users =  userService.GetUserByTeamId(idTeam);
        
        if(users.Any())
            throw new Exception("Team has users. You must change the team of the users before deleting the team");
        
        return repository.DeleteTeam(idTeam);
    }

    public Team GetTeamByName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException( nameof(name),"Attribute name is required");
        
        return repository.GetTeamByName(name);
    }
    
    public bool AddTeamSeed(TeamInsertDto teamDto)
    {
        Team? existingTeam = repository.GetTeamByName(teamDto.Name);

        if (existingTeam is not null)
            return false;

        Team team = new()
        {
            Name = teamDto.Name,
            DateCreation = DateTime.Now,
            Enabled = teamDto.Enabled,
        };

        return repository.CreateTeam(team);
    }

    public List<Team> GetTeams()
    {
        return repository.GetTeams();
    }

    public Team? GetTeamById(int idTeam)
    {
        return repository.GetTeamById(idTeam);
    }
}