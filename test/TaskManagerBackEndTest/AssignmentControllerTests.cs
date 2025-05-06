using Microsoft.Extensions.Logging;

namespace TaskManagerBackEndTest;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagerBackEnd.Controllers;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Service;
using Xunit;

public class AssignmentControllerTests
{
    private readonly Mock<IAssignmentService> _mockAssignmentService;
    private readonly Mock<ILogger<AssignmentController>> _mockLogger;
    private readonly AssignmentController _controller;

    public AssignmentControllerTests()
    {
        _mockAssignmentService = new Mock<IAssignmentService>();
        _mockLogger = new Mock<ILogger<AssignmentController>>();
        _controller = new AssignmentController(_mockAssignmentService.Object, _mockLogger.Object);
    }

    [Fact]
    public void CreateAssignment_ValidAssignment_ReturnsOk()
    {
        // Arrange
        AssignmentPostDto assignmentDto = new();
        Assignment assignment = new();
        _mockAssignmentService.Setup(s => s.CreateAssignment(assignmentDto)).Returns(assignment);

        // Act
        ActionResult<bool> result = _controller.CreateAssignment(assignmentDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void CreateAssignment_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = _controller.CreateAssignment(new AssignmentPostDto());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void GetAssignmentById_ValidId_ReturnsOk()
    {
        // Arrange
        int id = 1;
        var assignment = new Assignment { IdTask = id, /* Propriedades do modelo */ };
        _mockAssignmentService.Setup(s => s.GetAssignment(id)).Returns(assignment);

        // Act
        var result = _controller.GetAssignmentById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void GetAssignmentById_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        int id = 1;
        _mockAssignmentService.Setup(s => s.GetAssignment(id)).Returns((Assignment)null);

        // Act
        var result = _controller.GetAssignmentById(id);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void DeleteAssignment_ValidId_ReturnsOk()
    {
        // Arrange
        int id = 1;
        _mockAssignmentService.Setup(s => s.DeleteAssignment(id)).Returns(true);

        // Act
        var result = _controller.DeleteAssignment(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.True((bool)okResult.Value);
    }

    [Fact]
    public void DeleteAssignment_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        int id = 1;
        _mockAssignmentService.Setup(s => s.DeleteAssignment(id)).Returns(false);

        // Act
        var result = _controller.DeleteAssignment(id);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}