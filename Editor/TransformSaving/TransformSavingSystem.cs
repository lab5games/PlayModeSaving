using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System;
using System.Collections.Generic;

namespace Lab5Games.PlayModeSaving.Editor
{
    [InitializeOnLoad]
    public static class TransformSavingSystem
    {
        static Dictionary<string, TransformSaving> _serializers = new Dictionary<string, TransformSaving>();


        static TransformSavingSystem()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch(state)
            {
                case PlayModeStateChange.EnteredPlayMode: Init(); break;
                case PlayModeStateChange.ExitingPlayMode: Save(); break;
                case PlayModeStateChange.EnteredEditMode: Load(); break;
            }
        }


        static void Init()
        {
            _serializers.Clear();
        }

        static void Save()
        {
            if (_serializers.Count > 0)
            {
                try
                {
                    foreach(var ts in _serializers.Values)
                    {
                        TransformSaveLoad.Save(ts);
                    }

                    //Debug.Log("Transforms saved");
                }
                catch(Exception ex)
                {
                    Debug.LogError("Saving transforms error: " + ex.Message);
                }
            }
        }

        static void Load()
        {
            if(_serializers.Count > 0)
            {
                try
                {
                    TransformSaving[] serializers = UnityEngine.Object.FindObjectsOfType<TransformSaving>();

                    if(serializers != null && serializers.Length > 0)
                    {
                        foreach(var ts in serializers)
                        {
                            if(_serializers.ContainsKey(ts.GUID))
                            {
                                string path = ts.gameObject.scene.path;
                                Scene scene = EditorSceneManager.GetSceneByPath(path);
                                
                                if(!scene.isDirty)
                                    EditorSceneManager.MarkSceneDirty(scene);

                                TransformSaveLoad.Load(ts);
                            }
                        }

                        //Debug.Log("Transforms loaded");
                    }
                }
                catch(Exception ex)
                {
                    Debug.LogError("Loading transforms error: " + ex.Message);
                }
            }
        }

        public static void AddTransform(TransformSaving ts)
        {
            if(!_serializers.ContainsKey(ts.GUID))
            {
                _serializers.Add(ts.GUID, ts);
            }
        }

        public static void RemoveTransform(TransformSaving ts)
        {
            if(_serializers.ContainsKey(ts.GUID))
            {
                _serializers.Remove(ts.GUID);
            }
        }
    }
}
