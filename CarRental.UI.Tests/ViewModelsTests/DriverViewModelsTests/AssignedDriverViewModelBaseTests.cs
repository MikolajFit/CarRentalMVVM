using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.UI.ViewModels.DriverViewModels;
using CarRental.UI.ViewModels.ObservableObjects;
using GalaSoft.MvvmLight.Messaging;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.DriverViewModelsTests
{
    public class AssignedDriverViewModelBaseTests
    {
        [Test]
        public void ShouldAssignCurrentDriverWhenReceivedMessage()
        {
            var driver = new DriverViewModel();
            var sut = new TestAssignedDriverViewModelBase();
            Messenger.Default.Send(driver);
            Assert.AreEqual(driver,sut.CurrentDriver);
        }
    }

    internal class TestAssignedDriverViewModelBase : AssignedDriverViewModelBase
    {

    }
}
