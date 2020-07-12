using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.Model.ApplicationLayer.Mappers;
using CarRental.Model.DomainModelLayer.Factories;
using CarRental.Model.DomainModelLayer.Interfaces;
using CarRental.Model.DomainModelLayer.Services;
using CarRental.Model.InfrastructureLayer;
using CarRental.UI.Mappers;
using CarRental.UI.Messages;
using CarRental.UI.Services;
using CarRental.UI.Utils.Interfaces;
using CarRental.UI.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests
{
    public class ViewModelLocatorTests
    {
        [Test]
        public void ShouldRegisterServices()
        {
            var sut = new ViewModelLocator();
            Assert.NotNull(SimpleIoc.Default.GetInstance<ICarViewModelMapper>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<IDriverViewModelMapper>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<IRentalViewModelMapper>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<IRentalAreaViewModelMapper>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<IPositionVewModelMapper>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<ITimerFactory>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<ICarRentalUnitOfWork>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<PositionService>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<DriverFactory>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<CarFactory>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<RentalAreaFactory>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<RentalFactory>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<CarMapper>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<DriverMapper>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<RentalAreaMapper>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<RentalMapper>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<IDriverService>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<ICarService>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<IRentalService>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<IRentalAreaService>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<IDialogService>());
            Assert.NotNull(SimpleIoc.Default.GetInstance<IMessengerService>());
        }

        [Test]
        public void ShouldRegisterBaseViewModels()
        {
            var sut = new ViewModelLocator();
            Assert.NotNull(sut.LoginViewModel);
            Assert.NotNull(sut.RegisterDriverViewModel);
           
        }

        [Test]
        public void ShouldRegisterDriverViewModels()
        {
            var sut = new ViewModelLocator();
            ViewModelLocator.RegisterDriverViewModels();
            Assert.NotNull(sut.DriverRentalsViewModel);
            Assert.NotNull(sut.DriverMainViewModel);
            Assert.NotNull(sut.RentCarViewModel);
            Assert.NotNull(sut.ActiveRentalSessionViewModel);
            Assert.NotNull(sut.DriverAccountViewModel);
        }

        [Test]
        public void ShouldRegisterAdminViewModels()
        {
            var sut = new ViewModelLocator();
            ViewModelLocator.RegisterAdminViewModels();
            Assert.NotNull(sut.AdminMainViewModel);
            Assert.NotNull(sut.CarsManagementViewModel);
            Assert.NotNull(sut.DriversManagementViewModel);
            Assert.NotNull(sut.RentalsManagementViewModel);
            Assert.NotNull(sut.RentalAreaManagementViewModel);
        }

        [Test]
        public void ShouldCleanup()
        {
            bool wasCalled= false;
            Messenger.Default.Register<CleanupMessage>(this, message => wasCalled =true);
            var sut = new ViewModelLocator();
            ViewModelLocator.Cleanup();
            Assert.True(wasCalled);
        }


        [TearDown]
        public void TearDown()
        {
            SimpleIoc.Default.Reset();
        }
    }
}
