namespace src.TaskManagerBackEnd.Repository;

public interface IUserRepository
{
    public bool AddMember(User user);
    public bool UpdateMember(User user);
    public bool DeleteUser(int id);
    public User? GetUserById(int id);
    public User GetUserByEmail(string email);
    public bool UpdatePassword(int idUser, string newPassword, string salt);
    public List<User> GetUserByTeamId(int idTeam);
    List<User> GetUsers();
}