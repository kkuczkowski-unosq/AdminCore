using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.Event;
using AdminCore.WebApi.Controllers;
using AdminCore.WebApi.Mappings;
using AdminCore.WebApi.Models.Event;
using AutoFixture;
using AutoMapper;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdminCore.WebApi.Tests.Controllers
{
  public class WorkingFromHomeControllerTests : BaseControllerTest
  {
    private readonly WorkingFromHomeController _controller;
    private readonly IEventService _eventService;
    private readonly IFixture _fixture;
    private readonly IMapper _mapper;

    public WorkingFromHomeControllerTests()
    {
      _eventService = Substitute.For<IEventService>();
      _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new WebMappingProfile())));
      _fixture = new Fixture();
      _fixture.Customize<EventDto>(x => x.Without(z => z.EventDates));
      _controller = new WorkingFromHomeController(_eventService, _mapper, Substitute.For<IAuthenticatedUser>());
    }

    [Fact]
    public void CreateWorkingFromHome_WhenCalledWithCorrectType_ReturnsWorkingFromHome()
    {
      // Arrange
      var createViewModel = _fixture.Create<CreateEventViewModel>();

      // Act
      var result = _controller.CreateWorkingFromHome(createViewModel);

      // Assert

      VerifyActionResult(result);
      _eventService.Received(1).CreateEvent(Arg.Any<EventDateDto>(), EventTypes.WorkingFromHome, 1);
    }

    [Fact]
    public void GetAllWorkingFromHomeEvents_WhenCalled_ReturnsAllWorkingFromHomeEvents()
    {
      // Arrange
      const int numOfWfhEvents = 9;
      var wfhEvents = _fixture.CreateMany<EventDto>(numOfWfhEvents).ToList();

      _eventService.GetEventByType(EventTypes.WorkingFromHome).Returns(wfhEvents);
      _mapper.Map<List<EventViewModel>>(wfhEvents);

      // Act
      var result = _controller.GetAllWorkingFromHomeEvents();

      // Assert
      _eventService.Received(1).GetEventByType(EventTypes.WorkingFromHome);
      var returnedWfhEvents = RetrieveValueFromActionResult<List<EventViewModel>>(result);
      Assert.Equal(numOfWfhEvents, returnedWfhEvents.Count);
    }

    [Fact]
    public void GetAllWorkingFromHomeEventsBId_WhenCalled_ReturnsAllWorkingFromHomeEventsById()
    {
      // Arrange
      const int eventId = 1;
      var wfhEvent = _fixture.Create<EventDto>();

      _eventService.GetEvent(eventId).Returns(wfhEvent);
      _mapper.Map<EventViewModel>(wfhEvent);

      // Act
      var result = _controller.GetWorkingFromHomeById(eventId);

      // Assert
      _eventService.Received(1).GetEvent(eventId);
      var returnedWfhEvent = RetrieveValueFromActionResult<EventViewModel>(result);
      Assert.NotNull(returnedWfhEvent);
    }

    [Fact]
    public void GetAllWorkingFromHomeEventsByEmployeeId_WhenCalled_ReturnsAllWorkingFromHomeEventsByEmployeeId()
    {
      // Arrange
      const int employeeId = 1;
      const int numOfWfhEvents = 9;
      var wfhEvents = _fixture.CreateMany<EventDto>(numOfWfhEvents).ToList();

      _eventService.GetEventsByEmployeeId(employeeId, EventTypes.WorkingFromHome).Returns(wfhEvents);
      _mapper.Map<List<EventViewModel>>(wfhEvents);

      // Act
      var result = _controller.GetWorkingFromHomeByEmployeeId(1);

      // Assert
      _eventService.Received(1).GetEventsByEmployeeId(employeeId, EventTypes.WorkingFromHome);
      var returnedWfhEvents = RetrieveValueFromActionResult<List<EventViewModel>>(result);
      Assert.Equal(numOfWfhEvents, returnedWfhEvents.Count);
    }
  }
}