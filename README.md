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

Note that the basic algorithm being used at this point is pretty simple:
	- When the lift is idle, the first person to make a request defines the current direction of lift movement
	- The lift will continue moving in the current direction until all satisfiable requests have been satisfied 
		(eg if moving upwards, only requests from people who are currently higher than the lift, and who want to move upwards, will be serviced))
	- Then the lift will change direction and satisfy all satisfiable requests in the new direction
	- When the lift is idle, it will return to the ground floor
	- It is possible that in a tall building with people making short journeys between floors, it might be more efficient to satisfy some slightly-lower upwards requests before switching to downwards requests, but for now we are not taking that into account
		It seems reasonable to assume that in most buildings, most journeys are either to or from the ground floor anyway.
		But a more sophisticated future version of the software might have analytics which examine the most-frequently-made journeys and change behaviour in response?
		...or just at any point in time, examine all pending requests and calculate which journeys would be most efficient 
			(although the definition of 'efficient' could presumably change depending on whose needs you are trying to satisfy)
			(and there is probably a danger that some poor person would get left stranded on the top floor because fetching them would never represent the most efficient use of the resource)
	
Tests which might need writing:
When_lift_moves_up_and_then_down_then_it_should_not_try_to_return_to_its_previous_up_destination
	Remove destination from queue on arrival!
Stops should be visited in the correct order (if people on floors 3, 5 7 and 9 are all travelling upwards but make their requests in a different order, they should still be visited in ascending order)
When the lift reaches the highest stop on an upwards journey, it should go back down again (picking people up along the way if applicable).
	However, this may involve moving further upwards, if the highest downwards-moving client is actually higher than the lift's current location
When the lift reaches the lowest stop on a downwards journey, it should either 
	a) pick up the lowest client on a list of upwards-moving clients and start moving upwards
	or b) go back down to the ground floor
If new downwards-moving requests are made while the lift is moving downwards, they only get picked up if their location is equal to or lower than the lift's current location.
If new upwards-moving requests are made while the lift is moving upwards, they only get picked up if their location is equal to or higher than the lift's current location.
Simpler lift-stopping algorithm:
	When the lift arrives at a floor it is supposed to stop at, either because it is picking someone up from there or because it is dropping someone off (or both),
	it will wait a certain amount of time (during which we can assume it has opened and closed its doors) and then move on.
	Any new Move requests made during this time are simply added to the queue.
		! This means that the current functionality needs to take into account that when someone makes a Move request and the lift is not moving, this may be because it is waiting for a Wait() timer to complete.
		or in other words, when a Wait timer completes, if the lift has already started moving again, it can quietly die.
		! But what if a Wait timer is started, then a Move request is made, then the lift arrives at a new floor and a new Wait timer is kicked off, and then the PREVIOUS Wait timer expires?
		Presumably all existing Wait timers should be killed whenever a new one is kicked off.
More sophisticated lift-stopping algorithm:
	We assume that whenever the lift stops, it opens its doors, and then closes them when it is ready to move on.
	When lift reaches floor where person is supposed to be waiting, it will move on after either	
		a) somebody tells the lift where they want to go
		b) a certain amount of time has elapsed
	When lift reaches floor where person is supposed to be exiting, it will move on after either
		a) somebody exits the lift
		b) a certain amount of time has elapsed
	When lift reaches floor where person is supposed to be waiting AND person is supposed to be exiting, 
	it will ignore the exiting requirement and prioritise the boarding requirement, ie it will move on after either	
		a) somebody tells the lift where they want to go
		b) a certain amount of time has elapsed
		
Possible technologies for a UI (recommended by Braithers):
	Ruby + Sinatra
	Angular