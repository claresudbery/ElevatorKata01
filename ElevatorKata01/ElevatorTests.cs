using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ElevatorKata01
{
    [TestFixture]
    public class ElevatorTests : ILiftMonitor
    {
        private Lift _currentLift;
        private List<Floor> _floorsVisited = new List<Floor>();

        [Test]
        public void GivenIAmOnGroundFloorWhenICallLiftToFirstFloorThenLiftGoesToFirstFloor()
        {
            // Arrange
            var theLift = new Lift(Floor.Ground);
            
            // Act
            Floor result = theLift.Call(Floor.First);

            // Assert
            Assert.That(result, Is.EqualTo(Floor.First));
        }

        [Test]
        public void GivenIAmOnGroundFloorWhenIRequestTravelToTheFirstFloorThenLiftTakesMeToTheFirstFloor()
        {
            // Arrange
            var theLift = new Lift(Floor.Ground);

            // Act
            Floor result = theLift.Move(Floor.First);

            // Assert
            Assert.That(result, Is.EqualTo(Floor.First));
        }

        [Test]
        public void Given_lift_is_on_groundfloor_and_personA_is_on_the_firstfloor_and_personB_is_on_the_second_floor_and_both_people_want_to_ascend_When_lift_is_called_then_it_will_fetch_personA_first()
        {
            // Arrange
            var theLift = new ObservableLift();
            theLift.Subscribe(this);
            theLift.CurrentFloor = Floor.Ground;

            // Act
            theLift.Move(Floor.Second, Floor.Third);
            theLift.Move(Floor.First, Floor.Third);

            // Assert
            Assert.That(_floorsVisited[0], Is.EqualTo(Floor.Ground));
            Assert.That(_floorsVisited[1], Is.EqualTo(Floor.First));
            Assert.That(_floorsVisited[2], Is.EqualTo(Floor.Second));
            Assert.That(_floorsVisited[3], Is.EqualTo(Floor.Third));

            // Clean up
            theLift.Dispose();
        }

        public void OnNext(Floor currentFloor)
        {
            _floorsVisited.Add(currentFloor);
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