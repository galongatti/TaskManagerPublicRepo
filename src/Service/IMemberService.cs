using src.TaskManagerBackEnd.DTO;

namespace src.TaskManagerBackEnd.Service;

public interface IMemberService
{
    /// <summary>
    ///     Adds a new member to the repository.
    /// </summary>
    /// <param name="user">The user data transfer object containing the details of the member to be added.</param>
    /// <returns>
    ///     A boolean value indicating whether the member was successfully added.
    /// </returns>
    public bool AddMember(UserInsertDTO user);

    public bool UpdateMember(UserUpdateDTO user);
    public bool DeleteMember(string id);
    public User GetMemberById(int id);
    public User GetMemberByEmail(string email);
}