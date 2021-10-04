using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Lab5Games.PlayModeSaving.Editor
{
    public static class TransformSaveLoad
    {
        public static void Save(TransformSaving ts)
        {
            Save(ts.GetInstanceFileName(), ts.Serialize());
        }

        public static void Load(TransformSaving ts)
        {
            ts.Deserialize(Load(ts.GetInstanceFileName()));
        }

        public static void Save(string saveName, SerializedTransform saveData)
        {
            string dir = Path.GetDirectoryName(saveName);
            
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (FileStream stream = new FileStream(saveName, FileMode.OpenOrCreate))
            {
                GetBinaryFormatter().Serialize(stream, saveData);
            }
        }

        public static SerializedTransform Load(string saveName)
        {
            using (FileStream stream = new FileStream(saveName, FileMode.Open))
            {
                return GetBinaryFormatter().Deserialize(stream) as SerializedTransform ;
            }
        }

        private static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SurrogateSelector selector = new SurrogateSelector();

            Vector3SerializationSurrogate v3Surrogate = new Vector3SerializationSurrogate();
            QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate();

            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3Surrogate);
            selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);

            formatter.SurrogateSelector = selector;

            return formatter;
        }
    }
}
