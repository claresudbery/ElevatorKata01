﻿using System;
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
    public partial class SingleLiftTests : ILiftMonitor
    {
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

        private void VerifyAllMarkers()
        {
            try
            {
                Assert.That(_testStarted, Is.EqualTo(true), "Test scheduler was never kicked off!");
                Assert.That(_liftStatuses.Count, Is.EqualTo(_numExpectedStatuses));
                Assert.That(_expectedLiftStatuses.Count, Is.GreaterThan(1), "No expected events were marked for testing!");

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

        private SingleLiftTests LiftExpectToStopAt(int floor)
        {
            _millisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;
            _millisecondsTakenByMostRecentEvent = TimeConstants.WaitTime + TimeConstants.FloorInterval;
            _numExpectedStatuses++;
            _currentLiftFloor = floor;
            return this;
        }

        private SingleLiftTests LiftExpectToLeaveFrom(int floor)
        {
            return LiftExpectToVisit(floor);
        }

        private SingleLiftTests LiftExpectToVisit(int floor)
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