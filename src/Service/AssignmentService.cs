using Microsoft.OpenApi.Extensions;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Repository;

namespace TaskManagerBackEnd.Service;

public class AssignmentService(IAssignmentRepository repository) : IAssignmentService
{
    public int? CreateAssignment(AssignmentInsertDto taskInsertDto)
    {
        Assignment assignment = new()
        {
            Title = taskInsertDto.Title,
            Deadline = taskInsertDto.Deadline,
            Description = taskInsertDto.Description,
            Status = taskInsertDto.Status,
            DateCreation = DateTime.Now,
            IdUser = taskInsertDto.IdUser,
        };
        
        return repository.CreateAssignment(assignment);
    }

    public bool UpdateAssignment(AssignmentUpdateDto taskUpdateDto)
    {
        Assignment assignment = new()
        {
            IdTask = taskUpdateDto.IdTask,
            Title = taskUpdateDto.Title,
            Deadline = taskUpdateDto.Deadline,
            Description = taskUpdateDto.Description,
            Status = taskUpdateDto.Status,
            DateConclusion = taskUpdateDto.DateConclusion,
            IdUser = taskUpdateDto.IdUser
        };
        
        return repository.UpdateAssignment(assignment);
    }

    public bool DeleteAssignment(int taskId)
    {
        throw new NotImplementedException();
    }

    public Assignment? GetAssignment(int taskId)
    {
        return repository.GetAssignment(taskId);
    }

    public List<Assignment> GetAssignments()
    {
        return repository.GetAssignments();
    }

    public List<Assignment> GetAssignmentsByUserId(int[] usersId)
    {
        return repository.GetAssignmentsByUserId(usersId);
    }

    public List<Assignment> GetAssignmentsByTeamsId(int[] teamId)
    {
        return repository.GetAssignmentsByTeamsId(teamId);
    }
}