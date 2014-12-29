using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(Flanger))]
	public class FlangerInspector : Editor{
		private string inspectorName = "Flanger";
		public override void OnInspectorGUI(){
			float outFloat = 0;
			Flanger component = (Flanger) target;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.Rate, 0.1f, 8, "Rate", "Speed of oscillation measured in hertz (oscillations per second)", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Rate");
					component.Rate = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSlider(component.Intensity, 0.1f, 0.9f, "Intensity", "Severity of the flange effect", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Intensity");
					component.Intensity = outFloat;
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