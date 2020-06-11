using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.UI.Messages;
using CarRental.UI.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests
{
    public class CustomBaseViewModelTests
    {
        [Test]
        public void ShouldNotUpdatePropertyWhenIfCleanupWasCalled()
        {
            var sut = new TestCustomViewModelBase();
            Messenger.Default.Send(new CleanupMessage(""));
            Messenger.Default.Send(new NotificationMessage(""));
            Assert.IsNull(sut.TestProperty);
        }
    }
    internal class TestCustomViewModelBase : CustomViewModelBase
    {
        public TestCustomViewModelBase()
        {
            Messenger.Default.Register<NotificationMessage>(this, ChangeTestProperty);
        }

        private void ChangeTestProperty(NotificationMessage obj)
        {
            TestProperty = obj.Notification;
        }

        public string TestProperty;
    }
}
