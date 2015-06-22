using System;
using System.Collections.Generic;

namespace ElevatorKata01
{
    public class ObservableLift : IObservable<LiftStatus>, IDisposable
    {
        private readonly List<IObserver<LiftStatus>> _observers = new List<IObserver<LiftStatus>>();
        private readonly int _currentFloor;
        private Direction _currentDirection;

        public ObservableLift(int startingFloor)
        {
            _currentFloor = startingFloor;
            _currentDirection = Direction.None;
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
                MoveUpwards();
            }
            else
            {
                MoveDownwards();
            }
        }

        private void MoveUpwards()
        {
            _currentDirection = Direction.Up;
            NotifyObserversOfCurrentStatus();
        }

        private void MoveDownwards()
        {
            _currentDirection = Direction.Down;
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