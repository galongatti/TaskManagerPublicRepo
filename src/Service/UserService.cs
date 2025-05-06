using src.TaskManagerBackEnd;
using src.TaskManagerBackEnd.Repository;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Mappers;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Service;

public class UserService(
    IUserRepository repository,
    IConfiguration configuration,
    IAssignmentService assignmentService,
    IServiceProvider serviceProvider) : IUserService
{
    private const int Iteration = 3;

    private readonly string? _pepper = configuration["HashPepper"] ??
                                       throw new ArgumentNullException(nameof(configuration), "Pepper not found");

    /// <summary>
    ///     Adds a new member to the repository.
    /// </summary>
    /// <param name="userDto">The user data transfer object containing the details of the member to be added.</param>
    /// <returns>
    ///     A boolean value indicating whether the member was successfully added.
    /// </returns>
    public bool AddUser(UserInsertDTO userDto)
    {
        src.TaskManagerBackEnd.UserService userServiceExist = GetUserByEmail(userDto.Email);

        if (userServiceExist is not null)
            throw new Exception("User already exists");

        if (string.IsNullOrWhiteSpace(userDto.Password))
            throw new ArgumentException("Password is required");

        string salt = HashPassword.GenerateSalt();

        string password = HashPassword.ComputeHash(userDto.Password, salt, _pepper!, Iteration);

        src.TaskManagerBackEnd.UserService userService = userDto.MapToModel();
        userService.Salt = salt;
        userService.Password = password;

        return repository.AddMember(userService);
    }

    public bool UpdateUser(UserUpdateDto userDto)
    {
        src.TaskManagerBackEnd.UserService? userAux = GetUserById(userDto.IdUser);

        if (userAux is null) throw new Exception("User does not exists");

        src.TaskManagerBackEnd.UserService userService = userDto.MapToModel();

        return repository.UpdateMember(userService);
    }

    public bool DeleteUser(int id)
    {
        int[] idUser = { id };
        List<Assignment> assignments = assignmentService.GetAssignmentsByUserId(idUser);
        if (assignments.Any()) throw new Exception("The user has assignments found.");

        return repository.DeleteUser(id);
    }

    public src.TaskManagerBackEnd.UserService? GetUserById(int id)
    {
        src.TaskManagerBackEnd.UserService? user = repository.GetUserById(id);
        return user;
    }

    public src.TaskManagerBackEnd.UserService GetUserByEmail(string email)
    {
        return repository.GetUserByEmail(email);
    }

    public bool UpdatePassword(UserUpdatePasswordDto userDto)
    {
        _ = GetUserById(userDto.IdUser) ?? throw new Exception("User not found");

        string salt = HashPassword.GenerateSalt();
        string password = HashPassword.ComputeHash(userDto.NewPassword, salt, _pepper!, Iteration);

        return repository.UpdatePassword(userDto.IdUser, password, salt);
    }

    public bool CheckPassword(UserLoginDto userLoginDto)
    {
        src.TaskManagerBackEnd.UserService userServiceAux = GetUserByEmail(userLoginDto.Email);

        if (userServiceAux is null) throw new Exception("User does not exists");

        string passwordUserLoginDto =
            HashPassword.ComputeHash(userLoginDto.Password, userServiceAux.Salt, _pepper!, Iteration);

        return passwordUserLoginDto.Equals(userServiceAux.Password);
    }

    public List<src.TaskManagerBackEnd.UserService> GetUserByTeamId(int idTeam)
    {
        return repository.GetUserByTeamId(idTeam);
    }

    public List<src.TaskManagerBackEnd.UserService> GetUsers(bool includeTeam = false)
    {
        List<src.TaskManagerBackEnd.UserService> users = repository.GetUsers();
        return users;
    }

    /// <summary>
    ///     Adds a new member to the repository.
    /// </summary>
    /// <param name="userDto">The user data transfer object containing the details of the member to be added.</param>
    /// <returns>
    ///     A boolean value indicating whether the member was successfully added.
    /// </returns>
    public bool AddUserSeed(UserInsertDTO userDto)
    {
        string salt = HashPassword.GenerateSalt();
        string password = HashPassword.ComputeHash(userDto.Password, salt, _pepper, Iteration);

        src.TaskManagerBackEnd.UserService userService = userDto.MapToModel();
        userService.Salt = salt;
        userService.Password = password;

        return repository.AddMember(userService);
    }
}