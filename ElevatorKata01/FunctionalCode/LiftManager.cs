using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using ElevatorKata01.Elements;

namespace ElevatorKata01.FunctionalCode
{
    public class LiftManager : IObservable<LiftStatus>, IDisposable, ILiftMonitor, ILiftRequestHandler
    {
        private readonly List<IObserver<LiftStatus>> _observers = new List<IObserver<LiftStatus>>();
        private readonly IScheduler _scheduler;
        private readonly List<ObservableLift> _theLifts = new List<ObservableLift>();

        public LiftManager(IScheduler scheduler, List<string> liftNames)
        {
            _scheduler = scheduler;
            _theLifts.Add(new ObservableLift(0, scheduler));
            _theLifts[0].Subscribe(this);
        }

        public IDisposable Subscribe(IObserver<LiftStatus> observer)
        {
            _observers.Add(observer);
            return this;
        }

        public void Dispose()
        {
            _theLifts[0].Dispose();
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }

        public void MakeUpwardsRequestFrom(int destinationFloor)
        {
            _theLifts[0].MakeUpwardsRequestFrom(destinationFloor);
        }

        public void MakeDownwardsRequestFrom(int destinationFloor)
        {
            _theLifts[0].MakeDownwardsRequestFrom(destinationFloor);
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