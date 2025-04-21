using Dapper;
using Npgsql;
using src.TaskManagerBackEnd;
using src.TaskManagerBackEnd.Repository;
using TaskManagerBackEnd.Connection;

namespace TaskManagerBackEnd.Repository;

public class UserRepository(ConnectionDb connection) : IUserRepository
{
    public bool AddMember(User user)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();
        int res = connection1.Execute(@"
                                     INSERT INTO tasks.user (email, password, post, datecreation, enabled, name, salt, idteam)
                                     VALUES (@Email, @Password, @Post, @DateCreation, @Enabled, @Name, @Salt, @IdTeam)
                                     ",
            new
            {
                user.Email, user.Password, user.Post, DateCreation = DateTime.Today, user.Enabled,
                user.Name, user.Salt, user.IdTeam
            });
        connection.Dispose();
        return res > 0;
    }

    public bool UpdateMember(User user)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();
        int res = connection1.Execute(@"
                                     UPDATE tasks.user
                                     SET email = @Email, post = @Post, enabled = @Enabled, name = @Name, idteam = @IdTeam
                                     WHERE iduser = @IdUser
                                     ", new
        {
            user.Email, user.Post, user.Enabled, user.Name, user.IdTeam, user.IdUser
        });
        connection.Dispose();
        return res > 0;
    }

    public bool DeleteUser(int id)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();
        int res = connection1.Execute(@"DELETE FROM tasks.user WHERE iduser = @IdUser", new { IdUser = id });
        connection.Dispose();
        return res > 0;
    }

    public User? GetUserById(int id)
    {
        using NpgsqlConnection connection1 = connection.OpenConnection();
        User? user = connection1.QueryFirstOrDefault<User>(@"
                                     SELECT iduser as IdUser, email as Email, password as Password, name as Name, post as Post, datecreation as DateCreation, enabled as Enabled, salt as Salt, idteam as IdTeam
                                     FROM tasks.user
                                     WHERE iduser = @IdUser
                                     ", new { IdUser = id });
        return user;
    }

    public User GetUserByEmail(string email)
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
        return connection1.Query<User>(@"SELECT * FROM tasks.user").ToList();
    }
}