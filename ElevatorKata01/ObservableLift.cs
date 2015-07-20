using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace ElevatorKata01
{
    public class ObservableLift : IObservable<LiftStatus>, IDisposable
    {
        private readonly List<IObserver<LiftStatus>> _observers = new List<IObserver<LiftStatus>>();
        private int _currentFloor;
        private Direction _currentDirection;
        private IObservable<int> _liftEngine = null;
        private IDisposable _liftEngineSubscription = null;
        private readonly List<int> _goingUp = new List<int>();
        private readonly List<int> _goingDown = new List<int>();
        private IScheduler _scheduler;

        private const int TopFloor = 30;
        private const int BottomFloor = -10;

        public ObservableLift(int startingFloor, IScheduler scheduler)
        {
            _currentFloor = startingFloor;
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

        public bool NotMoving
        {
            get
            {
                return (_currentDirection == Direction.None);
            }
        }

        public void Move(int destinationFloor)
        {
            if (destinationFloor > _currentFloor)
            {
                _goingUp.Add(destinationFloor);
                if (NotMoving)
                {
                    MoveUpwards();
                }
            }
            else
            {
                _goingDown.Add(destinationFloor);
                if (NotMoving)
                {
                    MoveDownwards();
                }
            }
        }

        public void Call(int destinationFloor)
        {
            Move(destinationFloor);
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

            _liftEngineSubscription = _liftEngine.Subscribe
                (
                    ArrivedAtFloorOnTheWayDown
                );
        }

        public bool NoUpFloors
        {
            get
            {
                return !_goingUp.Any();
            }
        }

        public bool NoDownFloors
        {
            get
            {
                return !_goingDown.Any();
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

        public int NextUpFloor
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

        public int NextDownFloor
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
            _currentDirection = Direction.None;
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
                        CurrentDirection = _currentDirection,
                        CurrentFloor = _currentFloor
                    }
                );
            }
        }
    }
}