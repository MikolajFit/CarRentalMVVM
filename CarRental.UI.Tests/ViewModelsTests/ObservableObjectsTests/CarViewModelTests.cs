using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.DomainModelLayer.Models;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.ObservableObjectsTests
{
    public class CarViewModelTests
    {
        [Test]
        public void ShouldReturnTrueIfPropertiesAreValid()
        {
            var sut = new CarViewModel 
                {
                    RegistrationNumber = "Valid",
                    PricePerMinute = "10",
                    TotalDistance = "10"
                };
            Assert.True(sut.IsValid);
        }
        [Test]
        [TestCase("RegistrationNumber", "Registration number cannot be empty!")]
        [TestCase("PricePerMinute", "Enter valid price")]
        [TestCase("TotalDistance", "Enter valid number")]
        public void ShouldReturnErrorMessageIfPropertyIsNotValid(string propertyName, string errorMessage)
        {
            var sut = new CarViewModel();

            Assert.AreEqual(errorMessage,sut[propertyName]);
        }

        [Test]
        public void ShouldSetProperties()
        {
            var currentLatitude = "";
            var currentLongitude = "";
            var currentDistance = "";
            var carStatus = CarStatus.Free;
            var sut = new CarViewModel()
            {
                CurrentLatitude = currentLatitude,
                CurrentLongitude = currentLongitude,
                CarStatus = carStatus,
                CurrentDistance = currentDistance
            };
            Assert.AreEqual(currentDistance,sut.CurrentDistance);
            Assert.AreEqual(currentLatitude,sut.CurrentLatitude);
            Assert.AreEqual(currentLongitude,sut.CurrentLongitude);
            Assert.AreEqual(carStatus,sut.CarStatus);


        }
    }
}
