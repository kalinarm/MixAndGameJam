using UnityEngine;
using System.Collections;
using UnityEditor;
namespace MG
{
    [CustomEditor(typeof(SoundTester), true)]
    public class SoundTesterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);

            if (GUILayout.Button("Play Audio Trigger"))
            {
                ((SoundTester)target).PlayAudioTrigger();
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}
