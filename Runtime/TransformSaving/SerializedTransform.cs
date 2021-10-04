using UnityEngine;

namespace Lab5Games.PlayModeSaving
{
    [System.Serializable]
    public class SerializedTransform 
    {
        public bool deepSave;

        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public SerializedTransform[] children;
    }
}
