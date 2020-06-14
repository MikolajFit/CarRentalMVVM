using System;
using System.Linq;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.Tests.TestHelpers;
using NUnit.Framework;

namespace CarRental.Model.Tests
{
    //End to end DriverService tests
    public class DriverServiceTests
    {
        private TestContainer TestContainer { get; set; }

        [SetUp]
        public void Setup()
        {
            TestContainer = new TestContainer();
        }

        [Test]
        public void ShouldProperlyCreateAndReturnDriverWithParameters()
        {
            //arrange
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Mikołaj",
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };
            var expected = driver;

            //act
            TestContainer.DriverService.CreateDriver(driver);
            var actual = TestContainer.DriverService.GetAllDrivers().First();


            //assert
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.LastName, actual.LastName);
            Assert.AreEqual(expected.LicenseNumber, actual.LicenseNumber);
        }

        [Test]
        public void ShouldThrowExceptionIfAddDriverWithNoLicenseNumber()
        {
            //assign
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Mikołaj",
                LastName = "Fitowski"
            };
            //act
            //assert
            Assert.Throws<Exception>((() => TestContainer.DriverService.CreateDriver(driver)));
        }

        [Test]
        public void ShouldThrowExceptionIfAddDriverWithNoLFirstName()
        {
            //assign
            var driver = new DriverDTO
            {
                Id = new Guid(),
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };
            //act
            //assert
            Assert.Throws<Exception>((() => TestContainer.DriverService.CreateDriver(driver)));
        }

        [Test]
        public void ShouldThrowExceptionIfAddDriverWithNoLastName()
        {
            //assign
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Mikołaj",
                LicenseNumber = "CDJ813"
            };
            //act
            //assert
            Assert.Throws<Exception>((() => TestContainer.DriverService.CreateDriver(driver)));
        }

        [Test]
        public void ShouldThrowExceptionIfAddExistingDriver()
        {
            //assign
            var driver = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Mikolaj",
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };
            //act
            TestContainer.DriverService.CreateDriver(driver);
            //assert
            Assert.Throws<Exception>((() => TestContainer.DriverService.CreateDriver(driver)));


        }
    }
}