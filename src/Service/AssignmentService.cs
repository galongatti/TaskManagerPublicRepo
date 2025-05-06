using Microsoft.OpenApi.Extensions;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Enum;
using TaskManagerBackEnd.Mappers;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Repository;

namespace TaskManagerBackEnd.Service;

public class AssignmentService(IAssignmentRepository repository) : IAssignmentService
{
    public Assignment? CreateAssignment(AssignmentPostDto taskPostDto)
    {
        Assignment assignment = taskPostDto.MapPostToModel();
        int? id = repository.CreateAssignment(assignment);
        
        if(id is null) throw new Exception("Assignment not created");

        return GetAssignment(id.Value);
    }

    public Assignment? UpdateAssignment(AssignmentPutDto taskPutDto)
    {
        Assignment assignment = taskPutDto.MapPutToModel();
        bool result = repository.UpdateAssignment(assignment);
        if(result is false) throw new Exception("Assignment not updated");

        return GetAssignment(assignment.IdTask);
    }

    public bool DeleteAssignment(int taskId)
    {

        Assignment assignment = GetAssignment(taskId);
        
        if (assignment is null) throw new Exception("Assignment not found");
        
        if(assignment.Status == StatusAssignmet.InProgress || assignment.Status == StatusAssignmet.Concluded) 
            throw new Exception("Assignment must be pending or canceled to be deleted");

        return repository.DeleteAssignment(taskId);
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