#region COPYRIGHT (c) 2020 Motorola Solutions, Inc.

// COPYRIGHT (c) 2020 Motorola Solutions, Inc.
// ALL RIGHTS RESERVED
// MOTOROLA SOLUTIONS CONFIDENTIAL RESTRICTED
// The copyright to the computer program(s) herein is the property of Motorola Solutions, Inc.
// The programs may be used or copied only with the written permission of Motorola Solutions, Inc.

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.UI.Messages;
using CarRental.UI.Services;
using CarRental.UI.Tests.ViewModelsTests;
using GalaSoft.MvvmLight.Messaging;
using NUnit.Framework;

namespace CarRental.UI.Tests.ServicesTests
{
    public class MessengerServiceTests
    {
        [Test]
        public void ShouldSendMessage()
        {
            var sut = new MessengerService();
            var received = false;
            Messenger.Default.Register<bool>(this, (message) => { received = message; });
            sut.Send(true);

            Assert.True(received);
        }

        [TearDown]
        public void Cleanup()
        {
            Messenger.Default.Unregister(this);
        }
    }
}
