using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(Reverb))]
	public class ReverbInspector : Editor{
		private string inspectorName = "Reverb";

		public override void OnInspectorGUI(){
			float outFloat = 0;
			Reverb component = (Reverb) target;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.ReverbTime, 0.5f, 10, "Reverb Time", "Duration of reverbations", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Reverb Time");
					component.ReverbTime = outFloat;
					EditorGUIUtility.ExitGUI();
				}
				if(InspectorUtils.FloatSlider(component.ReverbGain, 0.5f, 5, "Reverb Gain", "Volume of reverberations", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Reverb Gain");
					component.ReverbGain = outFloat;
					EditorGUIUtility.ExitGUI();
				}
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