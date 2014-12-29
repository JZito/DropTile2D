using UnityEngine;
using System;
using System.Collections;

namespace Audial.Utils{
	public class CombFilter : BufferedComponent{

		public CombFilter(float delayLength, float gain) : base(delayLength, gain){}

		public float ProcessSample(int channel, float sample){
			float sampleOut =  base.ProcessSample(channel, sample);
			return sampleOut;
		}
	}
}
