using ElevatorKata01.FunctionalCode;
using NUnit.Framework;
using Rhino.Mocks;

namespace ElevatorKata01.Tests.Tests
{
    [TestFixture]
    public class XpManSampleTests
    {
        [Test]
        public void When_person_calls_lift_to_higher_floor_number_then_lift_starts_moving_upwards()
        {
            ILiftEngine liftEngine = MockRepository.GenerateMock<ILiftEngine>();

            liftEngine.Stub(x => x.Subscribe(null)).IgnoreArguments().Return(null);
            //liftEngine.Stub(x => x.Travel());
        }
    }
}