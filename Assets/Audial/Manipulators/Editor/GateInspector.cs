using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(Gate))]
	public class GateInspector : Editor{
		private string inspectorName = "Gate";

		float MAX_inputGain = 0;
		float MAX_outputGain = 0;
		float MAX_gateState = 0;

		public override void OnInspectorGUI(){
			float outFloat = 0;
			Gate component = (Gate) target;

			MAX_inputGain *= 0.95f;
			MAX_outputGain *= 0.95f;
			MAX_gateState *= 0.95f;
			
			MAX_inputGain = MAX_inputGain > component.MAX_inputGain ? MAX_inputGain : component.MAX_inputGain;
			MAX_outputGain = MAX_outputGain > component.MAX_outputGain ? MAX_outputGain : component.MAX_outputGain;
			MAX_gateState = MAX_gateState > component.MAX_gateState ? MAX_gateState : component.MAX_gateState;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.InputGain, 0, 3, "Input Gain", "Volume going into device", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Input Gain");
					component.InputGain = outFloat;
					EditorGUIUtility.ExitGUI();
				}
				InspectorUtils.VolumeMeter(MAX_inputGain);
			}GUILayout.EndVertical();

			GUILayout.BeginVertical ("Box");{
				if(InspectorUtils.FloatSlider(component.Threshold, 0, 1, "Threshold", "Volume at which gate is triggered", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Threshold");
					component.Threshold = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSlider(component.Attack, 0, 1, "Attack", "How quickly the gate opens", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Attack");
					component.Attack = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSlider(component.Release, 0, 1, "Release", "How quickly the gate closes", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Released");
					component.Release = outFloat;
					EditorGUIUtility.ExitGUI();
				}
				InspectorUtils.VolumeMeter(MAX_gateState);
			}GUILayout.EndVertical();

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.OutputGain, 0, 5, "Output Gain", "Volume going out of the device", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Output Gain");
					component.OutputGain = outFloat;
					EditorGUIUtility.ExitGUI();
				}
				InspectorUtils.VolumeMeter(MAX_outputGain);
			}GUILayout.EndVertical();

			component.runEffectInEditMode = GUILayout.Toggle(component.runEffectInEditMode, new GUIContent("Run Effect In Edit Mode ("+component.runTime.ToString()+" ms)","Enable/disable component when the editor is not playing.\nEnable to reduce CPU usage"));
			Repaint();
		}
	}
}