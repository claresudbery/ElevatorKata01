using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using ElevatorKata01.Elements;

namespace ElevatorKata01.FunctionalCode
{
    public class LiftManager : IObservable<LiftStatus>, IDisposable, IObserver<LiftStatus>, ILiftRequestHandler
    {
        private readonly List<IObserver<LiftStatus>> _observers = new List<IObserver<LiftStatus>>();
        private readonly IScheduler _scheduler;
        private ObservableLift _theLift;

        public LiftManager(IScheduler scheduler)
        {
            _scheduler = scheduler;
            _theLift = new ObservableLift(0, scheduler);
            _theLift.Subscribe(this);
        }

        public IDisposable Subscribe(IObserver<LiftStatus> observer)
        {
            _observers.Add(observer);
            return this;
        }

        public void Dispose()
        {
            _theLift.Dispose();
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }

        public void MakeUpwardsRequestFrom(int destinationFloor)
        {
            _theLift.MakeUpwardsRequestFrom(destinationFloor);
        }

        public void MakeDownwardsRequestFrom(int destinationFloor)
        {
            _theLift.MakeDownwardsRequestFrom(destinationFloor);
        }

        public void OnNext(LiftStatus liftStatus)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(liftStatus);
            }
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