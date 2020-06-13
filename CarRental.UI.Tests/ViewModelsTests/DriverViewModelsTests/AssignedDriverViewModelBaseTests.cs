using CarRental.UI.ViewModels.DriverViewModels;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.DomainModelLayer.Policies;
using GalaSoft.MvvmLight.Messaging;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.DriverViewModelsTests
{
    public class AssignedDriverViewModelBaseTests
    {
        [Test]
        public void ShouldAssignCurrentDriverWhenReceivedMessage()
        {
            var driver = new DriverViewModel
            {
                FreeMinutesPolicy = PoliciesEnum.Standard
            };
            var sut = new TestAssignedDriverViewModelBase();
            Messenger.Default.Send(driver);
            Assert.AreEqual(driver, sut.CurrentDriver);
        }
    }

    internal class TestAssignedDriverViewModelBase : AssignedDriverViewModelBase
    {
    }
}