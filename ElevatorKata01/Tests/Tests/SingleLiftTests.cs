using ElevatorKata01.Elements;
using ElevatorKata01.FunctionalCode;
using ElevatorKata01.Tests.Helpers;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace ElevatorKata01.Tests.Tests
{
    [TestFixture]
    public class SingleLiftTests
    {
        private readonly TestScheduler _testScheduler;
        readonly IndividualLiftTestHelper _individualLiftTestHelper;

        private const int GroundFloor = 0;
        private const int FirstFloor = 1;
        private const int SecondFloor = 2;
        private const int ThirdFloor = 3;
        private const int FourthFloor = 4;
        private const int FifthFloor = 5;
        private const int SixthFloor = 6;

        public SingleLiftTests()
        {
            _testScheduler = new TestScheduler();
            _individualLiftTestHelper = new IndividualLiftTestHelper("Default Lift", _testScheduler);
        }

        [Test]
        public void When_person_in_lift_enters_a_higher_floor_number_then_lift_starts_moving_upwards()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeRequestToMoveTo(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            _individualLiftTestHelper.StartTest();
            
            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_lower_floor_number_then_lift_starts_moving_downwards()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(ThirdFloor);

            // Act
            _individualLiftTestHelper.MakeRequestToMoveTo(FirstFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_notifies_direction_and_location_for_every_floor_it_passes()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeRequestToMoveTo(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_notifies_its_current_location()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeRequestToMoveTo(FourthFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_goes_to_that_floor_and_stops_moving()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeRequestToMoveTo(FourthFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToVisit(ThirdFloor);
            _individualLiftTestHelper.ExpectToStopAt(FourthFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_higher_floor_number_then_lift_starts_moving_upwards()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_lower_floor_number_then_lift_starts_moving_downwards()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(ThirdFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(FirstFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_notifies_direction_and_location_for_every_floor_it_passes()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_notifies_its_current_location_when_setting_off()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(FirstFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_goes_to_that_floor_and_stops_moving()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(FourthFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToVisit(ThirdFloor);
            _individualLiftTestHelper.ExpectToStopAt(FourthFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_has_no_pending_requests_then_it_will_return_to_ground_floor()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_has_no_pending_requests_then_it_will_return_to_ground_floor()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeDownwardsRequestFrom(-2, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(-1);
            _individualLiftTestHelper.ExpectToStopAt(-2).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(-2).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(-1);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_created_away_from_ground_floor_and_no_requests_are_made_it_will_return_to_ground_floor()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(ThirdFloor);

            // Act
            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_two_people_on_different_floors_call_lift_upwards_then_lift_will_stop_at_lower_person_first_even_though_higher_person_made_first_request()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(FourthFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToStopAt(SecondFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_moving_upwards_and_person_between_lift_and_destination_calls_lift_downwards_then_lift_will_not_pick_them_up()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(FourthFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(SecondFloor).Mark(Direction.Up);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_two_people_on_different_floors_call_lift_upwards_and_first_person_tries_to_go_downwards_then_lift_will_go_up_for_other_person_first()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(FourthFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToStopAt(SecondFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(SecondFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(ThirdFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(FourthFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_downwards_request_is_made_after_upwards_request_then_lift_will_visit_downwards_person_after_servicing_upwards_request()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(FourthFloor).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(ThirdFloor);
            _individualLiftTestHelper.ExpectToStopAt(SecondFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_moves_up_and_then_down_then_it_should_not_try_to_return_to_its_previous_up_destination()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _individualLiftTestHelper.ExpectToStopAt(FourthFloor);

            _individualLiftTestHelper.ExpectToLeaveFrom(FourthFloor);
            _individualLiftTestHelper.ExpectToVisit(ThirdFloor);
            _individualLiftTestHelper.ExpectToStopAt(SecondFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(FirstFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(SecondFloor);
            _individualLiftTestHelper.ExpectToStopAt(FirstFloor).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(FirstFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_highest_stop_on_upwards_journey_but_next_downwards_request_is_higher_up_then_it_will_keep_moving_upwards_but_then_come_down()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(FifthFloor, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _individualLiftTestHelper.ExpectToStopAt(FourthFloor);

            _individualLiftTestHelper.ExpectToLeaveFrom(FourthFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(FifthFloor).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(FifthFloor).Mark(Direction.Down);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_highest_stop_on_upwards_journey_and_next_downwards_request_is_lower_down_then_it_will_go_down_to_that_caller_and_then_continue_down()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(SecondFloor, true);

            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _individualLiftTestHelper.ExpectToStopAt(FourthFloor);

            _individualLiftTestHelper.ExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(ThirdFloor);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(FirstFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(SecondFloor).Mark(Direction.Down);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_highest_stop_on_upwards_journey_and_there_are_no_downwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _individualLiftTestHelper.ExpectToStopAt(FourthFloor);

            _individualLiftTestHelper.ExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(ThirdFloor);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }
        
        [Test]
        public void When_lift_is_below_ground_and_reaches_highest_stop_on_upwards_journey_but_next_downwards_request_is_higher_up_then_it_will_keep_moving_upwards_but_then_come_down()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(-6);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(-3, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-6);
            _individualLiftTestHelper.ExpectToVisit(-5);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(-1, shouldBeActedUponImmediately:false);

            _individualLiftTestHelper.ExpectToVisit(-4);
            _individualLiftTestHelper.ExpectToStopAt(-3);

            _individualLiftTestHelper.MakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-3);
            _individualLiftTestHelper.ExpectToStopAt(-2);

            _individualLiftTestHelper.ExpectToLeaveFrom(-2).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(-1).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);
            
            _individualLiftTestHelper.ExpectToLeaveFrom(-1).Mark(Direction.Down);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_highest_stop_on_upwards_journey_and_next_downwards_request_is_lower_down_then_it_will_go_down_to_that_caller_and_then_continue_down()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(-6);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(-3, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-6);
            _individualLiftTestHelper.ExpectToVisit(-5);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(-5, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(-4);
            _individualLiftTestHelper.ExpectToStopAt(-3);

            _individualLiftTestHelper.MakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-3);
            _individualLiftTestHelper.ExpectToStopAt(-2);

            _individualLiftTestHelper.ExpectToLeaveFrom(-2).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(-3);
            _individualLiftTestHelper.ExpectToVisit(-4);
            _individualLiftTestHelper.ExpectToStopAt(-5).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-6, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-5).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToStopAt(-6);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }
        
        [Test]
        public void When_lift_is_below_ground_and_reaches_highest_stop_on_upwards_journey_and_there_are_no_downwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(-6);
 
            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(-3, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-6);
            _individualLiftTestHelper.ExpectToVisit(-5);
            _individualLiftTestHelper.ExpectToVisit(-4);
            _individualLiftTestHelper.ExpectToStopAt(-3);

            _individualLiftTestHelper.MakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-3);
            _individualLiftTestHelper.ExpectToStopAt(-2);

            _individualLiftTestHelper.ExpectToLeaveFrom(-2).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(-1);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();
            
            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_lowest_stop_on_downwards_journey_but_next_upwards_request_is_lower_down_then_it_will_keep_moving_downwards_but_then_come_up()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeDownwardsRequestFrom(-3, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(-1);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(-5, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(-2);
            _individualLiftTestHelper.ExpectToStopAt(-3);

            _individualLiftTestHelper.MakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-3);
            _individualLiftTestHelper.ExpectToStopAt(-4);

            _individualLiftTestHelper.ExpectToLeaveFrom(-4).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToStopAt(-5).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-5).Mark(Direction.Up);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_lowest_stop_on_downwards_journey_and_next_upwards_request_is_higher_up_then_it_will_go_up_to_that_caller_and_then_continue_up()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeDownwardsRequestFrom(-3, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(-1);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(-2, true);

            _individualLiftTestHelper.ExpectToVisit(-2);
            _individualLiftTestHelper.ExpectToStopAt(-3);

            _individualLiftTestHelper.MakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-3);
            _individualLiftTestHelper.ExpectToStopAt(-4);

            _individualLiftTestHelper.ExpectToLeaveFrom(-4).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(-3);
            _individualLiftTestHelper.ExpectToVisit(-2).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-1, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-2).Mark(Direction.Up);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_lowest_stop_on_downwards_journey_and_there_are_no_upwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeDownwardsRequestFrom(-3, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(-1);
            _individualLiftTestHelper.ExpectToVisit(-2);
            _individualLiftTestHelper.ExpectToStopAt(-3);

            _individualLiftTestHelper.MakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-3);
            _individualLiftTestHelper.ExpectToStopAt(-4);

            _individualLiftTestHelper.ExpectToLeaveFrom(-4).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(-3);
            _individualLiftTestHelper.ExpectToVisit(-2);
            _individualLiftTestHelper.ExpectToVisit(-1);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_lowest_stop_on_downwards_journey_but_next_upwards_request_is_lower_down_then_it_will_keep_moving_downwards_but_then_come_up()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(SixthFloor);

            // Act
            _individualLiftTestHelper.MakeDownwardsRequestFrom(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(SixthFloor);
            _individualLiftTestHelper.ExpectToVisit(FifthFloor);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(FourthFloor);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _individualLiftTestHelper.ExpectToStopAt(SecondFloor);

            _individualLiftTestHelper.ExpectToLeaveFrom(SecondFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToStopAt(FirstFloor).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(FirstFloor).Mark(Direction.Up);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_lowest_stop_on_downwards_journey_and_next_upwards_request_is_higher_up_then_it_will_go_up_to_that_caller_and_then_continue_up()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(SixthFloor);

            // Act
            _individualLiftTestHelper.MakeDownwardsRequestFrom(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(SixthFloor);
            _individualLiftTestHelper.ExpectToVisit(FifthFloor);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(FifthFloor, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(FourthFloor);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _individualLiftTestHelper.ExpectToStopAt(SecondFloor);

            _individualLiftTestHelper.ExpectToLeaveFrom(SecondFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(ThirdFloor);
            _individualLiftTestHelper.ExpectToVisit(FourthFloor);
            _individualLiftTestHelper.ExpectToStopAt(FifthFloor).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(SixthFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(FifthFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(SixthFloor);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_lowest_stop_on_downwards_journey_and_there_are_no_upwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(SixthFloor);

            // Act
            _individualLiftTestHelper.MakeDownwardsRequestFrom(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(SixthFloor);
            _individualLiftTestHelper.ExpectToVisit(FifthFloor);
            _individualLiftTestHelper.ExpectToVisit(FourthFloor);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _individualLiftTestHelper.ExpectToStopAt(SecondFloor);

            _individualLiftTestHelper.ExpectToLeaveFrom(SecondFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_lift_is_moving_upwards_when_a_new_upwards_request_comes_in_from_a_lower_floor_it_is_not_processed_until_after_any_downwards_requests()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(FourthFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(SixthFloor, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(SecondFloor);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(ThirdFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(FourthFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(FifthFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(FourthFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(FifthFloor);

            _individualLiftTestHelper.ExpectToLeaveFrom(FifthFloor);
            _individualLiftTestHelper.ExpectToStopAt(SixthFloor).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(SixthFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(FifthFloor);
            _individualLiftTestHelper.ExpectToVisit(FourthFloor);
            _individualLiftTestHelper.ExpectToVisit(ThirdFloor);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(FirstFloor).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(FirstFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_lift_is_moving_downwards_when_a_new_downwards_request_comes_in_from_a_higher_floor_it_is_not_processed_until_after_any_upwards_requests()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeDownwardsRequestFrom(-4, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(-1);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(-6, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(-2);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(-1, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(-3).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToStopAt(-4);

            _individualLiftTestHelper.MakeRequestToMoveTo(-5, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-4).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToStopAt(-5);

            _individualLiftTestHelper.ExpectToLeaveFrom(-5);
            _individualLiftTestHelper.ExpectToStopAt(-6).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-6).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(-5);
            _individualLiftTestHelper.ExpectToVisit(-4);
            _individualLiftTestHelper.ExpectToVisit(-3);
            _individualLiftTestHelper.ExpectToVisit(-2);
            _individualLiftTestHelper.ExpectToVisit(-1).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToStopAt(-1).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(-1).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_two_or_more_requests_are_made_between_floors_they_are_all_serviced_correctly()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(9, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(1);
            _individualLiftTestHelper.ExpectToVisit(2);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(5, shouldBeActedUponImmediately: false);
            _individualLiftTestHelper.MakeDownwardsRequestFrom(4, shouldBeActedUponImmediately: false);
            _individualLiftTestHelper.MakeUpwardsRequestFrom(7, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(3);
            _individualLiftTestHelper.ExpectToVisit(4);
            _individualLiftTestHelper.ExpectToStopAt(5).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(6, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(5);
            _individualLiftTestHelper.ExpectToStopAt(6);

            _individualLiftTestHelper.ExpectToLeaveFrom(6);
            _individualLiftTestHelper.ExpectToStopAt(7).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(8, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(7);
            _individualLiftTestHelper.ExpectToStopAt(8);

            _individualLiftTestHelper.ExpectToLeaveFrom(8);
            _individualLiftTestHelper.ExpectToStopAt(9).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(10, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(9);
            _individualLiftTestHelper.ExpectToStopAt(10);

            _individualLiftTestHelper.ExpectToLeaveFrom(10).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(9);
            _individualLiftTestHelper.ExpectToVisit(8);
            _individualLiftTestHelper.ExpectToVisit(7);
            _individualLiftTestHelper.ExpectToVisit(6);
            _individualLiftTestHelper.ExpectToVisit(5);
            _individualLiftTestHelper.ExpectToStopAt(4).Mark(Direction.None);
            
            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_asked_to_move_to_a_floor_which_it_has_already_been_called_to_it_handles_it_correctly()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(2, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(1);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(4, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToStopAt(2);

            _individualLiftTestHelper.MakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(2);
            _individualLiftTestHelper.ExpectToVisit(3);
            _individualLiftTestHelper.ExpectToStopAt(4).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(4).Mark(Direction.Up);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_asked_to_move_to_a_floor_which_it_has_already_been_asked_to_move_to_it_handles_it_correctly()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(2, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(1);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToStopAt(2);

            _individualLiftTestHelper.MakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(2);
            _individualLiftTestHelper.ExpectToStopAt(3);

            _individualLiftTestHelper.MakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(3);
            _individualLiftTestHelper.ExpectToStopAt(4).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(4).Mark(Direction.Up);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_called_to_a_floor_which_it_has_already_been_asked_to_move_to_it_handles_it_correctly()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(2, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(1);
            _individualLiftTestHelper.ExpectToStopAt(2);

            _individualLiftTestHelper.MakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(2);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(4, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(3);
            _individualLiftTestHelper.ExpectToStopAt(4).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(4).Mark(Direction.Up);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_called_to_a_floor_which_it_has_already_been_called_to_it_handles_it_correctly()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(4, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(1);
            _individualLiftTestHelper.ExpectToVisit(2);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(4, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(3);
            _individualLiftTestHelper.ExpectToStopAt(4).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(4).Mark(Direction.Up);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_several_upwards_requests_come_through_in_a_different_order_then_they_should_still_be_visited_in_the_correct_order()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(13, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(1);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(11, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(2);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToStopAt(3).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(3);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(7, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(4);
            _individualLiftTestHelper.ExpectToStopAt(5).Mark(Direction.None);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(6, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(5);
            _individualLiftTestHelper.ExpectToStopAt(6).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(7, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(6);
            _individualLiftTestHelper.ExpectToStopAt(7).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(15, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(7);
            _individualLiftTestHelper.ExpectToVisit(8);
            _individualLiftTestHelper.ExpectToVisit(9);
            _individualLiftTestHelper.ExpectToVisit(10);
            _individualLiftTestHelper.ExpectToStopAt(11).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(15, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(11);
            _individualLiftTestHelper.ExpectToVisit(12);
            _individualLiftTestHelper.ExpectToStopAt(13).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(17, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(13);
            _individualLiftTestHelper.ExpectToVisit(14);
            _individualLiftTestHelper.ExpectToStopAt(15).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(15);
            _individualLiftTestHelper.ExpectToVisit(16);
            _individualLiftTestHelper.ExpectToStopAt(17).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(17).Mark(Direction.Down);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_several_downwards_requests_come_through_in_a_different_order_then_they_should_still_be_visited_in_the_correct_order()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeDownwardsRequestFrom(-13, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(-1);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(-11, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(-2);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(-3, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToStopAt(-3).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-5, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-3);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(-7, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(-4);
            _individualLiftTestHelper.ExpectToStopAt(-5).Mark(Direction.None);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(-6, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-5);
            _individualLiftTestHelper.ExpectToStopAt(-6).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-7, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-6);
            _individualLiftTestHelper.ExpectToStopAt(-7).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-7);
            _individualLiftTestHelper.ExpectToVisit(-8);
            _individualLiftTestHelper.ExpectToVisit(-9);
            _individualLiftTestHelper.ExpectToVisit(-10);
            _individualLiftTestHelper.ExpectToStopAt(-11).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-11);
            _individualLiftTestHelper.ExpectToVisit(-12);
            _individualLiftTestHelper.ExpectToStopAt(-13).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-17, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-13);
            _individualLiftTestHelper.ExpectToVisit(-14);
            _individualLiftTestHelper.ExpectToStopAt(-15).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(-15);
            _individualLiftTestHelper.ExpectToVisit(-16);
            _individualLiftTestHelper.ExpectToStopAt(-17).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_several_downwards_requests_and_several_upwards_requests_come_through_in_a_different_order_then_they_should_still_be_visited_in_the_correct_order()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeDownwardsRequestFrom(-13, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(-1);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(-11, shouldBeActedUponImmediately: false);
            _individualLiftTestHelper.MakeUpwardsRequestFrom(5, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(-2);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(-3, shouldBeActedUponImmediately: false);
            _individualLiftTestHelper.MakeUpwardsRequestFrom(1, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToStopAt(-3).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-5, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-3);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(-7, shouldBeActedUponImmediately: false);
            _individualLiftTestHelper.MakeUpwardsRequestFrom(10, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(-4);
            _individualLiftTestHelper.ExpectToStopAt(-5).Mark(Direction.None);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(-6, shouldBeActedUponImmediately: true);
            _individualLiftTestHelper.MakeUpwardsRequestFrom(2, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToLeaveFrom(-5);
            _individualLiftTestHelper.ExpectToStopAt(-6).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-7, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-6);
            _individualLiftTestHelper.ExpectToStopAt(-7).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-7);
            _individualLiftTestHelper.ExpectToVisit(-8);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(9, shouldBeActedUponImmediately: false);
            _individualLiftTestHelper.MakeUpwardsRequestFrom(7, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToVisit(-9);
            _individualLiftTestHelper.ExpectToVisit(-10);
            _individualLiftTestHelper.ExpectToStopAt(-11).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-11);
            _individualLiftTestHelper.ExpectToVisit(-12);
            _individualLiftTestHelper.ExpectToStopAt(-13).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(-17, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(-13);
            _individualLiftTestHelper.ExpectToVisit(-14);
            _individualLiftTestHelper.ExpectToStopAt(-15).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(-15);
            _individualLiftTestHelper.ExpectToVisit(-16);
            _individualLiftTestHelper.ExpectToStopAt(-17).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(-17).Mark(Direction.Up);

            _individualLiftTestHelper.ExpectToVisit(-16).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(-15);
            _individualLiftTestHelper.ExpectToVisit(-14);
            _individualLiftTestHelper.ExpectToVisit(-13);
            _individualLiftTestHelper.ExpectToVisit(-12);
            _individualLiftTestHelper.ExpectToVisit(-11);
            _individualLiftTestHelper.ExpectToVisit(-10);
            _individualLiftTestHelper.ExpectToVisit(-9);
            _individualLiftTestHelper.ExpectToVisit(-8);
            _individualLiftTestHelper.ExpectToVisit(-7);
            _individualLiftTestHelper.ExpectToVisit(-6);
            _individualLiftTestHelper.ExpectToVisit(-5);
            _individualLiftTestHelper.ExpectToVisit(-4);
            _individualLiftTestHelper.ExpectToVisit(-3);
            _individualLiftTestHelper.ExpectToVisit(-2);
            _individualLiftTestHelper.ExpectToVisit(-1);
            _individualLiftTestHelper.ExpectToVisit(0).Mark(Direction.Up);

            _individualLiftTestHelper.ExpectToStopAt(1).Mark(Direction.None);
            _individualLiftTestHelper.ExpectToLeaveFrom(1).Mark(Direction.Up);

            _individualLiftTestHelper.ExpectToStopAt(2).Mark(Direction.None);
            _individualLiftTestHelper.ExpectToLeaveFrom(2).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(3);
            _individualLiftTestHelper.ExpectToVisit(4);

            _individualLiftTestHelper.ExpectToStopAt(5).Mark(Direction.None);
            _individualLiftTestHelper.ExpectToLeaveFrom(5).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(6);

            _individualLiftTestHelper.ExpectToStopAt(7).Mark(Direction.None);
            _individualLiftTestHelper.ExpectToLeaveFrom(7).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(8);

            _individualLiftTestHelper.ExpectToStopAt(9).Mark(Direction.None);
            _individualLiftTestHelper.ExpectToLeaveFrom(9).Mark(Direction.Up);

            _individualLiftTestHelper.ExpectToStopAt(10).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_multiple_move_requests_are_made_after_lift_has_stopped_in_response_to_call_they_are_all_serviced_correctly()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(1, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToStopAt(1).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(2, true);
            _individualLiftTestHelper.MakeRequestToMoveTo(-1, false);
            _individualLiftTestHelper.MakeRequestToMoveTo(5, false);
            _individualLiftTestHelper.MakeRequestToMoveTo(-6, false);

            _individualLiftTestHelper.ExpectToLeaveFrom(1).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(2).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(2).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(3);
            _individualLiftTestHelper.ExpectToVisit(4);
            _individualLiftTestHelper.ExpectToStopAt(5).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(5).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(4);
            _individualLiftTestHelper.ExpectToVisit(3);
            _individualLiftTestHelper.ExpectToVisit(2);
            _individualLiftTestHelper.ExpectToVisit(1);
            _individualLiftTestHelper.ExpectToVisit(0);
            _individualLiftTestHelper.ExpectToStopAt(-1).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(-1).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(-2);
            _individualLiftTestHelper.ExpectToVisit(-3);
            _individualLiftTestHelper.ExpectToVisit(-4);
            _individualLiftTestHelper.ExpectToVisit(-5);
            _individualLiftTestHelper.ExpectToStopAt(-6).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_called_while_already_stopped_somewhere_else_it_will_respond_to_the_new_request()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToStopAt(FirstFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_lift_has_finished_up_requests_and_the_next_downwards_request_comes_from_higher_up_when_a_new_up_request_comes_in_while_travelling_to_the_down_request_then_the_up_request_will_be_ignored_until_later()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(FirstFloor, true);

            _individualLiftTestHelper.MakeDownwardsRequestFrom(FourthFloor, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToStopAt(FirstFloor);

            _individualLiftTestHelper.ExpectToLeaveFrom(FirstFloor).Mark(Direction.Up);

            _individualLiftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToVisit(ThirdFloor);
            _individualLiftTestHelper.ExpectToStopAt(FourthFloor).Mark(Direction.None);

            _individualLiftTestHelper.MakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(ThirdFloor);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_person_calls_lift_to_go_upwards__When_person_then_makes_downwards_move_request__Then_lift_services_all_upwards_requests_before_moving_downwards()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: true);
            _individualLiftTestHelper.MakeUpwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToStopAt(FirstFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(ThirdFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(FirstFloor);
            _individualLiftTestHelper.ExpectToStopAt(SecondFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: false);

            _individualLiftTestHelper.ExpectToLeaveFrom(SecondFloor).Mark(Direction.Up);
            _individualLiftTestHelper.ExpectToStopAt(ThirdFloor);

            _individualLiftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToVisit(SecondFloor);
            _individualLiftTestHelper.ExpectToVisit(FirstFloor);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_person_calls_lift_to_go_upwards__When_person_then_makes_downwards_move_request_but_there_are_no_pending_up_requests__Then_lift_will_go_downwards_as_requested()
        {
            // Arrange
            _individualLiftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _individualLiftTestHelper.MakeUpwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _individualLiftTestHelper.ExpectToStopAt(FirstFloor);

            _individualLiftTestHelper.MakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _individualLiftTestHelper.ExpectToLeaveFrom(FirstFloor).Mark(Direction.Down);
            _individualLiftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _individualLiftTestHelper.StartTest();

            // Assert
            _individualLiftTestHelper.VerifyAllMarkers();
        }
    }
}