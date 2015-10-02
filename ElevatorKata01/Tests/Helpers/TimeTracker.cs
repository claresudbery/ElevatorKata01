namespace ElevatorKata01.Tests.Helpers
{
    public class TimeTracker
    {
        private int _millisecondsSinceTestStarted;
        private LiftManagerTestHelper _liftManagerTestHelper;

        public int MillisecondsSinceTestStarted
        {
            get { return _millisecondsSinceTestStarted; }
            set
            {
                _millisecondsSinceTestStarted = value;
                if (null != _liftManagerTestHelper)
                {
                    _liftManagerTestHelper.AdjustMillisecondsSinceTestStarted(_millisecondsSinceTestStarted);
                }
            }
        }

        public TimeTracker(LiftManagerTestHelper liftManagerTestHelper)
        {
            _liftManagerTestHelper = liftManagerTestHelper;
        }
    }
}