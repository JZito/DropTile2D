using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Audial{

	[ExecuteInEditMode]
	public class Delay : MonoBehaviour {
		
		private float sampleRate;
		void Awake(){
			sampleRate = AudioSettings.outputSampleRate;
			ChangeDelay();
		}

		private float[,] delayBuffer;
		private int index = 0;
		
		[SerializeField]
		private float _BPM = 120;
		public float BPM{
			get{
				return _BPM;
			}
			set{
				_BPM = Mathf.Clamp(value,40,300);
				ChangeDelay();
			}
		}
		
		[SerializeField]
		private int _delayCount = 3;
		public int DelayCount{
			get{
				return _delayCount;
			}
			set{
				_delayCount = Mathf.Clamp(value,1,8);
				ChangeDelay();
			}
		}
		
		[SerializeField]
		private int _delayUnit = 8;
		public int DelayUnit{
			get{
				return _delayUnit;
			}
			set{
				_delayUnit = Mathf.Clamp(value, 1, 32);
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

		[SerializeField]
		private float _pan = 0;
		public float Pan{
			get{
				return _pan;
			}
			set{
				_pan = Mathf.Clamp(value,-1,1);
			}
		}

		public bool PingPong = false;
		
		private float delayLength;
		private int delaySamples;
		private float output = 0;

		private void ChangeDelay(){
			delayLength = ((float)DelayCount*(60*4/BPM)/(float)DelayUnit);
			delaySamples = (int)(delayLength * sampleRate);
			delayBuffer = new float[2,delaySamples];
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

		void ClearBuffer(){
			delayBuffer = null;
		}

		void Update(){
			if(!runEffectInEditMode&&!Application.isPlaying){
				runEffect = false;
				ClearBuffer();
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
			if(delayBuffer==null){
				ChangeDelay();
			}


			float[] tempDelay = new float[channels];
			float[] panMod = new float[2]{1,1};
			if(Pan>0){
				panMod[0] = 1-Mathf.Abs(Pan);
			}else if(Pan<0){
				panMod[1] = 1-Mathf.Abs(Pan);
			}

			for (var i = 0; i < data.Length; i = i + channels){
				index %= delaySamples;

				if(PingPong){
					for(var c = 0; c < channels; c++){
						tempDelay[c] = delayBuffer[c, index];
						delayBuffer[c,index] = 0;
					}

					for (var c = 0; c < channels; c++){
						float dry = data[i+c];
						float wet = tempDelay[(c+1)%channels];
						output = dry * (1-DryWet) + wet * DryWet;
						data[i+c] = (float)(output);

						delayBuffer[c, index] += wet * DecayLength;
						delayBuffer[(c+1)%channels,index] += dry * panMod[c];
					}
				}else{

					for (var c = 0; c < channels; c++){
						tempDelay[c] = delayBuffer[c,index];
						delayBuffer[c,index] = 0;

						float dry = data[i+c];
						float wet = tempDelay[c];
						output = dry * (1-DryWet) + wet * DryWet;
						data[i+c] = (float)(output);

						delayBuffer[c, index] += wet * DecayLength;
						delayBuffer[c,index] += dry*panMod[c];
					}
				}

				index++;		
			}
#if UNITY_EDITOR
			stopwatch.Stop();
			runTime = Mathf.Round((float)stopwatch.Elapsed.TotalMilliseconds*100)/100;// stopwatch.ElapsedMilliseconds;
#endif
		}
	}
}