using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(Crusher))]
	public class CrusherInspector : Editor{
		private string inspectorName = "Crusher";
		public override void OnInspectorGUI(){
			float outFloat = 0;
			int outInt = 0;
			Crusher component = (Crusher) target;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.IntSlider(component.BitDepth, 1, 32, "Bit Depth", "Precision of audio samples. Less precision is more distorted.", out outInt)){
					InspectorUtils.RecordObject(component, inspectorName+" - Bit Depth");
					component.BitDepth = outInt;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSliderExponential(component.SampleRate, 0.001f, 1, "Sample Rate", "Sample rate effects the resolution of the sound. Smaller value increases grittiness.", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Sample Rate");
					component.SampleRate = outFloat;
					EditorGUIUtility.ExitGUI();
				}
			}GUILayout.EndVertical();

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.DryWet, 0, 1, "Dry/Wet", "Dry/Wet ratio", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Dry/Wet");
					component.DryWet = outFloat;
					EditorGUIUtility.ExitGUI();
					
				}
			}GUILayout.EndVertical();

			component.runEffectInEditMode = GUILayout.Toggle(component.runEffectInEditMode, new GUIContent("Run Effect In Edit Mode ("+component.runTime.ToString()+" ms)","Enable/disable component when the editor is not playing.\nEnable to reduce CPU usage"));
			Repaint();
		}
	}
}