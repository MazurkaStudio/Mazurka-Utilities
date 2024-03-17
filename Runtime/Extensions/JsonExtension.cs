using System.Collections.Generic;

namespace TheMazurkaStudio.Utilities.Serialization
{
    public static class JsonExtension 
    {
        public static void SerializeDictionary<T0, T1>(this Dictionary<T0, T1> data, out T0[] keyList, out T1[] valueList)
        {
            var size = data.Count;
            
            keyList = new T0[size];
            valueList = new T1[size];
            
            var index = 0;
            
            foreach (var d in data)
            {
                keyList[index] = d.Key;
                valueList[index] = d.Value;
                index++;
            }
        }
        public static void DeserializeDictionary<T0, T1>(this Dictionary<T0, T1> data, T0[] deserializedKeyData, T1[] deserializedValueData) 
        {
            if (deserializedKeyData == null || deserializedValueData == null) return;
            
            for (var i = 0; i < deserializedKeyData.Length; i++)
            {
                data.Add(deserializedKeyData[i], deserializedValueData[i]);
            }
        }
    }
}
