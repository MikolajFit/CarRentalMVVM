using System;
using System.Linq;
using DDD.CarRental.Tests.TestHelpers;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.DomainModelLayer.Interfaces;
using DDD.CarRentalLib.DomainModelLayer.Models;
using DDD.CarRentalLib.DomainModelLayer.Services;
using NUnit.Framework;

namespace DDD.CarRental.Tests
{
    //End to end RentalService tests
    public class RentalServiceTests
    {
        private TestContainer TestContainer { get; set; }

        [SetUp]
        public void Setup()
        {
            TestContainer = new TestContainer();
        }

        [Test]
        public void ShouldProperlyCreateAndReturnRentalWithParametersWhenTakingCar()
        {
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Mikołaj",
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };
            TestContainer.DriverService.CreateDriver(driver);

            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);

            var car = new CarDTO
            {
                Id = new Guid(),
                RegistrationNumber = "KR12345",
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147
            };
            TestContainer.CarService.CreateCar(car);


            var rentalId = new Guid();
            var startTime = new DateTime(2020, 04, 19);

            TestContainer.RentalService.TakeCar(rentalId, car.Id, driver.Id, startTime);
            var actual = TestContainer.RentalService.GetAllRentals().First();


            Assert.AreEqual(rentalId, actual.Id);
            Assert.AreEqual(car.RegistrationNumber, actual.RegistrationNumber);
            Assert.AreEqual(car.Id, actual.CarId);
            Assert.AreEqual(driver.Id, actual.DriverId);
            Assert.AreEqual(driver.FirstName + " " + driver.LastName, actual.DriverName);
            Assert.AreEqual(startTime, actual.StartDateTime);
            Assert.AreEqual(0, actual.Total);
            Assert.IsNull(actual.StopDateTime);
        }

        [Test]
        public void ShouldChangeCarStatusUponRenting()
        {
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Mikołaj",
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };
            TestContainer.DriverService.CreateDriver(driver);

            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);

            var car = new CarDTO
            {
                Id = new Guid(),
                RegistrationNumber = "KR12345",
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147
            };
            TestContainer.CarService.CreateCar(car);


            var rentalId = new Guid();
            var startTime = new DateTime(2020, 04, 19);

            TestContainer.RentalService.TakeCar(rentalId, car.Id, driver.Id, startTime);
            var actual = TestContainer.CarService.GetAllCars().First();
            Assert.AreEqual(CarStatus.Taken, actual.Status);
        }


        [Test]
        public void ShouldThrowExceptionWhenRentingNotExistingCar()
        {
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Mikołaj",
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };
            TestContainer.DriverService.CreateDriver(driver);

            var rentalId = new Guid();
            var startTime = new DateTime(2020, 04, 19);


            Assert.Throws<Exception>(() =>
                TestContainer.RentalService.TakeCar(rentalId, new Guid(), driver.Id, startTime));
        }

        [Test]
        public void ShouldThrowExceptionWhenRentingAlreadyRentedCar()
        {
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Mikołaj",
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };
            TestContainer.DriverService.CreateDriver(driver);

            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);

            var car = new CarDTO
            {
                Id = new Guid(),
                RegistrationNumber = "KR12345",
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147
            };
            TestContainer.CarService.CreateCar(car);

            var rentalId = new Guid();
            var startTime = new DateTime(2020, 04, 19);

            TestContainer.RentalService.TakeCar(rentalId, car.Id, driver.Id, startTime);

            Assert.Throws<Exception>(() => TestContainer.RentalService.TakeCar(rentalId, car.Id, driver.Id, startTime));
        }

        [Test]
        public void ShouldThrowExceptionWhenRentingWithNotExistingDriver()
        {
            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);
            var car = new CarDTO
            {
                Id = new Guid(),
                RegistrationNumber = "KR12345",
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147
            };
            TestContainer.CarService.CreateCar(car);

            var rentalId = new Guid();
            var startTime = new DateTime(2020, 04, 19);


            Assert.Throws<Exception>(() =>
                TestContainer.RentalService.TakeCar(rentalId, car.Id, new Guid(), startTime));
        }

        [Test]
        public void ShouldThrowExceptionWhenCannotFindRentalUponReturningCar()
        {
            Assert.Throws<Exception>(() => TestContainer.RentalService.ReturnCar(new Guid(), new DateTime()));
        }

        [Test]
        public void ShouldProperlyUpdateRentalStopDateWhenReturningCar()
        {
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Mikołaj",
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };
            TestContainer.DriverService.CreateDriver(driver);

            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);

            var car = new CarDTO
            {
                Id = new Guid(),
                RegistrationNumber = "KR12345",
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147
            };
            TestContainer.CarService.CreateCar(car);


            var rentalId = new Guid();
            var startTime = new DateTime(2020, 04, 19);

            TestContainer.RentalService.TakeCar(rentalId, car.Id, driver.Id, startTime);
            var stopTime = new DateTime(2020, 04, 20);
            TestContainer.RentalService.ReturnCar(rentalId, stopTime);
            var actual = TestContainer.RentalService.GetAllRentals().First();
            Assert.AreEqual(stopTime, actual.StopDateTime);
        }

        [Test]
        public void ShouldThrowExceptionWhenReturningCarWithEarlierStopTimeThanStartTime()
        {
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Mikołaj",
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };
            TestContainer.DriverService.CreateDriver(driver);

            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);

            var car = new CarDTO
            {
                Id = new Guid(),
                RegistrationNumber = "KR12345",
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147
            };
            TestContainer.CarService.CreateCar(car);

            var rentalId = new Guid();
            var startTime = new DateTime(2020, 04, 19);

            TestContainer.RentalService.TakeCar(rentalId, car.Id, driver.Id, startTime);
            var stopTime = new DateTime(2020, 04, 18);
            Assert.Throws<Exception>(() => TestContainer.RentalService.ReturnCar(rentalId, stopTime));
        }

        [Test]
        [TestCase(2520, "Mezczyzn", 50.044958, 19.937624)]
        [TestCase(2522.5, "Mezczyzn", 50.098694, 19.857368)]
        [TestCase(3240, "Kobieta", 50.044958, 19.937624)]
        [TestCase(3242.5, "Kobieta", 50.098694, 19.857368)]
        public void ShouldProperlyCalculateTotalWhenReturningCar(decimal expectedTotal, string driverName,
            double afterRentalLatitude, double afterRentalLongitude)
        {
            TestContainer.ChangePositionService(new StubPositionService(TestContainer.UnitOfWork,
                new Position(afterRentalLatitude, afterRentalLongitude)));
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = driverName,
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };
            TestContainer.DriverService.CreateDriver(driver);

            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);

            var car = new CarDTO
            {
                Id = new Guid(),
                RegistrationNumber = "KR12345",
                PricePerMinute = 2.5m,
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147
            };
            TestContainer.CarService.CreateCar(car);


            var rentalId = new Guid();
            var startTime = new DateTime(2020, 04, 19);

            TestContainer.RentalService.TakeCar(rentalId, car.Id, driver.Id, startTime);
            var stopTime = new DateTime(2020, 04, 20);
            TestContainer.RentalService.ReturnCar(rentalId, stopTime);
            var actual = TestContainer.RentalService.GetAllRentals().First();
            Assert.AreEqual(expectedTotal, actual.Total);
        }

        [Test]
        public void ShouldThrowExceptionIfTryingToRentCarOutsideOfAreaOfRentalBonds()
        {
            TestContainer.ChangePositionService(new StubPositionService(TestContainer.UnitOfWork,
                new Position(50.957236, 19.045147)));
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Mikolaj",
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };
            TestContainer.DriverService.CreateDriver(driver);

            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);

            var car = new CarDTO
            {
                Id = new Guid(),
                RegistrationNumber = "KR12345",
                PricePerMinute = 2.5m,
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147
            };
            TestContainer.CarService.CreateCar(car);

            var rentalId = new Guid();
            var startTime = new DateTime(2020, 04, 19);

            TestContainer.RentalService.TakeCar(rentalId, car.Id, driver.Id, startTime);
            var stopTime = new DateTime(2020, 04, 20);
            TestContainer.RentalService.ReturnCar(rentalId, stopTime);
            rentalId = new Guid();
            startTime = new DateTime(2020, 04, 21);
            Assert.Throws<Exception>(() => TestContainer.RentalService.TakeCar(rentalId, car.Id, driver.Id, startTime));
        }


        [Test]
        public void ShouldChangeCarPositionWhenReturningCar()
        {
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Mikołaj",
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };
            TestContainer.DriverService.CreateDriver(driver);
            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);
            var startingLatitude = 50.057236;
            var startingLongitude = 19.945147;
            var startingCurrentDistance = 100;
            var startingTotalDistance = 20000;
            var car = new CarDTO
            {
                Id = new Guid(),
                RegistrationNumber = "KR12345",
                CurrentLatitude = startingLatitude,
                CurrentLongitude = startingLongitude,
                CurrentDistance = startingCurrentDistance,
                TotalDistance = startingTotalDistance,
                RentalAreaId = rentalArea.Id
            };
            TestContainer.CarService.CreateCar(car);

            var rentalId = new Guid();
            var startTime = new DateTime(2020, 04, 19);

            TestContainer.RentalService.TakeCar(rentalId, car.Id, driver.Id, startTime);
            var stopTime = new DateTime(2020, 04, 20);
            TestContainer.RentalService.ReturnCar(rentalId, stopTime);
            var actual = TestContainer.CarService.GetAllCars().First();
            Assert.AreNotEqual(startingLatitude, actual.CurrentLatitude);
            Assert.AreNotEqual(startingLongitude, actual.CurrentLongitude);
            Assert.AreNotEqual(startingCurrentDistance, actual.CurrentDistance);
            Assert.AreNotEqual(startingTotalDistance, actual.TotalDistance);
        }
    }

    internal class StubPositionService : PositionService
    {
        private readonly ICarRentalUnitOfWork _unitOfWork;

        public StubPositionService(ICarRentalUnitOfWork unitOfWork, Position carPosition) : base(unitOfWork)
        {
            CarPosition = carPosition;
            _unitOfWork = unitOfWork;
        }

        public Position CarPosition { get; set; }

        public override void UpdateCarPosition(Guid carId)
        {
            var car = _unitOfWork.CarRepository.Get(carId) ??
                      throw new Exception($"Could not find car '{carId}'.");
            var newPosition = new Position(CarPosition.Latitude,
                CarPosition.Longitude);
            var newDistance = car.CurrentPosition.GetDistanceTo(newPosition, car.TotalDistance.Unit);
            car.AddDistance(newDistance);
            car.UpdatePosition(newPosition);
            _unitOfWork.Commit();
        }
    }
}