using System;
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
        private bool _moving;
        private Direction _currentDirection;
        private IDisposable _liftEngineSubscription = null;
        private IDisposable _waitTimerSubscription = null;
        private readonly List<int> _goingUp = new List<int>();
        private readonly List<int> _goingDown = new List<int>();
        private readonly IScheduler _scheduler;

        public ObservableLift(int startingFloor, IScheduler scheduler)
        {
            _currentFloor = startingFloor;
            _moving = false;
            _currentDirection = Direction.None;
            _scheduler = scheduler;
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
                if (_currentDirection != Direction.Down && UpFloorsWaiting)
                {
                    MoveUpwards();
                }
                else
                {
                    MoveDownwards();
                }
            }
        }

        private void MoveUpwards()
        {
            _liftEngineSubscription = Observable.Generate
                (
                    _currentFloor,
                    i => i <= LastUpFloor,
                    i => i + 1, // iterator
                    i => i, // actual value
                    i => TimeSpan.FromMilliseconds(TimeConstants.FloorInterval),
                    _scheduler
                )
                .Subscribe
                (
                    ArrivedAtFloorOnTheWayUp
                );

            _currentDirection = Direction.Up;
            _moving = true;
        }

        private void MoveDownwards()
        {
            _liftEngineSubscription = Observable.Generate
                (
                    _currentFloor,
                    i => i >= LastDownFloor,
                    i => i - 1, // iterator
                    i => i, // actual value
                    i => TimeSpan.FromMilliseconds(TimeConstants.FloorInterval),
                    _scheduler
                )
                .Subscribe
                (
                    ArrivedAtFloorOnTheWayDown
                );

            _currentDirection = Direction.Down;
            _moving = true;
        }

        private void ArrivedAtFloorOnTheWayUp(int floor)
        {
            // TODO: What if we somehow find ourselves going up past the top floor??

            if (floor == NextUpFloor)
            {
                _currentFloor = floor;
                Stop();
                RemoveUpFloorFromDestinations(floor);
            }
            else
            {
                _currentFloor = floor;
                NotifyObserversOfCurrentStatus();
            }
        }

        private void ArrivedAtFloorOnTheWayDown(int floor)
        {
            // TODO: What if we somehow find ourselves going down past the bottom floor??

            if (floor == NextDownFloor)
            {
                Stop();
                RemoveDownFloorFromDestinations(floor);
            }

            _currentFloor = floor;

            NotifyObserversOfCurrentStatus();
        }

        private void Stop()
        {
            _moving = false;
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
                        CurrentDirection = _moving ? _currentDirection : Direction.None,
                        CurrentFloor = _currentFloor
                    }
                );
            }
        }

        private bool NotMoving
        {
            get
            {
                return (_moving == false);
            }
        }

        private bool NoUpFloors
        {
            get
            {
                return !_goingUp.Any();
            }
        }

        private bool NoDownFloors
        {
            get
            {
                return !_goingDown.Any();
            }
        }

        private bool UpFloorsWaiting
        {
            get
            {
                return _goingUp.Any();
            }
        }

        private bool DownFloorsWaiting
        {
            get
            {
                return _goingDown.Any();
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

        private void CheckForUpFloors(string itemRequested)
        {
            if (NoUpFloors)
            {
                throw new Exception(itemRequested + " was requested, but GoingUp is empty");
            }
        }

        private void CheckForDownFloors(string itemRequested)
        {
            if (NoDownFloors)
            {
                throw new Exception(itemRequested + " was requested, but GoingDown is empty");
            }
        }

        private int LastUpFloor
        {
            get
            {
                CheckForUpFloors("LastUpFloor");
                return _goingUp.Max();
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

        private int LastDownFloor
        {
            get
            {
                CheckForDownFloors("LastDownFloor");
                return _goingDown.Min();
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