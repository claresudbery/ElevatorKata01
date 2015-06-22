using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

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

    internal class LiftSpares
    {
        private IObservable<int> _liftEngine;
        private IDisposable _liftEngineSubscription;

        public int CurrentFloor { get; private set; }
        public Direction CurrentDirection { get; private set; }
        public bool Moving { get; private set; }
        public List<int> GoingUp { get; private set; }
        public List<int> GoingDown { get; private set; }

        public LiftSpares(
            IObservable<int> internalControlPanel,
            IObservable<Call> externalControlPanel)
        {
            GoingUp = new List<int>();
            GoingDown = new List<int>();

            internalControlPanel.Subscribe
            (
                Move
            );

            externalControlPanel.Subscribe
            (
                Call
            );

            throw new NotImplementedException();
        }

        private void ArrivedAtFloorOnTheWayUp(int floor)
        {
            // TODO: What if we somehow find ourselves going up past the top floor??
            if (floor == NextUpFloor)
            {
                Stop();
                Wait(); 
            }
            CurrentFloor = floor;
            throw new NotImplementedException();
        }

        private void ArrivedAtFloorOnTheWayDown(int floor)
        {
            // TODO: What if we somehow find ourselves going up past the lowest floor there is??
            throw new NotImplementedException();
        }

        private void Stop()
        {
            _liftEngineSubscription.Dispose();
            Moving = false;
            throw new NotImplementedException();
        }

        private void Wait()
        {
            throw new NotImplementedException();
        }

        private void CallUp(int destinationFloor)
        {
            GoingUp.Add(destinationFloor);
            if (NotMoving)
            {
                MoveUpwards();
            }

            throw new NotImplementedException();
        }

        private void CallDown(int destinationFloor)
        {
            GoingDown.Add(destinationFloor);
            if (NotMoving)
            {
                MoveDownwards();
            }

            throw new NotImplementedException();
        }

        public void Call(Call call)
        {
            throw new NotImplementedException();
        }

        public void Move(int destinationFloor)
        {
            if (destinationFloor > CurrentFloor)
            {
                GoingUp.Add(destinationFloor);
                MoveUpwards();
            }
            else
            {
                GoingDown.Add(destinationFloor);
                MoveDownwards();
            }
            throw new NotImplementedException();
        }

        private void MoveDownwards()
        {
            throw new NotImplementedException();
        }

        private void MoveUpwards()
        {
            if (GoingUp.Any())
            {
                _liftEngine = Observable.Generate
                (
                    0,
                    i => i < LastUpFloor,
                    i => i + 1, // iterator
                    i => i + 1, // actual value? Shouldn't use same val as iterator?
                    i => TimeSpan.FromSeconds(1)
                );

                Moving = true;
                CurrentDirection = Direction.Up;

                _liftEngineSubscription = _liftEngine.Subscribe
                (
                    ArrivedAtFloorOnTheWayUp
                );
            }
            else
            {
                MoveDownwards();
            }

            throw new NotImplementedException();
        }

        public bool NotMoving
        {
            get
            {
                return !Moving;
            }
        }

        public bool NoUpFloors
        {
            get
            {
                return !GoingUp.Any();
            }
        }

        public bool NoDownFloors
        {
            get
            {
                return !GoingDown.Any();
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
                return GoingUp.Max();
            }
        }

        public int NextUpFloor
        {
            get
            {
                CheckForUpFloors("NextUpFloor");
                return GoingUp.Where(i => i > CurrentFloor).Min();
            }
        }

        private int LastDownFloor
        {
            get
            {
                CheckForDownFloors("LastDownFloor");
                return GoingDown.Min();
            }
        }

        public int NextDownFloor
        {
            get
            {
                CheckForDownFloors("NextDownFloor");
                return GoingDown.Where(i => i < CurrentFloor).Max();
            }
        }

        private void StopWaitingUp()
        {
            if (CurrentFloor == LastUpFloor)
            {
                MoveDownwards();
            }
            else
            {
                MoveUpwards();
            }
            throw new NotImplementedException();
        }

        private void StopWaitingDown()
        {
            throw new NotImplementedException();
        }
    }
}