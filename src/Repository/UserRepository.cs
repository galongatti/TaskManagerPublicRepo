using Dapper;
using Npgsql;
using src.TaskManagerBackEnd;
using src.TaskManagerBackEnd.Repository;
using TaskManagerBackEnd.Connection;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Repository;

public class UserRepository(ConnectionDb connection) : IUserRepository
{
    public bool AddMember(User user)
    {
        int? idTeam = user.Team?.IdTeam;

        using NpgsqlConnection connection1 = connection.OpenConnection();
        int res = connection1.Execute(@"
                                     INSERT INTO tasks.user (email, password, post, datecreation, enabled, name, salt, idteam)
                                     VALUES (@Email, @Password, @Post, @DateCreation, @Enabled, @Name, @Salt, @IdTeam)
                                     ",
            new
            {
                user.Email, user.Password, user.Post, DateCreation = DateTime.Today, user.Enabled,
                user.Name, user.Salt, idTeam
            });
        connection.Dispose();
        return res > 0;
    }

    public bool UpdateMember(User user)
    {
        int? idTeam = user.Team?.IdTeam;

        using NpgsqlConnection connection1 = connection.OpenConnection();
        int res = connection1.Execute(@"
                                     UPDATE tasks.user
                                     SET email = @Email, post = @Post, enabled = @Enabled, name = @Name, idteam = @IdTeam
                                     WHERE iduser = @IdUser
                                     ", new
        {
            user.Email, user.Post, user.Enabled, user.Name, idTeam, user.IdUser
        });
        connection.Dispose();
        return res > 0;
    }

    public bool DeleteUser(int id)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();
        int res = connection1.Execute(@"DELETE FROM tasks.user WHERE iduser = @IdUser", new { IdUser = id });
        return res > 0;
    }

    public User? GetUserById(int id)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();

        string sql = @"
            SELECT 
                u.IdUser, u.Email, u.Password, u.Name, u.Post, u.DateCreation, u.Enabled, u.Salt, u.IdTeam,
                t.IdTeam, t.Name, t.Enabled, t.DateCreation
            FROM tasks.user u
            LEFT JOIN tasks.team t ON u.IdTeam = t.IdTeam
            WHERE u.IdUser = @IdUser";
        User? user = connection1.Query<User, Team?, User>(sql, (u, t) =>
        {
            u.Team = t is not null ?  new Team()
            {
                Enabled = t.Enabled,
                DateCreation = t.DateCreation,
                Name = t.Name,
                IdTeam = t.IdTeam
            } : null;
            return u;
        }, new { IdUser = id }, splitOn: "IdTeam").FirstOrDefault();
        return user;
    }

    public User? GetUserByEmail(string email)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();
        User? user = connection1.QueryFirstOrDefault<User>(@"
                                     SELECT iduser as IdUser, email as Email, password as Password, name as Name, post as Post, datecreation as DateCreation, enabled as Enabled, salt as Salt, idteam as IdTeam
                                     FROM tasks.user
                                     WHERE email = @Email
                                     ", new { Email = email });
        return user;
    }

    public bool UpdatePassword(int idUser, string newPassword, string salt)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();
        int res = connection1.Execute("UPDATE tasks.user SET password = @Password, salt = @Salt WHERE iduser = @IdUser",
            new { Password = newPassword, IdUser = idUser, Salt = salt });
        return res > 0;
    }

    public List<User> GetUserByTeamId(int idTeam)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();
        return connection1.Query<User>(@"
                                     SELECT * FROM tasks.user
                                     WHERE idteam = @IdTeam
                                     ", new { IdTeam = idTeam }).ToList();
    }

    public List<User> GetUsers()
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();

        string sql = @"
            SELECT 
                u.IdUser, u.Email, u.Password, u.Name, u.Post, u.DateCreation, u.Enabled, u.Salt, u.IdTeam,
                t.IdTeam, t.Name, t.Enabled, t.DateCreation
            FROM tasks.user u
            LEFT JOIN tasks.team t ON u.IdTeam = t.IdTeam
            WHERE u.IdUser = @IdUser";
        List<User>? users = connection1.Query<User, Team?, User>(sql, (u, t) =>
        {
            u.Team = t is not null ?  new Team()
            {
                Enabled = t.Enabled,
                DateCreation = t.DateCreation,
                Name = t.Name,
                IdTeam = t.IdTeam
            } : null;
            return u;
        }, splitOn: "IdTeam").ToList();
        return users;
    }
}