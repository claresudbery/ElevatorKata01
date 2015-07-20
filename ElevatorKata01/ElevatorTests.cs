using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace ElevatorKata01
{
    [TestFixture]
    public class ElevatorTests : ILiftMonitor
    {
        private List<Floor> _floorsVisited = new List<Floor>();
        private List<LiftStatus> _liftStatuses = new List<LiftStatus>();

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
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.Move(ThirdFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

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
            theLift.Move(FirstFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

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
            theLift.Move(ThirdFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

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
        public void When_person_in_lift_enters_a_higher_floor_number_then_lift_engine_is_asked_to_move_upwards_and_then_stopped_when_it_reaches_destination()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);

            // Act
            theLift.Move(ThirdFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

            // Assert
            Assert.That(_liftStatuses.Count, Is.EqualTo(4));

            Assert.That(_liftStatuses[0].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[0].CurrentFloor, Is.EqualTo(GroundFloor));

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
            theLift.Move(FirstFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

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
            theLift.Move(FourthFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

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
            theLift.Move(FourthFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

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
            theLift.Call(ThirdFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

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
            theLift.Call(FirstFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

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
            theLift.Call(ThirdFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

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
        public void When_person_calls_lift_to_higher_floor_number_then_lift_moves_upwards_and_then_stops_when_it_reaches_destination()
        {
            // Arrange
            var testScheduler = new TestScheduler();
            var theLift = new ObservableLift(GroundFloor, testScheduler);
            _liftStatuses.Clear();
            theLift.Subscribe(this);
            
            // Act
            theLift.Call(ThirdFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);
            
            // Assert
            Assert.That(_liftStatuses.Count, Is.EqualTo(4));
            
            Assert.That(_liftStatuses[0].CurrentDirection, Is.EqualTo(Direction.Up));
            Assert.That(_liftStatuses[0].CurrentFloor, Is.EqualTo(GroundFloor));
            
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
            theLift.Call(FirstFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

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
            theLift.Call(FourthFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

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
            theLift.Call(FourthFloor);
            testScheduler.Start();
            testScheduler.AdvanceBy(5);

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(1));

            Assert.That(_liftStatuses[_liftStatuses.Count - 1].CurrentDirection, Is.EqualTo(Direction.None));
            Assert.That(_liftStatuses[_liftStatuses.Count - 1].CurrentFloor, Is.EqualTo(FourthFloor));
        }

        //[Test]
        //public void Given_lift_is_on_groundfloor_and_personA_is_on_the_firstfloor_and_personB_is_on_the_second_floor_and_both_people_want_to_ascend_When_lift_is_called_then_it_will_fetch_personA_first()
        //{
        //    // Arrange
        //    var theLift = new ObservableLift();
        //    theLift.Subscribe(this);
        //    theLift.CurrentFloor = Floor.Ground;

        //    // Act
        //    theLift.Move(Floor.Second, Floor.Third);
        //    theLift.Move(Floor.First, Floor.Third);

        //    // Assert
        //    Assert.That(_floorsVisited[0], Is.EqualTo(Floor.Ground));
        //    Assert.That(_floorsVisited[1], Is.EqualTo(Floor.First));
        //    Assert.That(_floorsVisited[2], Is.EqualTo(Floor.Second));
        //    Assert.That(_floorsVisited[3], Is.EqualTo(Floor.Third));

        //    // Clean up
        //    theLift.Dispose();
        //}

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