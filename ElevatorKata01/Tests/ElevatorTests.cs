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
    [TestFixture]
    public class ElevatorTests : ILiftMonitor
    {
            //// Arrange
            //LiftMakeStartAt(xx);

            //// Act
            //_theLift.MakeUpwardsRequestFrom(xx);

            //LiftExpectToLeaveFrom(xx);
            //LiftExpectToVisit(xx);
            //LiftExpectToStopAt(xx);
            //LiftMakeRequestToMoveTo(xx, shouldBeActedUponImmediately: true);
            //LiftMakeUpwardsRequestFrom(xx, shouldBeActedUponImmediately: true);
            //LiftMakeDownwardsRequestFrom(xx, shouldBeActedUponImmediately: true);

            //StartTest();

            //// Assert
            //VerifyAllMarkers();

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
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MoveTo(ThirdFloor);
            testScheduler.Start();

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(0));

            Assert.That(_liftStatuses[0].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[0].CurrentFloor, Is.EqualTo(GroundFloor));
        }

        [Test]
        public void When_person_in_lift_enters_a_lower_floor_number_then_lift_starts_moving_downwards()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(ThirdFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MoveTo(FirstFloor);
            testScheduler.Start();

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(0));

            Assert.That(_liftStatuses[0].CurrentDirection, Is.EqualTo(Direction.Down));
            Assert.That(_liftStatuses[0].CurrentFloor, Is.EqualTo(ThirdFloor));
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_notifies_direction_and_location_for_every_floor_it_passes()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MoveTo(ThirdFloor);
            testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(TimeConstants.FloorInterval).Ticks * 5);

            // Assert
            Assert.That(_liftStatuses.Count, Is.EqualTo(4));

            Assert.That(_liftStatuses[0].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[0].CurrentFloor, Is.EqualTo(GroundFloor));

            Assert.That(_liftStatuses[1].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[1].CurrentFloor, Is.EqualTo(FirstFloor));

            Assert.That(_liftStatuses[2].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[2].CurrentFloor, Is.EqualTo(SecondFloor));

            Assert.That(_liftStatuses[3].CurrentDirection, Is.EqualTo(Direction.None));
            Assert.That(_liftStatuses[3].CurrentFloor, Is.EqualTo(ThirdFloor));
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_notifies_its_current_location()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MoveTo(FirstFloor);
            testScheduler.Start();

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(0));

            Assert.That(_liftStatuses[0].CurrentFloor, Is.EqualTo(GroundFloor));
        }

        [Test]
        public void When_person_in_lift_enters_a_floor_number_then_lift_goes_to_that_floor()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MoveTo(FourthFloor);
            testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(TimeConstants.FloorInterval).Ticks * 6);

            // Assert
            Assert.That(_liftStatuses.Count, Is.EqualTo(5));

            Assert.That(_liftStatuses[4].CurrentFloor, Is.EqualTo(FourthFloor));
        }

        [Test]
        public void When_lift_arrives_at_new_floor_after_person_in_lift_makes_request_then_lift_stops_moving()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MoveTo(FourthFloor);
            testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(TimeConstants.FloorInterval).Ticks * 6);

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(1));

            Assert.That(_liftStatuses[_liftStatuses.Count - 1].CurrentDirection, Is.EqualTo(Direction.None));
            Assert.That(_liftStatuses[_liftStatuses.Count - 1].CurrentFloor, Is.EqualTo(FourthFloor));
        }

        [Test]
        public void When_person_calls_lift_to_higher_floor_number_then_lift_starts_moving_upwards()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MakeUpwardsRequestFrom(ThirdFloor);
            testScheduler.Start();

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(0));

            Assert.That(_liftStatuses[0].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[0].CurrentFloor, Is.EqualTo(GroundFloor));
        }

        [Test]
        public void When_person_calls_lift_to_lower_floor_number_then_lift_starts_moving_downwards()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(ThirdFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MakeDownwardsRequestFrom(FirstFloor);
            testScheduler.Start();

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(0));

            Assert.That(_liftStatuses[0].CurrentDirection, Is.EqualTo(Direction.Down));
            Assert.That(_liftStatuses[0].CurrentFloor, Is.EqualTo(ThirdFloor));
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_notifies_direction_and_location_for_every_floor_it_passes()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MakeUpwardsRequestFrom(ThirdFloor);
            testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(TimeConstants.FloorInterval).Ticks * 5);

            // Assert
            Assert.That(_liftStatuses.Count, Is.EqualTo(4));

            Assert.That(_liftStatuses[0].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[0].CurrentFloor, Is.EqualTo(GroundFloor));

            Assert.That(_liftStatuses[1].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[1].CurrentFloor, Is.EqualTo(FirstFloor));

            Assert.That(_liftStatuses[2].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[2].CurrentFloor, Is.EqualTo(SecondFloor));

            Assert.That(_liftStatuses[3].CurrentDirection, Is.EqualTo(Direction.None));
            Assert.That(_liftStatuses[3].CurrentFloor, Is.EqualTo(ThirdFloor));
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_notifies_its_current_location()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MakeUpwardsRequestFrom(FirstFloor);
            testScheduler.Start();

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(0));

            Assert.That(_liftStatuses[0].CurrentFloor, Is.EqualTo(GroundFloor));
        }

        [Test]
        public void When_person_calls_lift_to_floor_number_then_lift_goes_to_that_floor()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MakeUpwardsRequestFrom(FourthFloor);
            testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(TimeConstants.FloorInterval).Ticks * 6);

            // Assert
            Assert.That(_liftStatuses.Count, Is.EqualTo(5));

            Assert.That(_liftStatuses[4].CurrentFloor, Is.EqualTo(FourthFloor));
        }

        [Test]
        public void When_lift_arrives_at_new_floor_after_person_calls_lift_then_lift_stops_moving()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MakeUpwardsRequestFrom(FourthFloor);
            testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(TimeConstants.FloorInterval).Ticks * 6);

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(1));

            Assert.That(_liftStatuses[_liftStatuses.Count - 1].CurrentDirection, Is.EqualTo(Direction.None));
            Assert.That(_liftStatuses[_liftStatuses.Count - 1].CurrentFloor, Is.EqualTo(FourthFloor));
        }

        [Test]
        public void When_two_people_on_different_floors_call_lift_upwards_then_lift_will_stop_at_lower_person_first_even_though_higher_person_made_first_request()
        {
            // Arrange
            int betweenFirstAndSecondFloors = (2 * TimeConstants.FloorInterval) + 500;
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MakeUpwardsRequestFrom(FourthFloor);
            testScheduler.Schedule(TimeSpan.FromMilliseconds(betweenFirstAndSecondFloors), () => theLift.MakeUpwardsRequestFrom(SecondFloor));
            testScheduler.Start();

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(1));

            Assert.That(_liftStatuses[2].CurrentDirection, Is.EqualTo(Direction.None));
            Assert.That(_liftStatuses[2].CurrentFloor, Is.EqualTo(SecondFloor));
        }

        [Test]
        public void When_lift_is_moving_upwards_and_person_between_lift_and_destination_calls_lift_downwards_then_lift_will_not_pick_them_up()
        {
            // Arrange
            int betweenFirstAndSecondFloors = (2 * TimeConstants.FloorInterval) + 500;
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MakeUpwardsRequestFrom(FourthFloor);
            testScheduler.Schedule(TimeSpan.FromMilliseconds(betweenFirstAndSecondFloors), () => theLift.MakeDownwardsRequestFrom(SecondFloor));
            testScheduler.Start();

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(1));

            Assert.That(_liftStatuses[2].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[2].CurrentFloor, Is.EqualTo(SecondFloor));
        }

        [Test]
        public void When_two_people_on_different_floors_call_lift_upwards_and_first_person_tries_to_go_downwards_then_lift_will_go_up_for_other_person_first()
        {
            // Arrange
            int betweenFirstAndSecondFloors = (2 * TimeConstants.FloorInterval) + 500;
            int afterSecondFloor = (3 * TimeConstants.FloorInterval) + 500;
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MakeUpwardsRequestFrom(FourthFloor);
            testScheduler.Schedule(TimeSpan.FromMilliseconds(betweenFirstAndSecondFloors), () => theLift.MakeUpwardsRequestFrom(SecondFloor));
            testScheduler.Schedule(TimeSpan.FromMilliseconds(afterSecondFloor), () => theLift.MoveTo(GroundFloor));
            testScheduler.Start();

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(1));

            Assert.That(_liftStatuses[3].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[3].CurrentFloor, Is.EqualTo(SecondFloor));

            Assert.That(_liftStatuses[4].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[4].CurrentFloor, Is.EqualTo(ThirdFloor));

            Assert.That(_liftStatuses[5].CurrentDirection, Is.EqualTo(Direction.None));
            Assert.That(_liftStatuses[5].CurrentFloor, Is.EqualTo(FourthFloor));
        }

        [Test]
        public void When_downwards_request_is_made_after_upwards_request_then_lift_will_visit_downwards_person_after_servicing_upwards_request()
        {
            // Arrange
            int betweenFirstAndSecondFloors = (2 * TimeConstants.FloorInterval) + 500;
            int afterStoppingOnThirdFloor = (8 * TimeConstants.FloorInterval) + 500;
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.MakeUpwardsRequestFrom(ThirdFloor);
            testScheduler.Schedule(TimeSpan.FromMilliseconds(betweenFirstAndSecondFloors), () => theLift.MakeDownwardsRequestFrom(SecondFloor));
            testScheduler.Schedule(TimeSpan.FromMilliseconds(afterStoppingOnThirdFloor), () => theLift.MoveTo(FourthFloor));
            testScheduler.Start();

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(1));

            Assert.That(_liftStatuses[3].CurrentDirection, Is.EqualTo(Direction.None));
            Assert.That(_liftStatuses[3].CurrentFloor, Is.EqualTo(ThirdFloor));

            Assert.That(_liftStatuses[4].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[4].CurrentFloor, Is.EqualTo(ThirdFloor));

            Assert.That(_liftStatuses[5].CurrentDirection, Is.EqualTo(Direction.None));
            Assert.That(_liftStatuses[5].CurrentFloor, Is.EqualTo(FourthFloor));

            Assert.That(_liftStatuses[6].CurrentDirection, Is.EqualTo(Direction.Down));
            Assert.That(_liftStatuses[6].CurrentFloor, Is.EqualTo(FourthFloor));

            Assert.That(_liftStatuses[8].CurrentDirection, Is.EqualTo(Direction.None));
            Assert.That(_liftStatuses[8].CurrentFloor, Is.EqualTo(SecondFloor));
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
        
        private void VerifyAllMarkers()
        {
            Assert.That(_testStarted, Is.EqualTo(true), "Test scheduler was never kicked off!");
            Assert.That(_liftStatuses.Count, Is.EqualTo(_numExpectedStatuses));

            foreach (var expectedStatus in _expectedLiftStatuses)
            {
                Assert.That(_liftStatuses[expectedStatus.StatusIndex].CurrentDirection, Is.EqualTo(expectedStatus.Status.CurrentDirection));
                Assert.That(_liftStatuses[expectedStatus.StatusIndex].CurrentFloor, Is.EqualTo(expectedStatus.Status.CurrentFloor));
            }

            _theLift.Dispose();
            EnsureThatAllScheduledEventsAreRunThroughToCompletion();
            _testStarted = false;
        }
        
        private void EnsureThatAllScheduledEventsAreRunThroughToCompletion()
        {
            _testScheduler.Start();
        }

        private void Mark(Direction direction)
        {
            _expectedLiftStatuses.Add(new ExpectedLiftStatus
            {
                StatusIndex = _numExpectedStatuses - 1,
                SecondsSinceTestStarted = _millisecondsSinceTestStarted / 1000m,
                Status = new LiftStatus
                {
                    CurrentDirection = direction,
                    CurrentFloor = _currentLiftFloor
                }
            });
        }

        private void StartTest()
        {
            _testStarted = true;
            _millisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted).Ticks);
        }

        private void LiftMakeRequestToMoveTo(int floor, bool shouldBeActedUponImmediately)
        {
            AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            _testScheduler.Schedule(
                TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval), 
                () => _theLift.MoveTo(floor));
        }

        private void LiftMakeDownwardsRequestFrom(int floor, bool shouldBeActedUponImmediately)
        {
            AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            _testScheduler.Schedule(
                TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval), 
                () => _theLift.MakeDownwardsRequestFrom(floor));
        }

        private void LiftMakeUpwardsRequestFrom(int floor, bool shouldBeActedUponImmediately)
        {
            AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            _testScheduler.Schedule(
                TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval), 
                () => _theLift.MakeUpwardsRequestFrom(floor));
        }

        private void AmendMostRecentEventTimeIfNecessary(bool shouldBeActedUponImmediately)
        {
            _millisecondsTakenByMostRecentEvent = shouldBeActedUponImmediately
                ? TimeConstants.FloorInterval + TimeConstants.BetweenFloorsInterval
                : _millisecondsTakenByMostRecentEvent;
        }

        private ElevatorTests LiftExpectToStopAt(int floor)
        {
            _millisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;
            _millisecondsTakenByMostRecentEvent = TimeConstants.WaitTime + TimeConstants.FloorInterval;
            _numExpectedStatuses++;
            _currentLiftFloor = floor;
            return this;
        }

        private ElevatorTests LiftExpectToLeaveFrom(int floor)
        {
            return LiftExpectToVisit(floor);
        }

        private ElevatorTests LiftExpectToVisit(int floor)
        {
            _millisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;
            _millisecondsTakenByMostRecentEvent = TimeConstants.FloorInterval;
            _numExpectedStatuses++;
            _currentLiftFloor = floor;
            return this;
        }

        private void LiftMakeStartAt(int floor)
        {
            _liftStatuses.Clear();
            _expectedLiftStatuses.Clear();

            _millisecondsSinceTestStarted = TimeConstants.FloorInterval;
            _millisecondsTakenByMostRecentEvent = 0;
            _numExpectedStatuses = 0;

            _theLift = new ObservableLift(floor, _testScheduler);
            _theLift.Subscribe(this);
        }

        public void OnNext(LiftStatus currentLiftStatus)
        {
            _liftStatuses.Add(currentLiftStatus);
        }

        public void OnError(Exception error)
        {
            // Do nothing
        }

        public void OnCompleted()
        {
            // Do nothing
        }
    }
}