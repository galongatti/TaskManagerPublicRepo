using Dapper;
using Npgsql;
using TaskManagerBackEnd.Connection;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Repository;

public class TeamRepository : ITeamRepository
{
    private readonly ConnectionDb _connection;

    public TeamRepository(ConnectionDb connection)
    {
        _connection = connection;
    }

    public bool CreateTeam(Team team)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();

        int res = connection.Execute(@"
                INSERT INTO tasks.team(name, datecreation, enabled) values (@Name, @DateCreation, @Enabled);
        ", new
        {
            team.Name, team.DateCreation, team.Enabled
        });

        return res > 0;
    }

    public Team? GetTeamByName(string name)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        Team? res = connection.QueryFirstOrDefault<Team>(
            @"SELECT idteam, name, datecreation, enabled 
                    FROM tasks.team WHERE name = @Name",
            new { Name = name });

        return res;
    }

    public Team? GetTeamById(int idTeam)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        Team? res = connection.QueryFirstOrDefault<Team>(
            @"SELECT idteam, name, datecreation, enabled 
                    FROM tasks.team WHERE idteam = @IdTeam",
            new { IdTeam = idTeam });

        return res;
    }

    public bool DeleteTeam(int idTeam)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        int res = connection.Execute(@"DELETE FROM tasks.team WHERE idteam = @IdTeam", new { IdTeam = idTeam });
        return res > 0;
    }

    public bool UpdateTeam(Team team)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();

        int res = connection.Execute(@"
                UPDATE tasks.team
                SET name = @Name, enabled = @Enabled
                WHERE idteam = @IdTeam;
        ", new
        {
            team.Name, team.Enabled, team.IdTeam
        });

        return res > 0;
    }

    public List<Team> GetTeams()
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        return connection.Query<Team>(@"SELECT * FROM tasks.team").ToList();
    }
}