using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(Saturator))]
	public class SaturatorInspector : Editor{
		private string inspectorName = "Saturator";

		float MAX_inputGain = 0;

		public override void OnInspectorGUI(){
			float outFloat = 0;
			Saturator component = (Saturator) target;

			MAX_inputGain *= 0.95f;

			MAX_inputGain = MAX_inputGain > component.MAX_inputGain ? MAX_inputGain : component.MAX_inputGain;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.InputGain, 0, 3, "Input Gain", "Volume going into device", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Input Gain");
					component.InputGain = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				InspectorUtils.VolumeMeter(MAX_inputGain);
			}GUILayout.EndVertical();

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.Threshold, 0, 1, "Threshold", "Volume at which saturation is triggered", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Threshold");
					component.Threshold = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSlider(component.Amount, 0.0001f, 1, "Amount", "The amount of saturation applied to the audio source.", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Amount");
					component.Amount = outFloat;
					EditorGUIUtility.ExitGUI();
				}
			}GUILayout.EndVertical();

			component.runEffectInEditMode = GUILayout.Toggle(component.runEffectInEditMode, new GUIContent("Run Effect In Edit Mode ("+component.runTime.ToString()+" ms)","Enable/disable component when the editor is not playing.\nEnable to reduce CPU usage"));
			Repaint();
		}
	}
}