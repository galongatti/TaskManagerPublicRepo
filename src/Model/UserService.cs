using TaskManagerBackEnd.Model;

namespace src.TaskManagerBackEnd;

/// <summary>
///     Represents a user in the Task Manager system.
/// </summary>
public class UserService
{
    /// <summary>
    ///     Gets or sets the unique identifier for the user.
    /// </summary>
    public int IdUser { get; set; }

    /// <summary>
    ///     Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     Gets or sets the password of the user.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    ///     Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the post of the user.
    /// </summary>
    public string Post { get; set; }

    /// <summary>
    ///     Gets or sets the date when the user was created.
    /// </summary>
    public DateTime DateCreation { get; set; }

    /// <summary>
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    ///     Gets or sets the salt used for hashing the user's password.
    /// </summary>
    public string Salt { get; set; }
    
    /// <summary>
    /// Gets or sets the team object associated with the user.
    /// </summary>
    public Team? Team { get; set; }
    

}