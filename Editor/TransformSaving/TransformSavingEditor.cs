using UnityEngine;
using UnityEditor;

namespace Lab5Games.PlayModeSaving.Editor
{
    [CustomEditor(typeof(TransformSaving))]
    public class TransformSavingEditor : UnityEditor.Editor
    {
        TransformSaving _serializer;

        private void OnEnable()
        {
            _serializer = target as TransformSaving;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = EditorApplication.isPlaying;
            ToggleSaveDuringPlay(EditorGUILayout.Toggle("Save During Play", _serializer.saveDuringPlay));
        }


        void ToggleSaveDuringPlay(bool saving)
        {
            if(saving != _serializer.saveDuringPlay)
            {
                if(saving)
                {
                    TransformSavingSystem.AddTransform(_serializer);
                }
                else
                {
                    TransformSavingSystem.RemoveTransform(_serializer);
                }
            }

            _serializer.saveDuringPlay = saving;
        }
    }
}