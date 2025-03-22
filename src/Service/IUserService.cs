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
    public bool AddMember(UserInsertDTO user);

    public bool UpdateMember(UserUpdateDto user);
    public bool DeleteUser(int id);
    public User? GetUserById(int id);
    public User? GetUserByEmail(string email);
    
    public bool UpdatePassword(UserUpdatePasswordDto user);
    public bool CheckPassword(UserLoginDto user);
}