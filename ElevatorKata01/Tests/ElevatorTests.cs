using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using ElevatorKata01.Elements;
using ElevatorKata01.FunctionalCode;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace ElevatorKata01.Tests
{
    [TestFixture]
    public class ElevatorTests : ILiftMonitor
    {
        private readonly List<LiftStatus> _liftStatuses = new List<LiftStatus>();

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
            theLift.Move(FirstFloor);
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
            theLift.Move(FourthFloor);
            testScheduler.Start();

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
            theLift.Call(FirstFloor);
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
            theLift.Call(FourthFloor);
            testScheduler.Start();

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
            theLift.Call(FourthFloor);
            testScheduler.Schedule(TimeSpan.FromMilliseconds(betweenFirstAndSecondFloors), () => theLift.Call(SecondFloor));
            testScheduler.Start();

            // Assert
            Assert.That(_liftStatuses.Count, Is.GreaterThan(1));

            Assert.That(_liftStatuses[2].CurrentDirection, Is.EqualTo(Direction.None));
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
            theLift.Call(FourthFloor);
            testScheduler.Schedule(TimeSpan.FromMilliseconds(betweenFirstAndSecondFloors), () => theLift.Call(SecondFloor));
            testScheduler.Schedule(TimeSpan.FromMilliseconds(afterSecondFloor), () => theLift.Move(GroundFloor));
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