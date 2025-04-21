using src.TaskManagerBackEnd;
using src.TaskManagerBackEnd.Repository;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Service;

public class UserService(IUserRepository repository, IConfiguration configuration, IAssignmentService assignmentService, IServiceProvider serviceProvider) : IUserService
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
        User userExist = GetUserByEmail(userDto.Email);

        if (userExist is not null)
            throw new Exception("User already exists");

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
        return repository.AddMember(user);
    }

    public bool UpdateUser(UserUpdateDto userDto)
    {
        User? userAux = GetUserById(userDto.IdUser);

        if (userAux is null) throw new Exception("User does not exists");

        User user = new()
        {
            IdUser = userDto.IdUser,
            Email = userDto.Email,
            Post = userDto.Post,
            Enabled = userDto.Enabled,
            Name = userDto.Name,
            IdTeam = userDto.IdTeam
        };

        return repository.UpdateMember(user);
    }

    public bool DeleteUser(int id)
    {
        int[] idUser = { id };
        List<Assignment> assignments = assignmentService.GetAssignmentsByUserId(idUser);
        if(assignments.Any()) throw new Exception("The user has assignments found.");
        
        return repository.DeleteUser(id);
    }

    public User? GetUserById(int id, bool includeTeam = false)
    {
        User user = repository.GetUserById(id);
        if (user is null) throw new Exception("User not found");
        if (!includeTeam) return user;
        
        ITeamService teamService = serviceProvider.GetRequiredService<ITeamService>() ??
                                   throw new ArgumentNullException(nameof(ITeamService), "TeamService not found");
        
        Team team = teamService.GetTeamById(user.IdTeam.Value);
        user.Team = team;
        return user;
    }

    public User GetUserByEmail(string email)
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
        User userAux = GetUserByEmail(userLoginDto.Email);

        if (userAux is null) throw new Exception("User does not exists");

        string passwordUserLoginDto =
            HashPassword.ComputeHash(userLoginDto.Password, userAux.Salt, _pepper!, Iteration);

        return passwordUserLoginDto.Equals(userAux.Password);
    }

    public List<User> GetUserByTeamId(int idTeam)
    {
        return repository.GetUserByTeamId(idTeam);
    }

    public List<User> GetUsers(bool includeTeam = false)
    {
        List<User> users = repository.GetUsers();
        
        if (!includeTeam) return users;
        
        ITeamService teamService = serviceProvider.GetRequiredService<ITeamService>() ??
                                   throw new ArgumentNullException(nameof(ITeamService), "TeamService not found");
        
        List<Team> teams = teamService.GetTeams();
        
        List<User> usersTeams = (from u in users
                                join t in teams on u.IdTeam equals t.IdTeam
                                select new User
                                {
                                    IdUser = u.IdUser,
                                    Email = u.Email,
                                    Post = u.Post,
                                    DateCreation = u.DateCreation,
                                    Enabled = u.Enabled,
                                    Name = u.Name,
                                    Salt = u.Salt,
                                    Password = u.Password,
                                    IdTeam = t.IdTeam,
                                    Team = t
                                }).ToList();
        return usersTeams;
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
        User userExist = GetUserByEmail(userDto.Email);

        if (userExist is not null)
            return false;

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


        return repository.AddMember(user);
    }
}