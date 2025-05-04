using Microsoft.OpenApi.Extensions;
using src.TaskManagerBackEnd;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;

namespace TaskManagerBackEnd.Mappers;

public static class AssignmentMapper
{
    public static AssignmentDto MapModelToDto(this Assignment user)
    {
        return new AssignmentDto
        {
           IdTask =  user.IdTask,
           Title =   user.Title,
           Description =   user.Description,
           DateCreation = user.DateCreation,
           DateConclusion =  user.DateConclusion,
           IdUser =   user.IdUser,
           Status =   user.Status,
           Deadline =  user.Deadline
        };
    }
    
    public static Assignment MapPostToModel(this AssignmentPostDto assignmentPostDto)
    {
        return new Assignment
        {
            Title = assignmentPostDto.Title,
            Description = assignmentPostDto.Description,
            Deadline = assignmentPostDto.Deadline,
            Status = assignmentPostDto.Status,
            DateCreation = DateTime.Now,
            IdUser = assignmentPostDto.IdUser
        };
    }
    
    public static Assignment MapPutToModel(this AssignmentPutDto assignmentPutDto)
    {
        return new Assignment
        {
            IdTask = assignmentPutDto.IdTask,
            Title = assignmentPutDto.Title,
            Description = assignmentPutDto.Description,
            Deadline = assignmentPutDto.Deadline,
            Status = assignmentPutDto.Status,
            DateConclusion = assignmentPutDto.DateConclusion,
            IdUser = assignmentPutDto.IdUser
        };
    }
}