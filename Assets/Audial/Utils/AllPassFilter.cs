using UnityEngine;
using System;
using System.Collections;

namespace Audial.Utils{
	public class AllPassFilter : BufferedComponent{
		
		public AllPassFilter(float delayLength, float gain) : base(delayLength, gain){}
		
		public float ProcessSample(int channel, float sample){
			float sampleOut = base.ProcessSample(channel,sample);
			return sampleOut - gain * sample;
		}
	}
}
