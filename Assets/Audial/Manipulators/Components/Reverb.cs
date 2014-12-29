using System;
using UnityEngine;
using System.Collections;

namespace Audial{
	
	[ExecuteInEditMode]
	public class Reverb : MonoBehaviour {
		
		void Awake(){
			Audial.Utils.Settings.SampleRate = AudioSettings.outputSampleRate;
			Initialize();
		}

		[SerializeField]
		private float _reverbTime = 1.55f;
		public float ReverbTime{
			get{
				return _reverbTime;
			}
			set{
				_reverbTime = Mathf.Clamp(value,0.5f,10);
				Callibrate();
			}
		}

		[SerializeField]
		private float _reverbGain = 1;
		public float ReverbGain{
			get{
				return _reverbGain;
			}
			set{
				_reverbGain = Mathf.Clamp(value,0.5f,5);
			}
		}

		[SerializeField]
		private float _dryWet = 0.16f;
		public float DryWet{
			get{
				return _dryWet;
			}
			set{
				_dryWet = Mathf.Clamp(value,0,1);
			}
		}

		private Audial.Utils.CombFilter[] combFilters;
		private Audial.Utils.AllPassFilter[] allPassFilters;

		void Initialize(){
			combFilters = new Audial.Utils.CombFilter[4];
			combFilters[0] = new Audial.Utils.CombFilter(29.7f, 1);
			combFilters[1] = new Audial.Utils.CombFilter(37.1f, 1);
			combFilters[2] = new Audial.Utils.CombFilter(41.1f, 1);
			combFilters[3] = new Audial.Utils.CombFilter(43.7f, 1);

			Callibrate();

			allPassFilters = new Audial.Utils.AllPassFilter[2];
			allPassFilters[0] = new Audial.Utils.AllPassFilter(5.0f, 1);
			allPassFilters[0].SetGainByDecayTime(1.683f);
			allPassFilters[1] = new Audial.Utils.AllPassFilter(1.7f, 1);
			allPassFilters[1].SetGainByDecayTime(2.232f);


		}

		void Callibrate(){
			if(combFilters==null)return;
			for(var i = 0; i < combFilters.Length; i++){
				combFilters[i].SetGainByDecayTime(ReverbTime*1000);
			}
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
			if(combFilters==null||allPassFilters==null){
				Initialize();
			}
			for (var i = 0; i < data.Length; i = i + channels){
				for (var c = 0; c < channels; c++){
					float combBase = data[i+c] * ReverbGain;
					for(var f = 0; f < combFilters.Length; f++){
						combBase += combFilters[f].ProcessSample(c,data[i+c]);
					}

					combBase /= combFilters.Length;

					float allPassBase = combBase / combFilters.Length;
					for(var a = 0; a < allPassFilters.Length; a++){
						allPassBase += allPassFilters[a].ProcessSample(c,combBase);
					}

					data[i+c] = data[i+c] * (1f-DryWet) + (allPassBase * ReverbGain / (allPassFilters.Length)) * DryWet ;
				}
				for(var c = 0; c < combFilters.Length; c++){
					combFilters[c].MoveIndex();
				}
				for(var a = 0; a < allPassFilters.Length; a++){
					allPassFilters[a].MoveIndex();
				}
			}
#if UNITY_EDITOR
			stopwatch.Stop();
			runTime = Mathf.Round((float)stopwatch.Elapsed.TotalMilliseconds*100)/100;// stopwatch.ElapsedMilliseconds;
#endif
		}
	}
}