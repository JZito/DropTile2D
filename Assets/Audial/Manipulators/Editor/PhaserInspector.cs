using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Audial{
	[CustomEditor(typeof(Phaser))]
	public class PhaserInspector : Editor{
		private string inspectorName = "Phaser";
		public override void OnInspectorGUI(){
			float outFloat = 0;
			Phaser component = (Phaser) target;

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.Rate, 0.1f, 8, "Rate", "Speed of oscillation measured in hertz (oscillations per second)", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Rate");
					component.Rate = outFloat;
				}

				if(InspectorUtils.FloatSlider(component.Width, 0, 1, "Width", "Width of oscillation", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Width");
					component.Width = outFloat;
				}

				if(InspectorUtils.FloatSlider(component.Intensity, 0f, 1f, "Intensity", "Severity of the phase effect", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Intensity");
					component.Intensity = outFloat;
				}
			}GUILayout.EndVertical();

			GUILayout.BeginVertical("Box");{
				if(InspectorUtils.FloatSlider(component.DryWet, 0, 1, "Dry/Wet", "Dry/Wet ratio", out outFloat)){
					InspectorUtils.RecordObject(component, inspectorName+" - Dry/Wet");
					component.DryWet = outFloat;
				}
			}GUILayout.EndVertical();

			component.runEffectInEditMode = GUILayout.Toggle(component.runEffectInEditMode, new GUIContent("Run Effect In Edit Mode ("+component.runTime.ToString()+" ms)","Enable/disable component when the editor is not playing.\nEnable to reduce CPU usage"));
			Repaint();
		}
	}
}