using UnityEngine;
using System.Collections;

namespace Audial.Utils{
	public class BufferedComponent {
		public float[,] buffer;
		private float loopTime;
		public float delayLength;
		public float decayLength;
		public int bufferLength;
		public float gain;
		public int readIndex = 0;
		public int writeIndex = 0;
		public int channelCount = 1;
		private float sampleRate = 0;

		public float DelayLength{
			get{return delayLength;}
			set{
				delayLength = value;
				Offset = (int)(delayLength*(Audial.Utils.Settings.SampleRate/1000));
			}
		}

		public BufferedComponent(float delayLength, float gain){
			this.DelayLength = delayLength;
			this.gain = gain;
			bufferLength = (int)Audial.Utils.Settings.SampleRate * 10;
			buffer = new float[channelCount, bufferLength];
		}

		public void SetGainByDecayTime(float decayLength){
			gain = Mathf.Pow(0.001f, delayLength/decayLength);
		}

		public float ProcessSample(int channel, float sample){
			if(channel>=channelCount){
				channelCount = channel+1;
				buffer = new float[channelCount, bufferLength];
			}
			readIndex = Offset > writeIndex ? (bufferLength + writeIndex - Offset) : writeIndex - Offset;
			float output = buffer[channel,readIndex];
			buffer[channel,writeIndex] = sample + output * gain;
			return output;
		}

		public float ProcessSample(float sample){
			readIndex = Offset > writeIndex ? (bufferLength + writeIndex - Offset) : writeIndex - Offset;
			float output = buffer[0,readIndex];
			buffer[0,writeIndex] = sample + output * gain;
			return output;
		}

		public void MoveIndex(){
			writeIndex = (writeIndex+1)%bufferLength;
		}

		public void Reset(){
			for(var b = 0; b < buffer.Length; b++){
				System.Array.Clear( buffer, 0, buffer.Length );
			}
			readIndex = 0;
		}

		[SerializeField]
		private int _offset = 0;
		public int Offset{
			get{return _offset;}
			set{_offset = (int)Mathf.Lerp(_offset,value,0.8f);}
		}
	}
}
