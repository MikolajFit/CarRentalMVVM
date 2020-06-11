using System;
using CarRental.UI.Mappers;
using CarRental.UI.Services;
using CarRental.UI.Utils.Interfaces;
using CarRental.UI.ViewModels.DriverViewModels;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using NSubstitute;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.DriverViewModelsTests
{
    public class DriverMainViewModelTests
    {
        private ActiveRentalSessionViewModel _activeRentalSessionViewModelMock;
        private IMessengerService _messengerServiceMock;
        private RentCarViewModel _rentCarViewModelMock;

        [SetUp]
        public void Setup()
        {
            _messengerServiceMock = Substitute.For<IMessengerService>();
            _activeRentalSessionViewModelMock = Substitute.For<ActiveRentalSessionViewModel>(
                Substitute.For<ITimerFactory>(), Substitute.For<IRentalService>(), Substitute.For<IDialogService>(),
                Substitute.For<IMessengerService>());
            _rentCarViewModelMock = Substitute.For<RentCarViewModel>(Substitute.For<ICarService>(),
                Substitute.For<IRentalService>(), Substitute.For<ICarViewModelMapper>(),
                Substitute.For<IRentalViewModelMapper>(), Substitute.For<IMessengerService>());
        }

        [Test]
        public void ShouldThrowExceptionIfParameterInConstructorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new DriverMainViewModel(_messengerServiceMock, null, _activeRentalSessionViewModelMock));
            Assert.Throws<ArgumentNullException>(() =>
                new DriverMainViewModel(_messengerServiceMock, _rentCarViewModelMock, null));
        }

        [Test]
        public void ShouldSetCurrentCarViewModelToRenCarViewModelUponInitialization()
        {
            var sut = new DriverMainViewModel(_messengerServiceMock, _rentCarViewModelMock,
                _activeRentalSessionViewModelMock);
            Assert.AreEqual(_rentCarViewModelMock, sut.CurrentRentCarViewModel);
        }

        [Test]
        public void ShouldSetCurrentCarViewModelToRentCarViewModelWhenSendProperMessage()
        {
            var sut = new DriverMainViewModel(_messengerServiceMock, _rentCarViewModelMock,
                _activeRentalSessionViewModelMock);
            sut.CurrentRentCarViewModel = _activeRentalSessionViewModelMock;
            Messenger.Default.Send(new NotificationMessage("Stop Car Rental"));
            Assert.AreEqual(_rentCarViewModelMock, sut.CurrentRentCarViewModel);
        }

        [Test]
        public void ShouldSetCurrentCarViewModelToActiveRentalSessionViewModelWhenSendProperMessage()
        {
            var sut = new DriverMainViewModel(_messengerServiceMock, _rentCarViewModelMock,
                _activeRentalSessionViewModelMock);
            Messenger.Default.Send(new NotificationMessage("Start Car Rental"));
            Assert.AreEqual(_activeRentalSessionViewModelMock, sut.CurrentRentCarViewModel);
        }
    }
}