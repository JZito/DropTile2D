using UnityEngine;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class Saturator : MonoBehaviour {

		[SerializeField]
		[Range(0,3)]
		private float _inputGain = 1;
		public float InputGain{
			get{
				return _inputGain;
			}
			set{
				_inputGain = Mathf.Clamp(value,0,3);
			}
		}
		
		[SerializeField]
		[Range(0,1)]
		private float _threshold = 0.247f;
		public float Threshold{
			get{
				return _threshold;
			}
			set{
				_threshold = Mathf.Clamp(value,0,1);
			}
		}

		[SerializeField]
		[Range(0,1)]
		public float _amount = 0.5f;
		public float Amount{
			get{
				return _amount;
			}
			set{
				_amount = Mathf.Clamp(value, 0, 1);
			}
		}

#if UNITY_EDITOR
		public bool runEffectInEditMode = true;
		private bool runEffect = true;

		public System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		public float runTime = 0;
		
		public float MAX_inputGain = 0;
		
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

		float input;
		float sampleAbs;
		float sampleSign;
		void OnAudioFilterRead(float[] data, int channels){
#if UNITY_EDITOR
			if(!runEffect)
				return;
			stopwatch.Reset();
			stopwatch.Start();
#endif
			if(Amount==0){
				return;
			}
			for (var c = 0; c < channels; c++){
				for(var i = 0; i < data.Length; i += channels){

					input = data[i+c] * InputGain;					
					sampleAbs = Mathf.Abs(input);
					sampleSign = Mathf.Sign(input);
					if(sampleAbs>1){
						input = ((Threshold+1)/2) * sampleSign;
					}else if(sampleAbs > Threshold){
						input = (Threshold + (sampleAbs-Threshold)/(1+Mathf.Pow((sampleAbs-Threshold)/(1-Amount),2))) * sampleSign;
					}
					
					data[i+c] = input;
				}
			}
#if UNITY_EDITOR
			stopwatch.Stop();
			runTime = Mathf.Round((float)stopwatch.Elapsed.TotalMilliseconds*100)/100;// stopwatch.ElapsedMilliseconds;
#endif
		}
	}
}