using UnityEngine;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class Crusher : MonoBehaviour {
		[SerializeField]
		[HideInInspector]
		private int _bitDepth = 8;
		private long m;
		public int BitDepth{
			get{return _bitDepth;}
			set{
				if(value==_bitDepth)return;
				_bitDepth = Mathf.Clamp(value, 1,32);
				Callibrate();
			}
		}

		[SerializeField]
		[Range(0.001f,1)]
		private float _sampleRate = 0.1f;
		public float SampleRate{
			get{return _sampleRate;}
			set{
				if(value==_sampleRate)return;
				_sampleRate = Mathf.Clamp(value, 0.001f,1);
			}
		}

		[SerializeField]
		[Range(0,1)]
		private float _dryWet = 1;
		public float DryWet{
			get{return _dryWet;}
			set{
				if(value == _dryWet)return;
				_dryWet = Mathf.Clamp(value,0,1);
			}
		}

		private float[] y;
		private float cnt = 0;

		private Utils.LFO lfo;
		void Awake(){
			y = new float[2]{0,0};
			Callibrate();
		}

		void Callibrate(){
			m = 1<<(_bitDepth-1);
			m = m < 0 ? int.MaxValue : m;
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

			for (var i = 0; i < data.Length; i = i + channels){
				cnt+=SampleRate;
				if(cnt>=1){
					cnt-=1;
					for(var c = 0; c < channels; c++){
						y[c]=(float)((long)(data[i+c]*m))/(float)m;
					}
				}

				for(var c = 0; c < channels; c++){
					float wet = y[c];
					data[i+c] = data[i+c]*(1-DryWet) + wet*DryWet;
				}
			}
#if UNITY_EDITOR
			stopwatch.Stop();
			runTime = Mathf.Round((float)stopwatch.Elapsed.TotalMilliseconds*100)/100;
#endif
		}
	}
}