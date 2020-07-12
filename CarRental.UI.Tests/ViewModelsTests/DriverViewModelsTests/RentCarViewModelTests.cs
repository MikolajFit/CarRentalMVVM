using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.Model.DomainModelLayer.Models;
using CarRental.UI.Mappers;
using CarRental.UI.Messages;
using CarRental.UI.Services;
using CarRental.UI.ViewModels.DriverViewModels;
using CarRental.UI.ViewModels.ObservableObjects;
using GalaSoft.MvvmLight.Messaging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.DriverViewModelsTests
{
    public class RentCarViewModelTests
    {
        private  ICarService _carServiceMock;
        private  ICarViewModelMapper _carViewModelMapperMock;
        private  IRentalService _rentalServiceMock;
        private  IRentalViewModelMapper _rentalViewModelMapperMock;
        private  IMessengerService _messengerServiceMock;

        [SetUp]
        public void Setup()
        {
            _carServiceMock = Substitute.For<ICarService>();
            _carViewModelMapperMock = Substitute.For<ICarViewModelMapper>();
            _rentalServiceMock = Substitute.For<IRentalService>();
            _rentalViewModelMapperMock = Substitute.For<IRentalViewModelMapper>();
            _messengerServiceMock = Substitute.For<IMessengerService>();
        }

        [Test]
        public void ShouldThrowExceptionIfArgumentsInConstructorAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RentCarViewModel(_carServiceMock,_rentalServiceMock,_carViewModelMapperMock,_rentalViewModelMapperMock,null));
            Assert.Throws<ArgumentNullException>(() => new RentCarViewModel(_carServiceMock,_rentalServiceMock,_carViewModelMapperMock,null,_messengerServiceMock));
            Assert.Throws<ArgumentNullException>(() => new RentCarViewModel(_carServiceMock,_rentalServiceMock,null,_rentalViewModelMapperMock,_messengerServiceMock));
            Assert.Throws<ArgumentNullException>(() => new RentCarViewModel(_carServiceMock,null,_carViewModelMapperMock,_rentalViewModelMapperMock,_messengerServiceMock));
            Assert.Throws<ArgumentNullException>(() => new RentCarViewModel(null,_rentalServiceMock,_carViewModelMapperMock,_rentalViewModelMapperMock,_messengerServiceMock));
        }

        [Test]
        public void ShouldPopulateAvailableCarList()
        {
            var carList = new List<CarDTO>()
            {
                new CarDTO()
            };
            var expectedCarViewModel = new CarViewModel();
            _carServiceMock.GetFreeCars().Returns(carList);
            _carViewModelMapperMock.Map(carList[0]).Returns(expectedCarViewModel);
            var sut = new RentCarViewModel(_carServiceMock, _rentalServiceMock, _carViewModelMapperMock,
                _rentalViewModelMapperMock, _messengerServiceMock);
            var availableCars = sut.AvailableCars;
            Assert.AreEqual(1,availableCars.Count);
            Assert.AreEqual(expectedCarViewModel,availableCars[0]);
        }

        [Test]
        public void ShouldNotPopulateAvailableCarListIfCarsAreNull()
        {
            var sut = new RentCarViewModel(_carServiceMock, _rentalServiceMock, _carViewModelMapperMock,
                _rentalViewModelMapperMock, _messengerServiceMock);
            Assert.IsEmpty(sut.AvailableCars);
        }

        [Test]
        [TestCase(DriverStatus.Active,null,true)]
        [TestCase(DriverStatus.Banned, "BANNED", false)]
        public void ShouldUpdatePropertiesAccordinglyToDriverStatusWhenAssigningLoggedInDriver(DriverStatus driverStatus, string driverBannedError, bool isCarListEnabled)
        {
            var driverViewModel = new DriverViewModel()
            {
                DriverStatus = driverStatus
            };
            var sut = new RentCarViewModel(_carServiceMock, _rentalServiceMock, _carViewModelMapperMock,
            _rentalViewModelMapperMock, _messengerServiceMock);
            sut.AssignLoggedInDriver(driverViewModel);
            Assert.AreEqual(driverBannedError,sut.DriverBannedError);
            Assert.AreEqual(sut.IsCarListEnabled,isCarListEnabled);
        }

        [Test]
        public void ShouldSendRentalViewModelMessageWithContinueRentalPropertyIfDriverHasActiveRental()
        {
            //assign
            var driverId = Guid.NewGuid();
            var driverViewModel = new DriverViewModel()
            {
                Id = driverId
            };
            var activeRentalDto = new RentalDTO();
            _rentalServiceMock.GetActiveRentalForDriver(driverId).Returns(activeRentalDto);

            var activeRentalViewModel = new RentalViewModel();
            _rentalViewModelMapperMock.Map(activeRentalDto).Returns(activeRentalViewModel);

            //act
            var sut = new RentCarViewModel(_carServiceMock, _rentalServiceMock, _carViewModelMapperMock, _rentalViewModelMapperMock, _messengerServiceMock);
            sut.AssignLoggedInDriver(driverViewModel);

            //assert
            _messengerServiceMock.Received().Send(Arg.Is<RentalViewModelMessage>(message =>message.RentalViewModel == activeRentalViewModel && message.MessageType == RentalViewModelMessageType.ContinueRental ));
            _messengerServiceMock.Received().Send(Arg.Is<NotificationMessage>(message =>message.Notification == "Start Car Rental"));
        }

        [Test]
        public void ShouldNotSendRentalViewModelMessageWithContinueRentalPropertyIfDriverDoesntHaveActiveRental()
        {
            var driverId = Guid.NewGuid();
            var driverViewModel = new DriverViewModel
            {
                Id = driverId
            };
            var sut = new RentCarViewModel(_carServiceMock, _rentalServiceMock, _carViewModelMapperMock,
                _rentalViewModelMapperMock, _messengerServiceMock);
            sut.AssignLoggedInDriver(driverViewModel);
            _messengerServiceMock.DidNotReceive().Send(Arg.Any<RentalViewModelMessage>());
            _messengerServiceMock.DidNotReceive().Send(Arg.Is<NotificationMessage>(message => message.Notification == "Start Car Rental"));
        }

        [Test]
        public void ShouldNotRentCarIfRequirementsAreNotFulfilled()
        {
            var driverId = Guid.NewGuid();
            var driverViewModel = new DriverViewModel
            {
                Id = driverId,
                DriverStatus = DriverStatus.Banned
            };
            var sut = new RentCarViewModel(_carServiceMock, _rentalServiceMock, _carViewModelMapperMock,
                _rentalViewModelMapperMock, _messengerServiceMock);
            sut.CurrentDriver = driverViewModel;
            sut.SelectedCar = null;
            sut.RentCarCommand.Execute(null);
            _rentalServiceMock.DidNotReceive().TakeCar(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<DateTime>());
        }

        [Test]
        public void ShouldRentCarIfRequirementsAreFulfilled()
        {
            var driverId = Guid.NewGuid();
            var driverViewModel = new DriverViewModel
            {
                Id = driverId,
                DriverStatus = DriverStatus.Active
            };
            var carId = Guid.NewGuid();
            var car= new CarViewModel()
            {
                Id = carId
            };
            var sut = new RentCarViewModel(_carServiceMock, _rentalServiceMock, _carViewModelMapperMock,
                _rentalViewModelMapperMock, _messengerServiceMock) {CurrentDriver = driverViewModel, SelectedCar = car};
            sut.RentCarCommand.Execute(null);

            _rentalServiceMock.Received().TakeCar(Arg.Any<Guid>(), carId, driverId, Arg.Any<DateTime>());
            _rentalViewModelMapperMock.Received().Map(Arg.Any<RentalDTO>());
            _messengerServiceMock.Received().Send(Arg.Is<RentalViewModelMessage>(message => message.MessageType == RentalViewModelMessageType.StartRental));
            _messengerServiceMock.Received().Send(Arg.Is<NotificationMessage>(message => message.Notification == "Start Car Rental"));
            Assert.Null(sut.ErrorString);
        }

        [Test]
        public void ShouldSetErrorStringIfFailedToRentCar()
        {
            var driverId = Guid.NewGuid();
            var driverViewModel = new DriverViewModel
            {
                Id = driverId,
                DriverStatus = DriverStatus.Active
            };
            var carId = Guid.NewGuid();
            var car = new CarViewModel()
            {
                Id = carId
            };
            _rentalServiceMock.When(x=>x.TakeCar(Arg.Any<Guid>(), carId, driverId, Arg.Any<DateTime>())).Do(x=> throw new Exception());
            var sut = new RentCarViewModel(_carServiceMock, _rentalServiceMock, _carViewModelMapperMock,
                    _rentalViewModelMapperMock, _messengerServiceMock)
                { CurrentDriver = driverViewModel, SelectedCar = car };
            sut.RentCarCommand.Execute(null);
            Assert.AreEqual("Could not rent car.",sut.ErrorString);
        }
    }
}
