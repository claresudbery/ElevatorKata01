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
        readonly LiftManagerTestHelper _liftTestHelper = new LiftManagerTestHelper();

        [Test]
        public void When_person_calls_lift_to_higher_floor_number_then_one_lift_will_start_moving_upwards()
        {
            // Arrange
            const string liftName = "Lift A";
            _liftTestHelper.MakeStart(new List<string> { liftName });

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(Floors.Third, shouldBeActedUponImmediately: true, expectedLiftName: liftName);

            _liftTestHelper.Lift(liftName).ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_lower_floor_number_then_one_lift_will_start_moving_downwards()
        {
            // Arrange
            const string liftName = "Lift A";
            _liftTestHelper.MakeStart(new List<string> { liftName });

            // Act
            _liftTestHelper.MakeDownwardsRequestFrom(-3, shouldBeActedUponImmediately: true, expectedLiftName: liftName);

            _liftTestHelper.Lift(liftName).ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_one_lift_is_moving_upwards__When_someone_makes_a_downwards_request__Then_a_second_lift_will_service_the_downwards_request()
        {
            // Arrange
            const string liftA = "Lift A";
            const string liftB = "Lift B";

            var liftNames = new List<string> {liftA, liftB};
            _liftTestHelper.MakeStart(liftNames);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: true, expectedLiftName: liftA);
            _liftTestHelper.Lift("Lift A").ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);

            _liftTestHelper.MakeDownwardsRequestFrom(2, shouldBeActedUponImmediately: true, expectedLiftName: liftB);
            _liftTestHelper.Lift("Lift B").ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);

            _liftTestHelper.Lift("Lift A").ExpectToVisit(Floors.First).Mark(Direction.Up);
            _liftTestHelper.Lift("Lift B").ExpectToVisit(Floors.First).Mark(Direction.Up);

            _liftTestHelper.Lift("Lift A").ExpectToVisit(Floors.Second).Mark(Direction.Up);
            _liftTestHelper.Lift("Lift B").ExpectToStopAt(Floors.Second).Mark(Direction.None);

            _liftTestHelper.Lift("Lift A").ExpectToStopAt(Floors.Third).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }
    }
}