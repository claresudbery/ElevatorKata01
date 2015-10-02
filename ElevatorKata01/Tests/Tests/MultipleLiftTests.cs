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
            const string liftName = "She'll lift you up and then she'll drop you down";
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
            const string liftName = "Full of annoying mirrors";
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
            const string liftA = "Claustrophobics' Delight";
            const string liftB = "Moving Cuboid";

            var liftNames = new List<string> {liftA, liftB};
            _liftManagerTestHelper.MakeStart(liftNames);

            // Act
            _liftManagerTestHelper.MakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: true, expectedLiftName: liftA);
            _liftManagerTestHelper.Lift(liftA).ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);

            _liftManagerTestHelper.MakeDownwardsRequestFrom(2, shouldBeActedUponImmediately: true, expectedLiftName: liftB);
            _liftManagerTestHelper.Lift(liftB).ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);

            _liftManagerTestHelper.Lift(liftA).ExpectToVisit(Floors.First).Mark(Direction.Up);
            _liftManagerTestHelper.Lift(liftB).ExpectToVisit(Floors.First).Mark(Direction.Up);

            _liftManagerTestHelper.Lift(liftA).ExpectToVisit(Floors.Second).Mark(Direction.Up);
            _liftManagerTestHelper.Lift(liftB).ExpectToStopAt(Floors.Second).Mark(Direction.None);

            _liftManagerTestHelper.Lift(liftA).ExpectToStopAt(Floors.Third).Mark(Direction.None);

            _liftManagerTestHelper.StartTest();

            // Assert
            _liftManagerTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_one_lift_is_moving_downwards__When_someone_makes_an_upwards_request__Then_a_second_lift_will_service_the_upwards_request()
        {
            // Arrange
            const string liftA = "Elevation for your mind";
            const string liftB = "Mundanity";

            var liftNames = new List<string> { liftA, liftB };
            _liftManagerTestHelper.MakeStart(liftNames);

            // Act
            _liftManagerTestHelper.MakeDownwardsRequestFrom(5, shouldBeActedUponImmediately: true, expectedLiftName: liftA);
            _liftManagerTestHelper.Lift(liftA).ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);
            _liftManagerTestHelper.Lift(liftA).ExpectToVisit(Floors.First).Mark(Direction.Up);
            _liftManagerTestHelper.Lift(liftA).ExpectToVisit(Floors.Second).Mark(Direction.Up);
            
            _liftManagerTestHelper.MakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: true, expectedLiftName: liftB);
            _liftManagerTestHelper.Lift(liftB).ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);

            _liftManagerTestHelper.Lift(liftA).ExpectToVisit(Floors.Third).Mark(Direction.Up);
            _liftManagerTestHelper.Lift(liftB).ExpectToVisit(Floors.First).Mark(Direction.Up);

            _liftManagerTestHelper.Lift(liftA).ExpectToVisit(Floors.Fourth).Mark(Direction.Up);
            _liftManagerTestHelper.Lift(liftB).ExpectToVisit(Floors.Second).Mark(Direction.Up);

            _liftManagerTestHelper.Lift(liftA).ExpectToStopAt(Floors.Fifth).Mark(Direction.None);
            _liftManagerTestHelper.Lift(liftB).ExpectToStopAt(Floors.Third).Mark(Direction.None);

            _liftManagerTestHelper.StartTest();

            // Assert
            _liftManagerTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_one_lift_is_moving_and_one_is_not__When_a_new_request_comes_in__Then_the_stationary_lift_will_service_that_request_if_it_can_get_there_quickest()
        {
            // Arrange
            const string liftForPoorlyDragons = "Lift for Poorly Dragons";
            const string portalToAnotherDimension = "Portal to Another Dimension";

            var liftNames = new List<string> { liftForPoorlyDragons, portalToAnotherDimension };
            _liftManagerTestHelper.MakeStart(liftNames);

            // Act
            _liftManagerTestHelper.MakeUpwardsRequestFrom(Floors.Fifth, shouldBeActedUponImmediately: true, expectedLiftName: liftForPoorlyDragons);

            _liftManagerTestHelper.Lift(liftForPoorlyDragons).ExpectToLeaveFrom(Floors.Ground);
            _liftManagerTestHelper.Lift(liftForPoorlyDragons).ExpectToVisit(Floors.First);
            _liftManagerTestHelper.Lift(liftForPoorlyDragons).ExpectToVisit(Floors.Second);
            _liftManagerTestHelper.Lift(liftForPoorlyDragons).ExpectToVisit(Floors.Third);

            _liftManagerTestHelper.MakeUpwardsRequestFrom(
                Floors.First, 
                shouldBeActedUponImmediately: true, 
                expectedLiftName: portalToAnotherDimension,
                liftWasPreviouslyIdle: true);
            
            _liftManagerTestHelper.Lift(portalToAnotherDimension).ExpectToLeaveFrom(Floors.Ground).Mark(Direction.Up);
            
            _liftManagerTestHelper.Lift(liftForPoorlyDragons).NoExpectations();
            
            _liftManagerTestHelper.StartTest();

            // Assert
            _liftManagerTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_all_four_lifts_are_moving__When_a_new_request_comes_in__Then_the_lift_which_can_get_there_quickest_will_service_that_request()
        {
            // NOT WRITTEN YET!

            //// Arrange
            //const string liftForPoorlyDragons = "Lift for Poorly Dragons";
            //const string portalToAnotherDimension = "Portal to Another Dimension";
            //const string yoyoOno = "Yo-Yo Oh-No";
            //const string badlyLabelledToilet = "Badly Labelled Toilet";

            //var liftNames = new List<string> { liftForPoorlyDragons, portalToAnotherDimension, yoyoOno, badlyLabelledToilet };
            //_liftManagerTestHelper.MakeStart(liftNames);

            //// Act
            //_liftManagerTestHelper.MakeDownwardsRequestFrom(5, shouldBeActedUponImmediately: true, expectedLiftName: liftForPoorlyDragons);

            //_liftManagerTestHelper.StartTest();

            //// Assert
            //_liftManagerTestHelper.VerifyAllMarkers();
        }
    }
}