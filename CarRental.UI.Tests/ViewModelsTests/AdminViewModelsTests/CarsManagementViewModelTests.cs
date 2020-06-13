using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CarRental.UI.Mappers;
using CarRental.UI.ViewModels.AdminViewModels;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.AdminViewModelsTests
{
    public class CarsManagementViewModelTests
    {
        private ICarService _carServiceMock;
        private ICarViewModelMapper _carViewModelMapperMock;
        private IRentalAreaService _rentalAreaServiceMock;
        private IRentalAreaViewModelMapper _rentalAreaViewModelMapperMock;

        [SetUp]
        public void Setup()
        {
            _carServiceMock = Substitute.For<ICarService>();
            _carViewModelMapperMock = Substitute.For<ICarViewModelMapper>();
            _rentalAreaServiceMock = Substitute.For<IRentalAreaService>();
            _rentalAreaViewModelMapperMock = Substitute.For<IRentalAreaViewModelMapper>();
        }

        [Test]
        public void ShouldThrowExceptionIfPassedNullArgumentsInConstructor()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CarsManagementViewModel(_carServiceMock, _rentalAreaServiceMock, _carViewModelMapperMock, null));
            Assert.Throws<ArgumentNullException>(() =>
                new CarsManagementViewModel(_carServiceMock, _rentalAreaServiceMock, null,
                    _rentalAreaViewModelMapperMock));
            Assert.Throws<ArgumentNullException>(() =>
                new CarsManagementViewModel(_carServiceMock, null, _carViewModelMapperMock,
                    _rentalAreaViewModelMapperMock));
            Assert.Throws<ArgumentNullException>(() => new CarsManagementViewModel(null, _rentalAreaServiceMock,
                _carViewModelMapperMock, _rentalAreaViewModelMapperMock));
        }

        [Test]
        public void ShouldNotPopulateCarListIfReturnedNull()
        {
            var sut = new CarsManagementViewModel(_carServiceMock, _rentalAreaServiceMock, _carViewModelMapperMock,
                _rentalAreaViewModelMapperMock);
            Assert.IsEmpty(sut.CarsCollection);
        }

        [Test]
        public void ShouldNotPopulateRentalAreaComboBoxListIfReturnedNull()
        {
            var sut = new CarsManagementViewModel(_carServiceMock, _rentalAreaServiceMock, _carViewModelMapperMock,
                _rentalAreaViewModelMapperMock);
            Assert.IsEmpty(sut.RentalAreas);
        }

        [Test]
        public void ShouldAddCarsToList()
        {
            var carListDto = new List<CarDTO>
            {
                new CarDTO()
            };
            var carViewModel = new CarViewModel();
            _carServiceMock.GetAllCars().Returns(carListDto);
            _carViewModelMapperMock.Map(carListDto[0]).Returns(carViewModel);
            var sut = new CarsManagementViewModel(_carServiceMock, _rentalAreaServiceMock, _carViewModelMapperMock,
                _rentalAreaViewModelMapperMock);
            Assert.AreEqual(1, sut.CarsCollection.Count);
            Assert.AreEqual(carViewModel, sut.CarsCollection.First());
        }

        [Test]
        public void ShouldAddRentalAreasToComboBoxList()
        {
            var rentalAreaListDto = new List<RentalAreaDTO>
            {
                new RentalAreaDTO()
            };
            var rentalAreaViewModel = new RentalAreaViewModel();
            _rentalAreaServiceMock.GetAllRentalAreas().Returns(rentalAreaListDto);
            _rentalAreaViewModelMapperMock.Map(rentalAreaListDto[0]).Returns(rentalAreaViewModel);
            var sut = new CarsManagementViewModel(_carServiceMock, _rentalAreaServiceMock, _carViewModelMapperMock,
                _rentalAreaViewModelMapperMock);
            Assert.AreEqual(1, sut.RentalAreas.Count);
            Assert.AreEqual(rentalAreaViewModel, sut.RentalAreas.First());
        }

        [Test]
        public void ShouldExecuteUpdateRentalAreaCommand()
        {
            var sut = new CarsManagementViewModel(_carServiceMock, _rentalAreaServiceMock, _carViewModelMapperMock,
                _rentalAreaViewModelMapperMock);
            sut.SelectedCar = new CarViewModel
            {
                RentalAreaId = Guid.NewGuid()
            };
            var rentalAreaViewModel = new RentalAreaViewModel
            {
                Id = sut.SelectedCar.RentalAreaId
            };
            sut.RentalAreas = new ObservableCollection<RentalAreaViewModel>
            {
                rentalAreaViewModel
            };

            sut.UpdateRentalAreaCombobox.Execute(null);
            Assert.AreEqual(rentalAreaViewModel, sut.SelectedRentalArea);
        }

        [Test]
        public void ShouldExecuteAddNewCarCommand()
        {
            var sut = new CarsManagementViewModel(_carServiceMock, _rentalAreaServiceMock, _carViewModelMapperMock,
                _rentalAreaViewModelMapperMock);
            sut.AddNewCarCommand.Execute(null);
            Assert.NotNull(sut.SelectedCar);
            Assert.False(sut.IsCarListEnabled);
        }

        [Test]
        public void ShouldNotExecuteSaveCarCommandIfCarIsNotValid()
        {
            var sut = new CarsManagementViewModel(_carServiceMock, _rentalAreaServiceMock, _carViewModelMapperMock,
                _rentalAreaViewModelMapperMock);
            sut.SaveCarCommand.Execute(null);
            _carServiceMock.DidNotReceive().CreateCar(Arg.Any<CarDTO>());
            _carServiceMock.DidNotReceive().UpdateCar(Arg.Any<CarDTO>());
        }

        [Test]
        public void ShouldSaveNewCarIfCarIsValid()
        {
            var sutSelectedCar = new CarViewModel
            {
                RentalAreaId = Guid.NewGuid(),
                Id = Guid.Empty,
                PricePerMinute = "2.6",
                RegistrationNumber = "Kr1234",
                TotalDistance = "20"
            };
            var sutSelectedRentalArea = new RentalAreaViewModel
            {
                Id = sutSelectedCar.RentalAreaId
            };
            _carViewModelMapperMock.Map(sutSelectedRentalArea, sutSelectedCar).Returns(new CarDTO());
            var sut = new CarsManagementViewModel(_carServiceMock, _rentalAreaServiceMock, _carViewModelMapperMock,
                _rentalAreaViewModelMapperMock);

            sut.SelectedCar = sutSelectedCar;
            sut.SelectedRentalArea = sutSelectedRentalArea;
            sut.SaveCarCommand.Execute(null);

            _carServiceMock.Received().CreateCar(Arg.Any<CarDTO>());
            Assert.True(sut.IsCarListEnabled);
            Assert.Null(sut.SaveErrorContent);
        }

        [Test]
        public void ShouldEditCarIfCarIsValid()
        {
            var sutSelectedCar = new CarViewModel
            {
                RentalAreaId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                PricePerMinute = "2.6",
                RegistrationNumber = "Kr1234",
                TotalDistance = "20"
            };
            var sutSelectedRentalArea = new RentalAreaViewModel
            {
                Id = sutSelectedCar.RentalAreaId
            };
            _carViewModelMapperMock.Map(sutSelectedRentalArea, sutSelectedCar).Returns(new CarDTO());
            var sut = new CarsManagementViewModel(_carServiceMock, _rentalAreaServiceMock, _carViewModelMapperMock,
                _rentalAreaViewModelMapperMock);

            sut.SelectedCar = sutSelectedCar;
            sut.SelectedRentalArea = sutSelectedRentalArea;
            sut.SaveCarCommand.Execute(null);

            _carServiceMock.Received().UpdateCar(Arg.Any<CarDTO>());
            Assert.True(sut.IsCarListEnabled);
            Assert.Null(sut.SaveErrorContent);
        }

        [Test]
        public void ShouldSetErrorMessageIfFailedToSaveCar()
        {
            var sutSelectedCar = new CarViewModel
            {
                RentalAreaId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                PricePerMinute = "2.6",
                RegistrationNumber = "Kr1234",
                TotalDistance = "20"
            };
            var sutSelectedRentalArea = new RentalAreaViewModel
            {
                Id = sutSelectedCar.RentalAreaId
            };
            _carViewModelMapperMock.Map(sutSelectedRentalArea, sutSelectedCar).Returns(new CarDTO());
            var message = "Error Message";
            _carServiceMock.When(c => c.UpdateCar(Arg.Any<CarDTO>())).Do(c => throw new Exception(message));
            var sut = new CarsManagementViewModel(_carServiceMock, _rentalAreaServiceMock, _carViewModelMapperMock,
                _rentalAreaViewModelMapperMock);

            sut.SelectedCar = sutSelectedCar;
            sut.SelectedRentalArea = sutSelectedRentalArea;
            sut.SaveCarCommand.Execute(null);

            Assert.AreEqual(message, sut.SaveErrorContent);
        }
    }
}