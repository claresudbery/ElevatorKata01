using ElevatorKata01.Elements;
using ElevatorKata01.FunctionalCode;
using NUnit.Framework;

namespace ElevatorKata01.Tests.Tests
{
    /// <summary>
    /// All the utility code is in ElevatorUtils.cs, so that the tests themselves can be kept separate.
    /// </summary>
    [TestFixture]
    public partial class SingleLiftTests
    {
        LiftTestHelper _liftTestHelper = new LiftTestHelper();

        private const int GroundFloor = 0;
        private const int FirstFloor = 1;
        private const int SecondFloor = 2;
        private const int ThirdFloor = 3;
        private const int FourthFloor = 4;
        private const int FifthFloor = 5;
        private const int SixthFloor = 6;

        [Test]
        public void When_person_in_lift_enters_a_higher_floor_number_then_lift_starts_moving_upwards()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeRequestToMoveTo(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_lower_floor_number_then_lift_starts_moving_downwards()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(ThirdFloor);

            // Act
            _liftTestHelper.LiftMakeRequestToMoveTo(FirstFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_notifies_direction_and_location_for_every_floor_it_passes()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeRequestToMoveTo(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(FirstFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(SecondFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_notifies_its_current_location()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeRequestToMoveTo(FourthFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_goes_to_that_floor_and_stops_moving()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeRequestToMoveTo(FourthFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);
            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToVisit(ThirdFloor);
            _liftTestHelper.LiftExpectToStopAt(FourthFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_higher_floor_number_then_lift_starts_moving_upwards()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_lower_floor_number_then_lift_starts_moving_downwards()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(ThirdFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(FirstFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_notifies_direction_and_location_for_every_floor_it_passes()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(FirstFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(SecondFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_notifies_its_current_location_when_setting_off()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(FirstFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_goes_to_that_floor_and_stops_moving()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(FourthFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);
            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToVisit(ThirdFloor);
            _liftTestHelper.LiftExpectToStopAt(FourthFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_has_no_pending_requests_then_it_will_return_to_ground_floor()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);
            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_has_no_pending_requests_then_it_will_return_to_ground_floor()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeDownwardsRequestFrom(-2, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(-1);
            _liftTestHelper.LiftExpectToStopAt(-2).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(-2).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(-1);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_created_away_from_ground_floor_and_no_requests_are_made_it_will_return_to_ground_floor()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(ThirdFloor);

            // Act
            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_two_people_on_different_floors_call_lift_upwards_then_lift_will_stop_at_lower_person_first_even_though_higher_person_made_first_request()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(FourthFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToStopAt(SecondFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_moving_upwards_and_person_between_lift_and_destination_calls_lift_downwards_then_lift_will_not_pick_them_up()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(FourthFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(SecondFloor).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_two_people_on_different_floors_call_lift_upwards_and_first_person_tries_to_go_downwards_then_lift_will_go_up_for_other_person_first()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(FourthFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToStopAt(SecondFloor);

            _liftTestHelper.LiftMakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(SecondFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(ThirdFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(FourthFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_downwards_request_is_made_after_upwards_request_then_lift_will_visit_downwards_person_after_servicing_upwards_request()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(FourthFloor).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(ThirdFloor);
            _liftTestHelper.LiftExpectToStopAt(SecondFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_moves_up_and_then_down_then_it_should_not_try_to_return_to_its_previous_up_destination()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor);

            _liftTestHelper.LiftMakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.LiftExpectToStopAt(FourthFloor);

            _liftTestHelper.LiftExpectToLeaveFrom(FourthFloor);
            _liftTestHelper.LiftExpectToVisit(ThirdFloor);
            _liftTestHelper.LiftExpectToStopAt(SecondFloor);

            _liftTestHelper.LiftMakeRequestToMoveTo(FirstFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(SecondFloor);
            _liftTestHelper.LiftExpectToStopAt(FirstFloor).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(FirstFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_highest_stop_on_upwards_journey_but_next_downwards_request_is_higher_up_then_it_will_keep_moving_upwards_but_then_come_down()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(FifthFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor);

            _liftTestHelper.LiftMakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.LiftExpectToStopAt(FourthFloor);

            _liftTestHelper.LiftExpectToLeaveFrom(FourthFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(FifthFloor).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(FifthFloor).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_highest_stop_on_upwards_journey_and_next_downwards_request_is_lower_down_then_it_will_go_down_to_that_caller_and_then_continue_down()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(SecondFloor, true);

            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor);

            _liftTestHelper.LiftMakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.LiftExpectToStopAt(FourthFloor);

            _liftTestHelper.LiftExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(ThirdFloor);
            _liftTestHelper.LiftExpectToVisit(SecondFloor).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(FirstFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(SecondFloor).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_highest_stop_on_upwards_journey_and_there_are_no_downwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);
            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor);

            _liftTestHelper.LiftMakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.LiftExpectToStopAt(FourthFloor);

            _liftTestHelper.LiftExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(ThirdFloor);
            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }
        
        [Test]
        public void When_lift_is_below_ground_and_reaches_highest_stop_on_upwards_journey_but_next_downwards_request_is_higher_up_then_it_will_keep_moving_upwards_but_then_come_down()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(-6);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(-3, true);

            _liftTestHelper.LiftExpectToLeaveFrom(-6);
            _liftTestHelper.LiftExpectToVisit(-5);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(-1, shouldBeActedUponImmediately:false);

            _liftTestHelper.LiftExpectToVisit(-4);
            _liftTestHelper.LiftExpectToStopAt(-3);

            _liftTestHelper.LiftMakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-3);
            _liftTestHelper.LiftExpectToStopAt(-2);

            _liftTestHelper.LiftExpectToLeaveFrom(-2).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(-1).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);
            
            _liftTestHelper.LiftExpectToLeaveFrom(-1).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_highest_stop_on_upwards_journey_and_next_downwards_request_is_lower_down_then_it_will_go_down_to_that_caller_and_then_continue_down()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(-6);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(-3, true);

            _liftTestHelper.LiftExpectToLeaveFrom(-6);
            _liftTestHelper.LiftExpectToVisit(-5);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(-5, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(-4);
            _liftTestHelper.LiftExpectToStopAt(-3);

            _liftTestHelper.LiftMakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-3);
            _liftTestHelper.LiftExpectToStopAt(-2);

            _liftTestHelper.LiftExpectToLeaveFrom(-2).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(-3);
            _liftTestHelper.LiftExpectToVisit(-4);
            _liftTestHelper.LiftExpectToStopAt(-5).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-6, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-5).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToStopAt(-6);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }
        
        [Test]
        public void When_lift_is_below_ground_and_reaches_highest_stop_on_upwards_journey_and_there_are_no_downwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(-6);
 
            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(-3, true);

            _liftTestHelper.LiftExpectToLeaveFrom(-6);
            _liftTestHelper.LiftExpectToVisit(-5);
            _liftTestHelper.LiftExpectToVisit(-4);
            _liftTestHelper.LiftExpectToStopAt(-3);

            _liftTestHelper.LiftMakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-3);
            _liftTestHelper.LiftExpectToStopAt(-2);

            _liftTestHelper.LiftExpectToLeaveFrom(-2).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(-1);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();
            
            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_lowest_stop_on_downwards_journey_but_next_upwards_request_is_lower_down_then_it_will_keep_moving_downwards_but_then_come_up()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeDownwardsRequestFrom(-3, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(-1);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(-5, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(-2);
            _liftTestHelper.LiftExpectToStopAt(-3);

            _liftTestHelper.LiftMakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-3);
            _liftTestHelper.LiftExpectToStopAt(-4);

            _liftTestHelper.LiftExpectToLeaveFrom(-4).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToStopAt(-5).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-5).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_lowest_stop_on_downwards_journey_and_next_upwards_request_is_higher_up_then_it_will_go_up_to_that_caller_and_then_continue_up()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeDownwardsRequestFrom(-3, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(-1);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(-2, true);

            _liftTestHelper.LiftExpectToVisit(-2);
            _liftTestHelper.LiftExpectToStopAt(-3);

            _liftTestHelper.LiftMakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-3);
            _liftTestHelper.LiftExpectToStopAt(-4);

            _liftTestHelper.LiftExpectToLeaveFrom(-4).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(-3);
            _liftTestHelper.LiftExpectToVisit(-2).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-1, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-2).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_lowest_stop_on_downwards_journey_and_there_are_no_upwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeDownwardsRequestFrom(-3, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(-1);
            _liftTestHelper.LiftExpectToVisit(-2);
            _liftTestHelper.LiftExpectToStopAt(-3);

            _liftTestHelper.LiftMakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-3);
            _liftTestHelper.LiftExpectToStopAt(-4);

            _liftTestHelper.LiftExpectToLeaveFrom(-4).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(-3);
            _liftTestHelper.LiftExpectToVisit(-2);
            _liftTestHelper.LiftExpectToVisit(-1);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_lowest_stop_on_downwards_journey_but_next_upwards_request_is_lower_down_then_it_will_keep_moving_downwards_but_then_come_up()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(SixthFloor);

            // Act
            _liftTestHelper.LiftMakeDownwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(SixthFloor);
            _liftTestHelper.LiftExpectToVisit(FifthFloor);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(FourthFloor);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor);

            _liftTestHelper.LiftMakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.LiftExpectToStopAt(SecondFloor);

            _liftTestHelper.LiftExpectToLeaveFrom(SecondFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToStopAt(FirstFloor).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(FirstFloor).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_lowest_stop_on_downwards_journey_and_next_upwards_request_is_higher_up_then_it_will_go_up_to_that_caller_and_then_continue_up()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(SixthFloor);

            // Act
            _liftTestHelper.LiftMakeDownwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(SixthFloor);
            _liftTestHelper.LiftExpectToVisit(FifthFloor);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(FifthFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(FourthFloor);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor);

            _liftTestHelper.LiftMakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.LiftExpectToStopAt(SecondFloor);

            _liftTestHelper.LiftExpectToLeaveFrom(SecondFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(ThirdFloor);
            _liftTestHelper.LiftExpectToVisit(FourthFloor);
            _liftTestHelper.LiftExpectToStopAt(FifthFloor).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(SixthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(FifthFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(SixthFloor);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_lowest_stop_on_downwards_journey_and_there_are_no_upwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(SixthFloor);

            // Act
            _liftTestHelper.LiftMakeDownwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(SixthFloor);
            _liftTestHelper.LiftExpectToVisit(FifthFloor);
            _liftTestHelper.LiftExpectToVisit(FourthFloor);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor);

            _liftTestHelper.LiftMakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.LiftExpectToStopAt(SecondFloor);

            _liftTestHelper.LiftExpectToLeaveFrom(SecondFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_lift_is_moving_upwards_when_a_new_upwards_request_comes_in_from_a_lower_floor_it_is_not_processed_until_after_any_downwards_requests()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(FourthFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(SixthFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(SecondFloor);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(ThirdFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(FourthFloor);

            _liftTestHelper.LiftMakeRequestToMoveTo(FifthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(FourthFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(FifthFloor);

            _liftTestHelper.LiftExpectToLeaveFrom(FifthFloor);
            _liftTestHelper.LiftExpectToStopAt(SixthFloor).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(SixthFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(FifthFloor);
            _liftTestHelper.LiftExpectToVisit(FourthFloor);
            _liftTestHelper.LiftExpectToVisit(ThirdFloor);
            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(FirstFloor).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(FirstFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_lift_is_moving_downwards_when_a_new_downwards_request_comes_in_from_a_higher_floor_it_is_not_processed_until_after_any_upwards_requests()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeDownwardsRequestFrom(-4, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(-1);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(-6, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(-2);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(-1, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(-3).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToStopAt(-4);

            _liftTestHelper.LiftMakeRequestToMoveTo(-5, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-4).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToStopAt(-5);

            _liftTestHelper.LiftExpectToLeaveFrom(-5);
            _liftTestHelper.LiftExpectToStopAt(-6).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-6).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(-5);
            _liftTestHelper.LiftExpectToVisit(-4);
            _liftTestHelper.LiftExpectToVisit(-3);
            _liftTestHelper.LiftExpectToVisit(-2);
            _liftTestHelper.LiftExpectToVisit(-1).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToStopAt(-1).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(-1).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_two_or_more_requests_are_made_between_floors_they_are_all_serviced_correctly()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(9, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(1);
            _liftTestHelper.LiftExpectToVisit(2);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(5, shouldBeActedUponImmediately: false);
            _liftTestHelper.LiftMakeDownwardsRequestFrom(4, shouldBeActedUponImmediately: false);
            _liftTestHelper.LiftMakeUpwardsRequestFrom(7, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(3);
            _liftTestHelper.LiftExpectToVisit(4);
            _liftTestHelper.LiftExpectToStopAt(5).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(6, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(5);
            _liftTestHelper.LiftExpectToStopAt(6);

            _liftTestHelper.LiftExpectToLeaveFrom(6);
            _liftTestHelper.LiftExpectToStopAt(7).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(8, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(7);
            _liftTestHelper.LiftExpectToStopAt(8);

            _liftTestHelper.LiftExpectToLeaveFrom(8);
            _liftTestHelper.LiftExpectToStopAt(9).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(10, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(9);
            _liftTestHelper.LiftExpectToStopAt(10);

            _liftTestHelper.LiftExpectToLeaveFrom(10).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(9);
            _liftTestHelper.LiftExpectToVisit(8);
            _liftTestHelper.LiftExpectToVisit(7);
            _liftTestHelper.LiftExpectToVisit(6);
            _liftTestHelper.LiftExpectToVisit(5);
            _liftTestHelper.LiftExpectToStopAt(4).Mark(Direction.None);
            
            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_asked_to_move_to_a_floor_which_it_has_already_been_called_to_it_handles_it_correctly()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(2, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(1);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(4, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToStopAt(2);

            _liftTestHelper.LiftMakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(2);
            _liftTestHelper.LiftExpectToVisit(3);
            _liftTestHelper.LiftExpectToStopAt(4).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(4).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_asked_to_move_to_a_floor_which_it_has_already_been_asked_to_move_to_it_handles_it_correctly()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(2, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(1);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToStopAt(2);

            _liftTestHelper.LiftMakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(2);
            _liftTestHelper.LiftExpectToStopAt(3);

            _liftTestHelper.LiftMakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(3);
            _liftTestHelper.LiftExpectToStopAt(4).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(4).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_called_to_a_floor_which_it_has_already_been_asked_to_move_to_it_handles_it_correctly()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(2, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(1);
            _liftTestHelper.LiftExpectToStopAt(2);

            _liftTestHelper.LiftMakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(2);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(4, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(3);
            _liftTestHelper.LiftExpectToStopAt(4).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(4).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_called_to_a_floor_which_it_has_already_been_called_to_it_handles_it_correctly()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(4, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(1);
            _liftTestHelper.LiftExpectToVisit(2);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(4, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(3);
            _liftTestHelper.LiftExpectToStopAt(4).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(4).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_several_upwards_requests_come_through_in_a_different_order_then_they_should_still_be_visited_in_the_correct_order()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(13, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(1);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(11, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(2);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToStopAt(3).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(3);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(7, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(4);
            _liftTestHelper.LiftExpectToStopAt(5).Mark(Direction.None);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(6, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(5);
            _liftTestHelper.LiftExpectToStopAt(6).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(7, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(6);
            _liftTestHelper.LiftExpectToStopAt(7).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(15, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(7);
            _liftTestHelper.LiftExpectToVisit(8);
            _liftTestHelper.LiftExpectToVisit(9);
            _liftTestHelper.LiftExpectToVisit(10);
            _liftTestHelper.LiftExpectToStopAt(11).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(15, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(11);
            _liftTestHelper.LiftExpectToVisit(12);
            _liftTestHelper.LiftExpectToStopAt(13).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(17, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(13);
            _liftTestHelper.LiftExpectToVisit(14);
            _liftTestHelper.LiftExpectToStopAt(15).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(15);
            _liftTestHelper.LiftExpectToVisit(16);
            _liftTestHelper.LiftExpectToStopAt(17).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(17).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_several_downwards_requests_come_through_in_a_different_order_then_they_should_still_be_visited_in_the_correct_order()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeDownwardsRequestFrom(-13, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(-1);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(-11, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(-2);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(-3, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToStopAt(-3).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-5, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-3);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(-7, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(-4);
            _liftTestHelper.LiftExpectToStopAt(-5).Mark(Direction.None);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(-6, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-5);
            _liftTestHelper.LiftExpectToStopAt(-6).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-7, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-6);
            _liftTestHelper.LiftExpectToStopAt(-7).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-7);
            _liftTestHelper.LiftExpectToVisit(-8);
            _liftTestHelper.LiftExpectToVisit(-9);
            _liftTestHelper.LiftExpectToVisit(-10);
            _liftTestHelper.LiftExpectToStopAt(-11).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-11);
            _liftTestHelper.LiftExpectToVisit(-12);
            _liftTestHelper.LiftExpectToStopAt(-13).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-17, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-13);
            _liftTestHelper.LiftExpectToVisit(-14);
            _liftTestHelper.LiftExpectToStopAt(-15).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(-15);
            _liftTestHelper.LiftExpectToVisit(-16);
            _liftTestHelper.LiftExpectToStopAt(-17).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_several_downwards_requests_and_several_upwards_requests_come_through_in_a_different_order_then_they_should_still_be_visited_in_the_correct_order()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeDownwardsRequestFrom(-13, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(-1);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(-11, shouldBeActedUponImmediately: false);
            _liftTestHelper.LiftMakeUpwardsRequestFrom(5, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(-2);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(-3, shouldBeActedUponImmediately: false);
            _liftTestHelper.LiftMakeUpwardsRequestFrom(1, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToStopAt(-3).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-5, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-3);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(-7, shouldBeActedUponImmediately: false);
            _liftTestHelper.LiftMakeUpwardsRequestFrom(10, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(-4);
            _liftTestHelper.LiftExpectToStopAt(-5).Mark(Direction.None);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(-6, shouldBeActedUponImmediately: true);
            _liftTestHelper.LiftMakeUpwardsRequestFrom(2, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToLeaveFrom(-5);
            _liftTestHelper.LiftExpectToStopAt(-6).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-7, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-6);
            _liftTestHelper.LiftExpectToStopAt(-7).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-7);
            _liftTestHelper.LiftExpectToVisit(-8);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(9, shouldBeActedUponImmediately: false);
            _liftTestHelper.LiftMakeUpwardsRequestFrom(7, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToVisit(-9);
            _liftTestHelper.LiftExpectToVisit(-10);
            _liftTestHelper.LiftExpectToStopAt(-11).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-11);
            _liftTestHelper.LiftExpectToVisit(-12);
            _liftTestHelper.LiftExpectToStopAt(-13).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(-17, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(-13);
            _liftTestHelper.LiftExpectToVisit(-14);
            _liftTestHelper.LiftExpectToStopAt(-15).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(-15);
            _liftTestHelper.LiftExpectToVisit(-16);
            _liftTestHelper.LiftExpectToStopAt(-17).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(-17).Mark(Direction.Up);

            _liftTestHelper.LiftExpectToVisit(-16).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(-15);
            _liftTestHelper.LiftExpectToVisit(-14);
            _liftTestHelper.LiftExpectToVisit(-13);
            _liftTestHelper.LiftExpectToVisit(-12);
            _liftTestHelper.LiftExpectToVisit(-11);
            _liftTestHelper.LiftExpectToVisit(-10);
            _liftTestHelper.LiftExpectToVisit(-9);
            _liftTestHelper.LiftExpectToVisit(-8);
            _liftTestHelper.LiftExpectToVisit(-7);
            _liftTestHelper.LiftExpectToVisit(-6);
            _liftTestHelper.LiftExpectToVisit(-5);
            _liftTestHelper.LiftExpectToVisit(-4);
            _liftTestHelper.LiftExpectToVisit(-3);
            _liftTestHelper.LiftExpectToVisit(-2);
            _liftTestHelper.LiftExpectToVisit(-1);
            _liftTestHelper.LiftExpectToVisit(0).Mark(Direction.Up);

            _liftTestHelper.LiftExpectToStopAt(1).Mark(Direction.None);
            _liftTestHelper.LiftExpectToLeaveFrom(1).Mark(Direction.Up);

            _liftTestHelper.LiftExpectToStopAt(2).Mark(Direction.None);
            _liftTestHelper.LiftExpectToLeaveFrom(2).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(3);
            _liftTestHelper.LiftExpectToVisit(4);

            _liftTestHelper.LiftExpectToStopAt(5).Mark(Direction.None);
            _liftTestHelper.LiftExpectToLeaveFrom(5).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(6);

            _liftTestHelper.LiftExpectToStopAt(7).Mark(Direction.None);
            _liftTestHelper.LiftExpectToLeaveFrom(7).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(8);

            _liftTestHelper.LiftExpectToStopAt(9).Mark(Direction.None);
            _liftTestHelper.LiftExpectToLeaveFrom(9).Mark(Direction.Up);

            _liftTestHelper.LiftExpectToStopAt(10).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_multiple_move_requests_are_made_after_lift_has_stopped_in_response_to_call_they_are_all_serviced_correctly()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(1, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToStopAt(1).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(2, true);
            _liftTestHelper.LiftMakeRequestToMoveTo(-1, false);
            _liftTestHelper.LiftMakeRequestToMoveTo(5, false);
            _liftTestHelper.LiftMakeRequestToMoveTo(-6, false);

            _liftTestHelper.LiftExpectToLeaveFrom(1).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToStopAt(2).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(2).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(3);
            _liftTestHelper.LiftExpectToVisit(4);
            _liftTestHelper.LiftExpectToStopAt(5).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(5).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(4);
            _liftTestHelper.LiftExpectToVisit(3);
            _liftTestHelper.LiftExpectToVisit(2);
            _liftTestHelper.LiftExpectToVisit(1);
            _liftTestHelper.LiftExpectToVisit(0);
            _liftTestHelper.LiftExpectToStopAt(-1).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(-1).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(-2);
            _liftTestHelper.LiftExpectToVisit(-3);
            _liftTestHelper.LiftExpectToVisit(-4);
            _liftTestHelper.LiftExpectToVisit(-5);
            _liftTestHelper.LiftExpectToStopAt(-6).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_called_while_already_stopped_somewhere_else_it_will_respond_to_the_new_request()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);
            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToStopAt(FirstFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_lift_has_finished_up_requests_and_the_next_downwards_request_comes_from_higher_up_when_a_new_up_request_comes_in_while_travelling_to_the_down_request_then_the_up_request_will_be_ignored_until_later()
        {
            // Arrange
            _liftTestHelper.LiftMakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.LiftMakeUpwardsRequestFrom(FirstFloor, true);

            _liftTestHelper.LiftMakeDownwardsRequestFrom(FourthFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.LiftExpectToStopAt(FirstFloor);

            _liftTestHelper.LiftExpectToLeaveFrom(FirstFloor).Mark(Direction.Up);

            _liftTestHelper.LiftMakeUpwardsRequestFrom(ThirdFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToVisit(ThirdFloor);
            _liftTestHelper.LiftExpectToStopAt(FourthFloor).Mark(Direction.None);

            _liftTestHelper.LiftMakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.LiftExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            _liftTestHelper.LiftExpectToVisit(ThirdFloor);
            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);
            _liftTestHelper.LiftExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.LiftExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            _liftTestHelper.LiftExpectToVisit(FirstFloor);
            _liftTestHelper.LiftExpectToVisit(SecondFloor);
            _liftTestHelper.LiftExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }
    }
}