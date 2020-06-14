using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.UI.Mappers;
using CarRental.UI.ViewModels.AdminViewModels;
using CarRental.UI.ViewModels.ObservableObjects;
using CarRental.UI.Views.AdminViews;
using NSubstitute;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.AdminViewModelsTests
{
    public class DriversManagementViewModelTests
    {
        private  IDriverService _driverServiceMock;
        private  IDriverViewModelMapper _driverViewModelMapperMock;

        [SetUp]
        public void Setup()
        {
            _driverServiceMock = Substitute.For<IDriverService>();
            _driverViewModelMapperMock = Substitute.For<IDriverViewModelMapper>();
        }

        [Test]
        public void ShouldThrowExceptionIfPassedNullArgumentsInConstructor()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new DriversManagementViewModel(null, _driverViewModelMapperMock));
            Assert.Throws<ArgumentNullException>(() =>
                new DriversManagementViewModel(_driverServiceMock, null));
        }

        [Test]
        public void ShouldNotAddDriversIfServiceReturnedNull()
        {
            var sut = new DriversManagementViewModel(_driverServiceMock, _driverViewModelMapperMock);
            Assert.IsEmpty(sut.DriversCollection);
        }

        [Test]
        public void ShouldAddDriversToList()
        {
            var driverDtoList = new List<DriverDTO>()
            {
                new DriverDTO()
            };
            var driverViewModel = new DriverViewModel();
            _driverServiceMock.GetAllDrivers().Returns(driverDtoList);
            _driverViewModelMapperMock.Map(driverDtoList[0]).Returns(driverViewModel);
            var sut = new DriversManagementViewModel(_driverServiceMock, _driverViewModelMapperMock);
            Assert.AreEqual(1,sut.DriversCollection.Count);
            Assert.AreEqual(driverViewModel, sut.DriversCollection.First());
        }

        [Test]
        public void ShouldNotExecuteSaveDriverIfDriverIsNotValid()
        {
            var sut = new DriversManagementViewModel(_driverServiceMock, _driverViewModelMapperMock);
            sut.SaveDriverCommand.Execute(null);
            _driverViewModelMapperMock.DidNotReceive().Map(Arg.Any<DriverViewModel>());
        }

        [Test]
        public void ShouldExecuteSaveDriverIfDriverIsValid()
        {
            _driverViewModelMapperMock.Map(Arg.Any<DriverViewModel>()).Returns(new DriverDTO());
            var sut = new DriversManagementViewModel(_driverServiceMock, _driverViewModelMapperMock);
            sut.SelectedDriver = new DriverViewModel()
            {
                FirstName = "Valid",
                LastName = "Valid",
                LicenseNumber = "Valid"
            };
            sut.SaveDriverCommand.Execute(null);
            _driverViewModelMapperMock.Received().Map(Arg.Any<DriverViewModel>());
            _driverServiceMock.Received().UpdateDriver(Arg.Any<DriverDTO>());
        }

        [Test]
        public void ShouldSetErrorContentIfFailedToSaveDriver()
        {
            _driverViewModelMapperMock.When(d=>d.Map(Arg.Any<DriverViewModel>())).Do(d=>throw new Exception());
            var sut = new DriversManagementViewModel(_driverServiceMock, _driverViewModelMapperMock);
            sut.SelectedDriver = new DriverViewModel()
            {
                FirstName = "Valid",
                LastName = "Valid",
                LicenseNumber = "Valid"
            };
            sut.SaveDriverCommand.Execute(null);
            Assert.NotNull(sut.SaveErrorContent);
        }
    }
}
