using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(SimpleDelay))]
	public class SimpleDelayInspector : Editor{
		private string inspectorName = "Simple Delay";

		public override void OnInspectorGUI(){
			float outFloat = 0;
			int outInt = 0;
			SimpleDelay component = (SimpleDelay) target;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.IntSlider(component.DelayLengthMS, 10, 3000, "Delay Length (ms)", "Offset of the delayed signal measured in milliseconds", out outInt)){
					InspectorUtils.RecordObject(component, inspectorName+" - Delay Length");
					component.DelayLengthMS = outInt;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSlider(component.DecayLength, 0.1f, 1, "Decay Length", "Length of signal decay", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Slope");
					component.DecayLength = outFloat;
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