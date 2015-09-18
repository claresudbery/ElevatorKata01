using ElevatorKata01.Elements;
using ElevatorKata01.FunctionalCode;
using NUnit.Framework;

namespace ElevatorKata01.Tests.Tests
{
    /// <summary>
    /// All the utility code is in ElevatorUtils.cs, so that the tests themselves can be kept separate.
    /// </summary>
    [TestFixture]
    public partial class SingleLiftTests : ILiftMonitor
    {
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
        public void When_lift_is_above_ground_and_has_no_pending_requests_then_it_will_return_to_ground_floor()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(ThirdFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);
            LiftExpectToVisit(SecondFloor);
            LiftExpectToStopAt(ThirdFloor).Mark(Direction.None);

            LiftExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            LiftExpectToVisit(SecondFloor);
            LiftExpectToVisit(FirstFloor);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_has_no_pending_requests_then_it_will_return_to_ground_floor()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeDownwardsRequestFrom(-2);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(-1);
            LiftExpectToStopAt(-2).Mark(Direction.None);

            LiftExpectToLeaveFrom(-2).Mark(Direction.Up);
            LiftExpectToVisit(-1);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_created_away_from_ground_floor_and_no_requests_are_made_it_will_return_to_ground_floor()
        {
            // Arrange
            LiftMakeStartAt(ThirdFloor);

            // Act
            LiftExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            LiftExpectToVisit(SecondFloor);
            LiftExpectToVisit(FirstFloor);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

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

        [Test]
        public void Given_lift_is_moving_upwards_when_a_new_upwards_request_comes_in_from_a_lower_floor_it_is_not_processed_until_after_any_downwards_requests()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(FourthFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);

            LiftMakeDownwardsRequestFrom(SixthFloor, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(SecondFloor);

            LiftMakeUpwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(ThirdFloor).Mark(Direction.Up);
            LiftExpectToStopAt(FourthFloor);

            LiftMakeRequestToMoveTo(FifthFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(FourthFloor).Mark(Direction.Up);
            LiftExpectToStopAt(FifthFloor);

            LiftExpectToLeaveFrom(FifthFloor);
            LiftExpectToStopAt(SixthFloor).Mark(Direction.None);

            LiftMakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(SixthFloor).Mark(Direction.Down);
            LiftExpectToVisit(FifthFloor);
            LiftExpectToVisit(FourthFloor);
            LiftExpectToVisit(ThirdFloor);
            LiftExpectToVisit(SecondFloor);
            LiftExpectToVisit(FirstFloor).Mark(Direction.Down);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            LiftExpectToStopAt(FirstFloor).Mark(Direction.None);

            LiftExpectToLeaveFrom(FirstFloor).Mark(Direction.Down);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void Given_lift_is_moving_downwards_when_a_new_downwards_request_comes_in_from_a_higher_floor_it_is_not_processed_until_after_any_upwards_requests()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeDownwardsRequestFrom(-4);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(-1);

            LiftMakeUpwardsRequestFrom(-6, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(-2);

            LiftMakeDownwardsRequestFrom(-1, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(-3).Mark(Direction.Down);
            LiftExpectToStopAt(-4);

            LiftMakeRequestToMoveTo(-5, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-4).Mark(Direction.Down);
            LiftExpectToStopAt(-5);

            LiftExpectToLeaveFrom(-5);
            LiftExpectToStopAt(-6).Mark(Direction.None);

            LiftMakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-6).Mark(Direction.Up);
            LiftExpectToVisit(-5);
            LiftExpectToVisit(-4);
            LiftExpectToVisit(-3);
            LiftExpectToVisit(-2);
            LiftExpectToVisit(-1).Mark(Direction.Up);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Down);
            LiftExpectToStopAt(-1).Mark(Direction.None);

            LiftExpectToLeaveFrom(-1).Mark(Direction.Up);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_two_or_more_requests_are_made_between_floors_they_are_all_serviced_correctly()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(9);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(1);
            LiftExpectToVisit(2);

            LiftMakeUpwardsRequestFrom(5, shouldBeActedUponImmediately: false);
            LiftMakeDownwardsRequestFrom(4, shouldBeActedUponImmediately: false);
            LiftMakeUpwardsRequestFrom(7, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(3);
            LiftExpectToVisit(4);
            LiftExpectToStopAt(5).Mark(Direction.None);

            LiftMakeRequestToMoveTo(6, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(5);
            LiftExpectToStopAt(6);

            LiftExpectToLeaveFrom(6);
            LiftExpectToStopAt(7).Mark(Direction.None);

            LiftMakeRequestToMoveTo(8, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(7);
            LiftExpectToStopAt(8);

            LiftExpectToLeaveFrom(8);
            LiftExpectToStopAt(9).Mark(Direction.None);

            LiftMakeRequestToMoveTo(10, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(9);
            LiftExpectToStopAt(10);

            LiftExpectToLeaveFrom(10).Mark(Direction.Down);
            LiftExpectToVisit(9);
            LiftExpectToVisit(8);
            LiftExpectToVisit(7);
            LiftExpectToVisit(6);
            LiftExpectToVisit(5);
            LiftExpectToStopAt(4).Mark(Direction.None);
            
            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_asked_to_move_to_a_floor_which_it_has_already_been_called_to_it_handles_it_correctly()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(2);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(1);

            LiftMakeUpwardsRequestFrom(4, shouldBeActedUponImmediately: false);

            LiftExpectToStopAt(2);

            LiftMakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(2);
            LiftExpectToVisit(3);
            LiftExpectToStopAt(4).Mark(Direction.None);

            LiftMakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(4).Mark(Direction.Up);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_asked_to_move_to_a_floor_which_it_has_already_been_asked_to_move_to_it_handles_it_correctly()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(2);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(1);

            LiftMakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: false);

            LiftExpectToStopAt(2);

            LiftMakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(2);
            LiftExpectToStopAt(3);

            LiftMakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(3);
            LiftExpectToStopAt(4).Mark(Direction.None);

            LiftMakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(4).Mark(Direction.Up);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_called_to_a_floor_which_it_has_already_been_asked_to_move_to_it_handles_it_correctly()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(2);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(1);
            LiftExpectToStopAt(2);

            LiftMakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(2);

            LiftMakeUpwardsRequestFrom(4, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(3);
            LiftExpectToStopAt(4).Mark(Direction.None);

            LiftMakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(4).Mark(Direction.Up);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_called_to_a_floor_which_it_has_already_been_called_to_it_handles_it_correctly()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(4);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(1);
            LiftExpectToVisit(2);

            LiftMakeUpwardsRequestFrom(4, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(3);
            LiftExpectToStopAt(4).Mark(Direction.None);

            LiftMakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(4).Mark(Direction.Up);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_several_upwards_requests_come_through_in_a_different_order_then_they_should_still_be_visited_in_the_correct_order()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(13);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(1);

            LiftMakeUpwardsRequestFrom(11, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(2);

            LiftMakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: false);

            LiftExpectToStopAt(3).Mark(Direction.None);

            LiftMakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(3);

            LiftMakeUpwardsRequestFrom(7, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(4);
            LiftExpectToStopAt(5).Mark(Direction.None);

            LiftMakeUpwardsRequestFrom(6, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(5);
            LiftExpectToStopAt(6).Mark(Direction.None);

            LiftMakeRequestToMoveTo(7, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(6);
            LiftExpectToStopAt(7).Mark(Direction.None);

            LiftMakeRequestToMoveTo(15, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(7);
            LiftExpectToVisit(8);
            LiftExpectToVisit(9);
            LiftExpectToVisit(10);
            LiftExpectToStopAt(11).Mark(Direction.None);

            LiftMakeRequestToMoveTo(15, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(11);
            LiftExpectToVisit(12);
            LiftExpectToStopAt(13).Mark(Direction.None);

            LiftMakeRequestToMoveTo(17, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(13);
            LiftExpectToVisit(14);
            LiftExpectToStopAt(15).Mark(Direction.None);

            LiftExpectToLeaveFrom(15);
            LiftExpectToVisit(16);
            LiftExpectToStopAt(17).Mark(Direction.None);

            LiftExpectToLeaveFrom(17).Mark(Direction.Down);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_several_downwards_requests_come_through_in_a_different_order_then_they_should_still_be_visited_in_the_correct_order()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeDownwardsRequestFrom(-13);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(-1);

            LiftMakeDownwardsRequestFrom(-11, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(-2);

            LiftMakeDownwardsRequestFrom(-3, shouldBeActedUponImmediately: false);

            LiftExpectToStopAt(-3).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-5, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-3);

            LiftMakeDownwardsRequestFrom(-7, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(-4);
            LiftExpectToStopAt(-5).Mark(Direction.None);

            LiftMakeDownwardsRequestFrom(-6, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-5);
            LiftExpectToStopAt(-6).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-7, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-6);
            LiftExpectToStopAt(-7).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-7);
            LiftExpectToVisit(-8);
            LiftExpectToVisit(-9);
            LiftExpectToVisit(-10);
            LiftExpectToStopAt(-11).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-11);
            LiftExpectToVisit(-12);
            LiftExpectToStopAt(-13).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-17, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-13);
            LiftExpectToVisit(-14);
            LiftExpectToStopAt(-15).Mark(Direction.None);

            LiftExpectToLeaveFrom(-15);
            LiftExpectToVisit(-16);
            LiftExpectToStopAt(-17).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_several_downwards_requests_and_several_upwards_requests_come_through_in_a_different_order_then_they_should_still_be_visited_in_the_correct_order()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeDownwardsRequestFrom(-13);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(-1);

            LiftMakeDownwardsRequestFrom(-11, shouldBeActedUponImmediately: false);
            LiftMakeUpwardsRequestFrom(5, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(-2);

            LiftMakeDownwardsRequestFrom(-3, shouldBeActedUponImmediately: false);
            LiftMakeUpwardsRequestFrom(1, shouldBeActedUponImmediately: false);

            LiftExpectToStopAt(-3).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-5, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-3);

            LiftMakeDownwardsRequestFrom(-7, shouldBeActedUponImmediately: false);
            LiftMakeUpwardsRequestFrom(10, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(-4);
            LiftExpectToStopAt(-5).Mark(Direction.None);

            LiftMakeDownwardsRequestFrom(-6, shouldBeActedUponImmediately: true);
            LiftMakeUpwardsRequestFrom(2, shouldBeActedUponImmediately: false);

            LiftExpectToLeaveFrom(-5);
            LiftExpectToStopAt(-6).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-7, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-6);
            LiftExpectToStopAt(-7).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-7);
            LiftExpectToVisit(-8);

            LiftMakeUpwardsRequestFrom(9, shouldBeActedUponImmediately: false);
            LiftMakeUpwardsRequestFrom(7, shouldBeActedUponImmediately: false);

            LiftExpectToVisit(-9);
            LiftExpectToVisit(-10);
            LiftExpectToStopAt(-11).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-11);
            LiftExpectToVisit(-12);
            LiftExpectToStopAt(-13).Mark(Direction.None);

            LiftMakeRequestToMoveTo(-17, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(-13);
            LiftExpectToVisit(-14);
            LiftExpectToStopAt(-15).Mark(Direction.None);

            LiftExpectToLeaveFrom(-15);
            LiftExpectToVisit(-16);
            LiftExpectToStopAt(-17).Mark(Direction.None);

            LiftExpectToLeaveFrom(-17).Mark(Direction.Up);

            LiftExpectToVisit(-16).Mark(Direction.Up);
            LiftExpectToVisit(-15);
            LiftExpectToVisit(-14);
            LiftExpectToVisit(-13);
            LiftExpectToVisit(-12);
            LiftExpectToVisit(-11);
            LiftExpectToVisit(-10);
            LiftExpectToVisit(-9);
            LiftExpectToVisit(-8);
            LiftExpectToVisit(-7);
            LiftExpectToVisit(-6);
            LiftExpectToVisit(-5);
            LiftExpectToVisit(-4);
            LiftExpectToVisit(-3);
            LiftExpectToVisit(-2);
            LiftExpectToVisit(-1);
            LiftExpectToVisit(0).Mark(Direction.Up);

            LiftExpectToStopAt(1).Mark(Direction.None);
            LiftExpectToLeaveFrom(1).Mark(Direction.Up);

            LiftExpectToStopAt(2).Mark(Direction.None);
            LiftExpectToLeaveFrom(2).Mark(Direction.Up);
            LiftExpectToVisit(3);
            LiftExpectToVisit(4);

            LiftExpectToStopAt(5).Mark(Direction.None);
            LiftExpectToLeaveFrom(5).Mark(Direction.Up);
            LiftExpectToVisit(6);

            LiftExpectToStopAt(7).Mark(Direction.None);
            LiftExpectToLeaveFrom(7).Mark(Direction.Up);
            LiftExpectToVisit(8);

            LiftExpectToStopAt(9).Mark(Direction.None);
            LiftExpectToLeaveFrom(9).Mark(Direction.Up);

            LiftExpectToStopAt(10).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_multiple_move_requests_are_made_after_lift_has_stopped_in_response_to_call_they_are_all_serviced_correctly()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(1);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToStopAt(1).Mark(Direction.None);

            LiftMakeRequestToMoveTo(2, true);
            LiftMakeRequestToMoveTo(-1, false);
            LiftMakeRequestToMoveTo(5, false);
            LiftMakeRequestToMoveTo(-6, false);

            LiftExpectToLeaveFrom(1).Mark(Direction.Up);
            LiftExpectToStopAt(2).Mark(Direction.None);

            LiftExpectToLeaveFrom(2).Mark(Direction.Up);
            LiftExpectToVisit(3);
            LiftExpectToVisit(4);
            LiftExpectToStopAt(5).Mark(Direction.None);

            LiftExpectToLeaveFrom(5).Mark(Direction.Down);
            LiftExpectToVisit(4);
            LiftExpectToVisit(3);
            LiftExpectToVisit(2);
            LiftExpectToVisit(1);
            LiftExpectToVisit(0);
            LiftExpectToStopAt(-1).Mark(Direction.None);

            LiftExpectToLeaveFrom(-1).Mark(Direction.Down);
            LiftExpectToVisit(-2);
            LiftExpectToVisit(-3);
            LiftExpectToVisit(-4);
            LiftExpectToVisit(-5);
            LiftExpectToStopAt(-6).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_called_while_already_stopped_somewhere_else_it_will_respond_to_the_new_request()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(ThirdFloor);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToVisit(FirstFloor);
            LiftExpectToVisit(SecondFloor);
            LiftExpectToStopAt(ThirdFloor).Mark(Direction.None);

            LiftMakeDownwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            LiftExpectToVisit(SecondFloor);
            LiftExpectToStopAt(FirstFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }

        [Test]
        public void Given_lift_has_finished_up_requests_and_the_next_downwards_request_comes_from_higher_up_when_a_new_up_request_comes_in_while_travelling_to_the_down_request_then_the_up_request_will_be_ignored_until_later()
        {
            // Arrange
            LiftMakeStartAt(GroundFloor);

            // Act
            _theLift.MakeUpwardsRequestFrom(FirstFloor);

            LiftMakeDownwardsRequestFrom(FourthFloor, shouldBeActedUponImmediately: false);

            LiftExpectToLeaveFrom(GroundFloor);
            LiftExpectToStopAt(FirstFloor);

            LiftExpectToLeaveFrom(FirstFloor).Mark(Direction.Up);

            LiftMakeUpwardsRequestFrom(ThirdFloor, shouldBeActedUponImmediately: true);

            LiftExpectToVisit(SecondFloor);
            LiftExpectToVisit(ThirdFloor);
            LiftExpectToStopAt(FourthFloor).Mark(Direction.None);

            LiftMakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            LiftExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            LiftExpectToVisit(ThirdFloor);
            LiftExpectToVisit(SecondFloor);
            LiftExpectToVisit(FirstFloor);
            LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            LiftExpectToVisit(FirstFloor);
            LiftExpectToVisit(SecondFloor);
            LiftExpectToStopAt(ThirdFloor).Mark(Direction.None);

            StartTest();

            // Assert
            VerifyAllMarkers();
        }
    }
}