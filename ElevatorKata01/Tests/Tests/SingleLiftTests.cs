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
        readonly IndividualLiftTestHelper _liftTestHelper;

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
            _liftTestHelper = new IndividualLiftTestHelper("Default Lift", _testScheduler);
        }

        [Test]
        public void When_person_in_lift_enters_a_higher_floor_number_then_lift_starts_moving_upwards()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeRequestToMoveTo(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            _liftTestHelper.StartTest();
            
            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_lower_floor_number_then_lift_starts_moving_downwards()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(ThirdFloor);

            // Act
            _liftTestHelper.MakeRequestToMoveTo(FirstFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_notifies_direction_and_location_for_every_floor_it_passes()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeRequestToMoveTo(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(FirstFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(SecondFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_notifies_its_current_location()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeRequestToMoveTo(FourthFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_goes_to_that_floor_and_stops_moving()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeRequestToMoveTo(FourthFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);
            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToVisit(ThirdFloor);
            _liftTestHelper.ExpectToStopAt(FourthFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_higher_floor_number_then_lift_starts_moving_upwards()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_lower_floor_number_then_lift_starts_moving_downwards()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(ThirdFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(FirstFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_notifies_direction_and_location_for_every_floor_it_passes()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(FirstFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(SecondFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_notifies_its_current_location_when_setting_off()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(FirstFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_goes_to_that_floor_and_stops_moving()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(FourthFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);
            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToVisit(ThirdFloor);
            _liftTestHelper.ExpectToStopAt(FourthFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_has_no_pending_requests_then_it_will_return_to_ground_floor()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);
            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_has_no_pending_requests_then_it_will_return_to_ground_floor()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeDownwardsRequestFrom(-2, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(-1);
            _liftTestHelper.ExpectToStopAt(-2).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(-2).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(-1);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_created_away_from_ground_floor_and_no_requests_are_made_it_will_return_to_ground_floor()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(ThirdFloor);

            // Act
            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_two_people_on_different_floors_call_lift_upwards_then_lift_will_stop_at_lower_person_first_even_though_higher_person_made_first_request()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(FourthFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);

            _liftTestHelper.MakeUpwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToStopAt(SecondFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_moving_upwards_and_person_between_lift_and_destination_calls_lift_downwards_then_lift_will_not_pick_them_up()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(FourthFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);

            _liftTestHelper.MakeDownwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(SecondFloor).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_two_people_on_different_floors_call_lift_upwards_and_first_person_tries_to_go_downwards_then_lift_will_go_up_for_other_person_first()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(FourthFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);

            _liftTestHelper.MakeUpwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToStopAt(SecondFloor);

            _liftTestHelper.MakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(SecondFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(ThirdFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(FourthFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_downwards_request_is_made_after_upwards_request_then_lift_will_visit_downwards_person_after_servicing_upwards_request()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);

            _liftTestHelper.MakeDownwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(FourthFloor).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(ThirdFloor);
            _liftTestHelper.ExpectToStopAt(SecondFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_moves_up_and_then_down_then_it_should_not_try_to_return_to_its_previous_up_destination()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);

            _liftTestHelper.MakeDownwardsRequestFrom(SecondFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToStopAt(ThirdFloor);

            _liftTestHelper.MakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.ExpectToStopAt(FourthFloor);

            _liftTestHelper.ExpectToLeaveFrom(FourthFloor);
            _liftTestHelper.ExpectToVisit(ThirdFloor);
            _liftTestHelper.ExpectToStopAt(SecondFloor);

            _liftTestHelper.MakeRequestToMoveTo(FirstFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(SecondFloor);
            _liftTestHelper.ExpectToStopAt(FirstFloor).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(FirstFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_highest_stop_on_upwards_journey_but_next_downwards_request_is_higher_up_then_it_will_keep_moving_upwards_but_then_come_down()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);

            _liftTestHelper.MakeDownwardsRequestFrom(FifthFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToStopAt(ThirdFloor);

            _liftTestHelper.MakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.ExpectToStopAt(FourthFloor);

            _liftTestHelper.ExpectToLeaveFrom(FourthFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(FifthFloor).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(FifthFloor).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_highest_stop_on_upwards_journey_and_next_downwards_request_is_lower_down_then_it_will_go_down_to_that_caller_and_then_continue_down()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);

            _liftTestHelper.MakeDownwardsRequestFrom(SecondFloor, true);

            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToStopAt(ThirdFloor);

            _liftTestHelper.MakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.ExpectToStopAt(FourthFloor);

            _liftTestHelper.ExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(ThirdFloor);
            _liftTestHelper.ExpectToVisit(SecondFloor).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(FirstFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(SecondFloor).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_highest_stop_on_upwards_journey_and_there_are_no_downwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);
            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToStopAt(ThirdFloor);

            _liftTestHelper.MakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.ExpectToStopAt(FourthFloor);

            _liftTestHelper.ExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(ThirdFloor);
            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }
        
        [Test]
        public void When_lift_is_below_ground_and_reaches_highest_stop_on_upwards_journey_but_next_downwards_request_is_higher_up_then_it_will_keep_moving_upwards_but_then_come_down()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(-6);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(-3, true);

            _liftTestHelper.ExpectToLeaveFrom(-6);
            _liftTestHelper.ExpectToVisit(-5);

            _liftTestHelper.MakeDownwardsRequestFrom(-1, shouldBeActedUponImmediately:false);

            _liftTestHelper.ExpectToVisit(-4);
            _liftTestHelper.ExpectToStopAt(-3);

            _liftTestHelper.MakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-3);
            _liftTestHelper.ExpectToStopAt(-2);

            _liftTestHelper.ExpectToLeaveFrom(-2).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(-1).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);
            
            _liftTestHelper.ExpectToLeaveFrom(-1).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_highest_stop_on_upwards_journey_and_next_downwards_request_is_lower_down_then_it_will_go_down_to_that_caller_and_then_continue_down()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(-6);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(-3, true);

            _liftTestHelper.ExpectToLeaveFrom(-6);
            _liftTestHelper.ExpectToVisit(-5);

            _liftTestHelper.MakeDownwardsRequestFrom(-5, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(-4);
            _liftTestHelper.ExpectToStopAt(-3);

            _liftTestHelper.MakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-3);
            _liftTestHelper.ExpectToStopAt(-2);

            _liftTestHelper.ExpectToLeaveFrom(-2).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(-3);
            _liftTestHelper.ExpectToVisit(-4);
            _liftTestHelper.ExpectToStopAt(-5).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-6, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-5).Mark(Direction.Down);
            _liftTestHelper.ExpectToStopAt(-6);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }
        
        [Test]
        public void When_lift_is_below_ground_and_reaches_highest_stop_on_upwards_journey_and_there_are_no_downwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(-6);
 
            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(-3, true);

            _liftTestHelper.ExpectToLeaveFrom(-6);
            _liftTestHelper.ExpectToVisit(-5);
            _liftTestHelper.ExpectToVisit(-4);
            _liftTestHelper.ExpectToStopAt(-3);

            _liftTestHelper.MakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-3);
            _liftTestHelper.ExpectToStopAt(-2);

            _liftTestHelper.ExpectToLeaveFrom(-2).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(-1);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();
            
            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_lowest_stop_on_downwards_journey_but_next_upwards_request_is_lower_down_then_it_will_keep_moving_downwards_but_then_come_up()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeDownwardsRequestFrom(-3, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(-1);

            _liftTestHelper.MakeUpwardsRequestFrom(-5, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(-2);
            _liftTestHelper.ExpectToStopAt(-3);

            _liftTestHelper.MakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-3);
            _liftTestHelper.ExpectToStopAt(-4);

            _liftTestHelper.ExpectToLeaveFrom(-4).Mark(Direction.Down);
            _liftTestHelper.ExpectToStopAt(-5).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-2, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-5).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_lowest_stop_on_downwards_journey_and_next_upwards_request_is_higher_up_then_it_will_go_up_to_that_caller_and_then_continue_up()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeDownwardsRequestFrom(-3, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(-1);

            _liftTestHelper.MakeUpwardsRequestFrom(-2, true);

            _liftTestHelper.ExpectToVisit(-2);
            _liftTestHelper.ExpectToStopAt(-3);

            _liftTestHelper.MakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-3);
            _liftTestHelper.ExpectToStopAt(-4);

            _liftTestHelper.ExpectToLeaveFrom(-4).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(-3);
            _liftTestHelper.ExpectToVisit(-2).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-1, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-2).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_below_ground_and_reaches_lowest_stop_on_downwards_journey_and_there_are_no_upwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeDownwardsRequestFrom(-3, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(-1);
            _liftTestHelper.ExpectToVisit(-2);
            _liftTestHelper.ExpectToStopAt(-3);

            _liftTestHelper.MakeRequestToMoveTo(-4, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-3);
            _liftTestHelper.ExpectToStopAt(-4);

            _liftTestHelper.ExpectToLeaveFrom(-4).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(-3);
            _liftTestHelper.ExpectToVisit(-2);
            _liftTestHelper.ExpectToVisit(-1);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_lowest_stop_on_downwards_journey_but_next_upwards_request_is_lower_down_then_it_will_keep_moving_downwards_but_then_come_up()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(SixthFloor);

            // Act
            _liftTestHelper.MakeDownwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(SixthFloor);
            _liftTestHelper.ExpectToVisit(FifthFloor);

            _liftTestHelper.MakeUpwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(FourthFloor);
            _liftTestHelper.ExpectToStopAt(ThirdFloor);

            _liftTestHelper.MakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.ExpectToStopAt(SecondFloor);

            _liftTestHelper.ExpectToLeaveFrom(SecondFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToStopAt(FirstFloor).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(FourthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(FirstFloor).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_lowest_stop_on_downwards_journey_and_next_upwards_request_is_higher_up_then_it_will_go_up_to_that_caller_and_then_continue_up()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(SixthFloor);

            // Act
            _liftTestHelper.MakeDownwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(SixthFloor);
            _liftTestHelper.ExpectToVisit(FifthFloor);

            _liftTestHelper.MakeUpwardsRequestFrom(FifthFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(FourthFloor);
            _liftTestHelper.ExpectToStopAt(ThirdFloor);

            _liftTestHelper.MakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.ExpectToStopAt(SecondFloor);

            _liftTestHelper.ExpectToLeaveFrom(SecondFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(ThirdFloor);
            _liftTestHelper.ExpectToVisit(FourthFloor);
            _liftTestHelper.ExpectToStopAt(FifthFloor).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(SixthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(FifthFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(SixthFloor);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_above_ground_and_reaches_lowest_stop_on_downwards_journey_and_there_are_no_upwards_requests_then_it_will_return_to_the_ground_floor()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(SixthFloor);

            // Act
            _liftTestHelper.MakeDownwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(SixthFloor);
            _liftTestHelper.ExpectToVisit(FifthFloor);
            _liftTestHelper.ExpectToVisit(FourthFloor);
            _liftTestHelper.ExpectToStopAt(ThirdFloor);

            _liftTestHelper.MakeRequestToMoveTo(SecondFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor);
            _liftTestHelper.ExpectToStopAt(SecondFloor);

            _liftTestHelper.ExpectToLeaveFrom(SecondFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(FirstFloor);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_lift_is_moving_upwards_when_a_new_upwards_request_comes_in_from_a_lower_floor_it_is_not_processed_until_after_any_downwards_requests()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(FourthFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);

            _liftTestHelper.MakeDownwardsRequestFrom(SixthFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(SecondFloor);

            _liftTestHelper.MakeUpwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(ThirdFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(FourthFloor);

            _liftTestHelper.MakeRequestToMoveTo(FifthFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(FourthFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(FifthFloor);

            _liftTestHelper.ExpectToLeaveFrom(FifthFloor);
            _liftTestHelper.ExpectToStopAt(SixthFloor).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(SixthFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(FifthFloor);
            _liftTestHelper.ExpectToVisit(FourthFloor);
            _liftTestHelper.ExpectToVisit(ThirdFloor);
            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(FirstFloor).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(FirstFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_lift_is_moving_downwards_when_a_new_downwards_request_comes_in_from_a_higher_floor_it_is_not_processed_until_after_any_upwards_requests()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeDownwardsRequestFrom(-4, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(-1);

            _liftTestHelper.MakeUpwardsRequestFrom(-6, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(-2);

            _liftTestHelper.MakeDownwardsRequestFrom(-1, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(-3).Mark(Direction.Down);
            _liftTestHelper.ExpectToStopAt(-4);

            _liftTestHelper.MakeRequestToMoveTo(-5, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-4).Mark(Direction.Down);
            _liftTestHelper.ExpectToStopAt(-5);

            _liftTestHelper.ExpectToLeaveFrom(-5);
            _liftTestHelper.ExpectToStopAt(-6).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-6).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(-5);
            _liftTestHelper.ExpectToVisit(-4);
            _liftTestHelper.ExpectToVisit(-3);
            _liftTestHelper.ExpectToVisit(-2);
            _liftTestHelper.ExpectToVisit(-1).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToStopAt(-1).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(-1).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_two_or_more_requests_are_made_between_floors_they_are_all_serviced_correctly()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(9, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(1);
            _liftTestHelper.ExpectToVisit(2);

            _liftTestHelper.MakeUpwardsRequestFrom(5, shouldBeActedUponImmediately: false);
            _liftTestHelper.MakeDownwardsRequestFrom(4, shouldBeActedUponImmediately: false);
            _liftTestHelper.MakeUpwardsRequestFrom(7, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(3);
            _liftTestHelper.ExpectToVisit(4);
            _liftTestHelper.ExpectToStopAt(5).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(6, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(5);
            _liftTestHelper.ExpectToStopAt(6);

            _liftTestHelper.ExpectToLeaveFrom(6);
            _liftTestHelper.ExpectToStopAt(7).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(8, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(7);
            _liftTestHelper.ExpectToStopAt(8);

            _liftTestHelper.ExpectToLeaveFrom(8);
            _liftTestHelper.ExpectToStopAt(9).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(10, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(9);
            _liftTestHelper.ExpectToStopAt(10);

            _liftTestHelper.ExpectToLeaveFrom(10).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(9);
            _liftTestHelper.ExpectToVisit(8);
            _liftTestHelper.ExpectToVisit(7);
            _liftTestHelper.ExpectToVisit(6);
            _liftTestHelper.ExpectToVisit(5);
            _liftTestHelper.ExpectToStopAt(4).Mark(Direction.None);
            
            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_asked_to_move_to_a_floor_which_it_has_already_been_called_to_it_handles_it_correctly()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(2, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(1);

            _liftTestHelper.MakeUpwardsRequestFrom(4, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToStopAt(2);

            _liftTestHelper.MakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(2);
            _liftTestHelper.ExpectToVisit(3);
            _liftTestHelper.ExpectToStopAt(4).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(4).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_asked_to_move_to_a_floor_which_it_has_already_been_asked_to_move_to_it_handles_it_correctly()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(2, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(1);

            _liftTestHelper.MakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToStopAt(2);

            _liftTestHelper.MakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(2);
            _liftTestHelper.ExpectToStopAt(3);

            _liftTestHelper.MakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(3);
            _liftTestHelper.ExpectToStopAt(4).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(4).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_called_to_a_floor_which_it_has_already_been_asked_to_move_to_it_handles_it_correctly()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(2, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(1);
            _liftTestHelper.ExpectToStopAt(2);

            _liftTestHelper.MakeRequestToMoveTo(4, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(2);

            _liftTestHelper.MakeUpwardsRequestFrom(4, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(3);
            _liftTestHelper.ExpectToStopAt(4).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(4).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_called_to_a_floor_which_it_has_already_been_called_to_it_handles_it_correctly()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(4, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(1);
            _liftTestHelper.ExpectToVisit(2);

            _liftTestHelper.MakeUpwardsRequestFrom(4, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(3);
            _liftTestHelper.ExpectToStopAt(4).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(4).Mark(Direction.Up);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_several_upwards_requests_come_through_in_a_different_order_then_they_should_still_be_visited_in_the_correct_order()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(13, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(1);

            _liftTestHelper.MakeUpwardsRequestFrom(11, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(2);

            _liftTestHelper.MakeUpwardsRequestFrom(3, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToStopAt(3).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(5, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(3);

            _liftTestHelper.MakeUpwardsRequestFrom(7, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(4);
            _liftTestHelper.ExpectToStopAt(5).Mark(Direction.None);

            _liftTestHelper.MakeUpwardsRequestFrom(6, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(5);
            _liftTestHelper.ExpectToStopAt(6).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(7, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(6);
            _liftTestHelper.ExpectToStopAt(7).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(15, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(7);
            _liftTestHelper.ExpectToVisit(8);
            _liftTestHelper.ExpectToVisit(9);
            _liftTestHelper.ExpectToVisit(10);
            _liftTestHelper.ExpectToStopAt(11).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(15, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(11);
            _liftTestHelper.ExpectToVisit(12);
            _liftTestHelper.ExpectToStopAt(13).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(17, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(13);
            _liftTestHelper.ExpectToVisit(14);
            _liftTestHelper.ExpectToStopAt(15).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(15);
            _liftTestHelper.ExpectToVisit(16);
            _liftTestHelper.ExpectToStopAt(17).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(17).Mark(Direction.Down);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_several_downwards_requests_come_through_in_a_different_order_then_they_should_still_be_visited_in_the_correct_order()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeDownwardsRequestFrom(-13, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(-1);

            _liftTestHelper.MakeDownwardsRequestFrom(-11, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(-2);

            _liftTestHelper.MakeDownwardsRequestFrom(-3, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToStopAt(-3).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-5, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-3);

            _liftTestHelper.MakeDownwardsRequestFrom(-7, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(-4);
            _liftTestHelper.ExpectToStopAt(-5).Mark(Direction.None);

            _liftTestHelper.MakeDownwardsRequestFrom(-6, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-5);
            _liftTestHelper.ExpectToStopAt(-6).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-7, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-6);
            _liftTestHelper.ExpectToStopAt(-7).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-7);
            _liftTestHelper.ExpectToVisit(-8);
            _liftTestHelper.ExpectToVisit(-9);
            _liftTestHelper.ExpectToVisit(-10);
            _liftTestHelper.ExpectToStopAt(-11).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-11);
            _liftTestHelper.ExpectToVisit(-12);
            _liftTestHelper.ExpectToStopAt(-13).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-17, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-13);
            _liftTestHelper.ExpectToVisit(-14);
            _liftTestHelper.ExpectToStopAt(-15).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(-15);
            _liftTestHelper.ExpectToVisit(-16);
            _liftTestHelper.ExpectToStopAt(-17).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_several_downwards_requests_and_several_upwards_requests_come_through_in_a_different_order_then_they_should_still_be_visited_in_the_correct_order()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeDownwardsRequestFrom(-13, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(-1);

            _liftTestHelper.MakeDownwardsRequestFrom(-11, shouldBeActedUponImmediately: false);
            _liftTestHelper.MakeUpwardsRequestFrom(5, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(-2);

            _liftTestHelper.MakeDownwardsRequestFrom(-3, shouldBeActedUponImmediately: false);
            _liftTestHelper.MakeUpwardsRequestFrom(1, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToStopAt(-3).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-5, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-3);

            _liftTestHelper.MakeDownwardsRequestFrom(-7, shouldBeActedUponImmediately: false);
            _liftTestHelper.MakeUpwardsRequestFrom(10, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(-4);
            _liftTestHelper.ExpectToStopAt(-5).Mark(Direction.None);

            _liftTestHelper.MakeDownwardsRequestFrom(-6, shouldBeActedUponImmediately: true);
            _liftTestHelper.MakeUpwardsRequestFrom(2, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToLeaveFrom(-5);
            _liftTestHelper.ExpectToStopAt(-6).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-7, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-6);
            _liftTestHelper.ExpectToStopAt(-7).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-7);
            _liftTestHelper.ExpectToVisit(-8);

            _liftTestHelper.MakeUpwardsRequestFrom(9, shouldBeActedUponImmediately: false);
            _liftTestHelper.MakeUpwardsRequestFrom(7, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToVisit(-9);
            _liftTestHelper.ExpectToVisit(-10);
            _liftTestHelper.ExpectToStopAt(-11).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-15, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-11);
            _liftTestHelper.ExpectToVisit(-12);
            _liftTestHelper.ExpectToStopAt(-13).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(-17, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(-13);
            _liftTestHelper.ExpectToVisit(-14);
            _liftTestHelper.ExpectToStopAt(-15).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(-15);
            _liftTestHelper.ExpectToVisit(-16);
            _liftTestHelper.ExpectToStopAt(-17).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(-17).Mark(Direction.Up);

            _liftTestHelper.ExpectToVisit(-16).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(-15);
            _liftTestHelper.ExpectToVisit(-14);
            _liftTestHelper.ExpectToVisit(-13);
            _liftTestHelper.ExpectToVisit(-12);
            _liftTestHelper.ExpectToVisit(-11);
            _liftTestHelper.ExpectToVisit(-10);
            _liftTestHelper.ExpectToVisit(-9);
            _liftTestHelper.ExpectToVisit(-8);
            _liftTestHelper.ExpectToVisit(-7);
            _liftTestHelper.ExpectToVisit(-6);
            _liftTestHelper.ExpectToVisit(-5);
            _liftTestHelper.ExpectToVisit(-4);
            _liftTestHelper.ExpectToVisit(-3);
            _liftTestHelper.ExpectToVisit(-2);
            _liftTestHelper.ExpectToVisit(-1);
            _liftTestHelper.ExpectToVisit(0).Mark(Direction.Up);

            _liftTestHelper.ExpectToStopAt(1).Mark(Direction.None);
            _liftTestHelper.ExpectToLeaveFrom(1).Mark(Direction.Up);

            _liftTestHelper.ExpectToStopAt(2).Mark(Direction.None);
            _liftTestHelper.ExpectToLeaveFrom(2).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(3);
            _liftTestHelper.ExpectToVisit(4);

            _liftTestHelper.ExpectToStopAt(5).Mark(Direction.None);
            _liftTestHelper.ExpectToLeaveFrom(5).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(6);

            _liftTestHelper.ExpectToStopAt(7).Mark(Direction.None);
            _liftTestHelper.ExpectToLeaveFrom(7).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(8);

            _liftTestHelper.ExpectToStopAt(9).Mark(Direction.None);
            _liftTestHelper.ExpectToLeaveFrom(9).Mark(Direction.Up);

            _liftTestHelper.ExpectToStopAt(10).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_multiple_move_requests_are_made_after_lift_has_stopped_in_response_to_call_they_are_all_serviced_correctly()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(1, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToStopAt(1).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(2, true);
            _liftTestHelper.MakeRequestToMoveTo(-1, false);
            _liftTestHelper.MakeRequestToMoveTo(5, false);
            _liftTestHelper.MakeRequestToMoveTo(-6, false);

            _liftTestHelper.ExpectToLeaveFrom(1).Mark(Direction.Up);
            _liftTestHelper.ExpectToStopAt(2).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(2).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(3);
            _liftTestHelper.ExpectToVisit(4);
            _liftTestHelper.ExpectToStopAt(5).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(5).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(4);
            _liftTestHelper.ExpectToVisit(3);
            _liftTestHelper.ExpectToVisit(2);
            _liftTestHelper.ExpectToVisit(1);
            _liftTestHelper.ExpectToVisit(0);
            _liftTestHelper.ExpectToStopAt(-1).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(-1).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(-2);
            _liftTestHelper.ExpectToVisit(-3);
            _liftTestHelper.ExpectToVisit(-4);
            _liftTestHelper.ExpectToVisit(-5);
            _liftTestHelper.ExpectToStopAt(-6).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void When_lift_is_called_while_already_stopped_somewhere_else_it_will_respond_to_the_new_request()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, true);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);
            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _liftTestHelper.MakeDownwardsRequestFrom(FirstFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(ThirdFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToStopAt(FirstFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }

        [Test]
        public void Given_lift_has_finished_up_requests_and_the_next_downwards_request_comes_from_higher_up_when_a_new_up_request_comes_in_while_travelling_to_the_down_request_then_the_up_request_will_be_ignored_until_later()
        {
            // Arrange
            _liftTestHelper.MakeStartAt(GroundFloor);

            // Act
            _liftTestHelper.MakeUpwardsRequestFrom(FirstFloor, true);

            _liftTestHelper.MakeDownwardsRequestFrom(FourthFloor, shouldBeActedUponImmediately: false);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor);
            _liftTestHelper.ExpectToStopAt(FirstFloor);

            _liftTestHelper.ExpectToLeaveFrom(FirstFloor).Mark(Direction.Up);

            _liftTestHelper.MakeUpwardsRequestFrom(ThirdFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToVisit(ThirdFloor);
            _liftTestHelper.ExpectToStopAt(FourthFloor).Mark(Direction.None);

            _liftTestHelper.MakeRequestToMoveTo(GroundFloor, shouldBeActedUponImmediately: true);

            _liftTestHelper.ExpectToLeaveFrom(FourthFloor).Mark(Direction.Down);
            _liftTestHelper.ExpectToVisit(ThirdFloor);
            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToVisit(FirstFloor);
            _liftTestHelper.ExpectToStopAt(GroundFloor).Mark(Direction.None);

            _liftTestHelper.ExpectToLeaveFrom(GroundFloor).Mark(Direction.Up);
            _liftTestHelper.ExpectToVisit(FirstFloor);
            _liftTestHelper.ExpectToVisit(SecondFloor);
            _liftTestHelper.ExpectToStopAt(ThirdFloor).Mark(Direction.None);

            _liftTestHelper.StartTest();

            // Assert
            _liftTestHelper.VerifyAllMarkers();
        }
    }
}