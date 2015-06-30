using UnityEngine;
using System;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class Compressor : MonoBehaviour {

		[SerializeField]
		public Audial.Utils.Envelope envelope;

		void Awake(){
			Audial.Utils.Settings.SampleRate = AudioSettings.outputSampleRate;
			envelope = new Audial.Utils.Envelope(Attack, Release);
		}
		
		[SerializeField]
		private float _inputGain = 1;
		public float InputGain{
			get{return _inputGain;}
			set{_inputGain = Mathf.Clamp(value,0,3);}
		}

		[SerializeField]
		private float _threshold = 0.247f;
		public float Threshold{
			get{return _threshold;}
			set{_threshold = Mathf.Clamp(value,0,1);}
		}

		[SerializeField]
		public float _slope = 1.727f;
		public float Slope{
			get{return _slope;}
			set{_slope = Mathf.Clamp(value,0,2);}
		}

		[SerializeField]
		private float _attack = 0.0001f;
		public float Attack{
			get{return _attack;}
			set{
				_attack = Mathf.Clamp(value, 0,1);
				envelope.Attack = _attack;
			}
		}

		[SerializeField]
		public float _release = 0.68f;
		public float Release{
			get{return _release;}
			set{
				_release = Mathf.Clamp(value, 0,1);
				envelope.Release = _release;
			}
		}

		[SerializeField]
		private float _dryGain = 0;
		public float DryGain{
			get{return _dryGain;}
			set{_dryGain = Mathf.Clamp(value,0,5);}
		}

		[SerializeField]
		private float _compressedGain = 1;
		public float CompressedGain{
			get{return _compressedGain;}
			set{_compressedGain = Mathf.Clamp(value,0,5);}
		}

		[SerializeField]
		private float _outputGain = 1;
		public float OutputGain{
			get{return _outputGain;}
			set{_outputGain = Mathf.Clamp(value, 0, 5);}
		}

		private float env = 0;

#if UNITY_EDITOR
		public bool runEffectInEditMode = true;
		private bool runEffect = true;

		public System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		public float runTime = 0;

		float mergedData;
		public float MAX_inputGain = 0;
		public float MAX_compressedGain = 0;
		public float MAX_dryGain = 0;
		public float MAX_outputGain = 0;
		public float MAX_gainReduction = 0;

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

		float[] input;
		float rms;
		float[] compressed;
		float[] dry;
		float compressMod;
		void OnAudioFilterRead(float[] data, int channels){
#if UNITY_EDITOR
			if(!runEffect)
				return;
			stopwatch.Reset();
			stopwatch.Start();
			MAX_inputGain = 0;
			MAX_outputGain = 0;
			MAX_compressedGain = 0;
			MAX_dryGain = 0;
			MAX_gainReduction = 0;
#endif
			if(envelope == null){
				return;
			}
			if(input==null||input.Length!=channels){
				input = new float[channels];
				compressed = new float[channels];
				dry = new float[channels];
			}
			for(var i = 0; i < data.Length; i += channels){
				rms = 0;
				for(var c = 0; c < channels; c++){
					input[c] = data[i+c] * InputGain;
					rms += input[c] * input[c];
				}
				rms = Mathf.Pow(rms, 1f/channels);

				env = envelope.ProcessSample(rms);

				compressMod = 1;
				if(env > Threshold){
					compressMod = Mathf.Clamp(compressMod - (env - Threshold) * Slope, 0, 1);
				}
		/*
				mergedData = 0;
				for(var c = 0; c < channels; c++){
					compressed[c] = input[c] * compressMod;
					mergedData += compressed[c] * compressed[c];
					data[i+c] = (compressed[c] * CompressedGain+input[c] * DryGain) * OutputGain;
				}
				mergedData = Mathf.Pow(mergedData, 1f/channels);
		*/
				#if UNITY_EDITOR
				MAX_inputGain = Mathf.Max(MAX_inputGain, rms);
			//	MAX_compressedGain = Mathf.Max(MAX_compressedGain, mergedData * CompressedGain);
				MAX_dryGain = Mathf.Max(MAX_dryGain, rms*DryGain);
				MAX_outputGain = Mathf.Max (MAX_outputGain, Mathf.Sqrt(Mathf.Pow(data[i],2)+Mathf.Pow(data[i+1],2)));
			//	MAX_gainReduction = Mathf.Max(MAX_gainReduction, rms - mergedData);
				#endif
			}
#if UNITY_EDITOR
			stopwatch.Stop();
			runTime = Mathf.Round((float)stopwatch.Elapsed.TotalMilliseconds*100)/100;// stopwatch.ElapsedMilliseconds;
#endif
		}
	}
}