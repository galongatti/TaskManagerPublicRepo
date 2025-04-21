using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Service;

public interface IAssignmentService
{
    int? CreateAssignment(AssignmentInsertDto task);
    bool UpdateAssignment(AssignmentUpdateDto task);
    bool DeleteAssignment(int taskId);
    Assignment? GetAssignment(int taskId);
    List<Assignment> GetAssignments();
    List<Assignment> GetAssignmentsByUserId(int[] usersId);
    List<Assignment> GetAssignmentsByTeamsId(int[] teamId);
}