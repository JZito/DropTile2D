using UnityEngine;
using System;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class Gate : MonoBehaviour {

		[SerializeField]
		Audial.Utils.Envelope envelope;
		private float sampleRate;

		void OnEnable(){
			sampleRate = AudioSettings.outputSampleRate;
			envelope = new Audial.Utils.Envelope(Attack, Release);
		}
		
		[SerializeField]
		private float _inputGain = 1;
		public float InputGain{
			get{return _inputGain;}
			set{
				if(value==_inputGain)return;
				_inputGain = Mathf.Clamp(value,0,3);
			}
		}

		[SerializeField]
		private float _threshold = 0.247f;
		public float Threshold{
			get{return _threshold;}
			set{
				if(value==_threshold)return;
				_threshold = Mathf.Clamp(value,0,1);
			}
		}

		[SerializeField]
		private float _attack = 0f;
		public float Attack{
			get{return _attack;}
			set{
				if(value==_attack)return;
				_attack = Mathf.Clamp(value, 0f, 1);
				envelope.Attack = _attack;
			}
		}

		[SerializeField]
		public float _release = 0.75f;
		public float Release{
			get{return _release;}
			set{
				if(value==_release)return;
				_release = Mathf.Clamp(value, 0, 1);
				envelope.Release = _release;
			}
		}

		[SerializeField]
		private float _outputGain = 1;
		public float OutputGain{
			get{return _outputGain;}
			set{
				if(value==_outputGain)return;
				_outputGain = Mathf.Clamp(value, 0, 5);
			}
		}

		private float env = 0;

#if UNITY_EDITOR
		public bool runEffectInEditMode = true;
		private bool runEffect = true;

		public System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		public float runTime = 0;
		
		public float MAX_inputGain = 0;
		public float MAX_outputGain = 0;
		public float MAX_gateState = 0;
		
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
			MAX_inputGain = 0;
			MAX_outputGain = 0;
			MAX_gateState = 0;
#endif
			for(var i = 0; i < data.Length; i += channels){
				data[i] *= InputGain;
				data[i+1] *= InputGain;
				float rms = Mathf.Sqrt(data[i]*data[i]+data[i+1]*data[i+1]);

				float env = envelope.ProcessSample(rms);

				float compressMod = 1;
				if(env < Threshold){
					compressMod = Mathf.Pow(env/4,4);
				}

				data[i] *= compressMod * OutputGain;
				data[i+1] *= compressMod * OutputGain;
#if UNITY_EDITOR
				float mergedData = (Mathf.Abs(data[i])+Mathf.Abs(data[i+1]))/2;
				MAX_inputGain = MAX_inputGain > Mathf.Abs(rms) ? MAX_inputGain : Mathf.Abs(rms);
				MAX_outputGain = MAX_outputGain > mergedData * OutputGain ? MAX_outputGain : mergedData * OutputGain;
				MAX_gateState = MAX_gateState > compressMod ? MAX_gateState : compressMod;
#endif
			}

#if UNITY_EDITOR
			stopwatch.Stop();
			runTime = Mathf.Round((float)stopwatch.Elapsed.TotalMilliseconds*100)/100;// stopwatch.ElapsedMilliseconds;
#endif
		}
	}
}