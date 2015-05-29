using NUnit.Framework;

namespace ElevatorKata01
{
    [TestFixture]
    public class ElevatorTests
    {
        [Test]
        public void WhenICallLiftToGroundFloorThenLiftGoesToGroundFloor()
        {
            // Arrange
            var theLift = new Lift(Floor.Ground);
            
            // Act
            Floor result = theLift.Call(Floor.First);

            // Assert
            Assert.That(result, Is.EqualTo(Floor.First));
        }

        [Test]
        public void WhenIAmOnGroundFloorAndIRequestTravelToTheFirstFloorThenLiftTakesMeToTheFirstFloor()
        {
            // Arrange
            var theLift = new Lift(Floor.Ground);

            // Act
            Floor result = theLift.Move(Floor.First);

            // Assert
            Assert.That(result, Is.EqualTo(Floor.First));
        }

        [Ignore]
        [Test]
        public void Given_lift_is_on_groundfloor_and_personA_is_on_the_firstfloor_and_personB_is_on_the_second_floor_and_both_people_want_to_ascend_When_lift_is_called_then_it_will_fetch_personA_first()
        {
            // Arrange
            //var theLift = new Lift();
            //theLift.CurrentFloor = Floor.Second;

            //// Act
            //var result = theLift.Call(Floor.Ground, Floor.First);

            //// Assert
            //Assert.That(theLift.CurrentFloor, Is.EqualTo(Floor.Ground));
        }
    }
}