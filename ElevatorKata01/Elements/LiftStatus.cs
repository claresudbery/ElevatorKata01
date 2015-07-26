using System;

namespace ElevatorKata01.Elements
{
    public struct LiftStatus
    {
        public Direction CurrentDirection;
        public int CurrentFloor;
        public TimeSpan Timestamp;
    }
}