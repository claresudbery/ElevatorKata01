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
	
A note on the test helpers:
The following methods are used to make the tests easier to write and easier to read:
	LiftMakeStartAt
	LiftExpectToLeaveFrom
	LiftExpectToVisit
	LiftExpectToStopAt
	LiftMakeUpwardsRequestFrom
	LiftMakeDownwardsRequestFrom
	LiftMakeRequestToMoveTo
	StartTest
	Mark
	VerifyAllMarkers
The reason these methods exist is that otherwise, you have to keep careful track of...
	exactly what events you expect to occur
	exactly how many events you expect to occur
	exactly what time you expect each event to occur (so that you can insert lift moves and calls in the correct places)
By using the new methods, you don't have to count events or keep track of time - the methods do that for you.
	Note that most of the methods are not actually making anything happen
	- they are just making a note of what we expect to happen, 
	and allowing us to make a note of the time and the order in which these events should happen.
	Thus there are two types of method:
		Those prefixed LiftExpectTo, which note expectations and at what time / in what order each expected event will happen
		Those prefixed LiftMake, which actually call methods on the lift itself
	However you can't have one without the other, because the LiftMake methods use the scheduler
		...and in order to use the scheduler, you need to know at what time you want the lift to be called
	Also, you need the LiftExpectTo methods to have noted the order of events so that you can verify expectations later on.
Then you use the Mark method to make a note of which events you want to verify, 
	and call VerifyAllMarkers to make the relevant assertions at the end of the test.
	The fact that you explicitly call LiftExpectToVisit, LiftExpectToStop etc means that there is a clear visible record of what you expect the lift to do.
	This has significantly reduced the quantity of pain in my head whilst writing these tests!

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
Stops should be visited in the correct order (if people on floors 3, 5 7 and 9 are all travelling upwards but make their requests in a different order, they should still be visited in ascending order)
	This has been tested on:
		upwards requests only
		downwards requests only
	This has NOT been tested on:
		a mixture of both
When two or more move requests are made after the lift has stopped in response to a call, they are all serviced correctly
	I'm not sure the test code will handle this correctly! Might affect the time intervals?
If somebody calls the lift while it is stopped on a floor because somebody called it there, it will start moving again
	At some point we will need to add something in which understands that first it needs to make sure that it has opened the doors and given people a chance to enter
	Also maybe we need to detect whether somebody is literally in the process of entering / leaving the lift before closing the doors??
If somebody calls the lift while it is stopped on a floor because somebody has just exited the lift, it will start moving again
	At some point we will need to add something in which understands that first it needs to make sure that it has opened the doors and given people a chance to enter
	Also maybe we need to detect whether somebody is literally in the process of entering / leaving the lift before closing the doors??
When lift is first created, if it is not on the ground floor, after waiting for a bit it returns to the ground floor
If we're moving downwards and somebody makes a new downwards call, we only process it if they are below where we currently are.
	...and if we're moving upwards and somebody makes a new upwards call, we only process it if they are above where we currently are.
If the lift runs out of upwards requests 
	... and starts processing downwards requests...
	... but the next downwards request is coming from a higher floor...
	... and while the lift is moving up to that floor, an upwards request comes in which lies between the lift's current location and the floor it is moving to...
	... what should happen??
		The problem is, that when the lift arrives at the location it has been called to, the caller might ask it to go to an even higher location than the one it was aiming for. 
			...meaning that it would have to overshoot.
			...and someone else might make an even higher upwards request while it processes the new one.
		One solution is that we say No, we are now processing downwards requests and we will ignore all upwards requests until we are done with our downwards requests.
			This is what is currently implemented, I think, because we say if direction is not Down then we are moving up (otherwise we are moving down)
			The problem with this is that users will see the lift is coming towards them, but ignoring them and going straight past.
		The alternative is that if an upwards request comes in which we are able to service on our way to the downwards request, we just cancel the downwards strategy and consider ourselves to be moving upwards again.
Consider all test scenarios and make sure they include negative floor numbers.
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
If the lift is called down to a negative floor, if no requests are made then it should return back up to the ground floor.
At some point in the future, we might want to be a bit more sophisticated about what "Wait" means:
	- Don't just wait five seconds for something to happen:
	- If you've reached the end of an upwards or downwards series of actions, just wait until someone has exited the lift, then change direction
		- needs to have awareness of people entering / leaving lift
		- might need to know the difference of arriving somewhere in order to deliver someone, and arriving somewhere in order to pick someone up
		
Possible technologies for a UI: 
	recommended by Braithers:
		Ruby + Sinatra
		Angular
		Node.js
	recommended by Google:
		Xamarin (based on my own quick Googling re what I could use to generate an app for Android and iOS, but using languages / technologies I already know (ie C# and Visual Studio))
			Details here: http://nnish.com/2013/06/12/how-i-built-an-android-app-in-c-visual-studio-in-less-than-24hrs/