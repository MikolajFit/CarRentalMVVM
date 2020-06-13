using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.UI.Services;
using CarRental.UI.ViewModels;
using CarRental.UI.ViewModels.AdminViewModels;
using GalaSoft.MvvmLight.Messaging;
using NSubstitute;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.AdminViewModelsTests
{
    public class AdminMainViewModelTests
    {

        private readonly IMessengerService _messengerServiceMock = Substitute.For<IMessengerService>();

        [Test]
        public void ShouldThrowExceptionIfPassedNullArgumentInConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => new AdminMainViewModel(null));
        }

        [Test]
        public void ShouldSendLogoutMessage()
        {
            var sut = new AdminMainViewModel(_messengerServiceMock);
            sut.LogoutCommand.Execute(null);
            _messengerServiceMock.Received().Send(Arg.Is<NotificationMessage>(m => m.Notification == "Logout"));
        }
    }
}
