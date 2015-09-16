using ElevatorKata01.Elements;

namespace ElevatorKata01.Tests.Helpers
{
    public struct ExpectedLiftStatus
    {
        public int StatusIndex;
        public LiftStatus Status;
        public decimal SecondsSinceTestStarted;
        public int LiftNumber;
    }
}