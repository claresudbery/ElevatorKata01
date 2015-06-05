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
            return Move(source);
        }

        public Floor Move(Floor destination)
        {
            _currentFloor = destination;

            return _currentFloor;
        }
    }
}