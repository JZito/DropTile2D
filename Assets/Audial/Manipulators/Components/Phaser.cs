using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Audial{
	
	[ExecuteInEditMode]
	public class Phaser : MonoBehaviour {

		private float output = 0;
		private float sampleRate;

		public Utils.LFO lfo;
		public Utils.AllPassFilter[] allPassFilters = new Utils.AllPassFilter[4];

		void Awake(){
			sampleRate = Audial.Utils.Settings.SampleRate = AudioSettings.outputSampleRate;
			ResetUtils();
		}

		void ResetUtils(){
			allPassFilters[0] = new Utils.AllPassFilter(Rate, Intensity);
			allPassFilters[1] = new Utils.AllPassFilter(Rate, Intensity);
			allPassFilters[2] = new Utils.AllPassFilter(Rate, Intensity);
			allPassFilters[3] = new Utils.AllPassFilter(Rate, Intensity);
			SetIntensity(Intensity);
			lfo = new Utils.LFO(Rate);
		}

		void SetIntensity(float i){
			for(var a = 0; a < allPassFilters.Length; a++){
				allPassFilters[a].gain = i*0.6f;
			}
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
		private float _width = 0.5f;
		public float Width{
			get{return _width;}
			set{
				if(_width == value)return;
				_width = Mathf.Clamp(value,0,1);
			}
		}
		
		[SerializeField]
		private float _intensity = 0.25f;
		public float Intensity{
			get{return _intensity;}
			set{
				_intensity = Mathf.Clamp(value,0f, 1);
				SetIntensity(_intensity);
			}
		}
		
		[SerializeField]
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

		private float fromMin = 0.43f;
		private float fromMax = 0.193f;
		private float toMin = 0.772f;
		private float toMax = 0.962f;
		
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
				float newOffset = Mathf.Lerp(Mathf.Lerp(fromMin,fromMax,Width), Mathf.Lerp(toMin,toMax,Width), lfo.GetValue()) * sampleRate / 1000;
				for(var a = 0; a < allPassFilters.Length; a++){
					allPassFilters[a].Offset = (int)newOffset;
					for (var c = 0; c < channels; c++){
						dry = data[i+c];
						wet = allPassFilters[a].ProcessSample(c,dry);
						output = dry * (1-DryWet/2) + wet * DryWet/2;
						data[i+c] = output;
					}
					allPassFilters[a].MoveIndex();
				}
				lfo.MoveIndex();
			}
			#if UNITY_EDITOR
			stopwatch.Stop();
			runTime = Mathf.Round((float)stopwatch.Elapsed.TotalMilliseconds*100)/100;// stopwatch.ElapsedMilliseconds;
			#endif
		}
	}
}