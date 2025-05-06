namespace src.TaskManagerBackEnd.Repository;

public interface IUserRepository
{
    public bool AddMember(UserService userService);
    public bool UpdateMember(UserService userService);
    public bool DeleteUser(int id);
    public UserService? GetUserById(int id);
    public UserService? GetUserByEmail(string email);
    public bool UpdatePassword(int idUser, string newPassword, string salt);
    public List<UserService> GetUserByTeamId(int idTeam);
    List<UserService> GetUsers();
}