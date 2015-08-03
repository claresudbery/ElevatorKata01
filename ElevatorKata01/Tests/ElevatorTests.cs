using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using ElevatorKata01.Elements;
using ElevatorKata01.FunctionalCode;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Assert = NUnit.Framework.Assert;

namespace ElevatorKata01.Tests
{
    /// <summary>
    /// All the utility code is in ElevatorUtils.cs, so that the tests themselves can be kept separate.
    /// </summary>
    [TestFixture]
    public partial class ElevatorTests : ILiftMonitor
    {
        private readonly List<LiftStatus> _liftStatuses = new List<LiftStatus>();
        private readonly List<ExpectedLiftStatus> _expectedLiftStatuses = new List<ExpectedLiftStatus>();

        private const int GroundFloor = 0;
        private const int FirstFloor = 1;
        private const int SecondFloor = 2;
        private const int ThirdFloor = 3;
        private const int FourthFloor = 4;
        private const int FifthFloor = 5;
        private const int SixthFloor = 6;

        private readonly TestScheduler _testScheduler = new TestScheduler();
        private ObservableLift _theLift;
        private int _millisecondsSinceTestStarted;
        private int _millisecondsTakenByMostRecentEvent;
        private int _numExpectedStatuses;
        private int _currentLiftFloor;
        private bool _testStarted = false;

        [Test]
        public void When_person_in_lift_enters_a_higher_floor_number_then_lift_starts_moving_upwards()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MoveTo(ThirdFloor);

            LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_lower_floor_number_then_lift_starts_moving_downwards()
        {
            // Arrange
            LiftMakeStartAt(ThirdFloor);

            // Act
            _theLift.MoveTo(FirstFloor);

            LiftExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_notifies_direction_and_location_for_every_floor_it_passes()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MoveTo(ThirdFloor);

            LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            LiftExpectToVisit(FirstFloor).Mark(Direction.Up);
            LiftExpectToVisit(SecondFloor).Mark(Direction.Up);
            LiftExpectToStopAt(ThirdFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_notifies_its_current_location()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MoveTo(FourthFloor);

            LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_goes_to_that_floor_and_stops_moving()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MoveTo(FourthFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);
            LiftExpectToVisit(SecondFloor);
            LiftExpectToVisit(ThirdFloor);
            LiftExpectToStopAt(FourthFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_higher_floor_number_then_lift_starts_moving_upwards()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(ThirdFloor);

            LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_lower_floor_number_then_lift_starts_moving_downwards()
        {
            // Arrange
            LiftMakeStartAt(ThirdFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(FirstFloor);

            LiftExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_notifies_direction_and_location_for_every_floor_it_passes()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(ThirdFloor);

            LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            LiftExpectToVisit(FirstFloor).Mark(Direction.Up);
            LiftExpectToVisit(SecondFloor).Mark(Direction.Up);
            LiftExpectToStopAt(ThirdFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_notifies_its_current_location_when_setting_off()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(FirstFloor);

            LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_goes_to_that_floor_and_stops_moving()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(FourthFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);
            LiftExpectToVisit(SecondFloor);
            LiftExpectToVisit(ThirdFloor);
            LiftExpectToStopAt(FourthFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_two_people_on_different_floors_call_lift_upwards_then_lift_will_stop_at_lower_person_first_even_though_higher_person_made_first_request()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(FourthFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);

            LiftMakeUpwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            LiftExpectToStopAt(SecondFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_moving_upwards_and_person_between_lift_and_destination_calls_lift_downwards_then_lift_will_not_pick_them_up()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(FourthFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);

            LiftMakeDownwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(SecondFloor).Mark(Direction.Up);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_two_people_on_different_floors_call_lift_upwards_and_first_person_tries_to_go_downwards_then_lift_will_go_up_for_other_person_first()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(FourthFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);

            LiftMakeUpwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: true);

            LiftExpectToStopAt(SecondFloor);

            LiftMakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(SecondFloor).Mark(Direction.Up);
            LiftExpectToVisit(ThirdFloor).Mark(Direction.Up);
            LiftExpectToStopAt(FourthFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_downwards_request_is_made_after_upwards_request_then_lift_will_visit_downwards_person_after_servicing_upwards_request()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(ThirdFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);

            LiftMakeDownwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(SecondFloor);
            LiftExpectToStopAt(ThirdFloor).Mark(Direction.None);

            LiftMakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(ThirdFloor).Mark(Direction.Up);
            LiftExpectToStopAt(FourthFloor).Mark(Direction.None);

            LiftExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            LiftExpectToVisit(ThirdFloor);
            LiftExpectToStopAt(SecondFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_moves_up_and_then_down_then_it_should_not_try_to_return_to_its_previous_up_destination()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(ThirdFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);

            LiftMakeDownwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(SecondFloor);
            LiftExpectToStopAt(ThirdFloor);

            LiftMakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(ThirdFloor);
            LiftExpectToStopAt(FourthFloor);

            LiftExpectToLeaveFrom(FourthFloor);
            LiftExpectToVisit(ThirdFloor);
            LiftExpectToStopAt(SecondFloor);

            LiftMakeRequestToMoveTo(FirstFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(SecondFloor);
            LiftExpectToStopAt(FirstFloor).Mark(Direction.None);

            LiftExpectToLeaveFrom(FirstFloor).Mark(Direction.Down);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_highest_stop_on_upwards_journey_but_next_downwards_request_is_higher_up_then_it_will_keep_moving_upwards_but_then_come_down()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(ThirdFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);

            LiftMakeDownwardsRequestFrom(FifthFloor, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(SecondFloor);
            LiftExpectToStopAt(ThirdFloor);

            LiftMakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(ThirdFloor);
            LiftExpectToStopAt(FourthFloor);

            LiftExpectToLeaveFrom(FourthFloor).Mark(Direction.Up);
            LiftExpectToStopAt(FifthFloor).Mark(Direction.None);

            LiftMakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(FifthFloor).Mark(Direction.Down);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_highest_stop_on_upwards_journey_and_next_downwards_request_is_lower_down_then_it_will_go_down_to_that_caller_and_then_continue_down()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(ThirdFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);

            _theLift.MakeDownwardsRequestFrom(SecondFloor);

            LiftExpectToVisit(SecondFloor);
            LiftExpectToStopAt(ThirdFloor);

            LiftMakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(ThirdFloor);
            LiftExpectToStopAt(FourthFloor);

            LiftExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            LiftExpectToVisit(ThirdFloor);
            LiftExpectToVisit(SecondFloor).Mark(Direction.None);

            LiftMakeRequestToMoveTo(FirstFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(SecondFloor).Mark(Direction.Down);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_highest_stop_on_upwards_journey_and_there_are_no_downwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(ThirdFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);
            LiftExpectToVisit(SecondFloor);
            LiftExpectToStopAt(ThirdFloor);

            LiftMakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(ThirdFloor);
            LiftExpectToStopAt(FourthFloor);

            LiftExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            LiftExpectToVisit(ThirdFloor);
            LiftExpectToVisit(SecondFloor);
            LiftExpectToVisit(FirstFloor);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }
        
        [Test]
        public void When_lift_is_below_ground_and_reaches_highest_stop_on_upwards_journey_but_next_downwards_request_is_higher_up_then_it_will_keep_moving_upwards_but_then_come_down()
        {
            // Arrange
            LiftMakeStartAt(-6);

            // Act
            _theLift.MakeUpwardsRequestFrom(-3);

            LiftExpectToLeaveFrom(-6);
            LiftExpectToVisit(-5);

            LiftMakeDownwardsRequestFrom(-1, shouldBeActedUponImmediately:false);

            LiftExpectToVisit(-4);
            LiftExpectToStopAt(-3);

            LiftMakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-3);
            LiftExpectToStopAt(-2);

            LiftExpectToLeaveFrom(-2).Mark(Direction.Up);
            LiftExpectToStopAt(-1).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);
            
            LiftExpectToLeaveFrom(-1).Mark(Direction.Down);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_highest_stop_on_upwards_journey_and_next_downwards_request_is_lower_down_then_it_will_go_down_to_that_caller_and_then_continue_down()
        {
            // Arrange
            LiftMakeStartAt(-6);

            // Act
            _theLift.MakeUpwardsRequestFrom(-3);

            LiftExpectToLeaveFrom(-6);
            LiftExpectToVisit(-5);

            LiftMakeDownwardsRequestFrom(-5, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(-4);
            LiftExpectToStopAt(-3);

            LiftMakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-3);
            LiftExpectToStopAt(-2);

            LiftExpectToLeaveFrom(-2).Mark(Direction.Down);
            LiftExpectToVisit(-3);
            LiftExpectToVisit(-4);
            LiftExpectToStopAt(-5).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-6, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-5).Mark(Direction.Down);
            LiftExpectToStopAt(-6);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }
        
        [Test]
        public void When_lift_is_below_ground_and_reaches_highest_stop_on_upwards_journey_and_there_are_no_downwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            LiftMakeStartAt(-6);
 
            // Act
            _theLift.MakeUpwardsRequestFrom(-3);

            LiftExpectToLeaveFrom(-6);
            LiftExpectToVisit(-5);
            LiftExpectToVisit(-4);
            LiftExpectToStopAt(-3);

            LiftMakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-3);
            LiftExpectToStopAt(-2);

            LiftExpectToLeaveFrom(-2).Mark(Direction.Up);
            LiftExpectToVisit(-1);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            StartTest();
            
            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_lowest_stop_on_downwards_journey_but_next_upwards_request_is_lower_down_then_it_will_keep_moving_downwards_but_then_come_up()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeDownwardsRequestFrom(-3);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(-1);

            LiftMakeUpwardsRequestFrom(-5, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(-2);
            LiftExpectToStopAt(-3);

            LiftMakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-3);
            LiftExpectToStopAt(-4);

            LiftExpectToLeaveFrom(-4).Mark(Direction.Down);
            LiftExpectToStopAt(-5).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-5).Mark(Direction.Up);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_lowest_stop_on_downwards_journey_and_next_upwards_request_is_higher_up_then_it_will_go_up_to_that_caller_and_then_continue_up()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeDownwardsRequestFrom(-3);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(-1);

            _theLift.MakeUpwardsRequestFrom(-2);

            LiftExpectToVisit(-2);
            LiftExpectToStopAt(-3);

            LiftMakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-3);
            LiftExpectToStopAt(-4);

            LiftExpectToLeaveFrom(-4).Mark(Direction.Up);
            LiftExpectToVisit(-3);
            LiftExpectToVisit(-2).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-1, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-2).Mark(Direction.Up);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_lowest_stop_on_downwards_journey_and_there_are_no_upwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeDownwardsRequestFrom(-3);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(-1);
            LiftExpectToVisit(-2);
            LiftExpectToStopAt(-3);

            LiftMakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-3);
            LiftExpectToStopAt(-4);

            LiftExpectToLeaveFrom(-4).Mark(Direction.Up);
            LiftExpectToVisit(-3);
            LiftExpectToVisit(-2);
            LiftExpectToVisit(-1);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_lowest_stop_on_downwards_journey_but_next_upwards_request_is_lower_down_then_it_will_keep_moving_downwards_but_then_come_up()
        {
            // Arrange
            LiftMakeStartAt(SixthFloor);

            // Act
            _theLift.MakeDownwardsRequestFrom(ThirdFloor);

            LiftExpectToLeaveFrom(SixthFloor);
            LiftExpectToVisit(FifthFloor);

            LiftMakeUpwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(FourthFloor);
            LiftExpectToStopAt(ThirdFloor);

            LiftMakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(ThirdFloor);
            LiftExpectToStopAt(SecondFloor);

            LiftExpectToLeaveFrom(SecondFloor).Mark(Direction.Down);
            LiftExpectToStopAt(FirstFloor).Mark(Direction.None);

            LiftMakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(FirstFloor).Mark(Direction.Up);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_lowest_stop_on_downwards_journey_and_next_upwards_request_is_higher_up_then_it_will_go_up_to_that_caller_and_then_continue_up()
        {
            // Arrange
            LiftMakeStartAt(SixthFloor);

            // Act
            _theLift.MakeDownwardsRequestFrom(ThirdFloor);

            LiftExpectToLeaveFrom(SixthFloor);
            LiftExpectToVisit(FifthFloor);

            LiftMakeUpwardsRequestFrom(FifthFloor, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(FourthFloor);
            LiftExpectToStopAt(ThirdFloor);

            LiftMakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(ThirdFloor);
            LiftExpectToStopAt(SecondFloor);

            LiftExpectToLeaveFrom(SecondFloor).Mark(Direction.Up);
            LiftExpectToVisit(ThirdFloor);
            LiftExpectToVisit(FourthFloor);
            LiftExpectToStopAt(FifthFloor).Mark(Direction.None);

            LiftMakeRequestToMoveTo(SixthFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(FifthFloor).Mark(Direction.Up);
            LiftExpectToStopAt(SixthFloor);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        // Lift is above the ground floor, and there are no waiting requests (in which case it returns DOWN to the ground floor)
        public void When_lift_is_above_ground_and_reaches_lowest_stop_on_downwards_journey_and_there_are_no_upwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            LiftMakeStartAt(SixthFloor);

            // Act
            _theLift.MakeDownwardsRequestFrom(ThirdFloor);

            LiftExpectToLeaveFrom(SixthFloor);
            LiftExpectToVisit(FifthFloor);
            LiftExpectToVisit(FourthFloor);
            LiftExpectToStopAt(ThirdFloor);

            LiftMakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(ThirdFloor);
            LiftExpectToStopAt(SecondFloor);

            LiftExpectToLeaveFrom(SecondFloor).Mark(Direction.Down);
            LiftExpectToVisit(FirstFloor);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }
    }
}