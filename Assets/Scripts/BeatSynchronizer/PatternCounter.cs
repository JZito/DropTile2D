﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SynchronizerData;

/// <summary>
/// This class is responsible for counting and notifying its observers when a beat in the specified pattern sequence occurs.
/// The accuracy of the counter is handled by loopTime, which controls how often it checks whether a beat has happened.
/// Higher settings for loopTime decreases load on the CPU, but will result in less accurate beat synchronization.
/// </summary>
public class PatternCounter : MonoBehaviour {

	public BeatValue[] beatValues;
	public int beatScalar = 1;
	public float loopTime = 30f;
	public AudioSource audioSource;
	public List<GameObject> observers = new List<GameObject> ();

	private float nextBeatSample;
	private float[] samplePeriods;
	private int sequenceIndex;
	private float currentSample;
	
	
	void Awake ()
	{
		float audioBpm = audioSource.GetComponent<BeatSynchronizer>().bpm;
		samplePeriods = new float[beatValues.Length];

		// Calculate number of samples between each beat in the sequence.
		for (int i = 0; i < beatValues.Length; ++i) {
			samplePeriods[i] = (60f / (audioBpm * BeatDecimalValues.values[(int)beatValues[i]])) * audioSource.clip.frequency;
			samplePeriods[i] *= beatScalar;
		}
		
		nextBeatSample = 0f;
		sequenceIndex = 0;
	}

	/// <summary>
	/// Initializes and starts the coroutine that checks for beat occurrences in the pattern. The nextBeatSample field is initialized to 
	/// exactly match up with the sample that corresponds to the time the audioSource clip started playing (via PlayScheduled).
	/// </summary>
	/// <param name="syncTime">Equal to the audio system's dsp time plus the specified delay time.</param>
	void StartPatternCheck (double syncTime)
	{
		nextBeatSample = (float)syncTime * audioSource.clip.frequency;
		StartCoroutine(PatternCheck());
	}

	/// <summary>
	/// Subscribe the PatternCheck() coroutine to the beat synchronizer's event.
	/// </summary>
	void OnEnable ()
	{
		BeatSynchronizer.OnAudioStart += StartPatternCheck;
	}

	/// <summary>
	/// Unsubscribe the PatternCheck() coroutine from the beat synchronizer's event.
	/// </summary>
	/// <remarks>
	/// This should NOT (and does not) call StopCoroutine. It simply removes the function that was added to the
	/// event delegate in OnEnable().
	/// </remarks>
	void OnDisable ()
	{
		BeatSynchronizer.OnAudioStart -= StartPatternCheck;
	}

	void Update ()
	{
	}

	/// <summary>
	/// This method checks if a beat has occurred in the pattern's sequence by comparing the current sample position of the audio  
	/// versus the next expected sample value. The frequency of the checks is controlled by the loopTime field.
	/// </summary>
	/// <remarks>
	/// The WaitForSeconds() yield statement places the execution of the coroutine right after the Update() call, so by 
	/// setting the loopTime to 0, this method will update as frequently as Update(). If even greater accuracy is 
	/// required, WaitForSeconds() can be replaced by WaitForFixedUpdate(), which will place this coroutine's execution
	/// right after FixedUpdate().
	/// </remarks>
	IEnumerator PatternCheck ()
	{
		while (audioSource.isPlaying) {
			currentSample = (float)AudioSettings.dspTime * audioSource.clip.frequency;
			
			if (currentSample >= nextBeatSample) {
				// Since this is a specific pattern of beats, we don't need to track different beat types.
				// Instead, client can index a custom beat counter to track which beat in the sequence has fired.

//				foreach (GameObject obj in observers) {
				for (int i = 0; i < observers.Count; i++)
				{
					GameObject obj = observers[i];
					if (obj != null)
					{
					int beatValueIndex = (sequenceIndex - 1 == -1 ? beatValues.Length -1 : sequenceIndex-1);
					obj.GetComponent<BeatObserver>().BeatNotify(beatValues[beatValueIndex]);
						if (obj.transform.position.x >= 16)
						{
							float volume = 1;
							float t = 0;
							while(t < 1)
							{
								t += Time.deltaTime/2f;
								volume = Mathf.Lerp(1, 0, t);
								obj.audio.volume = volume;
								if (obj.audio.volume == 0)
								{
//									print ("it's 0");
									observers.Remove(obj);
									DestroyObject(obj.gameObject);
								}
							}
						}
				//	if (obj.transform.position.x >= 20)
				//	{
				//		observers.Remove(obj);
				//		DestroyObject(obj.gameObject);
				//	}
					}
				}
				nextBeatSample += samplePeriods[sequenceIndex];
				sequenceIndex = (++sequenceIndex == beatValues.Length ? 0 : sequenceIndex);
			}

			yield return new WaitForSeconds(loopTime / 1000f);
		}
	}

}