using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Reflection;

namespace Audial{
	public static class InspectorUtils{
		public static bool FloatSlider(float field, float min, float max, string label, string toolTip, out float val){
			bool changed = false;
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();{
				val = EditorGUILayout.Slider(new GUIContent(label,toolTip),field,min,max);
			}EditorGUILayout.EndHorizontal();
			if(EditorGUI.EndChangeCheck()){
				changed = true;
			}
			return changed;
		}

		public static bool FloatSliderExponential(float field, float min, float max, string label, string toolTip, out float val){
			bool changed = false;
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();{
				val = Mathf.Pow(10,EditorGUILayout.Slider(new GUIContent(label,toolTip),Mathf.Log10(field),Mathf.Log10(min),Mathf.Log10(max)));
				val = EditorGUILayout.FloatField(val,GUILayout.Width(60));
			}EditorGUILayout.EndHorizontal();
			if(EditorGUI.EndChangeCheck()){
				changed = true;
			}
			return changed;
		}

		public static bool IntSlider(int field, int min, int max, string label, string toolTip, out int val){
			bool changed = false;
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();{
				val = EditorGUILayout.IntSlider(new GUIContent(label,toolTip),field,min,max);
			}EditorGUILayout.EndHorizontal();
			if(EditorGUI.EndChangeCheck()){
				changed = true;
			}
			return changed;
		}

		public static void RecordObject(MonoBehaviour component, string msg){
			#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2
			Undo.RegisterUndo(component, msg);
			#else
			Undo.RecordObject(component, msg);
			#endif
		}

		public static void VolumeMeter(float signal){
			Rect rect = EditorGUILayout.BeginVertical();
			EditorGUI.ProgressBar(rect,signal,"");
			GUILayout.Space(8);
			EditorGUILayout.EndVertical();
		}
	}
}
