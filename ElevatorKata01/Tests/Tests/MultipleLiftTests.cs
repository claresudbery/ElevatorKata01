using System;
using System.Collections.Generic;
using ElevatorKata01.Elements;
using ElevatorKata01.FunctionalCode;
using ElevatorKata01.Tests.Helpers;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace ElevatorKata01.Tests.Tests
{
    [TestFixture]
    public class MultipleLiftTests
    {
        readonly LiftTestHelper _liftTestHelper = new LiftTestHelper();

        [Test]
        public void When_person_calls_lift_to_higher_floor_number_then_one_lift_will_start_moving_upwards()
        {
            // Arrange
            _liftTestHelper.ManagerMakeStart();

            // Act
            _liftTestHelper.ManagerMakeUpwardsRequestFrom(Floors.Third, true);

            _liftTestHelper.LiftExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_lower_floor_number_then_one_lift_will_start_moving_downwards()
        {
            // Arrange
            _liftTestHelper.ManagerMakeStart();

            // Act
            _liftTestHelper.ManagerMakeDownwardsRequestFrom(-3, true);

            _liftTestHelper.LiftExpectToLeaveFrom(Floors.Ground).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_one_lift_is_moving_upwards__When_someone_makes_a_downwards_request__Then_a_second_lift_will_service_the_downwards_request()
        {
        }
    }
}