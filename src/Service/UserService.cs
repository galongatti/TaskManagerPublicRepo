using src.TaskManagerBackEnd;
using src.TaskManagerBackEnd.Repository;
using TaskManagerBackEnd.DTO;

namespace TaskManagerBackEnd.Service;

public class UserService : IUserService
{
    private const int Iteration = 3;
    private readonly string? _pepper;
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _pepper = configuration["HashPepper"] ?? throw new ArgumentNullException("Pepper not found");
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
            Password = HashPassword.ComputeHash(userDto.Password, salt, _pepper, Iteration),
            IdTeam = userDto.IdTeam
        };


        return _repository.AddMember(user);
    }

    public bool UpdateMember(UserUpdateDto userDto)
    {
        User? userAux = GetMemberById(userDto.IdUser);

        if (userAux is null) return false;

        User user = new()
        {
            IdUser = userDto.IdUser,
            Email = userDto.Email,
            Post = userDto.Post,
            Enabled = userDto.Enabled,
            Name = userDto.Name,
            IdTeam = userDto.IdTeam
        };

        return _repository.UpdateMember(user);
    }

    public bool DeleteMember(int id)
    {
        return _repository.DeleteMember(id);
    }

    public User? GetMemberById(int id)
    {
        return _repository.GetMemberById(id);
    }

    public User GetMemberByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public bool UpdatePassword(UserUpdatePasswordDto userDto)
    {

        _ = GetMemberById(userDto.IdUser) ?? throw new Exception("User not found");
        
        string salt = HashPassword.GenerateSalt();
        string password = HashPassword.ComputeHash(userDto.NewPassword, salt, _pepper, Iteration);

        return _repository.UpdatePassword(userDto.IdUser, password, salt);

    }
}