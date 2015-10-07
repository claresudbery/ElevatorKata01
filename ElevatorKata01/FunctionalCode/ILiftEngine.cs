using System;

namespace ElevatorKata01.FunctionalCode
{
    public interface ILiftEngine
    {
        IDisposable Subscribe(IObserver<int> observer);
        void Travel(int initialFloor, int destinationFloor);
    }
}