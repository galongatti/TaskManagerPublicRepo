using Dapper;
using Npgsql;
using src.TaskManagerBackEnd.Connection;

namespace src.TaskManagerBackEnd.Repository;

public class UserRepository : IUserRepository
{
    private readonly ConnectionDB _connection;

    public UserRepository(ConnectionDB connection)
    {
        _connection = connection;
    }

    public bool AddMember(User user)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        int res = connection.Execute(@"
                                     INSERT INTO tasks.user (email, password, post, datecreation, enabled, name, salt)
                                     VALUES (@Email, @Password, @post, @datecreation, @enabled, @name, @salt)
                                     ",
            new
            {
                user.Email, user.Password, post = user.Post, DateCreation = DateTime.Today, enabled = user.Enabled,
                name = user.Name, salt = user.Salt
            });
        _connection.Dispose();
        return res > 0;
    }

    public bool UpdateMember(User user)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        int res = connection.Execute(@"
                                     UPDATE tasks.user
                                     SET email = @Email, post = @post, enabled = @enabled, name = @name
                                     WHERE iduser = @IdUser
                                     ", new
        {
            user.Email, post = user.Post, enabled = user.Enabled, name = user.Name,
            user.IdUser
        });
        _connection.Dispose();
        return res > 0;
    }

    public bool DeleteMember(string id)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        int res = connection.Execute(@"
                                     UPDATE tasks.user
                                     SET enabled = false
                                     WHERE iduser = @IdUser
                                     ", new { IdUser = id });
        _connection.Dispose();
        return res > 0;
    }

    public User? GetMemberById(int id)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        User? user = connection.QueryFirstOrDefault<User>(@"
                                     SELECT iduser as IdUser, email as Email, password as Password, name as Name, post as Post, datecreation as DateCreation, enabled as Enabled, salt as Salt
                                     FROM tasks.user
                                     WHERE iduser = @IdUser
                                     ", new { IdUser = id });
        return user;
    }

    public User GetMemberByEmail(string email)
    {
        throw new NotImplementedException();
    }
}