# ElevatorKata01
elevator kata - first attempt (see http://blog.milesburton.com/2013/03/28/elevator-kata-mind-bending-pairing-exercise/)

- An elevator responds to calls containing a source floor and direction
- An elevator delivers passengers to requested floors
- Elevator calls are queued not necessarily FIFO
- You may validate passenger floor requests
- you may implement current floor monitor
- you may implement direction arrow
- you may implement doors (opening and closing)
- you may implement DING!
- there can be more than one elevator
- ?? Max number of lift occupants?

Related links:

ReactiveX installer:
https://www.microsoft.com/en-us/download/details.aspx?id=30708
 
ReactiveX tutorials (I’m working my way through “Curing the asynchronous blues with the Reactive Extensions for .NET” – see “Tutorials and Articles” at the bottom of the page):
https://msdn.microsoft.com/en-gb/data/gg577611

Using schedulers for testing:
http://www.introtorx.com/content/v1.0.10621.0/16_TestingRx.html
http://haacked.com/archive/2014/03/10/master-time-with-reactive-extensions/

http://www.quora.com/Is-there-any-public-elevator-scheduling-algorithm-standard

http://www.quora.com/Why-are-virtually-all-elevator-algorithms-so-inefficient

Paternoster: https://www.youtube.com/watch?v=Ro3Fc_yG3p0

SCHEDULERS AND REACTIVEX
I need to add some notes here about how the TestScheduler class works, because it's been confusing the hell out of me:
- If you want to schedule events at a particular time, you need to use the Schedule method
- The AdvanceBy and AdvanceTo methods use Ticks, which are tiny (one tick is equal to 100 nanoseconds or one ten-millionth of a second. There are 10,000 ticks in a millisecond)
- When you call Start, all actions scheduled so far are run at once
- When you call AdvanceBy, all actions scheduled in the specified time period are run at once
- When you pass a scheduler to an Observable.Generate call, this has the effect of scheduling the relevant events
	-- so a subsequent call to Start or AdvanceBy will run the scheduled events up to the specified point
	-- but they won't be run until Start or AdvanceBy or AdvanceTo are called


