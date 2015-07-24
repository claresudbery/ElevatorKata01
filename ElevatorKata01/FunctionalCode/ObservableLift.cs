﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ElevatorKata01.Elements;

namespace ElevatorKata01.FunctionalCode
{
    public class ObservableLift : IObservable<LiftStatus>, IDisposable
    {
        private readonly List<IObserver<LiftStatus>> _observers = new List<IObserver<LiftStatus>>();
        private int _currentFloor;
        private Direction _currentDirectionBeingProcessed;
        private Direction _currentDirectionActuallyMovingIn;
        private IDisposable _liftEngineSubscription = null;
        private IDisposable _waitTimerSubscription = null;
        private readonly List<int> _goingUp = new List<int>();
        private readonly List<int> _goingDown = new List<int>();
        private readonly IScheduler _scheduler;
        private const int GroundFloor = 0;

        private delegate void FloorOperator(int floor);
        private delegate int FloorReturner();

        public ObservableLift(int startingFloor, IScheduler scheduler)
        {
            _currentFloor = startingFloor;
            _currentDirectionBeingProcessed = Direction.None;
            _currentDirectionActuallyMovingIn = Direction.None;
            _scheduler = scheduler;
            WaitForNextInstruction();
        }

        public IDisposable Subscribe(IObserver<LiftStatus> observer)
        {
            _observers.Add(observer);
            return this;
        }

        public void Dispose()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }

        public void Move(int destinationFloor)
        {
            if (destinationFloor > _currentFloor)
            {
                ProcessUpwardsMoveRequest(destinationFloor);
            }
            else
            {
                ProcessDownwardsMoveRequest(destinationFloor);
            }
        }

        public void CallUp(int destinationFloor)
        {
            ProcessUpwardsMoveRequest(destinationFloor);
        }

        public void CallDown(int destinationFloor)
        {
            ProcessDownwardsMoveRequest(destinationFloor);
        }

        private void ProcessUpwardsMoveRequest(int destinationFloor)
        {
            _goingUp.Add(destinationFloor);

            MoveInCorrectDirection();
        }

        private void ProcessDownwardsMoveRequest(int destinationFloor)
        {
            _goingDown.Add(destinationFloor);

            MoveInCorrectDirection();
        }

        private void MoveInCorrectDirection()
        {
            if (NotMoving)
            {
                if (_currentDirectionBeingProcessed != Direction.Down && UpFloorsWaiting)
                {
                    MoveUpwards();
                }
                else if (DownFloorsWaiting)
                {
                    MoveDownwards();
                }
                else
                {
                    if (_currentFloor != GroundFloor)
                    {
                        ReturnToGroundFloor();
                    }
                }
            }
        }

        private void StartMoving(int nextFloor, Action<int> arrivedAtFloor)
        {
            _liftEngineSubscription = Observable.Generate
                (
                    _currentFloor,
                    floor => _currentFloor < nextFloor
                        ? HaveWeArrivedAtHigherDestinationYet(floor, nextFloor)
                        : HaveWeArrivedAtLowerDestinationYet(floor, nextFloor),
                    floor => _currentFloor < nextFloor
                        ? NextFloorUp(floor)
                        : NextFloorDown(floor), // iterator
                    floor => floor, // actual value
                    floor => TimeSpan.FromMilliseconds(TimeConstants.FloorInterval),
                    _scheduler
                )
                .Subscribe
                (
                    arrivedAtFloor
                );

            _currentDirectionActuallyMovingIn = _currentFloor < nextFloor ? Direction.Up : Direction.Down;
        }

        private void MoveUpwards()
        {
            _currentDirectionBeingProcessed = Direction.Up;

            StartMoving(NextUpFloor, ArrivedAtFloorOnTheWayUp);
        }

        private void MoveDownwards()
        {
            _currentDirectionBeingProcessed = Direction.Down;

            StartMoving(NextDownFloor, ArrivedAtFloorOnTheWayDown);
        }

        private void ReturnToGroundFloor()
        {
            _currentDirectionBeingProcessed = Direction.None;

            StartMoving(GroundFloor, ArrivedAtFloorOnTheWayToTheGroundFloor);
        }

        private void ArrivedAtFloor(
            int floor,
            int nextFloor,
            FloorOperator removeFloorFromDestinations)
        {
            if (floor == nextFloor)
            {
                _currentFloor = floor;
                Stop();
                removeFloorFromDestinations(floor);
            }
            else
            {
                _currentFloor = floor;
                NotifyObserversOfCurrentStatus();
            }
        }

        private void ArrivedAtFloorOnTheWayUp(int floor)
        {
            // TODO: What if we somehow find ourselves going up past the top floor??

            ArrivedAtFloor(floor, NextUpFloor, RemoveUpFloorFromDestinations);
        }

        private void ArrivedAtFloorOnTheWayDown(int floor)
        {
            // TODO: What if we somehow find ourselves going down past the bottom floor??

            ArrivedAtFloor(floor, NextDownFloor, RemoveDownFloorFromDestinations);
        }

        private void ArrivedAtFloorOnTheWayToTheGroundFloor(int floor)
        {
            // TODO: What if we somehow find ourselves going past the ground floor??

            ArrivedAtFloor(floor, GroundFloor, DoNothing);
        }

        private void Stop()
        {
            _currentDirectionActuallyMovingIn = Direction.None;
            _liftEngineSubscription.Dispose();
            NotifyObserversOfCurrentStatus();
            WaitForNextInstruction();
        }

        private void WaitForNextInstruction()
        {
            if (null != _waitTimerSubscription)
            {
                _waitTimerSubscription.Dispose();
                _waitTimerSubscription = null;
            }

            _waitTimerSubscription = Observable.Generate
                (
                    0,
                    i => i <= 1,
                    i => i + 1, // iterator
                    i => i, // actual value
                    i => TimeSpan.FromMilliseconds(TimeConstants.FloorInterval * 5),
                    _scheduler
                )
                .Subscribe
                (
                    StopWaiting
                );
        }

        private void StopWaiting(int i)
        {
            _waitTimerSubscription.Dispose();
            _waitTimerSubscription = null;

            MoveInCorrectDirection();
        }

        private void NotifyObserversOfCurrentStatus()
        {
            foreach (var observer in _observers)
            {
                observer.OnNext
                (
                    new LiftStatus
                    {
                        CurrentDirection = _currentDirectionActuallyMovingIn,
                        CurrentFloor = _currentFloor
                    }
                );
            }
        }

        private void RemoveUpFloorFromDestinations(int floor)
        {
            _goingUp.Remove(floor);
        }

        private void RemoveDownFloorFromDestinations(int floor)
        {
            _goingDown.Remove(floor);
        }

        private void DoNothing(int floor)
        {
            // Do nothing
        }

        private bool HaveWeArrivedAtHigherDestinationYet(int floor, int destinationFloor)
        {
            return floor <= destinationFloor;
        }

        private bool HaveWeArrivedAtLowerDestinationYet(int floor, int destinationFloor)
        {
            return floor >= destinationFloor;
        }

        private int NextFloorDown(int floor)
        {
            return floor - 1;
        }

        private int NextFloorUp(int floor)
        {
            return floor + 1;
        }

        private bool NotMoving
        {
            get
            {
                return (_currentDirectionActuallyMovingIn == Direction.None);
            }
        }

        private bool NoUpFloors
        {
            get
            {
                return !_goingUp.Any(i => i > _currentFloor);
            }
        }

        private bool NoDownFloors
        {
            get
            {
                return !_goingDown.Any(i => i < _currentFloor);
            }
        }

        private bool UpFloorsWaiting
        {
            get
            {
                return _goingUp.Any(i => i > _currentFloor);
            }
        }

        private bool DownFloorsWaiting
        {
            get
            {
                return _goingDown.Any(i => i < _currentFloor);
            }
        }

        private void CheckForUpFloors(string itemRequested)
        {
            if (NoUpFloors)
            {
                throw new Exception(itemRequested + " was requested, but GoingUp has no more floors above the current floor.");
            }
        }

        private void CheckForDownFloors(string itemRequested)
        {
            if (NoDownFloors)
            {
                throw new Exception(itemRequested + " was requested, but GoingDown has no more floors below the current floor.");
            }
        }

        private int NextUpFloor
        {
            get
            {
                CheckForUpFloors("NextUpFloor");
                return _goingUp.Where(i => i > _currentFloor).Min();
            }
        }

        private int NextDownFloor
        {
            get
            {
                CheckForDownFloors("NextDownFloor");
                return _goingDown.Where(i => i < _currentFloor).Max();
            }
        }
    }
}