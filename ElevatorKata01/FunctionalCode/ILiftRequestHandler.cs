namespace ElevatorKata01.FunctionalCode
{
    public interface ILiftRequestHandler
    {
        void MakeUpwardsRequestFrom(int destinationFloor);
        void MakeDownwardsRequestFrom(int destinationFloor);
    }
}