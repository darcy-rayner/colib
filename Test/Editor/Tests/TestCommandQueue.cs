using System;
using System.Collections.Generic;
using CoLib;
using NUnit.Framework;

namespace CoLib
{
[TestFixture]
[Category("CommandQueue")]
internal class TestCommandQueue
{
	const double DELTA_TIME_RATE = 1.0 / 30.0;

	[Test]
	public static void TestOrdering()
	{
		CommandQueue queue = new CommandQueue();
		
		string lastCalled = "";
		queue.Enqueue(
			Cmd.Do( () => {
				Assert.AreEqual(lastCalled, "");
				lastCalled = "a";
			}),
			Cmd.Do( () => {
				Assert.AreEqual(lastCalled, "a");
				lastCalled = "b";
			}),
			Cmd.Do( () => {
				Assert.AreEqual(lastCalled, "b");
				lastCalled = "c";
				// Any Command pushed to the queue now, should
				// execute after d.
				queue.Enqueue( Cmd.Do( () => {
					Assert.AreEqual(lastCalled, "d");
					lastCalled = "e";
				}));
			}),
			Cmd.Do( () => {
				Assert.AreEqual(lastCalled, "c");
				lastCalled = "d";
			})
		);
		
		// Fake time updating.
		while (!queue.Update(DELTA_TIME_RATE)) {}
		
		Assert.AreEqual(lastCalled, "e");
	}

	[Test]
	public static void TestTiming()
	{
		CommandQueue queue = new CommandQueue();
		
		const double FIRST_Command_DURATION = 4.5;
		const double SECOND_Command_DURATION = 1.0;
		const double WAIT_DURATION = 1.5;
		const int REPEAT_COUNT = 8640;
		double lastT = 0.0;
		
		// This test ensures that between alternating CommandDurations,
		// there is no accumulation of error in timing. We use a repeat
		// here to accentuate the error.
		queue.Enqueue(
			Cmd.Repeat(REPEAT_COUNT,
				Cmd.Sequence(
					Cmd.WaitForSeconds(WAIT_DURATION),
					Cmd.Do(() => lastT = 0.0),
					Cmd.Duration( (t) => {
						Assert.IsTrue(t <= 1.0);
						Assert.IsTrue(lastT <= t);
						lastT = t;
					}, FIRST_Command_DURATION),
					Cmd.Do(() => lastT = 0.0),
					Cmd.Parallel(
						Cmd.Duration( (t) => {} , SECOND_Command_DURATION / 2.0),
						// The following two  Duration Cmd should finish in the same Update call.
						Cmd.Duration( (t) => {}, SECOND_Command_DURATION - (DELTA_TIME_RATE / 2.0)),
						Cmd.Duration( (t) => {
							Assert.IsTrue(t <= 1.0);
							Assert.IsTrue(lastT <= t);
							lastT = t;
						}, SECOND_Command_DURATION)
					)
				)
			)
		);
		
		double totalTime = 0.0;
		while (!queue.Update(DELTA_TIME_RATE)) {
			totalTime += DELTA_TIME_RATE;
		}
		
		const double EXPECTED_TIME = (FIRST_Command_DURATION + SECOND_Command_DURATION + WAIT_DURATION) * REPEAT_COUNT;
		
		Assert.AreEqual(totalTime, EXPECTED_TIME, DELTA_TIME_RATE, "Time delta accumulation too large.");
	}

	[Test]
	public static void TestPausing()
	{
		CommandQueue queue = new CommandQueue();
		
		bool shouldBePaused = true; 
		bool secondCommandCalled = false;
		
		queue.Enqueue(
			Cmd.Do( () => queue.Paused = true),
			Cmd.Do( () => {
				Assert.AreEqual(shouldBePaused, false, "Executed Command while CommandQueue paused.");
				secondCommandCalled = true;
			})	
		);
		
		queue.Update(10.0);
		queue.Paused = false;
		shouldBePaused = false;
		queue.Update(10.0);
		Assert.AreEqual(secondCommandCalled, true, "Second Command never called.");
	}

	[Test]
	public static void TestCondition()
	{
		CommandQueue queue = new CommandQueue();
		
		int testCount = 0;
		int conditionCalledCount = 0;
		queue.Enqueue(
			Cmd.Repeat(4,
				Cmd.Condition(() => testCount % 2 == 0,
					Cmd.Do( () => testCount += 1),
					Cmd.Do( () => {
						Assert.IsTrue(testCount % 2 == 1, "Unexpected state in condition branch.");
						conditionCalledCount += 1;
						testCount += 1;
					})
				)
			),
			Cmd.Do(() => Assert.AreEqual(conditionCalledCount, 2))
		);
		
		while (!queue.Update(DELTA_TIME_RATE)) {}
	}

	[Test]
	public static void TestRecursiveUpdateFail()
	{
		
		CommandQueue queue = new CommandQueue();
		queue.Enqueue(
			Cmd.Queue(queue)
		);
		
		bool threwException = false;
		try {
			queue.Update(DELTA_TIME_RATE);	
		} catch (InvalidOperationException) {
			threwException = true;
		}
		
		Assert.IsTrue(threwException, "Failed to throw exception from invalid state.");
		
		queue = new CommandQueue();
		queue.Enqueue(
			Cmd.Do(() => {
				queue.Update(DELTA_TIME_RATE);
			})
		);
		
		try {
			queue.Update(DELTA_TIME_RATE);	
		} catch (InvalidOperationException) {
			threwException = true;
		}
		
		Assert.IsTrue(threwException, "Failed to throw exception from invalid state.");
		
	}

	[Test]
	public static void TestParallel()
	{
		CommandQueue queue = new CommandQueue();
		double a = 0.0, b = 0.0, c = 0.0;;
		queue.Enqueue(
			Cmd.Repeat(2,
				Cmd.Parallel(
					Cmd.Duration((t) => a = t, 4.0),
					Cmd.Duration((t) => b = t, 3.0),
					Cmd.Duration((t) => {
						c = t;
						Assert.IsTrue(b < c, "Runner not operating con-currently.");
						Assert.IsTrue(a < b, "Runner not operating con-currently.");
					}, 2.0)
				)
			)
		);
		
		while (!queue.Update(DELTA_TIME_RATE)) {}
		
		Assert.AreEqual(a, 1.0, 0.001);
		Assert.AreEqual(b, 1.0, 0.001);
		Assert.AreEqual(c, 1.0, 0.001);
	}

	[Test]
	public static void TestRepeatForever()
	{
		CommandQueue queue = new CommandQueue();
		const int MAX_REPEAT_COUNT = 100;
		int repeatCount = 0;
		queue.Enqueue(
			Cmd.RepeatForever( 
				Cmd.Do(() => {
					if (repeatCount < MAX_REPEAT_COUNT) { 
						repeatCount++; 
					} else {
						// Well there is one way to stop execution
						throw new Exception();
					}
				})
			)
		);	
		
		bool threwException = false;
		try {
			while (!queue.Update(DELTA_TIME_RATE)) {}
		} catch (Exception) {
			threwException = true;
		}
		
		Assert.IsTrue(threwException);
		Assert.AreEqual(repeatCount, MAX_REPEAT_COUNT);
	}

	[Test]
	public static void TestQueue()
	{
		CommandQueue mainQueue = new CommandQueue();
		CommandQueue secondQueue = new CommandQueue();
		
		bool firstCommandTriggered = false;
		bool secondCommandTriggered = false;
		bool thirdCommandTriggered = false;
		secondQueue.Enqueue(
			Cmd.Do(() => {
				Assert.IsTrue(!firstCommandTriggered);
				firstCommandTriggered = true;
			}),
			Cmd.Do(() => {
				Assert.IsTrue(firstCommandTriggered);
				Assert.IsTrue(!secondCommandTriggered);
				secondCommandTriggered = true;
			}),
			Cmd.Do(() => {
				Assert.IsTrue(firstCommandTriggered);
				Assert.IsTrue(secondCommandTriggered);
				Assert.IsTrue(!thirdCommandTriggered);
				thirdCommandTriggered = true;
			})
		);
		
		mainQueue.Enqueue(
			Cmd.Repeat(2,
				Cmd.Queue(secondQueue)
			)
		);
		
		while (!mainQueue.Update(DELTA_TIME_RATE)) {}
	}

	[Test]
	public static void TestWaitFrames()
	{
		int count = 0;
		CommandDelegate incr = Cmd.Do(() => ++count);
		CommandQueue queue = new CommandQueue();
		queue.Enqueue(
			Cmd.WaitForFrames(1),
			incr,
			Cmd.WaitForFrames(2),
			incr,
			Cmd.Repeat(3,
				Cmd.Sequence(
					Cmd.WaitForFrames(2),
					incr
				)
			)
		);
		
		queue.Update(0.1);
		Assert.AreEqual(count, 0);
		queue.Update(0.1);
		Assert.AreEqual(count, 1);
		queue.Update(0.1);
		Assert.AreEqual(count, 1);
		queue.Update(0.1);
		Assert.AreEqual(count, 2);
		queue.Update(0.1);
		Assert.AreEqual(count, 2);
		queue.Update(0.1);
		Assert.AreEqual(count, 3);
		queue.Update(0.1);
		Assert.AreEqual(count, 3);
		queue.Update(0.1);
		Assert.AreEqual(count, 4);
		queue.Update(0.1);
		Assert.AreEqual(count, 4);
	}
	
	public static IEnumerator<CommandDelegate> CoroutineOne(Ref<float> val, int depth, Ref<int> calledCount)
	{
		if (depth > 0) {
			++calledCount.Value;
			yield return Cmd.ChangeTo(val, 5.0f, 4.0f);
			yield return Cmd.WaitForSeconds(1.0f);
			yield return Cmd.ChangeBy(val, -4.0f, 4.0f);
			yield return null; // Wait for a single frame
			yield return Cmd.Coroutine(() => CoroutineOne(val, depth - 1, calledCount));
		}
		
	}

	[Test]
	public static void TestCoroutine()
	{
		float floatVal = 0.0f;
		Ref<float> floatRef = new Ref<float>(
			() => floatVal,
			t => floatVal = t
		);
		int calledCount = 0;
		
		Ref<int> calledCountRef = new Ref<int>(
			() => calledCount,
			t => calledCount = t
		);
		
		CommandQueue queue = new CommandQueue();
		queue.Enqueue(
			Cmd.Repeat(2,
				Cmd.Coroutine(() => CoroutineOne(floatRef, 2, calledCountRef))
			)
		);
		
		Assert.AreEqual(calledCount, 0);
		queue.Update(2.0f);
		Assert.AreEqual(calledCount, 1);
		Assert.AreEqual(floatVal, 2.5f, 0.001f);
		queue.Update(5.0f);
		Assert.AreEqual(calledCount, 1);
		Assert.AreEqual(floatVal, 3.0f, 0.001f);
		queue.Update(2.0f);
		Assert.AreEqual(calledCount, 1);
		queue.Update(0.0f); // yield return null;
		Assert.AreEqual(calledCount, 2);
		queue.Update(4.0f);
		Assert.AreEqual(calledCount, 2);
		Assert.AreEqual(floatVal, 5.0f, 0.001f);
		queue.Update(5.0f);
		Assert.AreEqual(calledCount, 2);
		queue.Update(0.0f); // yield return null;
		Assert.AreEqual(calledCount, 3);
		queue.Update(4.0f);
		Assert.AreEqual(calledCount, 3);
		Assert.AreEqual(floatVal, 5.0f, 0.001f);
	}

	[Test]
	public static void TestRequire()
	{
		CommandQueue queue = new CommandQueue();
		
		bool shouldStop = false;
		bool didFinish = false;
		int callCount = 0;
		
		queue.Enqueue(
			Cmd.RepeatForever(
				Cmd.Require( () => !shouldStop,
					() => Cmd.RepeatForever(
						Cmd.Sequence(
							Cmd.Do( () => callCount++),
							Cmd.WaitForFrames(1)
						)
					)
				),
				Cmd.Do( () => didFinish = true),
				Cmd.WaitForFrames(1)
			)
		);
		
		Assert.AreEqual(callCount, 0);
		queue.Update(1.0f);
		Assert.AreEqual(callCount, 1);
		queue.Update(1.0f);
		Assert.AreEqual(callCount, 2);
		queue.Update(1.0f);
		Assert.AreEqual(callCount, 3);
		
		// Require should only re-evaluate on next update.
		shouldStop = true;
		Assert.AreEqual(didFinish, false);
		queue.Update(1.0f);
		Assert.AreEqual(callCount, 3);
		Assert.AreEqual(didFinish, true);
		
		queue.Update(1.0f);
		Assert.AreEqual(callCount, 3);
		Assert.AreEqual(didFinish, true);
		shouldStop = false;
		
		queue.Update(1.0f);
		Assert.AreEqual(callCount, 4);
		Assert.AreEqual(didFinish, true);
	}

	[Test]
	public static void TestChooseRandom()
	{
		const int NUM_SELECTIONS = 1000;
		CommandQueue queue = new CommandQueue();
		
		int[] selections = new int[NUM_SELECTIONS];
		int i = 0;
		queue.Enqueue(
			Cmd.Repeat(NUM_SELECTIONS,
				Cmd.Do(() => selections[i] = 0),
				Cmd.ChooseRandom(
					Cmd.Do(() => selections[i] = 1),
					Cmd.Do(() => selections[i] = 2),
					Cmd.Do(() => selections[i] = 3),
					Cmd.Do(() => selections[i] = 4),
					Cmd.Do(() => selections[i] = 5),
					Cmd.Do(() => selections[i] = 6),
					Cmd.Do(() => selections[i] = 7),
					Cmd.Do(() => selections[i] = 8),
					Cmd.Do(() => selections[i] = 9),
					null
				),
				Cmd.Do( () => i++)
			)
		);
		
		queue.Update(1.0f);
		
		// The chance of every number being the same is 1 / 10 ^ (NUM_SELECTIONS - 1).
		bool allEqual = true;
		for (int c = 0; c < NUM_SELECTIONS - 1; c++)
		{
			if (selections[c] != selections[c + 1]) {
				allEqual = false;
				break;
			}
		}
		
		Assert.IsTrue(!allEqual, "All numbers were equal, this is either extremely unlikely, or a bug.");
	}

	[Test]
	public static void TestWhile()
	{
		CommandQueue queue = new CommandQueue();

		int i = 0;
		int c = 0;
		queue.Enqueue(
			Cmd.Repeat(5,
		        Cmd.Do( () => ++i),
				Cmd.While( () => i % 5 != 0,
		        	Cmd.Do( () => ++i),
		            Cmd.WaitForFrames(1),
		            Cmd.Do( () => ++c)
				),
		        Cmd.WaitForFrames(1)
			)
		);

		System.Action Update5 = ()=> { 
			for (int j = 0; j < 5; ++j) {
				queue.Update(0f);
			}
		};

		Update5();
		Assert.AreEqual(i, 5);
		Assert.AreEqual(c, 4);

		Update5();
		Assert.AreEqual(i, 10);
		Assert.AreEqual(c, 8);

		Update5();
		Assert.AreEqual(i, 15);
		Assert.AreEqual(c, 12);

		Update5();
		Assert.AreEqual(i, 20);
		Assert.AreEqual(c, 16);

		Update5();
		Assert.AreEqual(i, 25);
		Assert.AreEqual(c, 20);
	}

	[Test]
	public static void TestProcess()
	{
		CommandQueue queue = new CommandQueue();

		double time = 0.0;
		bool started = false;
		bool finished = false;

		queue.Enqueue(
			Cmd.Do( () => started = true),
			Cmd.Duration( t => time = t, 1.0),
			Cmd.Do( () => finished = true)
		);

		Assert.AreEqual(false, started);
		queue.Process();
		Assert.AreEqual(true, started);
		Assert.AreEqual(0.0, time);
		Assert.AreEqual(false, finished);
		queue.Process();
		Assert.AreEqual(0.0, time);
	}

	[Test]
	public static void TestRunToEnd()
	{
		CommandQueue queue = new CommandQueue();

		double time = 0.0;
		bool started = false;
		bool finished = false;

		queue.Enqueue(
			Cmd.Do( () => started = true),
			Cmd.Duration( t => time = t, 1.0),
			Cmd.Do( () => finished = true)
		);

		Assert.AreEqual(false, started);
		queue.RunToEnd();
		Assert.AreEqual(true, started);
		Assert.AreEqual(1.0, time);
		Assert.AreEqual(true, finished);
	}
}

}

