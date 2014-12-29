using UnityEngine;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class Fader : MonoBehaviour {

		[SerializeField]
		private float _gain = 1;
		public float Gain{
			get{
				return _gain;
			}
			set{
				_gain = Mathf.Clamp(value, 0, 3);
			}
		}

		public bool Mute = false;

#if UNITY_EDITOR
		public bool runEffectInEditMode = true;
		private bool runEffect = true;

		public System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		public float runTime = 0;
		
		public float MAX_gain = 0;
		
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

		Utils.LFO lfo = new Utils.LFO();

		void OnAudioFilterRead(float[] data, int channels){
#if UNITY_EDITOR
			if(!runEffect)
				return;
			stopwatch.Reset();
			stopwatch.Start();
			MAX_gain = 0;
#endif
			if(Mute){
				for(var i = 0; i < data.Length; i++){
					data[i] = 0;
				}
			}else{
				for(var i = 0; i < data.Length; i++){
					data[i] *= Gain;
#if UNITY_EDITOR
					MAX_gain = MAX_gain > Mathf.Abs(data[i]) ? MAX_gain : Mathf.Abs(data[i]);
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