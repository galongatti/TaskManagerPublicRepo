namespace src.TaskManagerBackEnd.Repository;

public interface IUserRepository
{
    public bool AddMember(User user);
    public bool UpdateMember(User user);
    public bool DeleteMember(string id);
    public User? GetMemberById(int id);
    public User GetMemberByEmail(string email);
}