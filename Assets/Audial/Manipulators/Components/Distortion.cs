using UnityEngine;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class Distortion : MonoBehaviour {

		[SerializeField]
		[Range(0,3)]
		private float _inputGain = 1;
		public float InputGain{
			get{return _inputGain;}
			set{
				_inputGain = Mathf.Clamp(value, 0, 3);
			}
		}

		[SerializeField]
		[Range(0.00001f,1)]
		private float _threshold = 0.036f;
		public float Threshold{
			get{
				return _threshold;
			}
			set{
				_threshold = Mathf.Clamp(value,0.00001f, 1);
			}
		}

		[SerializeField]
		[Range(0,1)]
		private float _dryWet = 0.258f;
		public float DryWet{
			get{
				return _dryWet;
			}
			set{
				_dryWet = Mathf.Clamp(value, 0, 1);
			}
		}

		[SerializeField]
		[Range(0,5)]
		private float _outputGain = 1;
		public float OutputGain{
			get{
				return _outputGain;
			}
			set{
				_outputGain = Mathf.Clamp(value, 0, 5);
			}
		}

#if UNITY_EDITOR
		public bool runEffectInEditMode = true;
		private bool runEffect = true;

		public System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		public float runTime = 0;
		
		public float MAX_inputGain = 0;
		public float MAX_outputGain = 0;
		
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
#endif
			for(var i = 0; i < data.Length; i += channels){
				for (var c = 0; c < channels; c++){

					data[i+c] *= InputGain;
#if UNITY_EDITOR
					MAX_inputGain = MAX_inputGain > Mathf.Abs(data[i+c]) ? MAX_inputGain : Mathf.Abs(data[i+c]);
#endif

					float distortedSample = data[i+c];
					if(Mathf.Abs(distortedSample)>Threshold){
						distortedSample = Mathf.Sign(distortedSample);
					}

					data[i+c] = (1-DryWet)*data[i+c] + DryWet * distortedSample;
					data[i+c] *= OutputGain;
#if UNITY_EDITOR
					MAX_outputGain = MAX_outputGain > data[i+c] ? MAX_outputGain : data[i+c];
#endif
				}
			}
#if UNITY_EDITOR
			stopwatch.Stop();
			runTime = Mathf.Round((float)stopwatch.Elapsed.TotalMilliseconds*100)/100;// stopwatch.ElapsedMilliseconds;
#endif
		}

	}
}
