using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(FoldbackDistortion))]
	public class FoldbackDistortionInspector : Editor{
		private string inspectorName = "Foldback Distortion";

		float MAX_inputGain = 0;
		float MAX_outputGain = 0;
		float MAX_gainReduction = 0;

		public override void OnInspectorGUI(){
			float outFloat = 0;
			FoldbackDistortion component = (FoldbackDistortion) target;

			MAX_inputGain *= 0.95f;
			MAX_outputGain *= 0.95f;

			MAX_inputGain = MAX_inputGain > component.MAX_inputGain ? MAX_inputGain : component.MAX_inputGain;
			MAX_outputGain = MAX_outputGain > component.MAX_outputGain ? MAX_outputGain : component.MAX_outputGain;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.InputGain, 0, 3, "Input Gain", "Volume going into device", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Input Gain");
					component.InputGain = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				InspectorUtils.VolumeMeter(MAX_inputGain);

				if(InspectorUtils.FloatSlider(component.SoftDistortAmount, 0, 1, "Soft Distort Amount", "Amount of distortion applied to the whole signal", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Soft Distort Amount");
					component.SoftDistortAmount = outFloat;
					EditorGUIUtility.ExitGUI();
				}
			}GUILayout.EndVertical();

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.Threshold, 0, 1, "Threshold", "Volume at which distortion is triggered", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Threshold");
					component.Threshold = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSlider(component.DistortAmount, 0.0001f, 1, "Distort Amount", "Amount of distortion applied starting at threshold", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Distort Amount");
					component.DistortAmount = outFloat;
					EditorGUIUtility.ExitGUI();
				}

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