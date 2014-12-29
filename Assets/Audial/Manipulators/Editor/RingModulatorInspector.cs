using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(RingModulator))]
	public class RingModulatorInspector : Editor{
		private string inspectorName = "Ring Modulator";
		public override void OnInspectorGUI(){
			float outFloat = 0;
			RingModulator component = (RingModulator) target;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSliderExponential(component.CarrierFrequency, 20f, 5000, "Carrier Frequency", "Speed of oscillation measured in hertz (oscillations per second)", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Rate");
					component.CarrierFrequency = outFloat;
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