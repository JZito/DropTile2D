using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Audial{

	[ExecuteInEditMode]
	public class SimpleDelay : MonoBehaviour {
		
		private float sampleRate;
		void Awake(){
			sampleRate = Audial.Utils.Settings.SampleRate = AudioSettings.outputSampleRate;
			combFilter = new Audial.Utils.CombFilter(DelayLengthMS,0.5f);
			ChangeDelay();
		}
		Audial.Utils.CombFilter combFilter;

		[SerializeField]
		private int _delayLengthMS = 120;
		private int DelayLengthMSPrev = 10;
		public int DelayLengthMS {
			get{
				return _delayLengthMS;
			}
			set{
				_delayLengthMS = Mathf.Clamp(value, 10, 3000);
				ChangeDelay();
			}
		}

		[SerializeField]
		private float _dryWet = 0.5f;
		public float DryWet{
			get{
				return _dryWet;
			}
			set{
				_dryWet = Mathf.Clamp(value,0,1);
			}
		}

		[SerializeField]
		private float _decayLength = 0.25f;
		public float DecayLength{
			get{
				return _decayLength;
			}
			set{
				_decayLength = Mathf.Clamp(value,0.1f, 1);
			}
		}
		
		private float delayLength;
		private int delaySamples;
		private float output = 0;
		
		private void ChangeDelay(){
			combFilter.DelayLength = DelayLengthMS;
		}

#if UNITY_EDITOR
		public bool runEffectInEditMode = true;
		private bool runEffect = true;

		public System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		public float runTime = 0;

		void SetRunEffectInEditMode(bool val){
			runEffectInEditMode = val;
			runEffect = val;
		}

		void Update(){
			if(!runEffectInEditMode&&!Application.isPlaying){
				runEffect = false;
				return;
			}
			runEffect = true;
			if(DelayLengthMSPrev != DelayLengthMS){
				DelayLengthMSPrev = DelayLengthMS;
				ChangeDelay();
			}
		}
#endif

		void OnAudioFilterRead(float[] data, int channels){
#if UNITY_EDITOR
			if(!runEffect)
				return;
			stopwatch.Reset();
			stopwatch.Start();
#endif

			float dry;
			float wet;

			combFilter.gain = DecayLength;

			for (var i = 0; i < data.Length; i = i + channels){
				for (var c = 0; c < channels; c++){
					dry = data[i+c];
					wet = combFilter.ProcessSample(c,dry);
					output = dry * (1-DryWet/2) + wet * DryWet/2;
					data[i+c] = output;
				}
				combFilter.MoveIndex();		
			}
#if UNITY_EDITOR
			stopwatch.Stop();
			runTime = Mathf.Round((float)stopwatch.Elapsed.TotalMilliseconds*100)/100;// stopwatch.ElapsedMilliseconds;
#endif
		}
	}
}