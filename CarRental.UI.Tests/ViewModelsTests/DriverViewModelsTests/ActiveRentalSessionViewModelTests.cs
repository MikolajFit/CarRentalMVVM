using System;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.UI.Messages;
using CarRental.UI.Services;
using CarRental.UI.Utils.Interfaces;
using CarRental.UI.ViewModels.DriverViewModels;
using CarRental.UI.ViewModels.ObservableObjects;
using GalaSoft.MvvmLight.Messaging;
using NSubstitute;
using NUnit.Framework;

namespace CarRental.UI.Tests.ViewModelsTests.DriverViewModelsTests
{
    public class ActiveRentalSessionViewModelTests
    {
        private IDialogService _dialogServiceMock;
        private IMessengerService _messengerServiceMock;
        private IRentalService _rentalServiceMock;
        private ITimerFactory _timerFactoryMock;

        [SetUp]
        public void Setup()
        {
            _dialogServiceMock = Substitute.For<IDialogService>();
            _messengerServiceMock = Substitute.For<IMessengerService>();
            _rentalServiceMock = Substitute.For<IRentalService>();
            _timerFactoryMock = Substitute.For<ITimerFactory>();
        }

        [Test]
        public void ShouldThrowExceptionIfArgumentsInConstructorAreNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ActiveRentalSessionViewModel(_timerFactoryMock, _rentalServiceMock, _dialogServiceMock, null));
            Assert.Throws<ArgumentNullException>(() =>
                new ActiveRentalSessionViewModel(_timerFactoryMock, _rentalServiceMock, null, _messengerServiceMock));
            Assert.Throws<ArgumentNullException>(() =>
                new ActiveRentalSessionViewModel(_timerFactoryMock, null, _dialogServiceMock, _messengerServiceMock));
            Assert.Throws<ArgumentNullException>(() =>
                new ActiveRentalSessionViewModel(null, _rentalServiceMock, _dialogServiceMock, _messengerServiceMock));
        }

        [Test]
        public void ShouldConfigureTimersUponInitialization()
        {
            var elapsedTimer = Substitute.For<ITimer>();
            var elapsedCostTimer = Substitute.For<ITimer>();
            _timerFactoryMock.CreateTimer().Returns(elapsedTimer, elapsedCostTimer);
            new ActiveRentalSessionViewModel(_timerFactoryMock, _rentalServiceMock, _dialogServiceMock,
                _messengerServiceMock);
            Assert.AreEqual(elapsedTimer.Interval.ToString(), TimeSpan.FromSeconds(1).ToString());
            Assert.AreEqual(elapsedCostTimer.Interval.ToString(), TimeSpan.FromSeconds(60).ToString());
        }

        [Test]
        [TestCase(0, "00:00:01")]
        [TestCase(86399, "01 days 00:00:00")]
        public void ShouldUpdateTimerTextUponEventRaised(int totalSeconds, string expectedTimeText)
        {
            var elapsedTimer = Substitute.For<ITimer>();
            var elapsedCostTimer = Substitute.For<ITimer>();
            _timerFactoryMock.CreateTimer().Returns(elapsedTimer, elapsedCostTimer);
            var sut = new ActiveRentalSessionViewModel(_timerFactoryMock, _rentalServiceMock, _dialogServiceMock,
                _messengerServiceMock)
            {
                TotalSeconds = totalSeconds
            };
            elapsedTimer.Tick += Raise.Event();
            Assert.AreEqual(totalSeconds + 1, sut.TotalSeconds);
            Assert.AreEqual(expectedTimeText, sut.TimerText);
        }

        [Test]
        public void ShouldUpdateEstimatedCostTextUponEventRaised()
        {
            var elapsedTimer = Substitute.For<ITimer>();
            var elapsedCostTimer = Substitute.For<ITimer>();
            _timerFactoryMock.CreateTimer().Returns(elapsedTimer, elapsedCostTimer);
            var sut = new ActiveRentalSessionViewModel(_timerFactoryMock, _rentalServiceMock, _dialogServiceMock,
                _messengerServiceMock)
            {
                TotalSeconds = 60, CurrentRental = new RentalViewModel {PricePerMinute = "2.5"}
            };
            elapsedCostTimer.Tick += Raise.Event();
            Assert.AreEqual("2.50", sut.EstimatedCost);
        }

        [Test]
        public void ShouldShowSummaryAndStopTimerWhenReturnCar()
        {
            var elapsedTimer = Substitute.For<ITimer>();
            var elapsedCostTimer = Substitute.For<ITimer>();
            _timerFactoryMock.CreateTimer().Returns(elapsedTimer, elapsedCostTimer);
            var rentalId = Guid.NewGuid();
            _rentalServiceMock.GetRental(rentalId).Returns(new RentalDTO {Total = 10});
            var sut = new ActiveRentalSessionViewModel(_timerFactoryMock, _rentalServiceMock, _dialogServiceMock,
                _messengerServiceMock)
            {
                CurrentRental = new RentalViewModel
                {
                    RentalId = rentalId,
                    PricePerMinute = "2.5"
                }
            };
            sut.StopRentalCommand.Execute(null);


            _rentalServiceMock.Received().ReturnCar(rentalId, Arg.Any<DateTime>());
            Assert.Null(sut.ErrorString);
            _messengerServiceMock.Received()
                .Send(Arg.Is<RefreshRentalsMessage>(m => m.Notification == "Rental Stopped!"));
            Assert.True(sut.IsRentalStopped);
            elapsedCostTimer.Received().Stop();
            elapsedTimer.Received().Stop();
            _rentalServiceMock.Received().GetRental(rentalId);
            _dialogServiceMock.Received().ShowMessage(Arg.Any<string>());
        }

        [Test]
        public void ShouldNotShowSummaryAndStopTimerWhenReturnCarIfCantFindRental()
        {
            var elapsedTimer = Substitute.For<ITimer>();
            var elapsedCostTimer = Substitute.For<ITimer>();
            _timerFactoryMock.CreateTimer().Returns(elapsedTimer, elapsedCostTimer);
            var rentalId = Guid.NewGuid();
            var sut = new ActiveRentalSessionViewModel(_timerFactoryMock, _rentalServiceMock, _dialogServiceMock,
                _messengerServiceMock)
            {
                CurrentRental = new RentalViewModel
                {
                    RentalId = rentalId,
                    PricePerMinute = "2.5"
                }
            };
            sut.StopRentalCommand.Execute(null);
            _rentalServiceMock.Received().ReturnCar(rentalId, Arg.Any<DateTime>());
            Assert.Null(sut.ErrorString);
            _messengerServiceMock.Received()
                .Send(Arg.Is<RefreshRentalsMessage>(m => m.Notification == "Rental Stopped!"));
            Assert.True(sut.IsRentalStopped);
            elapsedCostTimer.Received().Stop();
            elapsedTimer.Received().Stop();
            _rentalServiceMock.Received().GetRental(rentalId);
            _dialogServiceMock.DidNotReceive().ShowMessage(Arg.Any<string>());
        }

        [Test]
        public void ShouldConfigureActiveRentalViewWhenStartRental()
        {
            var elapsedTimer = Substitute.For<ITimer>();
            var elapsedCostTimer = Substitute.For<ITimer>();
            _timerFactoryMock.CreateTimer().Returns(elapsedTimer, elapsedCostTimer);
            var sut = new ActiveRentalSessionViewModel(_timerFactoryMock, _rentalServiceMock, _dialogServiceMock,
                _messengerServiceMock);
            var rentalViewModel = new RentalViewModel
            {
                RentalId = Guid.NewGuid(),
                PricePerMinute = "2.5"
            };
            Messenger.Default.Send(new RentalViewModelMessage(RentalViewModelMessageType.StartRental, rentalViewModel));
            Assert.AreEqual(rentalViewModel, sut.CurrentRental);
            _messengerServiceMock.Received()
                .Send(Arg.Is<RefreshRentalsMessage>(m => m.Notification == "Rental Started!"));
            elapsedCostTimer.Received().Start();
            elapsedTimer.Received().Start();
        }

        [Test]
        public void ShouldConfigureActiveRentalViewWhenContinueRental()
        {
            var elapsedTimer = Substitute.For<ITimer>();
            var elapsedCostTimer = Substitute.For<ITimer>();
            _timerFactoryMock.CreateTimer().Returns(elapsedTimer, elapsedCostTimer);
            var sut = new ActiveRentalSessionViewModel(_timerFactoryMock, _rentalServiceMock, _dialogServiceMock,
                _messengerServiceMock);
            var rentalViewModel = new RentalViewModel
            {
                RentalId = Guid.NewGuid(),
                PricePerMinute = "2.5",
                StartDateTime = new DateTime(2020, 03, 03)
            };
            Messenger.Default.Send(new RentalViewModelMessage(RentalViewModelMessageType.ContinueRental,
                rentalViewModel));
            Assert.AreEqual(rentalViewModel, sut.CurrentRental);
            Assert.NotZero(sut.TotalSeconds);
            elapsedCostTimer.Received().Start();
            elapsedTimer.Received().Start();
        }

        [Test]
        public void ShouldNotCloseActiveRentalSessionViewIfRentalWasNotStopped()
        {
            var elapsedTimer = Substitute.For<ITimer>();
            var elapsedCostTimer = Substitute.For<ITimer>();
            _timerFactoryMock.CreateTimer().Returns(elapsedTimer, elapsedCostTimer);
            var sut = new ActiveRentalSessionViewModel(_timerFactoryMock, _rentalServiceMock, _dialogServiceMock,
                _messengerServiceMock);
            sut.CloseActiveRentalSessionViewCommand.Execute(null);
            _messengerServiceMock.DidNotReceive()
                .Send(Arg.Is<NotificationMessage>(message => message.Notification == "Stop Car Rental"));
        }

        [Test]
        public void ShouldCloseActiveRentalSessionViewIfRentalWasStopped()
        {
            var elapsedTimer = Substitute.For<ITimer>();
            var elapsedCostTimer = Substitute.For<ITimer>();
            _timerFactoryMock.CreateTimer().Returns(elapsedTimer, elapsedCostTimer);
            var sut = new ActiveRentalSessionViewModel(_timerFactoryMock, _rentalServiceMock, _dialogServiceMock,
                _messengerServiceMock)
            {
                TotalSeconds = 10
            };
            sut.IsRentalStopped = true;
            sut.CloseActiveRentalSessionViewCommand.Execute(null);
            _messengerServiceMock.Received()
                .Send(Arg.Is<NotificationMessage>(message => message.Notification == "Stop Car Rental"));
            Assert.Zero(sut.TotalSeconds);
            Assert.False(sut.IsRentalStopped);
        }
    }
}