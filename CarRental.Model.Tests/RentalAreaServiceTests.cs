using System;
using System.Linq;
using CarRental.Model.Tests.TestHelpers;
using NUnit.Framework;

namespace CarRental.Model.Tests
{
    public class RentalAreaServiceTests
    {
        private TestContainer TestContainer { get; set; }

        [SetUp]
        public void Setup()
        {
            TestContainer = new TestContainer();
        }
        [Test]
        public void ShouldProperlyCreateAndReturnRentalAreaWithParameters()
        {
            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);
            var expected = rentalArea;
            var actual = TestContainer.RentalAreaService.GetAllRentalAreas().First();


            //assert
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.OutOfBondsPenaltyPerDistanceUnit, actual.OutOfBondsPenaltyPerDistanceUnit);
            Assert.AreEqual(expected.Area.Count, actual.Area.Count);
        }

        [Test]
        public void ShouldThrowExceptionIfAddExistingRentalArea()
        {
            //assign
            var rentalArea = Helper.CreateRentalAreaDTO();
            TestContainer.RentalAreaService.CreateRentalArea(rentalArea);
            //act
            //assert
            Assert.Throws<Exception>(() => TestContainer.RentalAreaService.CreateRentalArea(rentalArea));
        }
    }
}
