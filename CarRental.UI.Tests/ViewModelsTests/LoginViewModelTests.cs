using System;
using System.Collections.Generic;
using CarRental.UI.Mappers;
using CarRental.UI.Services;
using CarRental.UI.ViewModels;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using NSubstitute;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests
{
    public class LoginViewModelTests
    {
        private readonly IDriverService _driverServiceMock = Substitute.For<IDriverService>();
        private readonly IDriverViewModelMapper _driverViewModelMapperMock = Substitute.For<IDriverViewModelMapper>();
        private readonly IMessengerService _messengerServiceMock = Substitute.For<IMessengerService>();

        [Test]
        public void ShouldThrowExceptionIfPassedNullArgumentInConstructor()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new LoginViewModel(null, _driverViewModelMapperMock, _messengerServiceMock));
            Assert.Throws<ArgumentNullException>(() =>
                new LoginViewModel(_driverServiceMock, null, _messengerServiceMock));
            Assert.Throws<ArgumentNullException>(() =>
                new LoginViewModel(_driverServiceMock, _driverViewModelMapperMock, null));
        }

        [Test]
        public void ShouldPopulateDriverListViewWithProperDriverItems()
        {
            var driverDtoList = new List<DriverDTO>
            {
                new DriverDTO()
            };
            var driverViewModel = new DriverViewModel();
            _driverServiceMock.GetAllDrivers().Returns(driverDtoList);
            _driverViewModelMapperMock.Map(Arg.Any<DriverDTO>()).Returns(driverViewModel);
            var sut = new LoginViewModel(_driverServiceMock, _driverViewModelMapperMock, _messengerServiceMock);
            Assert.AreEqual(driverViewModel, sut.Drivers[0]);
        }

        [Test]
        public void ShouldSendNavigateToAdminViewMessage()
        {
            var sut = new LoginViewModel(_driverServiceMock, _driverViewModelMapperMock, _messengerServiceMock);
            sut.AdminLoginCommand.Execute(null);
            _messengerServiceMock.Received()
                .Send(Arg.Is<NotificationMessage>(m => m.Notification == "GoToAdminMainView"));
        }

        [Test]
        public void ShouldSendNavigateToRegisterViewMessage()
        {
            var sut = new LoginViewModel(_driverServiceMock, _driverViewModelMapperMock, _messengerServiceMock);
            sut.RegisterCommand.Execute(null);
            _messengerServiceMock.Received()
                .Send(Arg.Is<NotificationMessage>(m => m.Notification == "GoToRegisterWindow"));
        }

        [Test]
        public void ShouldSendNavigateToDriverViewMessageAndSelectedDriverMessage()
        {
            var sut = new LoginViewModel(_driverServiceMock, _driverViewModelMapperMock, _messengerServiceMock);
            var driver = new DriverViewModel();
            sut.SelectedDriver = driver;
            sut.LoginCommand.Execute(null);
            _messengerServiceMock.Received()
                .Send(Arg.Is<NotificationMessage>(m => m.Notification == "GoToDriverMainWindow"));
            _messengerServiceMock.Received().Send(driver);
        }

        [Test]
        public void ShouldNotExecuteLoginCommandWhenDriverIsNull()
        {
            var sut = new LoginViewModel(_driverServiceMock, _driverViewModelMapperMock, _messengerServiceMock);
            sut.LoginCommand.Execute(null);
            _messengerServiceMock.DidNotReceive()
                .Send(Arg.Is<NotificationMessage>(m => m.Notification == "GoToDriverMainWindow"));
            _messengerServiceMock.DidNotReceive().Send(Arg.Any<DriverViewModel>());
        }
    }
}