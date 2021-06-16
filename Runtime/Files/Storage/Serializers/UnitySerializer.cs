using System.Text;
using Egsp.Core;
using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// Стандартный сериализатор Unity, использующий JsonUtility.
    /// </summary>
    public class UnitySerializer : ISerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            var data = JsonUtility.ToJson(obj);

            return Encoding.UTF8.GetBytes(data);
        }

        public Option<T> Deserialize<T>(byte[] serializedData)
        {
            var obj = JsonUtility.FromJson<T>(Encoding.UTF8.GetString(serializedData)); ;
            return obj;
        }
    }
}