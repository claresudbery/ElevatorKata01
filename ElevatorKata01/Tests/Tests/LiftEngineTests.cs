using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using ElevatorKata01.Elements;
using ElevatorKata01.FunctionalCode;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace ElevatorKata01.Tests.Tests
{
    [TestFixture]
    public class LiftEngineTests : IObserver<int>
    {
        private readonly List<int> _floorsVisited = new List<int>();

        [Test]
        public void When_Lift_Moves_From_One_Floor_To_The_Next_Floor_Up_Then_Both_Floors_Will_Be_Visited_In_Order()
        {
            // Arrange
            _floorsVisited.Clear();
            TestScheduler testScheduler = new TestScheduler();
            LiftEngine liftEngine = new LiftEngine(testScheduler);
            liftEngine.Subscribe(this);

            // Act
            testScheduler.Schedule(
                TimeSpan.FromMilliseconds(0),
                () => liftEngine.Travel(1, 2));
            testScheduler.Start();

            // Assert
            Assert.That(_floorsVisited.Count, Is.EqualTo(2), "Expected two floor-visiting events");
            Assert.That(_floorsVisited[0], Is.EqualTo(1), "Expected to visit floor 1 first");
            Assert.That(_floorsVisited[1], Is.EqualTo(2), "Expected to visit floor 2 second");
        }

        [Test]
        public void When_Lift_Moves_From_One_Floor_To_The_Next_Floor_Down_Then_Both_Floors_Will_Be_Visited_In_Order()
        {
            // Arrange
            _floorsVisited.Clear();
            TestScheduler testScheduler = new TestScheduler();
            LiftEngine liftEngine = new LiftEngine(testScheduler);
            liftEngine.Subscribe(this);

            // Act
            testScheduler.Schedule(
                TimeSpan.FromMilliseconds(0),
                () => liftEngine.Travel(2, 1));
            testScheduler.Start();

            // Assert
            Assert.That(_floorsVisited.Count, Is.EqualTo(2), "Expected two floor-visiting events");
            Assert.That(_floorsVisited[0], Is.EqualTo(2), "Expected to visit floor 2 first");
            Assert.That(_floorsVisited[1], Is.EqualTo(1), "Expected to visit floor 1 second");
        }

        [Test]
        public void When_Lift_Moves_From_One_Floor_To_Another_Floor_Then_All_Floors_Will_Be_Visited_In_Order()
        {
            // Arrange
            _floorsVisited.Clear();
            TestScheduler testScheduler = new TestScheduler();
            LiftEngine liftEngine = new LiftEngine(testScheduler);
            liftEngine.Subscribe(this);

            // Act
            testScheduler.Schedule(
                TimeSpan.FromMilliseconds(0),
                () => liftEngine.Travel(7, 2));
            testScheduler.Start();

            // Assert
            Assert.That(_floorsVisited.Count, Is.EqualTo(6), "Expected six floor-visiting events");
            Assert.That(_floorsVisited[0], Is.EqualTo(7), "Expected to visit floor 7 first");
            Assert.That(_floorsVisited[1], Is.EqualTo(6), "Expected to visit floor 6 second");
            Assert.That(_floorsVisited[2], Is.EqualTo(5), "Expected to visit floor 5 third");
            Assert.That(_floorsVisited[3], Is.EqualTo(4), "Expected to visit floor 4 fourth");
            Assert.That(_floorsVisited[4], Is.EqualTo(3), "Expected to visit floor 3 fifth");
            Assert.That(_floorsVisited[5], Is.EqualTo(2), "Expected to visit floor 2 sixth");
        }

        [Test]
        public void When_Lift_Gets_A_New_Request_Mid_Flight_Then_All_Events_Are_Reset_Even_If_It_Makes_No_Sense()
        {
            // Arrange
            _floorsVisited.Clear();
            TestScheduler testScheduler = new TestScheduler();
            LiftEngine liftEngine = new LiftEngine(testScheduler);
            liftEngine.Subscribe(this);

            // Act
            testScheduler.Schedule(
                TimeSpan.FromMilliseconds(0),
                () => liftEngine.Travel(7, 2));

            testScheduler.Schedule(
                // Note that in order for the lift to have time to visit three floors, we need to give it four floor intervals
                TimeSpan.FromMilliseconds(TimeConstants.FloorInterval * 4),
                () => liftEngine.Travel(3, 5));

            testScheduler.Start();

            // Assert
            Assert.That(_floorsVisited.Count, Is.EqualTo(6), "Expected six floor-visiting events");

            Assert.That(_floorsVisited[0], Is.EqualTo(7), "Expected to visit floor 7 first");
            Assert.That(_floorsVisited[1], Is.EqualTo(6), "Expected to visit floor 6 second");
            Assert.That(_floorsVisited[2], Is.EqualTo(5), "Expected to visit floor 5 third");

            Assert.That(_floorsVisited[3], Is.EqualTo(3), "Expected to visit floor 3 fourth");
            Assert.That(_floorsVisited[4], Is.EqualTo(4), "Expected to visit floor 4 fifth");
            Assert.That(_floorsVisited[5], Is.EqualTo(5), "Expected to visit floor 5 sixth");
        }

        public void OnNext(int floorVisited)
        {
            _floorsVisited.Add(floorVisited);
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}