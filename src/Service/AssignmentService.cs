using Microsoft.OpenApi.Extensions;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Repository;

namespace TaskManagerBackEnd.Service;

public class AssignmentService(IAssignmentRepository repository) : IAssignmentService
{
    public int? CreateAssignment(AssignmentDTOInsert taskDTO)
    {
        Assignment assignment = new()
        {
            Title = taskDTO.Title,
            Deadline = taskDTO.Deadline,
            Description = taskDTO.Description,
            Status = taskDTO.Status,
            DateCreation = DateTime.Now,
            IdUser = taskDTO.IdUser,
        };
        
        return repository.CreateAssignment(assignment);
    }

    public bool UpdateAssignment(AssignmentDTOUpdate taskDTO)
    {
        Assignment assignment = new()
        {
            IdTask = taskDTO.IdTask,
            Title = taskDTO.Title,
            Deadline = taskDTO.Deadline,
            Description = taskDTO.Description,
            Status = taskDTO.Status,
            DateConclusion = taskDTO.DateConclusion,
            IdUser = taskDTO.IdUser
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

    public List<Assignment> GetAssignmentsByUserId(int userId)
    {
        return repository.GetAssignmentsByUserId(userId);
    }
}