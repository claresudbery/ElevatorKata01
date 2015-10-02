using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using ElevatorKata01.Elements;
using ElevatorKata01.FunctionalCode;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace ElevatorKata01.Tests.Helpers
{
    public class IndividualLiftTestHelper : ILiftMonitor
    {
        private readonly List<LiftStatus> _liftStatuses = new List<LiftStatus>();
        private readonly List<ExpectedLiftStatus> _expectedLiftStatuses = new List<ExpectedLiftStatus>();

        private readonly TestScheduler _testScheduler;
        private TimeTracker _timeTracker;
        private ObservableLift _theLift;
        private int _millisecondsTakenByMostRecentEvent;
        private int _numExpectedStatuses;
        private int _currentLiftFloor;
        private bool _testStarted = false;
        private bool _anyExpectations = true;

        public string LiftName { get; private set; }

        public IndividualLiftTestHelper(
            string liftName, 
            TestScheduler testScheduler,
            LiftManagerTestHelper liftManagerTestHelper = null)
        {
            LiftName = liftName;
            _timeTracker = new TimeTracker(liftManagerTestHelper);
            _testScheduler = testScheduler;
        }

        public void CheckExpectedStatuses()
        {
            if (_anyExpectations)
            {
                Assert.That(_liftStatuses.Count, Is.EqualTo(_numExpectedStatuses));
                Assert.That(_expectedLiftStatuses.Count, Is.GreaterThan(0), "No expected events were marked for testing!");
            }

            foreach (var expectedStatus in _expectedLiftStatuses)
            {
                Assert.That(
                    _liftStatuses[expectedStatus.StatusIndex].CurrentDirection, 
                    Is.EqualTo(expectedStatus.Status.CurrentDirection),
                    "Floor " + expectedStatus.Status.CurrentFloor
                        + ", Direction " + expectedStatus.Status.CurrentDirection
                        + ", Index " + expectedStatus.StatusIndex
                        + ", Name " + LiftName);

                Assert.That(
                    _liftStatuses[expectedStatus.StatusIndex].CurrentFloor, 
                    Is.EqualTo(expectedStatus.Status.CurrentFloor),
                    "Bad floor num: Floor " + expectedStatus.Status.CurrentFloor
                        + ", Direction " + expectedStatus.Status.CurrentDirection
                        + ", Index " + expectedStatus.StatusIndex
                        + ", Name " + LiftName);
            }
        }

        public void VerifyAllMarkers()
        {
            try
            {
                Assert.That(_testStarted, Is.EqualTo(true), "Test scheduler was never kicked off!");

                CheckExpectedStatuses();
            }
            finally
            {
                EnsureThatAllScheduledEventsAreRunThroughToCompletion();

                DisposeLift();

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
                SecondsSinceTestStarted = _timeTracker.MillisecondsSinceTestStarted / 1000m,
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

            _timeTracker.MillisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(_timeTracker.MillisecondsSinceTestStarted).Ticks);
        }

        public void StartTestForLiftManager()
        {
            _timeTracker.MillisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;
        }

        public void MakeRequestToMoveTo(int floor, bool shouldBeActedUponImmediately)
        {
            AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            Scheduler.Schedule(
                _testScheduler,
                TimeSpan.FromMilliseconds(_timeTracker.MillisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval),
                () => _theLift.MoveTo(floor));
        }

        public void MakeDownwardsRequestFrom(int floor, bool shouldBeActedUponImmediately)
        {
            AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            Scheduler.Schedule(_testScheduler, TimeSpan.FromMilliseconds(_timeTracker.MillisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval),
                () => _theLift.MakeDownwardsRequestFrom(floor));
        }

        public void MakeUpwardsRequestFrom(int floor, bool shouldBeActedUponImmediately)
        {
            AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            Scheduler.Schedule(_testScheduler, TimeSpan.FromMilliseconds(_timeTracker.MillisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval),
                () => _theLift.MakeUpwardsRequestFrom(floor));
        }

        public void AmendMostRecentEventTimeIfNecessary(bool shouldBeActedUponImmediately)
        {
            _millisecondsTakenByMostRecentEvent = shouldBeActedUponImmediately
                ? TimeConstants.FloorInterval + TimeConstants.BetweenFloorsInterval
                : _millisecondsTakenByMostRecentEvent;
        }

        public IndividualLiftTestHelper ExpectToStopAt(int floor)
        {
            _timeTracker.MillisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;

            _millisecondsTakenByMostRecentEvent = TimeConstants.WaitTime + TimeConstants.FloorInterval;
            _numExpectedStatuses++;
            _currentLiftFloor = floor;
            return this;
        }

        public IndividualLiftTestHelper ExpectToLeaveFrom(int floor)
        {
            return ExpectToVisit(floor);
        }

        public IndividualLiftTestHelper ExpectToVisit(int floor)
        {
            _timeTracker.MillisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;

            _millisecondsTakenByMostRecentEvent = TimeConstants.FloorInterval;
            _numExpectedStatuses++;
            _currentLiftFloor = floor;
            return this;
        }

        public void InitialiseTestData(int millisecondsSinceTestStarted)
        {
            _liftStatuses.Clear();
            _expectedLiftStatuses.Clear();

            _timeTracker.MillisecondsSinceTestStarted = millisecondsSinceTestStarted;

            _millisecondsTakenByMostRecentEvent = 0;
            _numExpectedStatuses = 0;
        }

        public void MakeStartAt(int floor)
        {
            InitialiseTestData(TimeConstants.FloorInterval);

            _theLift = new ObservableLift(floor, _testScheduler, LiftName);
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

        public void DisposeLift()
        {
            if (_theLift != null)
            {
                _theLift.Dispose();
            }
        }

        public void NoExpectations()
        {
            _anyExpectations = false;
        }

        public void UpdateMillisecondsSinceTestStarted(int millisecondsSinceTestStarted)
        {
            _timeTracker.MillisecondsSinceTestStarted = millisecondsSinceTestStarted;
        }
    }
}