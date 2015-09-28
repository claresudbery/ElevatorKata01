using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Security.Cryptography.X509Certificates;
using ElevatorKata01.Elements;

namespace ElevatorKata01.FunctionalCode
{
    public class LiftManager : IObservable<LiftStatus>, IDisposable, ILiftMonitor, ILiftRequestHandler
    {
        private readonly List<IObserver<LiftStatus>> _observers = new List<IObserver<LiftStatus>>();
        private readonly List<ObservableLift> _theLifts = new List<ObservableLift>();

        public LiftManager(IScheduler scheduler, List<string> liftNames)
        {
            foreach (string liftName in liftNames)
            {
                var newLift = new ObservableLift(0, scheduler, liftName);
                _theLifts.Add(newLift);
                newLift.Subscribe(this);
            }
        }

        public IDisposable Subscribe(IObserver<LiftStatus> observer)
        {
            _observers.Add(observer);
            return this;
        }

        public void Dispose()
        {
            foreach (ObservableLift lift in _theLifts)
            {
                lift.Dispose();
            }

            foreach (IObserver<LiftStatus> observer in _observers)
            {
                observer.OnCompleted();
            }
        }

        public void MakeUpwardsRequestFrom(int destinationFloor)
        {
            _theLifts.First(x => x.NotProcessingAnyRequests).MakeUpwardsRequestFrom(destinationFloor);
        }

        public void MakeDownwardsRequestFrom(int destinationFloor)
        {
            _theLifts.First(x => x.NotProcessingAnyRequests).MakeDownwardsRequestFrom(destinationFloor);
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