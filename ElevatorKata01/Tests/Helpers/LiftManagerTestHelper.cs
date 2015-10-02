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
        private readonly List<IndividualLiftTestHelper> _liftTestHelpers = new List<IndividualLiftTestHelper>();
        private readonly TestScheduler _testScheduler = new TestScheduler();
        private LiftManager _theLiftManager;
        private int _millisecondsSinceTestStarted;
        private bool _testStarted = false;

        public void VerifyAllMarkers()
        {
            try
            {
                Assert.That(_testStarted, Is.EqualTo(true), "Test scheduler was never kicked off!");
                
                foreach (var liftTestHelper in _liftTestHelpers)
                {
                    liftTestHelper.CheckExpectedStatuses();
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

        public void StartTest()
        {
            _testStarted = true;

            foreach (var liftTestHelper in _liftTestHelpers)
            {
                liftTestHelper.StartTestForLiftManager();
            }

            _testScheduler.AdvanceBy(TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted).Ticks);
        }

        public void AdjustMillisecondsSinceTestStarted(int possibleNewValue)
        {
            _millisecondsSinceTestStarted = Math.Max(_millisecondsSinceTestStarted, possibleNewValue);
        }

        public void MakeDownwardsRequestFrom(int floor, bool shouldBeActedUponImmediately, string expectedLiftName)
        {
            Lift(expectedLiftName).AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            Scheduler.Schedule(_testScheduler, TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval),
                               () => _theLiftManager.MakeDownwardsRequestFrom(floor));
        }

        public void MakeUpwardsRequestFrom(int floor, bool shouldBeActedUponImmediately, string expectedLiftName, bool liftWasPreviouslyIdle = false)
        {
            Lift(expectedLiftName).AmendMostRecentEventTimeIfNecessary(shouldBeActedUponImmediately);

            if (liftWasPreviouslyIdle)
            {
                Lift(expectedLiftName).UpdateMillisecondsSinceTestStarted(_millisecondsSinceTestStarted);
            }

            Scheduler.Schedule(_testScheduler, TimeSpan.FromMilliseconds(_millisecondsSinceTestStarted + TimeConstants.BetweenFloorsInterval),
                               () => _theLiftManager.MakeUpwardsRequestFrom(floor));
        }

        public IndividualLiftTestHelper Lift(string liftName)
        {
            return _liftTestHelpers.First(x => x.LiftName == liftName);
        }

        public void InitialiseTestData()
        {
            _millisecondsSinceTestStarted = TimeConstants.FloorInterval;
        }

        public void MakeStart(List<string> liftNames)
        {
            InitialiseTestData();

            _liftTestHelpers.Clear();

            foreach (var liftName in liftNames)
            {
                var newLiftTestHelper = new IndividualLiftTestHelper(liftName, _testScheduler, this);
                newLiftTestHelper.InitialiseTestData(_millisecondsSinceTestStarted);
                _liftTestHelpers.Add(newLiftTestHelper);
            }

            _theLiftManager = new LiftManager(_testScheduler, liftNames);
            _theLiftManager.Subscribe(this);
        }

        public void OnNext(LiftStatus currentLiftStatus)
        {
            Lift(currentLiftStatus.LiftName).OnNext(currentLiftStatus);
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