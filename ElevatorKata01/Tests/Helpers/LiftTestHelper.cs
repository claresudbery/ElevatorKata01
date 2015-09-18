using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using ElevatorKata01.Elements;
using ElevatorKata01.FunctionalCode;
using ElevatorKata01.Tests.Helpers;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

// ReSharper disable CheckNamespace 
// - this is test helper code but not actual test code, so I don't want it in the Tests folder
namespace ElevatorKata01.Tests.Tests
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// All the utility code is in here, so that the tests themselves can be kept separate.
    /// </summary>
    public class LiftTestHelper : ILiftMonitor
    {
        private readonly List<LiftStatus> _liftStatuses = new List<LiftStatus>();
        private readonly List<ExpectedLiftStatus> _expectedLiftStatuses = new List<ExpectedLiftStatus>();

        private readonly TestScheduler _testScheduler = new TestScheduler();
        private ObservableLift _theLift;
        private int _millisecondsSinceTestStarted;
        private int _millisecondsTakenByMostRecentEvent;
        private int _numExpectedStatuses;
        private int _currentLiftFloor;
        private bool _testStarted = false;

        public void VerifyAllMarkers()
        {
            try
            {
                Assert.That(_testStarted, Is.EqualTo(true), "Test scheduler was never kicked off!");
                Assert.That(_liftStatuses.Count, Is.EqualTo(_numExpectedStatuses));
                Assert.That(_expectedLiftStatuses.Count, Is.GreaterThan(0), "No expected events were marked for testing!");

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

        public void Mark(Direction direction)
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

        public void StartTest()
        {
            _testStarted = true;
            _millisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted).Ticks);
        }

        public void LiftMakeRequestToMoveTo(int floor, bool shouldBeActedUponImmediately)
        {
            AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            Scheduler.Schedule(
                _testScheduler, 
                TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval),
                () => _theLift.MoveTo(floor));
        }

        public void LiftMakeDownwardsRequestFrom(int floor, bool shouldBeActedUponImmediately)
        {
            AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            Scheduler.Schedule(_testScheduler, TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval),
                () => _theLift.MakeDownwardsRequestFrom(floor));
        }

        public void LiftMakeUpwardsRequestFrom(int floor, bool shouldBeActedUponImmediately)
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

        public LiftTestHelper LiftExpectToStopAt(int floor)
        {
            _millisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;
            _millisecondsTakenByMostRecentEvent = TimeConstants.WaitTime + TimeConstants.FloorInterval;
            _numExpectedStatuses++;
            _currentLiftFloor = floor;
            return this;
        }

        public LiftTestHelper LiftExpectToLeaveFrom(int floor)
        {
            return LiftExpectToVisit(floor);
        }

        public LiftTestHelper LiftExpectToVisit(int floor)
        {
            _millisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;
            _millisecondsTakenByMostRecentEvent = TimeConstants.FloorInterval;
            _numExpectedStatuses++;
            _currentLiftFloor = floor;
            return this;
        }

        public void LiftMakeStartAt(int floor)
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