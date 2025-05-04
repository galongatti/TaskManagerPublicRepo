using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Service;

public interface IAssignmentService
{
    Assignment? CreateAssignment(AssignmentPostDto task);
    Assignment? UpdateAssignment(AssignmentPutDto task);
    bool DeleteAssignment(int taskId);
    Assignment? GetAssignment(int taskId);
    List<Assignment> GetAssignments();
    List<Assignment> GetAssignmentsByUserId(int[] usersId);
    List<Assignment> GetAssignmentsByTeamsId(int[] teamId);
}