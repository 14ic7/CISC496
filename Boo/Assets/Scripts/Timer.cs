using UnityEngine;
using System.Collections;

public class Timer {

	private float timeRemaining;
	private float totalTime;
	private bool isRunning;
	private bool isFirstCycle;

	// Constructor. Set the time to count down from here.
	public Timer (float time) {
		totalTime = time;
		timeRemaining = totalTime;
		isRunning = false;
		isFirstCycle = true;
	}

	public void StartTimer () {
		timeRemaining = totalTime;
		isRunning = true;
		isFirstCycle = false;
	}

	public void UpdateTimer () {
		if (timeRemaining > 0) {
			timeRemaining -= Time.deltaTime;
		} else {
			isRunning = false;
		}
	}

	public void ResetTimer () {
		timeRemaining = totalTime;
		isRunning = true;
	}

	public bool IsRunning () {
		return isRunning;
	}

	public bool IsFirstCycle () {
		return isFirstCycle;
	}
}
