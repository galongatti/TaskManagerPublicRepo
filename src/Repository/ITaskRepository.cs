using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Repository;

public interface IAssignmentRepository
{
    int? CreateAssignment(Assignment task);
    bool UpdateAssignment(Assignment task);
    bool DeleteAssignment(int taskId);
    Assignment? GetAssignment(int taskId);
    List<Assignment> GetAssignments();
    List<Assignment> GetAssignmentsByUserId(int[] usersId);
    List<Assignment> GetAssignmentsByTeamsId(int[] teamId);
}