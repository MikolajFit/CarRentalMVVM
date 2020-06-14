using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.UI.Mappers;
using CarRental.UI.Messages;
using CarRental.UI.ViewModels.DriverViewModels;
using CarRental.UI.ViewModels.ObservableObjects;
using GalaSoft.MvvmLight.Messaging;
using NSubstitute;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.DriverViewModelsTests
{
    public class DriverRentalsViewModelTests
    {
        private IRentalService _rentalServiceMock;
        private IRentalViewModelMapper _rentalViewModelMapperMock;

        [SetUp]
        public void Setup()
        {
            _rentalServiceMock = Substitute.For<IRentalService>();
            _rentalViewModelMapperMock = Substitute.For<IRentalViewModelMapper>();
        }

        [Test]
        public void ShouldThrowExceptionIfPassedNullArgumentsInConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => new DriverRentalsViewModel(_rentalServiceMock, null));
            Assert.Throws<ArgumentNullException>(() => new DriverRentalsViewModel(null, _rentalViewModelMapperMock));
        }

        [Test]
        public void ShouldRefreshRentalListWhenDriverAssigned()
        {
            var driver = new DriverViewModel()
            {
                Id = Guid.NewGuid()
            };
            var rentalDtoList = new List<RentalDTO>()
            {
                new RentalDTO()
            };
            var rentalViewModel = new RentalViewModel();
            _rentalServiceMock.GetRentalsForDriver(driver.Id).Returns(rentalDtoList);
            _rentalViewModelMapperMock.Map(rentalDtoList[0]).Returns(rentalViewModel);
            var sut = new DriverRentalsViewModel(_rentalServiceMock,_rentalViewModelMapperMock);
            sut.AssignLoggedInDriver(driver);
            Assert.AreEqual(rentalViewModel,sut.DriverRentals[0]);
            Assert.AreEqual(sut.DriverRentals,sut.DriverRentalsCollection.SourceCollection);
        }

        [Test]
        public void ShouldNotRefreshRentalListIfServiceReturnedNull()
        {
            var driver = new DriverViewModel()
            {
                Id = Guid.NewGuid()
            };
            var sut = new DriverRentalsViewModel(_rentalServiceMock, _rentalViewModelMapperMock);
            sut.AssignLoggedInDriver(driver);
            _rentalViewModelMapperMock.DidNotReceive().Map(Arg.Any<RentalDTO>());
        }

        [Test]
        public void ShouldFilterByStartDateTimeFrom()
        {
            var rentalViewModelList = new ObservableCollection<RentalViewModel>()
            {
                new RentalViewModel()
                {
                    RentalId = Guid.NewGuid(),
                    StartDateTime = new DateTime(2020, 03, 03)
                },
                new RentalViewModel()
                {
                    RentalId = Guid.NewGuid(),
                    StartDateTime = new DateTime(2020, 04, 03)
                }
            };
            var rentalDtos = new List<RentalDTO>()
            {
                new RentalDTO(),
                new RentalDTO()
            };
            _rentalServiceMock.GetRentalsForDriver(Arg.Any<Guid>()).Returns(rentalDtos);
            _rentalViewModelMapperMock.Map(rentalDtos[0]).Returns(rentalViewModelList[0]);
            _rentalViewModelMapperMock.Map(rentalDtos[1]).Returns(rentalViewModelList[1]);

            var sut = new DriverRentalsViewModel(_rentalServiceMock, _rentalViewModelMapperMock);
            sut.AssignLoggedInDriver(new DriverViewModel()
            {
                Id = Guid.NewGuid()
            });

            sut.SelectedStartDateFrom = new DateTime(2020,04,03);
            var collection = sut.DriverRentalsCollection;
           // sut.DriverRentalsCollection;
            Assert.True(collection.Contains(rentalViewModelList[1]));
            Assert.False(collection.Contains(rentalViewModelList[0]));
        }

        [Test]
        public void ShouldFilterByStartDateTimeTo()
        {
            var rentalViewModelList = new ObservableCollection<RentalViewModel>()
            {
                new RentalViewModel()
                {
                    RentalId = Guid.NewGuid(),
                    StartDateTime = new DateTime(2020, 03, 03)
                },
                new RentalViewModel()
                {
                    RentalId = Guid.NewGuid(),
                    StartDateTime = new DateTime(2020, 04, 03)
                }
            };
            var rentalDtos = new List<RentalDTO>()
            {
                new RentalDTO(),
                new RentalDTO()
            };
            _rentalServiceMock.GetRentalsForDriver(Arg.Any<Guid>()).Returns(rentalDtos);
            _rentalViewModelMapperMock.Map(rentalDtos[0]).Returns(rentalViewModelList[0]);
            _rentalViewModelMapperMock.Map(rentalDtos[1]).Returns(rentalViewModelList[1]);

            var sut = new DriverRentalsViewModel(_rentalServiceMock, _rentalViewModelMapperMock);
            sut.AssignLoggedInDriver(new DriverViewModel()
            {
                Id = Guid.NewGuid()
            });

            sut.SelectedStartDateTo = new DateTime(2020, 03, 03);
            var collection = sut.DriverRentalsCollection;
            // sut.DriverRentalsCollection;
            Assert.True(collection.Contains(rentalViewModelList[0]));
            Assert.False(collection.Contains(rentalViewModelList[1]));
        }

        [Test]
        public void ShouldFilterByStopTimeFrom()
        {
            var rentalViewModelList = new ObservableCollection<RentalViewModel>()
            {
                new RentalViewModel()
                {
                    RentalId = Guid.NewGuid(),
                    StopDateTime = new DateTime(2020, 03, 03)
                },
                new RentalViewModel()
                {
                    RentalId = Guid.NewGuid(),
                    StopDateTime = new DateTime(2020, 04, 03)
                }
            };
            var rentalDtos = new List<RentalDTO>()
            {
                new RentalDTO(),
                new RentalDTO()
            };
            _rentalServiceMock.GetRentalsForDriver(Arg.Any<Guid>()).Returns(rentalDtos);
            _rentalViewModelMapperMock.Map(rentalDtos[0]).Returns(rentalViewModelList[0]);
            _rentalViewModelMapperMock.Map(rentalDtos[1]).Returns(rentalViewModelList[1]);

            var sut = new DriverRentalsViewModel(_rentalServiceMock, _rentalViewModelMapperMock);
            sut.AssignLoggedInDriver(new DriverViewModel()
            {
                Id = Guid.NewGuid()
            });

            sut.SelectedStopDateFrom = new DateTime(2020, 04, 03);
            var collection = sut.DriverRentalsCollection;
            // sut.DriverRentalsCollection;
            Assert.True(collection.Contains(rentalViewModelList[1]));
            Assert.False(collection.Contains(rentalViewModelList[0]));
        }

        [Test]
        public void ShouldFilterByStopTimeTo()
        {
            var rentalViewModelList = new ObservableCollection<RentalViewModel>()
            {
                new RentalViewModel()
                {
                    RentalId = Guid.NewGuid(),
                    StopDateTime = new DateTime(2020, 03, 03)
                },
                new RentalViewModel()
                {
                    RentalId = Guid.NewGuid(),
                    StopDateTime = new DateTime(2020, 04, 03)
                }
            };
            var rentalDtos = new List<RentalDTO>()
            {
                new RentalDTO(),
                new RentalDTO()
            };
            _rentalServiceMock.GetRentalsForDriver(Arg.Any<Guid>()).Returns(rentalDtos);
            _rentalViewModelMapperMock.Map(rentalDtos[0]).Returns(rentalViewModelList[0]);
            _rentalViewModelMapperMock.Map(rentalDtos[1]).Returns(rentalViewModelList[1]);

            var sut = new DriverRentalsViewModel(_rentalServiceMock, _rentalViewModelMapperMock);
            sut.AssignLoggedInDriver(new DriverViewModel()
            {
                Id = Guid.NewGuid()
            });

            sut.SelectedStopDateTo = new DateTime(2020, 03, 03);
            var collection = sut.DriverRentalsCollection;
            // sut.DriverRentalsCollection;
            Assert.True(collection.Contains(rentalViewModelList[0]));
            Assert.False(collection.Contains(rentalViewModelList[1]));
        }

        [Test]
        public void ShouldRefreshRentalListWhenNewRentalAdded()
        {
            var rentalViewModelList = new ObservableCollection<RentalViewModel>()
            {
                new RentalViewModel()
                {
                    RentalId = Guid.NewGuid(),
                    StopDateTime = new DateTime(2020, 03, 03)
                },
                new RentalViewModel()
                {
                    RentalId = Guid.NewGuid(),
                    StopDateTime = new DateTime(2020, 04, 03)
                }
            };
            var rentalDtos = new List<RentalDTO>()
            {
                new RentalDTO(),
                new RentalDTO()
            };
            _rentalServiceMock.GetRentalsForDriver(Arg.Any<Guid>()).Returns(rentalDtos);
            _rentalViewModelMapperMock.Map(rentalDtos[0]).Returns(rentalViewModelList[0]);
            _rentalViewModelMapperMock.Map(rentalDtos[1]).Returns(rentalViewModelList[1]);

            var sut = new DriverRentalsViewModel(_rentalServiceMock, _rentalViewModelMapperMock);
            sut.AssignLoggedInDriver(new DriverViewModel()
            {
                Id = Guid.NewGuid()
            });

            Messenger.Default.Send(new RefreshRentalsMessage("message"));
            _rentalServiceMock.Received(2).GetRentalsForDriver(Arg.Any<Guid>());
            _rentalViewModelMapperMock.Received(4).Map(Arg.Any<RentalDTO>());

        }
    }
}