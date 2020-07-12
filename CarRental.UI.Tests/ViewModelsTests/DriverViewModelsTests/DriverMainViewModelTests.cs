using System;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.UI.Mappers;
using CarRental.UI.Services;
using CarRental.UI.Utils.Interfaces;
using CarRental.UI.ViewModels.DriverViewModels;
using CarRental.UI.ViewModels.ObservableObjects;
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
        private DriverAccountViewModel _driverAccountViewModelMock;
        private DriverRentalsViewModel _driverRentalsViewModelMock;

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
            _driverAccountViewModelMock = Substitute.For<DriverAccountViewModel>(Substitute.For<IDriverService>(),
                Substitute.For<IDriverViewModelMapper>());
            _driverRentalsViewModelMock = Substitute.For<DriverRentalsViewModel>(Substitute.For<IRentalService>(),
                Substitute.For<IRentalViewModelMapper>());
            _driverRentalsViewModelMock.CurrentDriver = new DriverViewModel() {Id = Guid.NewGuid()};
        }

        [Test]
        public void ShouldThrowExceptionIfParameterInConstructorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new DriverMainViewModel(null, _rentCarViewModelMock, _activeRentalSessionViewModelMock,_driverAccountViewModelMock,_driverRentalsViewModelMock));
            Assert.Throws<ArgumentNullException>(() => new DriverMainViewModel(_messengerServiceMock, null, _activeRentalSessionViewModelMock,_driverAccountViewModelMock,_driverRentalsViewModelMock));
            Assert.Throws<ArgumentNullException>(() => new DriverMainViewModel(_messengerServiceMock, _rentCarViewModelMock, null,_driverAccountViewModelMock,_driverRentalsViewModelMock));
            Assert.Throws<ArgumentNullException>(() => new DriverMainViewModel(_messengerServiceMock, _rentCarViewModelMock, _activeRentalSessionViewModelMock,null,_driverRentalsViewModelMock));
            Assert.Throws<ArgumentNullException>(() => new DriverMainViewModel(_messengerServiceMock, _rentCarViewModelMock, _activeRentalSessionViewModelMock,_driverAccountViewModelMock,null));
        }

        [Test]
        public void ShouldSetCurrentCarViewModelToRenCarViewModelUponInitialization()
        {
            var sut = new DriverMainViewModel(_messengerServiceMock, _rentCarViewModelMock, _activeRentalSessionViewModelMock, _driverAccountViewModelMock, _driverRentalsViewModelMock);
            Assert.AreEqual(_rentCarViewModelMock, sut.CurrentRentCarViewModel);
        }

        [Test]
        public void ShouldSetCurrentCarViewModelToRentCarViewModelWhenSendProperMessage()
        {
            var sut = new DriverMainViewModel(_messengerServiceMock, _rentCarViewModelMock, _activeRentalSessionViewModelMock, _driverAccountViewModelMock, _driverRentalsViewModelMock);
            sut.CurrentRentCarViewModel = _activeRentalSessionViewModelMock;
            Messenger.Default.Send(new NotificationMessage("Stop Car Rental"));
            Assert.AreEqual(_rentCarViewModelMock, sut.CurrentRentCarViewModel);
        }

        [Test]
        public void ShouldSetCurrentCarViewModelToActiveRentalSessionViewModelWhenSendProperMessage()
        {
            var sut = new DriverMainViewModel(_messengerServiceMock, _rentCarViewModelMock, _activeRentalSessionViewModelMock, _driverAccountViewModelMock, _driverRentalsViewModelMock);
            Messenger.Default.Send(new NotificationMessage("Start Car Rental"));
            Assert.AreEqual(_activeRentalSessionViewModelMock, sut.CurrentRentCarViewModel);
        }
    }
}