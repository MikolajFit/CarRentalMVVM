using System;
using CarRental.UI.Mappers;
using CarRental.UI.ViewModels.DriverViewModels;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.DriverViewModelsTests
{
    public class DriverAccountViewModelTests
    {
        private IDriverService _driverServiceMock;
        private IDriverViewModelMapper _driverViewModelMapperMock;

        [SetUp]
        public void Setup()
        {
            _driverServiceMock = Substitute.For<IDriverService>();
            _driverViewModelMapperMock = Substitute.For<IDriverViewModelMapper>();
        }

        [Test]
        public void ShouldThrowExceptionIfArgumentsInConstructorAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new DriverAccountViewModel(null, _driverViewModelMapperMock));
            Assert.Throws<ArgumentNullException>(() => new DriverAccountViewModel(_driverServiceMock, null));
        }

        [Test]
        public void ShouldChangeToEditMode()
        {
            var sut = new DriverAccountViewModel(_driverServiceMock, _driverViewModelMapperMock) {IsEditable = false};
            sut.ChangeToEditModeCommand.Execute(null);
            Assert.True(sut.IsEditable);
        }

        [Test]
        public void ShouldNotSaveChangesIfCriteriaAreNotFulfilled()
        {
            var sut = new DriverAccountViewModel(_driverServiceMock, _driverViewModelMapperMock) {IsEditable = false};
            sut.SaveChangesCommand.Execute(null);
            _driverViewModelMapperMock.DidNotReceive().Map(sut.CurrentDriver);
            _driverServiceMock.DidNotReceive().UpdateDriver(Arg.Any<DriverDTO>());
        }

        [Test]
        public void ShouldUpdateDriverIfSaveChangesCommandExecutedSuccessfully()
        {
            var sut = new DriverAccountViewModel(_driverServiceMock, _driverViewModelMapperMock)
            {
                IsEditable = true,
                CurrentDriver = new DriverViewModel { FirstName = "Valid", LastName = "Valid", LicenseNumber = "Valid" }
            };
            var driverDto = new DriverDTO();
            _driverViewModelMapperMock.Map(sut.CurrentDriver).Returns(driverDto);
            sut.SaveChangesCommand.Execute(null);
            _driverViewModelMapperMock.Received().Map(sut.CurrentDriver);
            _driverServiceMock.Received().UpdateDriver(driverDto);
            Assert.False(sut.IsEditable);
            Assert.Null(sut.ErrorString);
        }

        [Test]
        public void ShouldCreateErrorMessageIfFailedToSaveChanges()
        {
            var sut = new DriverAccountViewModel(_driverServiceMock, _driverViewModelMapperMock)
            {
                IsEditable = true,
                CurrentDriver = new DriverViewModel {FirstName = "Valid", LastName = "Valid", LicenseNumber = "Valid"}
            };
            _driverViewModelMapperMock.Map(sut.CurrentDriver).Throws(new Exception());
            sut.SaveChangesCommand.Execute(null);
            _driverServiceMock.DidNotReceive().UpdateDriver(Arg.Any<DriverDTO>());
            Assert.AreEqual("Could not save changes.", sut.ErrorString);
        }
    }
}