using System;
using System.Reactive.Concurrency;
using ElevatorKata01.Elements;
using ElevatorKata01.FunctionalCode;
using ElevatorKata01.Tests.Helpers;
using NUnit.Framework;

// ReSharper disable CheckNamespace 
// - this is test helper code but not actual test code, so I don't want it in the Tests folder
namespace ElevatorKata01.Tests.Tests
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// All the utility code is in here, so that the tests themselves can be kept separate.
    /// </summary>
    public partial class ElevatorTests : ILiftMonitor
    {
        private void VerifyAllMarkers()
        {
            try
            {
                Assert.That(_testStarted, Is.EqualTo(true), "Test scheduler was never kicked off!");
                Assert.That((object) _liftStatuses.Count, Is.EqualTo(_numExpectedStatuses));

                foreach (var expectedStatus in _expectedLiftStatuses)
                {
                    Assert.That(
                        _liftStatuses[expectedStatus.StatusIndex].CurrentDirection, 
                        Is.EqualTo(expectedStatus.Status.CurrentDirection),
                        "Floor " + expectedStatus.Status.CurrentFloor
                            + ", Direction " + expectedStatus.Status.CurrentDirection 
                            + ", Index " + expectedStatus.StatusIndex);

                    Assert.That(
                        _liftStatuses[expectedStatus.StatusIndex].CurrentFloor, 
                        Is.EqualTo(expectedStatus.Status.CurrentFloor),
                        "Floor " + expectedStatus.Status.CurrentFloor
                            + ", Direction " + expectedStatus.Status.CurrentDirection 
                            + ", Index " + expectedStatus.StatusIndex);
                }
            }
            finally
            {
                EnsureThatAllScheduledEventsAreRunThroughToCompletion();
                _theLift.Dispose();
                _testStarted = false;
            }
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

            Scheduler.Schedule(
                _testScheduler, 
                TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval),
                () => _theLift.MoveTo(floor));
        }

        private void LiftMakeDownwardsRequestFrom(int floor, bool shouldBeActedUponImmediately)
        {
            AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            Scheduler.Schedule(_testScheduler, TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval),
                () => _theLift.MakeDownwardsRequestFrom(floor));
        }

        private void LiftMakeUpwardsRequestFrom(int floor, bool shouldBeActedUponImmediately)
        {
            AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            Scheduler.Schedule(_testScheduler, TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval),
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