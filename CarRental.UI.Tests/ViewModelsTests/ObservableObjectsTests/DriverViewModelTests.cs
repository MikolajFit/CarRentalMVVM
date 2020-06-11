using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.UI.ViewModels.ObservableObjects;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.ObservableObjectsTests
{
    public class DriverViewModelTests
    {
        [Test]
        public void ShouldReturnTrueIfPropertiesAreValid()
        {
            var sut = new DriverViewModel()
            {
                FirstName = "Valid",
                LastName = "Name",
                LicenseNumber = "10"
            };
            Assert.True(sut.IsValid);
        }
        [Test]
        [TestCase("FirstName", "First name cannot be empty!")]
        [TestCase("LastName", "Last name cannot be empty!")]
        [TestCase("LicenseNumber", "License number cannot be empty!")]
        public void ShouldReturnErrorMessageIfPropertyIsNotValid(string propertyName, string errorMessage)
        {
            var sut = new DriverViewModel();

            Assert.AreEqual(errorMessage, sut[propertyName]);
        }
    }
}
