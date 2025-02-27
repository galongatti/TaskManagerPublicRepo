using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Repository;

namespace TaskManagerBackEnd.Service;

public class MemberService : IMemberService
{
    private readonly IUserRepository _repository;

    public MemberService(IUserRepository repository)
    {
        _repository = repository;
    }
    
    
    public bool AddMember(UserInsertDTO user)
    {
        return _repository.AddMember(user);
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