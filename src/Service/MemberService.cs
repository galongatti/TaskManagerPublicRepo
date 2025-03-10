using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Repository;

namespace TaskManagerBackEnd.Service;

public class MemberService : IMemberService
{
    private const int Iteration = 3;
    private readonly string? _pepper;
    private readonly IUserRepository _repository;

    public MemberService(IUserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _pepper = configuration["Hash:pepper"];
    }

    /// <summary>
    ///     Adds a new member to the repository.
    /// </summary>
    /// <param name="userDto">The user data transfer object containing the details of the member to be added.</param>
    /// <returns>
    ///     A boolean value indicating whether the member was successfully added.
    /// </returns>
    public bool AddMember(UserInsertDTO userDto)
    {
        string salt = HashPassword.GenerateSalt();

        User user = new()
        {
            Email = userDto.Email,
            Post = userDto.Post,
            DateCreation = DateTime.Today,
            Enabled = userDto.Enabled,
            Name = userDto.Name,
            Salt = salt,
            Password = HashPassword.ComputeHash(userDto.Password, salt, _pepper, Iteration)
        };


        return _repository.AddMember(user);
    }

    public bool UpdateMember(UserUpdateDTO userDto)
    {
        User? userAux = GetMemberById(userDto.IdUser);

        if (userAux is null) return false;

        User user = new()
        {
            IdUser = userDto.IdUser,
            Email = userDto.Email,
            Post = userDto.Post,
            Enabled = userDto.Enabled,
            Name = userDto.Name
        };

        return _repository.UpdateMember(user);
    }

    public bool DeleteMember(string id)
    {
        throw new NotImplementedException();
    }

    public User? GetMemberById(int id)
    {
        return _repository.GetMemberById(id);
    }

    public User GetMemberByEmail(string email)
    {
        throw new NotImplementedException();
    }
}