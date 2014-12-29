using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(Compressor))]
	public class CompressorInspector : Editor{
		private string inspectorName = "Compressor";

		float MAX_inputGain = 0;
		float MAX_compressedGain = 0;
		float MAX_dryGain = 0;
		float MAX_outputGain = 0;
		float MAX_gainReduction = 0;

		public override void OnInspectorGUI(){
			float outFloat = 0;
			Compressor component = (Compressor) target;

			MAX_inputGain *= 0.95f;
			MAX_compressedGain *= 0.95f;
			MAX_dryGain *= 0.95f;
			MAX_outputGain *= 0.95f;
			MAX_gainReduction *= 0.95f;

			MAX_inputGain = Mathf.Max(MAX_inputGain, component.MAX_inputGain);
			MAX_compressedGain = Mathf.Max(MAX_compressedGain, component.MAX_compressedGain);
			MAX_dryGain = Mathf.Max(MAX_dryGain, component.MAX_dryGain);
			MAX_outputGain = Mathf.Max(MAX_outputGain, component.MAX_outputGain);
			MAX_gainReduction = Mathf.Max (MAX_gainReduction, component.MAX_gainReduction);

			GUILayout.Label(component.envelope.attackCoeff.ToString());

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.InputGain, 0, 3, "Input Gain", "Volume going into device", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Input Gain");
					component.InputGain = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				InspectorUtils.VolumeMeter(MAX_inputGain);
			}GUILayout.EndVertical();

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.Threshold, 0, 1, "Threshold", "Volume at which compression is triggered", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Threshold");
					component.Threshold = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSlider(component.Slope, 0, 2, "Slope", "Amount of compression", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Slope");
					component.Slope = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSlider(component.Attack, 0, 1, "Attack", "How quickly the component impacts sound", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Attack");
					component.Attack = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSlider(component.Release, 0, 1, "Release", "How quickly the audio returns to normal", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Released");
					component.Release = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				InspectorUtils.VolumeMeter(1-MAX_gainReduction);
			}GUILayout.EndVertical();

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.DryGain, 0, 5, "Dry Gain", "Amount of the dry signal into mix", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Dry Gain");
					component.DryGain = outFloat;
					EditorGUIUtility.ExitGUI();
				}
				
				InspectorUtils.VolumeMeter(MAX_dryGain);

				if(InspectorUtils.FloatSlider(component.CompressedGain, 0, 5, "Compressed Gain", "Amount of the compressed signal into mix", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Compressed Gain");
					component.CompressedGain = outFloat;
					EditorGUIUtility.ExitGUI();
				}
				
				InspectorUtils.VolumeMeter(MAX_compressedGain);

				if(InspectorUtils.FloatSlider(component.OutputGain, 0, 5, "Output Gain", "Volume going out of the device", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Output Gain");
					component.OutputGain = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				InspectorUtils.VolumeMeter(MAX_outputGain);
			}GUILayout.EndVertical();

			component.runEffectInEditMode = GUILayout.Toggle(component.runEffectInEditMode, new GUIContent("Run Effect In Edit Mode ("+component.runTime.ToString()+" ms)","Enable/disable component when the editor is not playing.\nEnable to reduce CPU usage"));
			Repaint();
		}
	}
}