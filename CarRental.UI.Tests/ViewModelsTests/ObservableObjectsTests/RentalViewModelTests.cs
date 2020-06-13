using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.UI.ViewModels.ObservableObjects;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.ObservableObjectsTests
{
    class RentalViewModelTests
    {
        [Test]
        public void ShouldSetProperties()
        {
            var regNumber = "";
            var total = 2.5m;
            var driverName = "";
            var sut = new RentalViewModel()
            {
                RegistrationNumber = regNumber,
                DriverName = driverName,
                Total = total
            };
            Assert.AreEqual(regNumber,sut.RegistrationNumber);
            Assert.AreEqual(total,sut.Total);
            Assert.AreEqual(driverName,sut.DriverName);
        }
    }
}
