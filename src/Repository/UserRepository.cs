using Dapper;
using Npgsql;
using src.TaskManagerBackEnd;
using src.TaskManagerBackEnd.Repository;
using TaskManagerBackEnd.Connection;

namespace TaskManagerBackEnd.Repository;

public class UserRepository : IUserRepository
{
    private readonly ConnectionDb _connection;

    public UserRepository(ConnectionDb connection)
    {
        _connection = connection;
    }

    public bool AddMember(User user)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        int res = connection.Execute(@"
                                     INSERT INTO tasks.user (email, password, post, datecreation, enabled, name, salt, idteam)
                                     VALUES (@Email, @Password, @Post, @DateCreation, @Enabled, @Name, @Salt, @IdTeam)
                                     ",
            new
            {
                user.Email, user.Password, user.Post, DateCreation = DateTime.Today, user.Enabled,
                user.Name, user.Salt, user.IdTeam
            });
        _connection.Dispose();
        return res > 0;
    }

    public bool UpdateMember(User user)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        int res = connection.Execute(@"
                                     UPDATE tasks.user
                                     SET email = @Email, post = @Post, enabled = @Enabled, name = @Name, idteam = @IdTeam
                                     WHERE iduser = @IdUser
                                     ", new
        {
            user.Email, user.Post, user.Enabled, user.Name, user.IdTeam, user.IdUser
        });
        _connection.Dispose();
        return res > 0;
    }

    public bool DeleteUser(int id)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        int res = connection.Execute(@"DELETE FROM tasks.user WHERE iduser = @IdUser", new { IdUser = id });
        _connection.Dispose();
        return res > 0;
    }

    public User? GetUserById(int id)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        User? user = connection.QueryFirstOrDefault<User>(@"
                                     SELECT iduser as IdUser, email as Email, password as Password, name as Name, post as Post, datecreation as DateCreation, enabled as Enabled, salt as Salt, idteam as IdTeam
                                     FROM tasks.user
                                     WHERE iduser = @IdUser
                                     ", new { IdUser = id });
        return user;
    }

    public User GetUserByEmail(string email)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        User? user = connection.QueryFirstOrDefault<User>(@"
                                     SELECT iduser as IdUser, email as Email, password as Password, name as Name, post as Post, datecreation as DateCreation, enabled as Enabled, salt as Salt, idteam as IdTeam
                                     FROM tasks.user
                                     WHERE email = @Email
                                     ", new { Email = email });
        return user;
    }

    public bool UpdatePassword(int idUser, string newPassword, string salt)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        int res = connection.Execute("UPDATE tasks.user SET password = @Password, salt = @Salt WHERE iduser = @IdUser",
            new { Password = newPassword, IdUser = idUser, Salt = salt });
        return res > 0;
    }
}