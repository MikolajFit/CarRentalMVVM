using System;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.UI.Mappers;
using CarRental.UI.Services;
using CarRental.UI.ViewModels;
using CarRental.UI.ViewModels.ObservableObjects;
using GalaSoft.MvvmLight.Messaging;
using NSubstitute;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests
{
    public class RegisterDriverViewModelTests
    {
        private IDriverService _driverServiceMock;
        private IDriverViewModelMapper _driverViewModelMapperMock;
        private IMessengerService _messengerServiceMock;

        [SetUp]
        public void Setup()
        {
            _driverServiceMock = Substitute.For<IDriverService>();
            _driverViewModelMapperMock = Substitute.For<IDriverViewModelMapper>();
            _messengerServiceMock = Substitute.For<IMessengerService>();
        }

        [Test]
        public void ShouldThrowExceptionIfPassedNullArgumentInConstructor()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new RegisterDriverViewModel(null, _driverViewModelMapperMock, _messengerServiceMock));
            Assert.Throws<ArgumentNullException>(() =>
                new RegisterDriverViewModel(_driverServiceMock, null, _messengerServiceMock));
            Assert.Throws<ArgumentNullException>(() =>
                new RegisterDriverViewModel(_driverServiceMock, _driverViewModelMapperMock, null));
        }

        [Test]
        public void ShouldProperlyCreateNewDriverWhenInitialized()
        {
            var sut = new RegisterDriverViewModel(_driverServiceMock, _driverViewModelMapperMock,
                _messengerServiceMock);
            Assert.NotNull(sut.CurrentDriver);
            Assert.AreNotEqual(Guid.Empty, sut.CurrentDriver.Id);
        }

        [Test]
        public void ShouldNotExecuteRegisterCommandWhenDriverIsNotValid()
        {
            var sut = new RegisterDriverViewModel(_driverServiceMock, _driverViewModelMapperMock,
                _messengerServiceMock);
            sut.RegisterDriverCommand.Execute(null);
            _driverViewModelMapperMock.DidNotReceive().Map(Arg.Any<DriverViewModel>());
            _driverServiceMock.DidNotReceive().CreateDriver(Arg.Any<DriverDTO>());
            _messengerServiceMock.DidNotReceive().Send(Arg.Any<NotificationMessage>());
        }

        [Test]
        public void ShouldExecuteRegisterCommandWhenDriverIsValid()
        {
            var sut = new RegisterDriverViewModel(_driverServiceMock, _driverViewModelMapperMock,
                _messengerServiceMock);
            sut.CurrentDriver.FirstName = "Valid";
            sut.CurrentDriver.LastName = "Valid";
            sut.CurrentDriver.LicenseNumber = "Valid";
            sut.RegisterDriverCommand.Execute(null);
            _driverViewModelMapperMock.Received().Map(sut.CurrentDriver);
            _driverServiceMock.Received().CreateDriver(Arg.Any<DriverDTO>());
            _messengerServiceMock.Received()
                .Send(Arg.Is<NotificationMessage>(m => m.Notification == "Close Register Window"));
        }
    }
}