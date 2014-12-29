using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(Delay))]
	public class DelayInspector : Editor{
		private string inspectorName = "Delay";

		public override void OnInspectorGUI(){
			float outFloat = 0;
			int outInt = 0;
			Delay component = (Delay) target;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.BPM, 40, 300, "BPM", "Beats Per Minute of audio track", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - BPM");
					component.BPM = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.IntSlider(component.DelayCount, 1, 8, "Delay Count", "How many beats (measured in Delay Units) to delay track", out outInt)){
					InspectorUtils.RecordObject(component, inspectorName+" - Delay Count");
					component.DelayCount = outInt;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.IntSlider(component.DelayUnit, 1, 32, "Delay Unit", "Size Delay Units - larger number results in smaller units", out outInt)){
					InspectorUtils.RecordObject(component, inspectorName+" - Delay Unit");
					component.DelayUnit = outInt;
					EditorGUIUtility.ExitGUI();
				}

			}GUILayout.EndVertical();

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.Pan, -1, 1, "Pan", "Pan direction of delayed tracks - Ranges from -1 (left) to 1 (right). A value of 0 is centered.", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - BPM");
					component.Pan = outFloat;
					EditorGUIUtility.ExitGUI();
				}
				component.PingPong = GUILayout.Toggle(component.PingPong, new GUIContent("Ping Pong","Determines whether panning stays in one direction or bounces back and forth between speaker channels."));
			}GUILayout.EndVertical();

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.DecayLength, 0.1f, 1, "Decay Length", "How long the delay persists", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Decay Length");
					component.DecayLength = outFloat;
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