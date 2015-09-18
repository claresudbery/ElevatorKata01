using System;
using System.Collections.Generic;
using ElevatorKata01.Elements;
using ElevatorKata01.FunctionalCode;
using ElevatorKata01.Tests.Helpers;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace ElevatorKata01.Tests.Tests
{
    /// <summary>
    /// All the utility code is in ElevatorUtils.cs, so that the tests themselves can be kept separate.
    /// </summary>
    [TestFixture]
    public class MultipleLiftTests : ILiftMonitor
    {
        private readonly List<LiftStatus> _liftStatuses = new List<LiftStatus>();
        private readonly TestScheduler _testScheduler = new TestScheduler();

        [Test]
        public void When_person_calls_lift_to_higher_floor_number_then_one_lift_will_start_moving_upwards()
        {
            // Arrange
            _liftStatuses.Clear();

            var liftManager = new LiftManager(_testScheduler);
            liftManager.Subscribe(this);

            var expectedLiftStatus = new ExpectedLiftStatus
            {
                StatusIndex = 0,
                SecondsSinceTestStarted = TimeConstants.FloorInterval / 1000m,
                Status = new LiftStatus
                {
                    CurrentDirection = Direction.Up,
                    CurrentFloor = 0
                }
            };

            // Act
            liftManager.MakeUpwardsRequestFrom(3);
            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds((double)(2 * expectedLiftStatus.SecondsSinceTestStarted * 1000)).Ticks);

            // Assert
            try
            {
                Assert.That(_liftStatuses.Count, Is.EqualTo(1));

                Assert.That(
                    _liftStatuses[0].CurrentDirection,
                    Is.EqualTo(expectedLiftStatus.Status.CurrentDirection),
                    "Floor " + expectedLiftStatus.Status.CurrentFloor
                    + ", Direction " + expectedLiftStatus.Status.CurrentDirection
                    + ", Index " + expectedLiftStatus.StatusIndex);

                Assert.That(
                    _liftStatuses[0].CurrentFloor,
                    Is.EqualTo(expectedLiftStatus.Status.CurrentFloor),
                    "Floor " + expectedLiftStatus.Status.CurrentFloor
                    + ", Direction " + expectedLiftStatus.Status.CurrentDirection
                    + ", Index " + expectedLiftStatus.StatusIndex);
            }
            finally
            {
                _testScheduler.Start();
                liftManager.Dispose();
            }
        }

        [Test]
        public void When_person_calls_lift_to_lower_floor_number_then_one_lift_will_start_moving_downwards()
        {
            // Arrange
            _liftStatuses.Clear();

            var liftManager = new LiftManager(_testScheduler);
            liftManager.Subscribe(this);

            var expectedLiftStatus = new ExpectedLiftStatus
            {
                StatusIndex = 0,
                SecondsSinceTestStarted = TimeConstants.FloorInterval / 1000m,
                Status = new LiftStatus
                {
                    CurrentDirection = Direction.Down,
                    CurrentFloor = 0
                }
            };

            // Act
            liftManager.MakeDownwardsRequestFrom(-3);
            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds((double)(2 * expectedLiftStatus.SecondsSinceTestStarted * 1000)).Ticks);

            // Assert
            try
            {
                Assert.That(_liftStatuses.Count, Is.EqualTo(1));

                Assert.That(
                    _liftStatuses[0].CurrentDirection,
                    Is.EqualTo(expectedLiftStatus.Status.CurrentDirection),
                    "Floor " + expectedLiftStatus.Status.CurrentFloor
                    + ", Direction " + expectedLiftStatus.Status.CurrentDirection
                    + ", Index " + expectedLiftStatus.StatusIndex);

                Assert.That(
                    _liftStatuses[0].CurrentFloor,
                    Is.EqualTo(expectedLiftStatus.Status.CurrentFloor),
                    "Floor " + expectedLiftStatus.Status.CurrentFloor
                    + ", Direction " + expectedLiftStatus.Status.CurrentDirection
                    + ", Index " + expectedLiftStatus.StatusIndex);
            }
            finally
            {
                _testScheduler.Start();
                liftManager.Dispose();
            }
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