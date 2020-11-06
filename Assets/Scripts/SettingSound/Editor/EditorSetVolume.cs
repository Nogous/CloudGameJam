using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace PackageCreator.Sounds
{
    [CustomEditor(typeof(SetVolume))]
    public class EditorSetVolume : Editor
    {
        SerializedProperty mixer;
        SerializedProperty slider;
        SerializedProperty exposedMixerParameter;
        SerializedProperty textVolume;
        float _sliderDataSave;

        private void OnEnable()
        {
            mixer = serializedObject.FindProperty("mixer");
            slider = serializedObject.FindProperty("slider");
            exposedMixerParameter = serializedObject.FindProperty("_nameExposedParameter");
            textVolume = serializedObject.FindProperty("textObject");
        }

        private void OnDisable()
        {
            
        }

        public override void OnInspectorGUI()
        {
            SetVolume script = (SetVolume)target;

            EditorGUILayout.LabelField("Settings Volume Slider", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(mixer, new GUIContent("ref Audio Mixer"));
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(slider, new GUIContent("ref UI Slider"));
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(exposedMixerParameter, new GUIContent("Name ExposedMixer"));
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Actual Saved State Volume", EditorStyles.boldLabel);
            GUI.enabled = false;
            _sliderDataSave = EditorGUILayout.Slider(PlayerPrefs.GetFloat(script._nameExposedParameter, 1f), 0f, 1f);
            GUI.enabled = true;
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Settings Text Volume", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(textVolume, new GUIContent("ref Text Object"));


            serializedObject.ApplyModifiedProperties();
        }
    }
}