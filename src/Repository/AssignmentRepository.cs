using Dapper;
using Npgsql;
using TaskManagerBackEnd.Connection;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Repository;

public class AssignmentRepository(ConnectionDb connectionDb) : IAssignmentRepository
{
    public int? CreateAssignment(Assignment task)
    {
        using NpgsqlConnection connection = connectionDb.OpenConnection();

        int idtask = connection.Execute(@"
            INSERT INTO tasks.task(title, description, datecreation, deadline, iduser, status)
            values(@Title, @Description, @DateCreation, @Deadline, @IdUser, @Status)
            returning idtask;
            ", new
        {
            Title = task.Title,
            Description = task.Description,
            DateCreation = task.DateCreation,
            Deadline = task.Deadline,
            IdUser = task.IdUser,
            Status = task.Status,
        });
        return idtask;
    }

    public bool UpdateAssignment(Assignment task)
    {
        using NpgsqlConnection connection = connectionDb.OpenConnection();

        int res = connection.Execute(@"
            UPDATE tasks.task
            SET title = @Title, description = @Description, deadline = @Deadline, iduser = @IdUser, status = @Status, dateconclusion = @DateConclusion
            WHERE idtask = @IdTask
            ", new
        {
            IdTask = task.IdTask,
            Title = task.Title,
            Description = task.Description,
            Deadline = task.Deadline,
            IdUser = task.IdUser,
            Status = task.Status,
            DateConclusion = task.DateConclusion
        });

        return res > 0;
    }

    public bool DeleteAssignment(int taskId)
    {
        throw new NotImplementedException();
    }

    public Assignment? GetAssignment(int taskId)
    {
        using NpgsqlConnection connection = connectionDb.OpenConnection();
        return connection.Query<Assignment>(@"
            SELECT idtask, title, description, datecreation, deadline, iduser, status, dateconclusion
            FROM tasks.task WHERE idtask = @IdTask
            ", new { IdTask = taskId }).FirstOrDefault();
    }

    public List<Assignment> GetAssignments()
    {
        using NpgsqlConnection connection = connectionDb.OpenConnection();
        return connection.Query<Assignment>(@"
            SELECT idtask, title, description, datecreation, deadline, iduser, status, dateconclusion
            FROM tasks.task
            ").ToList();
    }

    public List<Assignment> GetAssignmentsByUserId(int[] usersId)
    {
        using NpgsqlConnection connection = connectionDb.OpenConnection();
        return connection.Query<Assignment>(@"
            SELECT idtask, title, description, datecreation, deadline, iduser, status, dateconclusion
            FROM tasks.task
            WHERE iduser in @IdUser
            ", new
        {
            IdUser = usersId
        }).ToList();
    }


    public List<Assignment> GetAssignmentsByTeamsId(int[] teamId)
    {
        using NpgsqlConnection connection = connectionDb.OpenConnection();
        return connection.Query<Assignment>(@"
            SELECT idtask, title, description, datecreation, deadline, iduser, status, dateconclusion
            FROM tasks.task
            WHERE iduser = ANY(SELECT iduser FROM tasks.user WHERE idteam = ANY(@IdTeam))", new
        {
            IdTeam = teamId
        }).ToList();
    }
}