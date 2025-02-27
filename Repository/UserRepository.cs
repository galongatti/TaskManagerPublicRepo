using Dapper;
using Npgsql;
using TaskManagerBackEnd.Connection;
using TaskManagerBackEnd.DTO;

namespace TaskManagerBackEnd.Repository;

public class UserRepository : IUserRepository
{
    
    private readonly ConnectionDB _connection;

    public UserRepository(ConnectionDB connection)
    {
        _connection = connection;
    }

    public bool AddMember(UserInsertDTO user)
    {
        using NpgsqlConnection connection = _connection.OpenConnection();
        int res = connection.Execute(@"
                                     INSERT INTO tasks.user (email, passoword, post, datecreation, enabled, name)
                                     VALUES (@Email, @Password, @post, @datecreation, @enabled, @name)
                                     ", new { Email = user.Email, Password = user.Password, post = user.Post, DateCreation = DateTime.Today, enabled = user.Enabled, name = user.Name });
        _connection.Dispose();
        return res > 0;
    }

    public bool UpdateMember(UserUpdateDTO user)
    {
        throw new NotImplementedException();
    }

    public bool DeleteMember(string id)
    {
        throw new NotImplementedException();
    }

    public User GetMemberById(int id)
    {
        throw new NotImplementedException();
    }

    public User GetMemberByEmail(string email)
    {
        throw new NotImplementedException();
    }
}