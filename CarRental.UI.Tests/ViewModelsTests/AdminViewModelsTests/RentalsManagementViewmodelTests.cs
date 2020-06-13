using System;
using System.Collections.Generic;
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
    public class RentalsManagementViewmodelTests
    {
        private IRentalService _rentalServiceMock;
        private IRentalViewModelMapper _rentalViewModelMapperMock;

        [SetUp]
        public void Setup()
        {
            _rentalServiceMock = Substitute.For<IRentalService>();
            _rentalViewModelMapperMock = Substitute.For<IRentalViewModelMapper>();
        }

        [Test]
        public void ShouldThrowExceptionIfPassedNullArgumentsInConstructor()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new RentalsManagementViewModel(null, _rentalViewModelMapperMock));
            Assert.Throws<ArgumentNullException>(() => new RentalsManagementViewModel(_rentalServiceMock, null));
        }

        [Test]
        public void ShouldNotAddRentalsIfServiceReturnedNull()
        {
            var sut = new RentalsManagementViewModel(_rentalServiceMock, _rentalViewModelMapperMock);
            Assert.IsEmpty(sut.RentalsCollection);
            Assert.IsEmpty(sut.RentalsObservableCollection);
        }

        [Test]
        public void ShouldAddRentalsUponInitialization()
        {
            var rentalsDto = new List<RentalDTO>
            {
                new RentalDTO()
            };
            var rentalViewModel = new RentalViewModel();
            _rentalServiceMock.GetAllRentals().Returns(rentalsDto);
            _rentalViewModelMapperMock.Map(rentalsDto[0]).Returns(rentalViewModel);
            var sut = new RentalsManagementViewModel(_rentalServiceMock, _rentalViewModelMapperMock);
            Assert.AreEqual(1, sut.RentalsObservableCollection.Count);
            Assert.AreEqual(sut.RentalsObservableCollection, sut.RentalsCollection.SourceCollection);
            Assert.AreEqual(rentalViewModel, sut.RentalsObservableCollection.First());
        }

        [Test]
        public void ShouldFilterActiveRentals()
        {
            var rentalsDto = new List<RentalDTO>
            {
                new RentalDTO(),
                new RentalDTO()
            };
            var rentalViewModelList = new List<RentalViewModel>()
            {
                new RentalViewModel()
                {
                    StopDateTime = new DateTime()
                },
                new RentalViewModel()
        };
            _rentalServiceMock.GetAllRentals().Returns(rentalsDto);
            _rentalViewModelMapperMock.Map(rentalsDto[0]).Returns(rentalViewModelList[0]);
            _rentalViewModelMapperMock.Map(rentalsDto[1]).Returns(rentalViewModelList[1]);
            var sut = new RentalsManagementViewModel(_rentalServiceMock, _rentalViewModelMapperMock);
            sut.IsActiveRentalsSelected = true;
            Assert.True(sut.RentalsCollection.Contains(rentalViewModelList[1]));
            Assert.False(sut.RentalsCollection.Contains(rentalViewModelList[0]));
        }
    }
}