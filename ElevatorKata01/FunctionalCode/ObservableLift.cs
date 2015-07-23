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
        private IObservable<int> _liftEngine = null;
        private IDisposable _liftEngineSubscription = null;
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
                MoveUp(destinationFloor);
            }
            else
            {
                MoveDown(destinationFloor);
            }
        }

        public void MoveUp(int destinationFloor)
        {
            _goingUp.Add(destinationFloor);

            MoveInCorrectDirection();
        }

        public void MoveDown(int destinationFloor)
        {
            _goingDown.Add(destinationFloor);

            MoveInCorrectDirection();
        }

        public void CallUp(int destinationFloor)
        {
            MoveUp(destinationFloor);
        }

        public void CallDown(int destinationFloor)
        {
            MoveDown(destinationFloor);
        }

        private bool NotMoving
        {
            get
            {
                return (_moving == false);
            }
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
            _liftEngine = Observable.Generate
                (
                    _currentFloor,
                    i => i <= LastUpFloor,
                    i => i + 1, // iterator
                    i => i, // actual value
                    i => TimeSpan.FromMilliseconds(TimeConstants.FloorInterval),
                    _scheduler
                );

            _currentDirection = Direction.Up;
            _moving = true;

            _liftEngineSubscription = _liftEngine.Subscribe
                (
                    ArrivedAtFloorOnTheWayUp
                );
        }

        private void MoveDownwards()
        {
            _liftEngine = Observable.Generate
                (
                    _currentFloor,
                    i => i >= LastDownFloor,
                    i => i - 1, // iterator
                    i => i, // actual value
                    i => TimeSpan.FromMilliseconds(TimeConstants.FloorInterval),
                    _scheduler
                );

            _currentDirection = Direction.Down;
            _moving = true;

            _liftEngineSubscription = _liftEngine.Subscribe
                (
                    ArrivedAtFloorOnTheWayDown
                );
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

        private void ArrivedAtFloorOnTheWayUp(int floor)
        {
            // TODO: What if we somehow find ourselves going up past the top floor??

            if (floor == NextUpFloor)
            {
                _currentFloor = floor;
                Stop();
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
            }

            _currentFloor = floor;

            NotifyObserversOfCurrentStatus();
        }

        private void Stop()
        {
            _moving = false;
            _liftEngineSubscription.Dispose();
            NotifyObserversOfCurrentStatus();
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
    }
}