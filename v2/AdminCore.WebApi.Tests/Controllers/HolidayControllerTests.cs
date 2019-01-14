using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.Event;
using AdminCore.WebApi.Controllers;
using AdminCore.WebApi.Mappings;
using AdminCore.WebApi.Models.Holiday;
using AutoFixture;
using AutoMapper;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdminCore.WebApi.Tests.Controllers
{
  public class HolidayControllerTests : BaseControllerTest
  {
    private readonly HolidayController _controller;

    private readonly IAuthenticatedUser _authenticatedUser;

    private readonly IEventService _eventService;

    private readonly IFixture _fixture;

    private readonly IMapper _mapper;

    private readonly IEventMessageService _eventMessageService;

    public HolidayControllerTests()
    {
      _authenticatedUser = Substitute.For<IAuthenticatedUser>();
      _authenticatedUser.RetrieveUserId().Returns(1);
      _eventMessageService = Substitute.For<IEventMessageService>();
      _eventService = Substitute.For<IEventService>();
      _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new WebMappingProfile())));
      _fixture = new Fixture();
      _fixture.Customize<EventDto>(x => x.Without(z => z.EventDates));
      _controller = new HolidayController(_authenticatedUser, _eventService, _eventMessageService, _mapper);
    }

    [Fact]
    public void CreateHoliday_WhenCalled_ReturnsCreatedHoliday()
    {
      // Arrange
      var createViewModel = _fixture.Create<CreateHolidayViewModel>();

      _authenticatedUser.RetrieveUserId().Returns(_authenticatedUser.RetrieveUserId());

      // Act
      var result = _controller.CreateHoliday(createViewModel);

      // Assert

      VerifyActionResult(result);
      _eventService.Received(1).CreateEvent(Arg.Any<EventDateDto>(), EventTypes.AnnualLeave);
    }

    [Fact]
    public void UpdateHoliday_WhenCalled_ReturnsUpdatedHoliday()
    {
      // Arrange
      var updateViewModel = _fixture.Create<UpdateEventViewModel>();

      _authenticatedUser.RetrieveUserId().Returns(_authenticatedUser.RetrieveUserId());

      // Act
      var result = _controller.UpdateHoliday(updateViewModel);

      // Assert

      VerifyActionResult(result);
      _eventService.Received(1).UpdateEvent(Arg.Any<EventDateDto>(), Arg.Any<string>());
    }

    [Fact]
    public void GetAllHolidays_WhenCalled_ReturnsAllHolidays()
    {
      // Arrange
      const int numOfHolidays = 9;
      var holidayEvents = _fixture.CreateMany<EventDto>(numOfHolidays).ToList();

      _eventService.GetEmployeeEvents(EventTypes.AnnualLeave).Returns(holidayEvents);
      _mapper.Map<List<HolidayViewModel>>(holidayEvents);

      // Act
      var result = _controller.GetAllHolidays();

      // Assert
      _eventService.Received(1).GetEmployeeEvents(EventTypes.AnnualLeave);
      var returnedHolidayEvents = RetrieveValueFromActionResult<List<HolidayViewModel>>(result);
      Assert.Equal(numOfHolidays, returnedHolidayEvents.Count);
    }

    [Fact]
    public void GetAllHolidaysByEmployeeId_WhenCalled_ReturnsAllHolidaysOfEmployeeId()
    {
      // Arrange
      const int employeeId = 1;
      const int numOfHolidays = 9;
      var holidayEvents = _fixture.CreateMany<EventDto>(numOfHolidays).ToList();

      _authenticatedUser.RetrieveUserId().Returns(_authenticatedUser.RetrieveUserId());
      _eventService.GetEventsByEmployeeId(employeeId, EventTypes.AnnualLeave).Returns(holidayEvents);
      _mapper.Map<List<HolidayViewModel>>(holidayEvents);

      // Act
      var result = _controller.GetHolidayByEmployeeId(employeeId);

      // Assert
      _eventService.Received(1).GetEventsByEmployeeId(employeeId, EventTypes.AnnualLeave);
      var returnedHolidayEvents = RetrieveValueFromActionResult<List<HolidayViewModel>>(result);
      Assert.Equal(numOfHolidays, returnedHolidayEvents.Count);
    }

    [Fact]
    public void GetHolidaysByHolidayId_WhenCalled_ReturnsHolidayOfHolidayId()
    {
      // Arrange
      const int eventId = 1;
      var eventDto = _fixture.Create<EventDto>();

      _eventService.GetEvent(eventId).Returns(eventDto);
      _mapper.Map<HolidayViewModel>(eventDto);

      // Act
      var result = _controller.GetHolidayByEventId(eventId);

      // Assert
      _eventService.Received(1).GetEvent(eventId);
      var returnedHoliday = RetrieveValueFromActionResult<HolidayViewModel>(result);
      Assert.NotNull(returnedHoliday);
    }

    [Fact]
    public void GetHolidayByDateBetween_WhenCalled_ReturnsAllHolidaysBetweenTwoDates()
    {
      // Arrange
      var startDate = new DateTime(2018, 12, 04);
      var endDate = new DateTime(2018, 12, 06);
      const int numOfHolidays = 9;
      var holidayEvents = _fixture.CreateMany<EventDto>(numOfHolidays).ToList();

      _eventService.GetByDateBetween(startDate, endDate, EventTypes.AnnualLeave).Returns(holidayEvents);
      _mapper.Map<List<HolidayViewModel>>(holidayEvents);

      // Act
      var result = _controller.GetHolidayByDateBetween(startDate.ToString(), endDate.ToString());

      // Assert
      _eventService.Received(1).GetByDateBetween(Arg.Any<DateTime>(), Arg.Any<DateTime>(), EventTypes.AnnualLeave);
      var returnedHolidayEvents = RetrieveValueFromActionResult<List<HolidayViewModel>>(result);
      Assert.Equal(numOfHolidays, returnedHolidayEvents.Count);
    }

    [Fact]
    public void GetHolidayByStatusApproved_WhenCalled_ReturnsAllHolidaysOfThatHolidayStatus()
    {
      // Arrange
      const int numOfHolidays = 9;
      var holidayEvents = _fixture.CreateMany<EventDto>(numOfHolidays).ToList();

      _eventService.GetEventByStatus(EventStatuses.Approved, EventTypes.AnnualLeave).Returns(holidayEvents);
      _mapper.Map<List<HolidayViewModel>>(holidayEvents);

      // Act
      var result = _controller.GetHolidayByStatusType((int)EventStatuses.Approved);

      // Assert
      _eventService.Received(1).GetEventByStatus(EventStatuses.Approved, EventTypes.AnnualLeave);
      var returnedHolidayEvents = RetrieveValueFromActionResult<List<HolidayViewModel>>(result);
      Assert.Equal(numOfHolidays, returnedHolidayEvents.Count);
    }

    [Fact]
    public void ApproveHoliday_WhenCalled_CallsApproveHoliday()
    {
      // Arrange
      _authenticatedUser.RetrieveUserId().Returns(_authenticatedUser.RetrieveUserId());
      var approvedViewModel = _fixture.Create<ApproveHolidayViewModel>();

      // Act
      var result = _controller.ApproveHoliday(approvedViewModel);

      // Assert
      VerifyActionResult(result);

      _eventService.Received(1).UpdateEventStatus(approvedViewModel.EventId, EventStatuses.Approved);
    }

    [Fact]
    public void CancelHoliday_WhenCalled_CallsCancelHoliday()
    {
      // Arrange
      _authenticatedUser.RetrieveUserId().Returns(_authenticatedUser.RetrieveUserId());
      var cancelledViewModel = _fixture.Create<CancelHolidayViewModel>();

      // Act
      var result = _controller.CancelHoliday(cancelledViewModel);

      // Assert
      VerifyActionResult(result);

      _eventService.Received(1).UpdateEventStatus(cancelledViewModel.EventId, EventStatuses.Cancelled);
    }

    [Fact]
    public void RejectHoliday_WhenCalled_ReturnsRejectedHoliday()
    {
      // Arrange
      _authenticatedUser.RetrieveUserId().Returns(_authenticatedUser.RetrieveUserId());
      var rejectedViewModel = _fixture.Create<RejectHolidayViewModel>();

      // Act
      var result = _controller.RejectHoliday(rejectedViewModel);

      // Assert
      VerifyActionResult(result);

      _eventService.Received(1).RejectEvent(rejectedViewModel.EventId, rejectedViewModel.Message);
    }

    [Fact]
    public void GetHolidayStats_WhenCalled_ReturnsHolidayStats()
    {
      // Arrange
      var holidayStatsDto = _fixture.Create<HolidayStatsDto>();

      _authenticatedUser.RetrieveUserId().Returns(_authenticatedUser.RetrieveUserId());
      _eventService.GetHolidayStatsForUser().Returns(holidayStatsDto);

      // Act
      var result = _controller.GetEmployeeHolidayStats();

      // Assert
      VerifyActionResult(result);

      _eventService.Received(1).GetHolidayStatsForUser();
    }
  }
}