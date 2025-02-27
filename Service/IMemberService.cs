using TaskManagerBackEnd.DTO;

namespace TaskManagerBackEnd.Service;

public interface IMemberService
{
    public bool AddMember(UserInsertDTO user);
    public bool UpdateMember(UserUpdateDTO user);
    public bool DeleteMember(string id);
    public User GetMemberById(int id);
    public User GetMemberByEmail(string email);
}