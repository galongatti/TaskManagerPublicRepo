using Dapper;
using Npgsql;
using TaskManagerBackEnd.Connection;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Repository;

public class TeamRepository(ConnectionDb connection) : ITeamRepository
{
    public bool CreateTeam(Team team)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();

        int res = connection1.Execute(@"
                INSERT INTO tasks.team(name, datecreation, enabled) values (@Name, @DateCreation, @Enabled);
        ", new
        {
            team.Name, team.DateCreation, team.Enabled
        });

        return res > 0;
    }

    public Team? GetTeamByName(string name)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();
        Team? res = connection1.QueryFirstOrDefault<Team>(
            @"SELECT idteam, name, datecreation, enabled 
                    FROM tasks.team WHERE name = @Name",
            new { Name = name });

        return res;
    }

    public Team? GetTeamById(int idTeam)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();
        Team? res = connection1.QueryFirstOrDefault<Team>(
            @"SELECT idteam, name, datecreation, enabled 
                    FROM tasks.team WHERE idteam = @IdTeam",
            new { IdTeam = idTeam });

        return res;
    }

    public bool DeleteTeam(int idTeam)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();
        int res = connection1.Execute(@"DELETE FROM tasks.team WHERE idteam = @IdTeam", new { IdTeam = idTeam });
        return res > 0;
    }

    public bool UpdateTeam(Team team)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();

        int res = connection1.Execute(@"
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
        using NpgsqlConnection connection1 = connection.OpenConnection();
        return connection1.Query<Team>(@"SELECT * FROM tasks.team").ToList();
    }
}