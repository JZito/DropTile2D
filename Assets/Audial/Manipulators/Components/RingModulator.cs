using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Audial{
	
	[ExecuteInEditMode]
	public class RingModulator : MonoBehaviour {

		private float output = 0;
		private float sampleRate;

		public Utils.LFO carrierLFO;

		void OnEnable(){
			sampleRate = Audial.Utils.Settings.SampleRate = AudioSettings.outputSampleRate;
			ResetUtils();
		}

		void ResetUtils(){
			carrierLFO = new Utils.LFO(1/CarrierFrequency);
		}
		
		[SerializeField]
		private float _carrierFrequency = 400f;
		public float CarrierFrequency {
			get{return _carrierFrequency;}
			set{
				_carrierFrequency = Mathf.Clamp(value,20,5000);
				carrierLFO.SetRate(1/_carrierFrequency);
			}
		}
		
		[SerializeField]
		private float _dryWet = 0.75f;
		public float DryWet{
			get{return _dryWet;}
			set{_dryWet = Mathf.Clamp(value,0f, 1);}
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

			for (var i = 0; i < data.Length; i = i + channels){
				for (var c = 0; c < channels; c++){
					dry = data[i+c];
					wet = dry * carrierLFO.GetValue();
					data[i+c] = dry*(1-DryWet) + wet * DryWet;
				}
				carrierLFO.MoveIndex();
			}
			#if UNITY_EDITOR
			stopwatch.Stop();
			runTime = Mathf.Round((float)stopwatch.Elapsed.TotalMilliseconds*100)/100;// stopwatch.ElapsedMilliseconds;
			#endif
		}
	}
}