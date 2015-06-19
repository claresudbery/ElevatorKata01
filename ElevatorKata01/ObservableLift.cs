using System;

namespace ElevatorKata01
{
    public class ObservableLift : IObservable<Floor>, IDisposable
    {
        private IObserver<Floor> _observer;
        private Floor _currentFloor;

        public IDisposable Subscribe(IObserver<Floor> observer)
        {
            _observer = observer;
            return this;
        }

        public Floor CurrentFloor 
        {
            get
            {
                return _currentFloor;
            }
            set
            {
                _currentFloor = value;
                _observer.OnNext(_currentFloor);
            }
        }

        public void Move(Floor from, Floor to)
        {
            CurrentFloor = from;
            CurrentFloor = to;
        }

        public void Dispose()
        {
            _observer.OnCompleted();
        }
    }
}