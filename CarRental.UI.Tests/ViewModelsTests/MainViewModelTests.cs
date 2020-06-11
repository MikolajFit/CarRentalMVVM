using System;
using CarRental.UI.Services;
using CarRental.UI.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using NSubstitute;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests
{
    public class MainViewModelTests
    {
        private readonly IMessengerService _messengerServiceMock = Substitute.For<IMessengerService>();

        [Test]
        public void ShouldThrowExceptionIfPassedNullArgumentInConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => new MainViewModel(null));
        }

        [Test]
        public void ShouldSendLogoutMessage()
        {
            var sut = new MainViewModel(_messengerServiceMock);
            sut.LogoutCommand.Execute(null);
            _messengerServiceMock.Received().Send(Arg.Is<NotificationMessage>(m => m.Notification == "Logout"));
        }
    }
}