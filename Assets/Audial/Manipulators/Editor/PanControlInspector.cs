using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(PanControl))]
	public class PanControlInspector : Editor{
		private string inspectorName = "Pan Control";

		float MAX_inputGain = 0;
		float MAX_outputGain = 0;
		float MAX_gainReduction = 0;

		public override void OnInspectorGUI(){
			float outFloat = 0;
			PanControl component = (PanControl) target;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.PanAmount, -1, 1, "Pan Amount", "Values range from -1 (left) to 1 (right). A value of 0 is centered.", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Pan Amount");
					component.PanAmount = outFloat;
					EditorGUIUtility.ExitGUI();
				}
			}GUILayout.EndVertical();

			component.runEffectInEditMode = GUILayout.Toggle(component.runEffectInEditMode, new GUIContent("Run Effect In Edit Mode ("+component.runTime.ToString()+" ms)","Enable/disable component when the editor is not playing.\nEnable to reduce CPU usage"));
			Repaint();
		}
	}
}