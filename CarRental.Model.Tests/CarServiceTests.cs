using System;
using System.Linq;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.DomainModelLayer.Models;
using CarRental.Model.Tests.TestHelpers;
using NUnit.Framework;

namespace CarRental.Model.Tests
{
    //End to end CarService tests
    public class CarServiceTests
    {
        private TestContainer TestContainer { get; set; }
        [SetUp]
        public void Setup()
        {
            TestContainer = new TestContainer();
        }
        
        [Test]
        public void ShouldProperlyCreateAndReturnCarWithParameters()
        {
            //arrange
            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);
            var car = new CarDTO()
            {
                Id = new Guid(),
                CurrentDistance = 15,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147,
                PricePerMinute = 2.5m,
                RegistrationNumber = "KR12345",
                TotalDistance = 20320,
                RentalAreaId = rentalArea.Id
            };

            var expected = car;

            //act
            TestContainer.CarService.CreateCar(car);
            var actual = TestContainer.CarService.GetAllCars().First();

            //assert
            Assert.AreEqual(expected.Id,actual.Id);
            Assert.AreEqual(expected.PricePerMinute,actual.PricePerMinute);
            Assert.AreEqual(expected.RegistrationNumber,actual.RegistrationNumber);
            Assert.AreEqual(expected.CurrentLatitude,actual.CurrentLatitude);
            Assert.AreEqual(expected.CurrentLongitude,actual.CurrentLongitude);
            Assert.AreEqual(expected.CurrentDistance,actual.CurrentDistance);
            Assert.AreEqual(expected.TotalDistance,actual.TotalDistance);
            Assert.AreEqual(CarStatus.Free,actual.Status);
            Assert.AreEqual(expected.RentalAreaId,actual.RentalAreaId);
        }

        [Test]
        public void ShouldThrowExceptionIfAddExistingCar()
        { 
            //assign
            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);
            var car = new CarDTO()
            {
                Id = new Guid(),
                RegistrationNumber = "KR12345",
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147,
                RentalAreaId = rentalArea.Id,
            };
            //act
            TestContainer.CarService.CreateCar(car);
            //assert
            Assert.Throws<Exception>((() => TestContainer.CarService.CreateCar(car)));
            
        }

        [Test]
        public void ShouldThrowExceptionIfAddCarWithNoRegistrationNumber()
        {
            //assign
            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);
            var car = new CarDTO()
            {
                Id = new Guid(),
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147,
            };
            //act
            //assert
            Assert.Throws<Exception>((() => TestContainer.CarService.CreateCar(car)));
        }
        [Test]
        public void ShouldThrowExceptionIfAddCarWithPositionOutOfAreaOfRentalBonds()
        {
            //assign
            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);
            var car = new CarDTO()
            {
                Id = new Guid(),
                RegistrationNumber = "KR12345",
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.457236,
                CurrentLongitude = 19.045147,
            };
            //act
            //assert
            Assert.Throws<Exception>((() => TestContainer.CarService.CreateCar(car)));
        }

        [Test]
        public void ShouldThrowExceptionIfAddExistingCarWithoutProperRentalArea()
        {
            //assign
            var car = new CarDTO()
            {
                Id = new Guid(),
                RegistrationNumber = "KR12345",
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147,
            };
            //act
            //assert
            Assert.Throws<Exception>((() => TestContainer.CarService.CreateCar(car)));
        }

        [Test]
        public void ShouldProperlyReparkCar()
        {
            //arrange
            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);
            var car = new CarDTO()
            {
                Id = new Guid(),
                CurrentDistance = 15,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147,
                PricePerMinute = 2.5m,
                RegistrationNumber = "KR12345",
                TotalDistance = 20320,
                RentalAreaId = rentalArea.Id
            };


            //act
            var afterParkLatitude = 50.057230;
            var afterParkLongitude = 19.945140;

            TestContainer.CarService.CreateCar(car);
            TestContainer.CarService.ReparkCar(car.Id, new PositionDTO()
            {
                Latitude = afterParkLatitude,
                Longitude = afterParkLongitude
            });
            var actual = TestContainer.CarService.GetAllCars().First();

            //assert
            Assert.AreEqual(afterParkLatitude, actual.CurrentLatitude);
            Assert.AreEqual(afterParkLongitude, actual.CurrentLongitude);
        }

        [Test]
        public void ShouldThrowExceptionIfReparkCarOutOfBonds()
        {
            //arrange
            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);
            var car = new CarDTO()
            {
                Id = new Guid(),
                CurrentDistance = 15,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147,
                PricePerMinute = 2.5m,
                RegistrationNumber = "KR12345",
                TotalDistance = 20320,
                RentalAreaId = rentalArea.Id
            };


            //act
            var afterParkLatitude = 40;
            var afterParkLongitude = 10;

            TestContainer.CarService.CreateCar(car);
            Assert.Throws<Exception>((() => TestContainer.CarService.ReparkCar(car.Id, new PositionDTO()
            {
                Latitude = afterParkLatitude,
                Longitude = afterParkLongitude
            })));


        }
    }
}