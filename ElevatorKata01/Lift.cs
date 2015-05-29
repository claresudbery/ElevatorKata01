namespace ElevatorKata01
{
    internal class Lift
    {
        private Floor _currentFloor;

        public Lift(Floor initialFloor)
        {
            _currentFloor = initialFloor;
        }

        public Floor Call(Floor source)
        {
            _currentFloor = source;

            return _currentFloor;
        }

        public Floor Move(Floor destination)
        {
            _currentFloor = destination;
            return _currentFloor;
        }
    }
}