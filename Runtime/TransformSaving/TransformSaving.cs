using UnityEngine;

namespace Lab5Games.PlayModeSaving
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GuidComponent))]
    public class TransformSaving : MonoBehaviour
    {
        public bool deepSave = false;

        public bool saveDuringPlay { get; set; }

        public string GUID => GetComponent<GuidComponent>().GetGuid().ToString();

        public SerializedTransform Serialize()
        {
            return SaveTransform(this.transform, this.deepSave);
        }

        public void Deserialize(SerializedTransform serializedTransform)
        {
            LoadTransform(this.transform, serializedTransform);
        }

        public string GetInstanceFileName()
        {
            return System.IO.Directory.GetCurrentDirectory() + $"/Temp/TransformSaving/{gameObject.name}_{GUID}.tsv";
        }

        static SerializedTransform SaveTransform(Transform target, bool deepSave)
        {
            SerializedTransform serialized = new SerializedTransform();

            serialized.deepSave = deepSave;
            serialized.position = target.localPosition;
            serialized.rotation = target.localRotation;
            serialized.scale = target.localScale;

            if(deepSave && target.childCount > 0)
            {
                serialized.children = new SerializedTransform[target.childCount];

                for(int i=0; i<target.childCount; i++)
                {
                    serialized.children[i] = SaveTransform(target.GetChild(i), deepSave);
                }
            }

            return serialized;
        }

        static void LoadTransform(Transform target, SerializedTransform serializedTransform)
        {
            target.localPosition = serializedTransform.position;
            target.localRotation = serializedTransform.rotation;
            target.localScale = serializedTransform.scale;

            if(serializedTransform.deepSave && target.childCount > 0)
            {
                for(int i=0; i<target.childCount; i++)
                {
                    LoadTransform(target.GetChild(i), serializedTransform.children[i]);
                }
            }
        }
    }
}