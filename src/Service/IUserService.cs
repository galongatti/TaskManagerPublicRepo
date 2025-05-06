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
    public src.TaskManagerBackEnd.UserService? GetUserById(int id);
    public src.TaskManagerBackEnd.UserService? GetUserByEmail(string email);
    
    public bool UpdatePassword(UserUpdatePasswordDto user);
    public bool CheckPassword(UserLoginDto user);
    public List<src.TaskManagerBackEnd.UserService> GetUserByTeamId(int idTeam);
    public List<src.TaskManagerBackEnd.UserService> GetUsers(bool includeTeam = false);

}