using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(StereoWidener))]
	public class StereoWidenerInspector : Editor{
		private string inspectorName = "Stereo Widener";

		public override void OnInspectorGUI(){
			float outFloat = 0;
			StereoWidener component = (StereoWidener) target;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.Width, 0, 2, "Width", "Amount spread throughout the stereo field. 0 is mono, 1 is normal stereo, 2 is extra wide", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Width");
					component.Width = outFloat;
					EditorGUIUtility.ExitGUI();
				}
			}GUILayout.EndVertical();

			component.runEffectInEditMode = GUILayout.Toggle(component.runEffectInEditMode, new GUIContent("Run Effect In Edit Mode ("+component.runTime.ToString()+" ms)","Enable/disable component when the editor is not playing.\nEnable to reduce CPU usage"));
			Repaint();
		}
	}
}