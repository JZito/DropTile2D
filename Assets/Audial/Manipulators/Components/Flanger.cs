using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Audial{
	
	[System.Serializable]
	[ExecuteInEditMode]
	public class Flanger : MonoBehaviour {

		private float sampleRate;
		private float output = 0;

		private Utils.LFO lfo;
		[SerializeField]
		private Utils.CombFilter combFilter;
		void Awake(){
			sampleRate = Audial.Utils.Settings.SampleRate = AudioSettings.outputSampleRate;
			ResetUtils();
		}

		void ResetUtils(){
			combFilter = new Utils.CombFilter(Intensity,0.5f);
			lfo = new Utils.LFO(Rate);
		}
		
		[SerializeField]
		private float _rate = 0.3f;
		public float Rate {
			get{return _rate;}
			set{
				if(_rate == value)return;
				_rate = Mathf.Clamp(value,0.1f,8);
				lfo.SetRate(_rate);
			}
		}
		
		[SerializeField]
		private float _intensity = 0.25f;
		public float Intensity{
			get{return _intensity;}
			set{
				_intensity = Mathf.Clamp(value,0.1f, 0.9f);
				combFilter.gain = _intensity;
			}
		}

		[SerializeField]
		[Range(0f,1)]
		private float _dryWet = 0.75f;
		public float DryWet{
			get{return _dryWet;}
			set{_dryWet = Mathf.Clamp(value,0f, 1);}
		}

		private float _offset = 0;
		private int Offset{
			get{return (int)_offset;}
			set{_offset = value;}
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
				combFilter.Offset = (int)Mathf.Lerp(1*sampleRate/1000,5*sampleRate/1000,lfo.GetValue());
				for (var c = 0; c < channels; c++){
					dry = data[i+c];
					wet = combFilter.ProcessSample(c,dry);
					output = dry * (1-DryWet/2) + wet * DryWet/2;
					data[i+c] = output;
				}
				combFilter.MoveIndex();	
				lfo.MoveIndex();
			}
			#if UNITY_EDITOR
			stopwatch.Stop();
			runTime = Mathf.Round((float)stopwatch.Elapsed.TotalMilliseconds*100)/100;// stopwatch.ElapsedMilliseconds;
			#endif
		}
	}
}