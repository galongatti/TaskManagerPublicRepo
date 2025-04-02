using src.TaskManagerBackEnd;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Repository;

namespace TaskManagerBackEnd.Service;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _repository;
    private readonly IUserService _userService;

    public TeamService(ITeamRepository repository, IUserService userService)
    {
        _repository = repository;
        _userService = userService;
    }

    public bool AddTeam(TeamInsertDto teamDto)
    {
        Team existingTeam = _repository.GetTeamByName(teamDto.Name);
        
        if(existingTeam is not null)
            throw new ArgumentException("Team already exists");

        Team team = new()
        {
            Name = teamDto.Name,
            DateCreation = DateTime.Now,
            Enabled = teamDto.Enabled,
        };

        return _repository.CreateTeam(team);
    }

    public bool UpdateTeam(TeamUpdateDto teamDto)
    {
        Team? existingTeam = _repository.GetTeamById(teamDto.IdTeam);
        
        if(existingTeam is null)
            throw new ArgumentException("Team does not exists");
        
        Team team = new()
        {
            Name = teamDto.Name,
            Enabled = teamDto.Enabled,
            IdTeam = teamDto.IdTeam
        };
        return _repository.UpdateTeam(team);
    }

    public bool DeleteTeam(int idTeam)
    {
        List<User> users =  _userService.GetUserByTeamId(idTeam);
        
        if(users.Any())
            throw new Exception("Team has users. You must change the team of the users before deleting the team");
        
        return _repository.DeleteTeam(idTeam);
    }

    public Team GetTeamByName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException( nameof(name),"Attribute name is required");
        
        return _repository.GetTeamByName(name);
    }
    
    public bool AddTeamSeed(TeamInsertDto teamDto)
    {
        Team? existingTeam = _repository.GetTeamByName(teamDto.Name);

        if (existingTeam is not null)
            return false;

        Team team = new()
        {
            Name = teamDto.Name,
            DateCreation = DateTime.Now,
            Enabled = teamDto.Enabled,
        };

        return _repository.CreateTeam(team);
    }

    public List<Team> GetTeams()
    {
        return _repository.GetTeams();
    }

    public Team? GetTeamById(int idTeam)
    {
        return _repository.GetTeamById(idTeam);
    }
}