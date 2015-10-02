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
        readonly LiftManagerTestHelper _liftManagerTestHelper = new LiftManagerTestHelper();
       
        [Test]
        public void When_person_calls_lift_to_higher_floor_number_then_one_lift_will_start_moving_upwards()
        {
            // Arrange
            const string liftName = "Lift A";
            _liftManagerTestHelper.MakeStart(new List<string> { liftName });

            // Act
            _liftManagerTestHelper.MakeUpwardsRequestFrom(Floors.Third, shouldBeActedUponImmediately: true, expectedLiftName: liftName);

            _liftManagerTestHelper.Lift(liftName).ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);

            _liftManagerTestHelper.StartTest();

            // Assert
            _liftManagerTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_lower_floor_number_then_one_lift_will_start_moving_downwards()
        {
            // Arrange
            const string liftName = "Lift A";
            _liftManagerTestHelper.MakeStart(new List<string> { liftName });

            // Act
            _liftManagerTestHelper.MakeDownwardsRequestFrom(-3, shouldBeActedUponImmediately: true, expectedLiftName: liftName);

            _liftManagerTestHelper.Lift(liftName).ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Down);

            _liftManagerTestHelper.StartTest();

            // Assert
            _liftManagerTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_one_lift_is_moving_upwards__When_someone_makes_a_downwards_request__Then_a_second_lift_will_service_the_downwards_request()
        {
            // Arrange
            const string liftA = "Lift A";
            const string liftB = "Lift B";

            var liftNames = new List<string> {liftA, liftB};
            _liftManagerTestHelper.MakeStart(liftNames);

            // Act
            _liftManagerTestHelper.MakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: true, expectedLiftName: liftA);
            _liftManagerTestHelper.Lift("Lift A").ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);

            _liftManagerTestHelper.MakeDownwardsRequestFrom(2, shouldBeActedUponImmediately: true, expectedLiftName: liftB);
            _liftManagerTestHelper.Lift("Lift B").ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);

            _liftManagerTestHelper.Lift("Lift A").ExpectToVisit(Floors.First).Mark(Direction.Up);
            _liftManagerTestHelper.Lift("Lift B").ExpectToVisit(Floors.First).Mark(Direction.Up);

            _liftManagerTestHelper.Lift("Lift A").ExpectToVisit(Floors.Second).Mark(Direction.Up);
            _liftManagerTestHelper.Lift("Lift B").ExpectToStopAt(Floors.Second).Mark(Direction.None);

            _liftManagerTestHelper.Lift("Lift A").ExpectToStopAt(Floors.Third).Mark(Direction.None);

            _liftManagerTestHelper.StartTest();

            // Assert
            _liftManagerTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_one_lift_is_moving_downwards__When_someone_makes_an_upwards_request__Then_a_second_lift_will_service_the_upwards_request()
        {
            // Arrange
            const string liftA = "Lift A";
            const string liftB = "Lift B";

            var liftNames = new List<string> { liftA, liftB };
            _liftManagerTestHelper.MakeStart(liftNames);

            // Act
            _liftManagerTestHelper.MakeDownwardsRequestFrom(5, shouldBeActedUponImmediately: true, expectedLiftName: liftA);
            _liftManagerTestHelper.Lift("Lift A").ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);
            _liftManagerTestHelper.Lift("Lift A").ExpectToVisit(Floors.First).Mark(Direction.Up);
            _liftManagerTestHelper.Lift("Lift A").ExpectToVisit(Floors.Second).Mark(Direction.Up);
            
            _liftManagerTestHelper.MakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: true, expectedLiftName: liftB);
            _liftManagerTestHelper.Lift("Lift B").ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);

            _liftManagerTestHelper.Lift("Lift A").ExpectToVisit(Floors.Third).Mark(Direction.Up);
            _liftManagerTestHelper.Lift("Lift B").ExpectToVisit(Floors.First).Mark(Direction.Up);

            _liftManagerTestHelper.Lift("Lift A").ExpectToVisit(Floors.Fourth).Mark(Direction.Up);
            _liftManagerTestHelper.Lift("Lift B").ExpectToVisit(Floors.Second).Mark(Direction.Up);

            _liftManagerTestHelper.Lift("Lift A").ExpectToStopAt(Floors.Fifth).Mark(Direction.None);
            _liftManagerTestHelper.Lift("Lift B").ExpectToStopAt(Floors.Third).Mark(Direction.None);

            _liftManagerTestHelper.StartTest();

            // Assert
            _liftManagerTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_four_lifts_are_all_moving__When_a_new_request_comes_in__Then_the_nearest_lift_moving_in_the_correct_direction_will_service_that_request()
        {
            // NOT WRITTEN YET!
            // Arrange
            const string liftA = "Lift A";
            const string liftB = "Lift B";

            var liftNames = new List<string> { liftA, liftB };
            _liftManagerTestHelper.MakeStart(liftNames);

            // Act
            //_liftManagerTestHelper.MakeDownwardsRequestFrom(5, shouldBeActedUponImmediately: true, expectedLiftName: liftA);

            _liftManagerTestHelper.StartTest();

            // Assert
            _liftManagerTestHelper.VerifyAllMarkers();
        }
    }
}