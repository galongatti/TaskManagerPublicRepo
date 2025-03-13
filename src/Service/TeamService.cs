using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Repository;

namespace TaskManagerBackEnd.Service;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _repository;

    public TeamService(ITeamRepository repository)
    {
        _repository = repository;
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

    public bool UpdateTeam(Team team)
    {
        throw new NotImplementedException();
    }

    public bool DeleteTeam(Team team)
    {
        throw new NotImplementedException();
    }

    public Team GetTeamByName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException( nameof(name),"Attribute name is required");
        
        return _repository.GetTeamByName(name);
    }
}