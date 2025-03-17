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
    public bool DeleteMember(int id);
    public User GetMemberById(int id);
    public User? GetMemberByEmail(string email);
    
    public bool UpdatePassword(UserUpdatePasswordDto user);
}