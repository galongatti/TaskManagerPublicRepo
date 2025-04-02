using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Service;

public interface IAssignmentService
{
    int? CreateAssignment(AssignmentDTOInsert task);
    bool UpdateAssignment(AssignmentDTOUpdate task);
    bool DeleteAssignment(int taskId);
    Assignment? GetAssignment(int taskId);
    List<Assignment> GetAssignments();
    List<Assignment> GetAssignmentsByUserId(int[] usersId);
    List<Assignment> GetAssignmentsByTeamsId(int[] teamId);
}