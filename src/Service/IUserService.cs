using src.TaskManagerBackEnd;
using TaskManagerBackEnd.DTO;

namespace TaskManagerBackEnd.Service;

public interface IUserService
{
    /// <summary>
    ///     Adds a new member to the repository.
    /// </summary>
    /// <param name="user">The user data transfer object containing the details of the member to be added.</param>
    /// <returns>
    ///     A boolean value indicating whether the member was successfully added.
    /// </returns>
    public bool AddUser(UserInsertDTO user);
    public bool AddUserSeed(UserInsertDTO user);

    public bool UpdateUser(UserUpdateDto user);
    public bool DeleteUser(int id);
    public User? GetUserById(int id, bool includeTeam = false);
    public User? GetUserByEmail(string email);
    
    public bool UpdatePassword(UserUpdatePasswordDto user);
    public bool CheckPassword(UserLoginDto user);
    public List<User> GetUserByTeamId(int idTeam);
    public List<User> GetUsers(bool includeTeam = false);

}