using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(StateVariableFilter))]
	public class StateVariableFilterInspector : Editor{
		private string inspectorName = "State Variable Filter";

		public override void OnInspectorGUI(){
			float outFloat = 0;
			StateVariableFilter component = (StateVariableFilter) target;

			GUILayout.BeginVertical("Box");{
				FilterState currentFilter = component.Filter;
				FilterState newFilter = (FilterState)EditorGUILayout.EnumPopup(new GUIContent("Filter State","Hi Band,low band, etc"),component.Filter);
				if(currentFilter!=newFilter){
					InspectorUtils.RecordObject(component, inspectorName+" - Filter State");
					component.Filter = newFilter;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSliderExponential(component.Frequency, 60, 12000, "Frequency", "Frequency of Filter", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Frequency");
					component.Frequency = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSlider(component.Resonance, 0, 1, "Resonance (Q)", "Resonance (Q) of filter", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Resonance");
					component.Resonance = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				if(InspectorUtils.FloatSlider(component.Drive, 0, 0.1f, "Drive", "Filter drive", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Drive");
					component.Drive = outFloat;
					EditorGUIUtility.ExitGUI();
				}

				bool isAdditive = false;
				switch(newFilter){
				case FilterState.BandAdd:
				case FilterState.LowShelf:
				case FilterState.HighShelf:
					isAdditive = true;
					break;
				default:
					isAdditive = false;
					break;
				}
				
				if(isAdditive){
					if(InspectorUtils.FloatSlider(component.AdditiveGain, -1, 1, "Additive Gain", "Amount of selected frequency band to add to original signal", out outFloat)){
						InspectorUtils.RecordObject(component, inspectorName+" - Additive Gain");
						component.AdditiveGain = outFloat;
						EditorGUIUtility.ExitGUI();
					}
				}
			}GUILayout.EndVertical();

			component.runEffectInEditMode = GUILayout.Toggle(component.runEffectInEditMode, new GUIContent("Run Effect In Edit Mode ("+component.runTime.ToString()+" ms)","Enable/disable component when the editor is not playing.\nEnable to reduce CPU usage"));
			Repaint();
		}
	}
}