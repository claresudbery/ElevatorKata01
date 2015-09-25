using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using ElevatorKata01.Elements;
using ElevatorKata01.FunctionalCode;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace ElevatorKata01.Tests.Helpers
{
    public class LiftManagerTestHelper : ILiftMonitor
    {
        //private readonly List<LiftStatus> _liftStatuses = new List<LiftStatus>();
        //private readonly List<ExpectedLiftStatus> _expectedLiftStatuses = new List<ExpectedLiftStatus>();

        private readonly List<IndividualLiftTestHelper> _liftTestHelpers = new List<IndividualLiftTestHelper>();
        private readonly TestScheduler _testScheduler = new TestScheduler();
        //private ObservableLift _theLift;
        private LiftManager _theLiftManager;
        //private int _millisecondsSinceTestStarted;
        //private int _millisecondsTakenByMostRecentEvent;
        //private int _numExpectedStatuses;
        //private int _currentLiftFloor;
        private bool _testStarted = false;

        public void VerifyAllMarkers()
        {
            try
            {
                Assert.That(_testStarted, Is.EqualTo(true), "Test scheduler was never kicked off!");
                
                foreach (var liftTestHelper in _liftTestHelpers)
                {
                    liftTestHelper.VerifyAllMarkers();
                }
            }
            finally
            {
                EnsureThatAllScheduledEventsAreRunThroughToCompletion();

                foreach (var liftTestHelper in _liftTestHelpers)
                {
                    liftTestHelper.DisposeLift();
                }

                if (_theLiftManager != null)
                {
                    _theLiftManager.Dispose();
                }

                _testStarted = false;
            }
        }

        private void EnsureThatAllScheduledEventsAreRunThroughToCompletion()
        {
            _testScheduler.Start();
        }

        //public void Mark(Direction direction)
        //{
        //    _expectedLiftStatuses.Add(new ExpectedLiftStatus
        //        {
        //            StatusIndex = _numExpectedStatuses - 1,
        //            SecondsSinceTestStarted = _millisecondsSinceTestStarted / 1000m,
        //            Status = new LiftStatus
        //                {
        //                    CurrentDirection = direction,
        //                    CurrentFloor = _currentLiftFloor
        //                }
        //        });
        //}

        public void StartTest()
        {
            _testStarted = true;

            foreach (var liftTestHelper in _liftTestHelpers)
            {
                liftTestHelper.StartTest();
            }
        }

        public void MakeDownwardsRequestFrom(ILiftRequestHandler liftRequestHandler, int floor, bool shouldBeActedUponImmediately)
        {
            AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            Scheduler.Schedule(_testScheduler, TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval),
                               () => liftRequestHandler.MakeDownwardsRequestFrom(floor));
        }

        public void MakeUpwardsRequestFrom(ILiftRequestHandler liftRequestHandler, int floor, bool shouldBeActedUponImmediately)
        {
            AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            Scheduler.Schedule(_testScheduler, TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval),
                               () => liftRequestHandler.MakeUpwardsRequestFrom(floor));
        }

        public void LiftMakeDownwardsRequestFrom(int floor, bool shouldBeActedUponImmediately)
        {
            MakeDownwardsRequestFrom(_theLift, floor, shouldBeActedUponImmediately);
        }

        public void LiftMakeUpwardsRequestFrom(int floor, bool shouldBeActedUponImmediately)
        {
            MakeUpwardsRequestFrom(_theLift, floor, shouldBeActedUponImmediately);
        }

        public void ManagerMakeDownwardsRequestFrom(int floor, bool shouldBeActedUponImmediately)
        {
            MakeDownwardsRequestFrom(_theLiftManager, floor, shouldBeActedUponImmediately);
        }

        public void ManagerMakeUpwardsRequestFrom(int floor, bool shouldBeActedUponImmediately)
        {
            MakeUpwardsRequestFrom(_theLiftManager, floor, shouldBeActedUponImmediately);
        }

        private void AmendMostRecentEventTimeIfNecessary(bool shouldBeActedUponImmediately)
        {
            _millisecondsTakenByMostRecentEvent = shouldBeActedUponImmediately
                                                      ? TimeConstants.FloorInterval + TimeConstants.BetweenFloorsInterval
                                                      : _millisecondsTakenByMostRecentEvent;
        }

        public LiftManagerTestHelper Lift(string liftName)
        {
            return this;
        }

        public LiftManagerTestHelper LiftExpectToStopAt(int floor)
        {
            _millisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;
            _millisecondsTakenByMostRecentEvent = TimeConstants.WaitTime + TimeConstants.FloorInterval;
            _numExpectedStatuses++;
            _currentLiftFloor = floor;
            return this;
        }

        public LiftManagerTestHelper LiftExpectToLeaveFrom(int floor)
        {
            return LiftExpectToVisit(floor);
        }

        public LiftManagerTestHelper LiftExpectToVisit(int floor)
        {
            _millisecondsSinceTestStarted += _millisecondsTakenByMostRecentEvent;
            _millisecondsTakenByMostRecentEvent = TimeConstants.FloorInterval;
            _numExpectedStatuses++;
            _currentLiftFloor = floor;
            return this;
        }

        public void InitialiseTestData()
        {
            _liftStatuses.Clear();
            _expectedLiftStatuses.Clear();

            _millisecondsSinceTestStarted = TimeConstants.FloorInterval;
            _millisecondsTakenByMostRecentEvent = 0;
            _numExpectedStatuses = 0;
        }

        public void LiftMakeStartAt(int floor)
        {
            InitialiseTestData();

            _theLift = new ObservableLift(floor, _testScheduler);
            _theLift.Subscribe(this);
        }

        public void ManagerMakeStart(List<string> liftNames)
        {
            InitialiseTestData();

            _theLiftManager = new LiftManager(_testScheduler, liftNames);
            _theLiftManager.Subscribe(this);
        }

        public void OnNext(LiftStatus currentLiftStatus)
        {
            _liftTestHelpers
                .First(x => x.LiftName == currentLiftStatus.LiftName)
                .OnNext(currentLiftStatus);
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