using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(Fader))]
	public class FaderInspector : Editor{
		private string inspectorName = "Fader";

		float MAX_gain = 0;

		public override void OnInspectorGUI(){
			float outFloat = 0;
			Fader component = (Fader) target;

			MAX_gain *= 0.95f;

			MAX_gain = MAX_gain > component.MAX_gain ? MAX_gain : component.MAX_gain;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.Gain, 0, 3, "Gain", "Gain control for device", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Gain");
					component.Gain = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				InspectorUtils.VolumeMeter(MAX_gain);
				component.Mute = GUILayout.Toggle(component.Mute,new GUIContent("Mute","Toggle track on and off"));
			}GUILayout.EndVertical();

			component.runEffectInEditMode = GUILayout.Toggle(component.runEffectInEditMode, new GUIContent("Run Effect In Edit Mode ("+component.runTime.ToString()+" ms)","Enable/disable component when the editor is not playing.\nEnable to reduce CPU usage"));
			Repaint();
		}
	}
}