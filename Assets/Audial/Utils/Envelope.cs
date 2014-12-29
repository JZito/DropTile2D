using UnityEngine;
using System.Collections;

namespace Audial.Utils{
	enum EnvelopeState {Idle, Attack, Decay, Release}
	[System.Serializable]
	public class Envelope{

		EnvelopeState envelopeState = EnvelopeState.Idle;
		float env = 0;

		float _attack = 0;
		public float attackCoeff = 0;
		public float Attack{
			get{return _attack;}
			set{

				_attack = value;
				attackCoeff = Mathf.Exp(-1/(Audial.Utils.Settings.SampleRate * _attack));
			}
		}

		float _release = 0;
		float releaseCoeff = 0;
		public float Release{
			get{return _release;}
			set{
				_release = value;
				releaseCoeff = Mathf.Exp(-1/(Audial.Utils.Settings.SampleRate * _release));
			}
		}

		float sustain = 1;

		float sampleRate = 0;

		public Envelope(float attack, float release){
			Attack = attack;
			Release = release;
		}

		public float ProcessSample(float sample){
			float theta = sample > env ? attackCoeff : releaseCoeff;
			env = (1-theta) * sample + theta * env;
			return env;
		}
	}
}
